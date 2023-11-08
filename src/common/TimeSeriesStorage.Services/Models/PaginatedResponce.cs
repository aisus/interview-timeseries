using TimeSeriesStorage.Services.Models;

namespace TimeSeriesStorage.Services;

public class PaginatedResponce<T>
{
        public int ItemsCount { get; } = default;

        public int Limit { get; } = default;

        public int Page { get; } = default;

        public List<T> Results { get; } = new List<T>();

        public PaginatedResponce(PageModel pageModel, int itemsCount, List<T> results)
        {
            (Limit, Page, ItemsCount, Results) = (pageModel.Limit, pageModel.Page, itemsCount, results);
        }

        public PaginatedResponce(PageModel pageModel)
        {
            (Limit, Page, ItemsCount, Results) = (pageModel.Limit, pageModel.Page, 0, new List<T>());
        }
}
