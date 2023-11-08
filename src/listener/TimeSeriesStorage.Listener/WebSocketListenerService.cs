using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TimeSeriesStorage.Data.Models;
using TimeSeriesStorage.Services;

namespace TimeSeriesStorage.Listener;

public class WebSocketListenerService : BackgroundService
{
    private readonly string wsConnectionUrl;

    private readonly ILogger<WebSocketListenerService> logger;
    private readonly IServiceProvider serviceProvider;

    public WebSocketListenerService(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<WebSocketListenerService> logger)
    {
        this.serviceProvider = serviceProvider;
        this.logger = logger;
        wsConnectionUrl = configuration.GetValue<string>("WsConnectionUrl") ?? string.Empty;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var socket = new ClientWebSocket();
            try
            {
                await socket.ConnectAsync(new Uri(wsConnectionUrl), stoppingToken);

                logger.LogInformation($"Listening for the WS connection...");
                await Receive(socket, stoppingToken);

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }

    private async Task Receive(ClientWebSocket socket, CancellationToken stoppingToken)
    {
        var buffer = new ArraySegment<byte>(new byte[2048]);
        while (!stoppingToken.IsCancellationRequested)
        {
            WebSocketReceiveResult result;
            using var ms = new MemoryStream();
            do
            {
                result = await socket.ReceiveAsync(buffer, stoppingToken);
                ms.Write(buffer.Array, buffer.Offset, result.Count);
            } while (!result.EndOfMessage);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                logger.LogInformation($"Websocket closed");
                break;
            }

            ms.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(ms, Encoding.UTF8);
            var data = await reader.ReadToEndAsync();
            logger.LogInformation($"Received data from websocket: {data}");

            try
            {
                var entry = JsonSerializer.Deserialize<MetricsEntry>(data);
                using var scope = serviceProvider.CreateScope();

                var metricsEntryService =
                    scope.ServiceProvider
                        .GetRequiredService<IMetricsEntryService>();

                await metricsEntryService.WriteEntry(entry);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        };
    }
}
