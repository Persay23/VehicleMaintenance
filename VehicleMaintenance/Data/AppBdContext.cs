using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Models;
namespace VehicleMaintenance.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}