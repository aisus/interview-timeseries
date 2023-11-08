using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace TimeSeriesStorage.Data.Models;

public class MetricsEntry
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; } = default;
    [JsonPropertyName("machine_id")]
    public Guid MachineId { get; set; } = default;
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; set; } = default;
}
