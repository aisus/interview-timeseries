using TimeSeriesStorage.Listener;
using Microsoft.EntityFrameworkCore;
using TimeSeriesStorage.Data;
using TimeSeriesStorage.Services;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((hostContext, services) =>
{
    var configuration = hostContext.Configuration;

    services.AddHostedService<WebSocketListenerService>();
    services.AddDbContext<TimeSeriesDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("TimescaleDB")));
    services.AddScoped<IMetricsEntryService, MetricsEntryService>();
});

builder.Build().Run();
