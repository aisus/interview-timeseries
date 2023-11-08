using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TimeSeriesStorage.Data.Models;

namespace TimeSeriesStorage.Data;

public class TimeSeriesDbContext : DbContext
{
    public DbSet<MetricsEntry> MetricsEntries { get; set; }
   
    public TimeSeriesDbContext() 
    { 
    }

    public TimeSeriesDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<MetricsEntry>().HasKey(m => new {m.Id, m.Timestamp});
    }
}