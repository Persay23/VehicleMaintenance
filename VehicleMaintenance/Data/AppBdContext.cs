using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Models.Entities;
namespace VehicleMaintenance.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleComponent> VehicleComponents { get; set; }
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
    public DbSet<MaintenanceRecordComponent> MaintenanceRecordComponents { get; set; }
    public DbSet<FuelEntry> FuelEntries { get; set; }
    public DbSet<Prediction> Predictions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.User)
            .WithMany(u => u.Vehicles)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        //modelBuilder.Entity<VehicleComponent>()
        //    .HasKey(c => c.VehicleComponentId); // should be renamed to VehicleComponentId to follow convention

        modelBuilder.Entity<VehicleComponent>()
            .HasOne(c => c.Vehicle)
            .WithMany(v => v.VehicleComponents)
            .HasForeignKey(c => c.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MaintenanceRecord>()
            .HasOne(mr => mr.Vehicle)
            .WithMany(v => v.MaintenanceRecords)
            .HasForeignKey(mr => mr.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MaintenanceRecordComponent>()
            .HasOne(mrc => mrc.MaintenanceRecord)
            .WithMany(mr => mr.MaintenanceRecordComponents)
            .HasForeignKey(mrc => mrc.MaintenanceRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MaintenanceRecordComponent>()
            .HasOne(mrc => mrc.Component)
            .WithMany(c => c.MaintenanceRecordComponents)
            .HasForeignKey(mrc => mrc.ComponentId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<FuelEntry>()
            .HasOne(le => le.Vehicle)
            .WithMany(v => v.FuelEntries)
            .HasForeignKey(le => le.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Prediction>()
            .HasOne(p => p.Vehicle)
            .WithMany(v => v.Predictions)
            .HasForeignKey(p => p.VehicleId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Prediction>()
            .HasOne<VehicleComponent>()
            .WithMany(c => c.Predictions)
            .HasForeignKey(p => p.VehicleComponentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}