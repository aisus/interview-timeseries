using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using TimeSeriesStorage.Data;
using TimeSeriesStorage.Data.Models;
using TimeSeriesStorage.Services.Extensions;

namespace TimeSeriesStorage.Services;

public class MetricsEntryService : IMetricsEntryService
{
    protected readonly TimeSeriesDbContext context;
    protected readonly ILogger<MetricsEntryService> logger;

    public MetricsEntryService(TimeSeriesDbContext context, ILogger<MetricsEntryService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<MetricsEntry> WriteEntry(MetricsEntry entry)
    {
        logger.LogInformation("Writing new metrics entry...");
        await context.AddAsync(entry);
        await context.SaveChangesAsync();
        return entry;
    }

    public async Task<PaginatedResponce<MetricsEntry>> QueryEntries<TKey>(
        Expression<Func<MetricsEntry, bool>> filter, 
        Expression<Func<MetricsEntry, TKey>> sortingKeySelector, 
        bool descending, 
        int page, 
        int limit)
    {
        logger.LogInformation("Querying metrics...");
        var query = context.MetricsEntries.WhereIf(filter, filter != null);

        if (descending)
        {
            query = query.OrderByDescending(sortingKeySelector);
        }
        else
        {
            query = query.OrderBy(sortingKeySelector);
        }

        return await query.GetPageAsync(new Models.PageModel(limit, page));
    }
}
