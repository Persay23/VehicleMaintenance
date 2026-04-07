using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Models.Entities;
namespace VehicleMaintenance.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleComponent> VehicleComponents { get; set; }
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
    public DbSet<MaintenanceRecordComponent> MaintenanceRecordComponents { get; set; }
    public DbSet<LiquidEntry> LiquidEntries { get; set; }
    public DbSet<Prediction> Predictions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.User)
            .WithMany(u => u.Vehicles)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VehicleComponent>()
            .HasKey(c => c.ComponentId); // why do i need this here? isnt it automatically set as the primary key since its named Id? and only for this one, every other entity has the same convention but doesnt need this line

        modelBuilder.Entity<VehicleComponent>()
            .HasOne(c => c.Vehicle)
            .WithMany(v => v.VehicleComponents)
            .HasForeignKey(c => c.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MaintenanceRecord>()
            .HasOne(mr => mr.Vehicle)
            .WithMany(v => v.MaintenanceRecords)
            .HasForeignKey(mr => mr.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MaintenanceRecordComponent>()
            .HasOne(mrc => mrc.MaintenanceRecord)
            .WithMany(mr => mr.MaintenanceRecordComponents)
            .HasForeignKey(mrc => mrc.MaintenanceRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MaintenanceRecordComponent>()
            .HasOne(mrc => mrc.Component)
            .WithMany(c => c.MaintenanceRecordComponents)
            .HasForeignKey(mrc => mrc.ComponentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LiquidEntry>()
            .HasOne(le => le.Vehicle)
            .WithMany(v => v.LiquidEntries)
            .HasForeignKey(le => le.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Prediction>()
            .HasOne(p => p.Vehicle)
            .WithMany(v => v.Predictions)
            .HasForeignKey(p => p.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}