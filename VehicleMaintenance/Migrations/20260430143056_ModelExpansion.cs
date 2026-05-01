using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class ModelExpansion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NextServiceRecommendedDate",
                table: "VehicleComponents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NextServiceRecommendedKm",
                table: "VehicleComponents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartNumber",
                table: "VehicleComponents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WarrantyDate",
                table: "VehicleComponents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarrantyKm",
                table: "VehicleComponents",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ServiceName",
                table: "MaintenanceRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceImageUrl",
                table: "MaintenanceRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "MaintenanceRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerComplaint",
                table: "MaintenanceRecordComponents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpectedLifetimeKm",
                table: "MaintenanceRecordComponents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpectedLifetimeYears",
                table: "MaintenanceRecordComponents",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextServiceRecommendedDate",
                table: "VehicleComponents");

            migrationBuilder.DropColumn(
                name: "NextServiceRecommendedKm",
                table: "VehicleComponents");

            migrationBuilder.DropColumn(
                name: "PartNumber",
                table: "VehicleComponents");

            migrationBuilder.DropColumn(
                name: "WarrantyDate",
                table: "VehicleComponents");

            migrationBuilder.DropColumn(
                name: "WarrantyKm",
                table: "VehicleComponents");

            migrationBuilder.DropColumn(
                name: "InvoiceImageUrl",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "CustomerComplaint",
                table: "MaintenanceRecordComponents");

            migrationBuilder.DropColumn(
                name: "ExpectedLifetimeKm",
                table: "MaintenanceRecordComponents");

            migrationBuilder.DropColumn(
                name: "ExpectedLifetimeYears",
                table: "MaintenanceRecordComponents");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceName",
                table: "MaintenanceRecords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
