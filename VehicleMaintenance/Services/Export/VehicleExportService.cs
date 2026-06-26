using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using VehicleMaintenance.Data;
using VehicleMaintenance.Models.Entities;

namespace VehicleMaintenance.Services.Export;

public partial class VehicleExportService(AppDbContext context) : IVehicleExportService
{
    private readonly AppDbContext _context = context;

    // Everything needed to render one vehicle's history. Loaded read-only.
    private sealed record ExportData(
        Vehicle Vehicle,
        VehicleComponent[] Components,
        MaintenanceRecord[] Records,
        FuelEntry[] Fuel,
        GeneralExpense[] Expenses);

    public async Task<VehicleExportFile?> ExportAsync(int vehicleId, string userId, string format, CancellationToken ct = default)
    {
        var vehicle = await _context.Vehicles
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.VehicleId == vehicleId, ct);

        // Not found OR not owned → caller returns 404 (don't leak existence).
        if (vehicle is null || vehicle.UserId != userId) return null;

        var components = await _context.VehicleComponents
            .AsNoTracking()
            .Where(c => c.VehicleId == vehicleId)
            .OrderBy(c => c.ComponentType)
            .ToArrayAsync(ct);

        var records = await _context.MaintenanceRecords
            .AsNoTracking()
            .Where(r => r.VehicleId == vehicleId)
            .Include(r => r.MaintenanceRecordComponents)
                .ThenInclude(mrc => mrc.Component)
            .OrderByDescending(r => r.ServiceDate)
            .ToArrayAsync(ct);

        var fuel = await _context.FuelEntries
            .AsNoTracking()
            .Where(f => f.VehicleId == vehicleId)
            .OrderByDescending(f => f.RefillDate)
            .ToArrayAsync(ct);

        var expenses = await _context.GeneralExpenses
            .AsNoTracking()
            .Where(e => e.VehicleId == vehicleId)
            .OrderByDescending(e => e.Date)
            .ToArrayAsync(ct);

        var data = new ExportData(vehicle, components, records, fuel, expenses);

