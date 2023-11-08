using System.Linq.Expressions;
using TimeSeriesStorage.Data.Models;

namespace TimeSeriesStorage.Services;

public interface IMetricsEntryService
{
    public Task<MetricsEntry> WriteEntry(MetricsEntry entry);

    public Task<PaginatedResponce<MetricsEntry>> QueryEntries<TKey>(
        Expression<Func<MetricsEntry, bool>> filter,
        Expression<Func<MetricsEntry, TKey>> sortingKeySelector,
        bool descending,
        int pageNumber,
        int pageSize);
}
