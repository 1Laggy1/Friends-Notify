using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Friends_Notify.Data
{
    internal class FriendsNotifyDbContextFactory : IDesignTimeDbContextFactory<FriendsNotifyDbContext>
    {
        public FriendsNotifyDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<FriendsNotifyDbContext>()
                .UseSqlite(config.GetConnectionString("MainConnectionString"))
                .Options;

            return new FriendsNotifyDbContext(options);

        }
    }
}
