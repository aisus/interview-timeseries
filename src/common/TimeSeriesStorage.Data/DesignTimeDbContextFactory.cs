using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TimeSeriesStorage.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TimeSeriesDbContext>
    {
        public TimeSeriesDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TimeSeriesDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("TimescaleDB"));

            return new TimeSeriesDbContext(optionsBuilder.Options);
        }
    }
}
