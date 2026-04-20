using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;

namespace VehicleMaintenance.Tests.Helpers
{
    public static class DbContextFactory
    {
        public static AppDbContext Create(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}