using Microsoft.EntityFrameworkCore;
using TemuFlix.Data;

namespace TemuFlix.Tests.Helpers
{
    public static class TestDbHelper
    {
        public static AppDbContext CreateInMemoryDb(string dbName = "TestDb")
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