        return format switch
        {
            "md"  => new VehicleExportFile(Encoding.UTF8.GetBytes(BuildMarkdown(data)), "text/markdown", FileName(vehicle, "md")),
            "pdf" => new VehicleExportFile(BuildPdf(data), "application/pdf", FileName(vehicle, "pdf")),
            _     => null,
        };
    }

    // ── shared formatting ───────────────────────────────────────────────────

    private static string Money(decimal v) => v.ToString("N2", CultureInfo.InvariantCulture) + " zł";
    private static string D(DateTime? d) => d?.ToString("yyyy-MM-dd") ?? "—";

    private static string FileName(Vehicle v, string ext)
    {
        var slug = SlugRegex().Replace($"{v.Brand}-{v.Model}".ToLowerInvariant(), "-").Trim('-');
        if (slug.Length == 0) slug = $"vehicle-{v.VehicleId}";
        return $"{slug}-history.{ext}";
    }

    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex SlugRegex();

    private static string VendorSuffix(MaintenanceRecord r)
    {
        var bits = new List<string>();
        if (!string.IsNullOrWhiteSpace(r.VendorOrShop)) bits.Add(r.VendorOrShop!);
        if (!string.IsNullOrWhiteSpace(r.TechnicianName)) bits.Add(r.TechnicianName!);
        return bits.Count > 0 ? " · " + string.Join(" · ", bits) : "";
    }

    private static string ComponentName(MaintenanceRecordComponent mrc) =>
        mrc.Component?.VehicleComponentName ?? mrc.Component?.ComponentType.ToString() ?? "—";

    // ── Markdown ────────────────────────────────────────────────────────────

    private static string BuildMarkdown(ExportData d)
    {
        var v = d.Vehicle;
        var sb = new StringBuilder();

        sb.AppendLine($"# {v.Brand} {v.Model} ({v.YearOfProduction}) — Service History");
        sb.AppendLine();
        sb.AppendLine($"_Generated {DateTime.UtcNow:yyyy-MM-dd} · AutoCare_");
        sb.AppendLine();

        sb.AppendLine("## Vehicle");
        sb.AppendLine();
        sb.AppendLine($"- **Type:** {v.VehicleType}");
        sb.AppendLine($"- **Engine / Fuel / Transmission:** {v.EngineType} / {v.FuelType} / {v.TransmissionType}");
        sb.AppendLine($"- **Mileage:** {v.Mileage:N0} km");
        sb.AppendLine($"- **Avg km/year:** {(v.AverageKmPerYear?.ToString("N0") ?? "—")}");
        sb.AppendLine();

        sb.AppendLine($"## Components ({d.Components.Length})");
        sb.AppendLine();
        if (d.Components.Length == 0) sb.AppendLine("_None recorded._");
        else
        {
            sb.AppendLine("| Type | Name | Brand | Installed | At km | Lifetime | State | Warranty |");
            sb.AppendLine("|---|---|---|---|---|---|---|---|");
            foreach (var c in d.Components)
            {
                var lifetime = $"{c.ExpectedLifetimeKm:N0} km / {c.ExpectedLifetimeYears} yr";
                var warranty = c.WarrantyDate is not null ? D(c.WarrantyDate)
                             : c.WarrantyKm is not null ? $"{c.WarrantyKm:N0} km" : "—";
                sb.AppendLine($"| {c.ComponentType} | {c.VehicleComponentName ?? "—"} | {c.VehicleComponentBrand ?? "—"} | {D(c.InstallationDate)} | {c.InstalledAtVehicleMileage:N0} | {lifetime} | {c.State} | {warranty} |");
            }
        }
        sb.AppendLine();

        sb.AppendLine($"## Maintenance Records ({d.Records.Length})");
        sb.AppendLine();
        if (d.Records.Length == 0) sb.AppendLine("_None recorded._");
        foreach (var r in d.Records)
        {
            sb.AppendLine($"### {D(r.ServiceDate)} — {r.ServiceName} ({r.ServiceType})");
            sb.AppendLine();
            sb.AppendLine($"- **Mileage:** {(r.Mileage is null ? "—" : $"{r.Mileage:N0} km")} · **Cost:** {Money(r.Cost)}{VendorSuffix(r)}");
            if (!string.IsNullOrWhiteSpace(r.Description)) sb.AppendLine($"- {r.Description}");
            foreach (var mrc in r.MaintenanceRecordComponents)
            {
                var work = string.IsNullOrWhiteSpace(mrc.WorkDescription) ? "" : $": {mrc.WorkDescription}";
                var cost = mrc.TotalCost is not null ? $" — {Money(mrc.TotalCost.Value)}" : "";
                sb.AppendLine($"  - {mrc.ComponentChangeType} · {ComponentName(mrc)}{work}{cost}");
            }
            sb.AppendLine();
        }

        sb.AppendLine($"## Fuel Log ({d.Fuel.Length})");
        sb.AppendLine();
        if (d.Fuel.Length == 0) sb.AppendLine("_None recorded._");
        else
        {
            sb.AppendLine("| Date | Mileage | Type | Litres | Cost | Price/L |");
            sb.AppendLine("|---|---|---|---|---|---|");
            foreach (var f in d.Fuel)
            {
                var perL = f.Amount > 0 ? Money(f.Cost / f.Amount) : "—";
                sb.AppendLine($"| {D(f.RefillDate)} | {f.Mileage:N0} | {f.FuelType} | {f.Amount:N2} | {Money(f.Cost)} | {perL} |");
            }
        }
        sb.AppendLine();

        sb.AppendLine($"## General Expenses ({d.Expenses.Length})");
        sb.AppendLine();
        if (d.Expenses.Length == 0) sb.AppendLine("_None recorded._");
        else
        {
            sb.AppendLine("| Date | Category | Cost | Description |");
            sb.AppendLine("|---|---|---|---|");
            foreach (var e in d.Expenses)
                sb.AppendLine($"| {D(e.Date)} | {e.ExpenseCategory} | {Money(e.Cost)} | {e.Description ?? "—"} |");
        }
        sb.AppendLine();

        var mTotal = d.Records.Sum(r => r.Cost);
        var fTotal = d.Fuel.Sum(f => f.Cost);
        var eTotal = d.Expenses.Sum(e => e.Cost);

        sb.AppendLine("## Cost Summary");
        sb.AppendLine();
        sb.AppendLine($"- **Maintenance:** {Money(mTotal)}");
        sb.AppendLine($"- **Fuel:** {Money(fTotal)}");
        sb.AppendLine($"- **Other expenses:** {Money(eTotal)}");
        sb.AppendLine($"- **Grand total:** {Money(mTotal + fTotal + eTotal)}");
        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine("_All amounts in PLN (zł)._");

        return sb.ToString();
    }

    // ── PDF (QuestPDF) ──────────────────────────────────────────────────────

    private static byte[] BuildPdf(ExportData d)
    {
        var v = d.Vehicle;
        var mTotal = d.Records.Sum(r => r.Cost);
        var fTotal = d.Fuel.Sum(f => f.Cost);
        var eTotal = d.Expenses.Sum(e => e.Cost);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(28);
                page.DefaultTextStyle(x => x.FontSize(9).FontColor(Colors.Grey.Darken3));

                page.Header().Column(h =>
                {
                    h.Item().Text($"{v.Brand} {v.Model} ({v.YearOfProduction})").FontSize(18).Bold().FontColor(Colors.Black);
                    h.Item().Text("Vehicle Service History").FontSize(11).FontColor(Colors.Grey.Medium);
                });

                page.Content().PaddingVertical(10).Column(col =>
                {
                    col.Spacing(12);

                    col.Item().Background(Colors.Grey.Lighten4).Padding(10).Column(c =>
                    {
                        c.Item().Text($"Type: {v.VehicleType}    Engine: {v.EngineType}    Fuel: {v.FuelType}    Transmission: {v.TransmissionType}");
                        c.Item().Text($"Mileage: {v.Mileage:N0} km    Avg: {(v.AverageKmPerYear?.ToString("N0") ?? "—")} km/yr");
                    });

                    // Components
                    Section(col, $"Components ({d.Components.Length})");
                    if (d.Components.Length == 0) col.Item().Text("None recorded.").Italic();
                    else col.Item().Table(t =>
                    {
                        t.ColumnsDefinition(c => { c.RelativeColumn(2); c.RelativeColumn(3); c.RelativeColumn(2); c.RelativeColumn(2); c.RelativeColumn(2); c.RelativeColumn(2); });
                        t.Header(h => { HeaderCell(h, "Type"); HeaderCell(h, "Name"); HeaderCell(h, "Brand"); HeaderCell(h, "Installed"); HeaderCell(h, "State"); HeaderCell(h, "Warranty"); });
                        foreach (var c in d.Components)
                        {
                            var warranty = c.WarrantyDate is not null ? D(c.WarrantyDate)
                                         : c.WarrantyKm is not null ? $"{c.WarrantyKm:N0} km" : "—";
                            BodyCell(t, c.ComponentType.ToString());
                            BodyCell(t, c.VehicleComponentName ?? "—");
                            BodyCell(t, c.VehicleComponentBrand ?? "—");
                            BodyCell(t, D(c.InstallationDate));
                            BodyCell(t, c.State.ToString());
                            BodyCell(t, warranty);
                        }
                    });

                    // Maintenance
                    Section(col, $"Maintenance Records ({d.Records.Length})");
                    if (d.Records.Length == 0) col.Item().Text("None recorded.").Italic();
                    else foreach (var r in d.Records)
                    {
                        col.Item().Column(rc =>
                        {
                            rc.Item().Text(t =>
                            {
                                t.Span($"{D(r.ServiceDate)} — ").SemiBold();
                                t.Span(r.ServiceName).Bold();
                                t.Span($"  ({r.ServiceType})").FontColor(Colors.Grey.Medium);
                            });
                            rc.Item().Text($"{(r.Mileage is null ? "—" : $"{r.Mileage:N0} km")} · {Money(r.Cost)}{VendorSuffix(r)}");
                            if (!string.IsNullOrWhiteSpace(r.Description))
                                rc.Item().Text(r.Description!).FontColor(Colors.Grey.Darken1);
                            foreach (var mrc in r.MaintenanceRecordComponents)
                            {
                                var work = string.IsNullOrWhiteSpace(mrc.WorkDescription) ? "" : $": {mrc.WorkDescription}";
                                var cost = mrc.TotalCost is not null ? $" — {Money(mrc.TotalCost.Value)}" : "";
                                rc.Item().PaddingLeft(12).Text($"• {mrc.ComponentChangeType} · {ComponentName(mrc)}{work}{cost}").FontColor(Colors.Grey.Darken1);
                            }
                        });
                    }

                    // Fuel
                    Section(col, $"Fuel Log ({d.Fuel.Length})");
                    if (d.Fuel.Length == 0) col.Item().Text("None recorded.").Italic();
                    else col.Item().Table(t =>
                    {
                        t.ColumnsDefinition(c => { c.RelativeColumn(2); c.RelativeColumn(2); c.RelativeColumn(2); c.RelativeColumn(2); c.RelativeColumn(2); c.RelativeColumn(2); });
                        t.Header(h => { HeaderCell(h, "Date"); HeaderCell(h, "Mileage"); HeaderCell(h, "Type"); HeaderCell(h, "Litres"); HeaderCell(h, "Cost"); HeaderCell(h, "Price/L"); });
                        foreach (var f in d.Fuel)
                        {
                            var perL = f.Amount > 0 ? Money(f.Cost / f.Amount) : "—";
                            BodyCell(t, D(f.RefillDate));
                            BodyCell(t, $"{f.Mileage:N0}");
                            BodyCell(t, f.FuelType.ToString());
                            BodyCell(t, $"{f.Amount:N2}");
                            BodyCell(t, Money(f.Cost));
                            BodyCell(t, perL);
                        }
                    });

                    // Expenses
                    Section(col, $"General Expenses ({d.Expenses.Length})");
                    if (d.Expenses.Length == 0) col.Item().Text("None recorded.").Italic();
                    else col.Item().Table(t =>
                    {
                        t.ColumnsDefinition(c => { c.RelativeColumn(2); c.RelativeColumn(2); c.RelativeColumn(2); c.RelativeColumn(4); });
                        t.Header(h => { HeaderCell(h, "Date"); HeaderCell(h, "Category"); HeaderCell(h, "Cost"); HeaderCell(h, "Description"); });
                        foreach (var e in d.Expenses)
                        {
                            BodyCell(t, D(e.Date));
                            BodyCell(t, e.ExpenseCategory.ToString());
                            BodyCell(t, Money(e.Cost));
                            BodyCell(t, e.Description ?? "—");
                        }
                    });

                    // Totals
                    Section(col, "Cost Summary");
                    col.Item().Column(s =>
                    {
                        s.Item().Text($"Maintenance:  {Money(mTotal)}");
                        s.Item().Text($"Fuel:  {Money(fTotal)}");
                        s.Item().Text($"Other expenses:  {Money(eTotal)}");
                        s.Item().PaddingTop(2).Text($"Grand total:  {Money(mTotal + fTotal + eTotal)}").Bold().FontSize(11).FontColor(Colors.Black);
                    });
                });

                page.Footer().AlignCenter().Text(t =>
                {
                    t.DefaultTextStyle(x => x.FontSize(8).FontColor(Colors.Grey.Medium));
                    t.Span($"Generated {DateTime.UtcNow:yyyy-MM-dd} · AutoCare · amounts in PLN (zł) · page ");
                    t.CurrentPageNumber();
                    t.Span("/");
                    t.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    private static void Section(QuestPDF.Fluent.ColumnDescriptor col, string title) =>
        col.Item().PaddingTop(4).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingBottom(2)
           .Text(title).FontSize(12).Bold().FontColor(Colors.Black);

    private static void HeaderCell(QuestPDF.Fluent.TableCellDescriptor header, string text) =>
        header.Cell().Background(Colors.Grey.Lighten3).PaddingVertical(3).PaddingHorizontal(4)
              .Text(text).SemiBold();

    private static void BodyCell(QuestPDF.Fluent.TableDescriptor table, string text) =>
        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).PaddingVertical(3).PaddingHorizontal(4)
             .Text(text);
}
