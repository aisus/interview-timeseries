using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TimeSeriesStorage.Services.Models;

namespace TimeSeriesStorage.Services.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> expr,
            bool condition)
        {
            return condition ? queryable.Where(expr) : queryable;
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, PageModel pageModel)
        {
            if (pageModel.Limit == 0)
                return queryable;
            else if (pageModel.Page <= 1)
                return queryable.Take(pageModel.Limit);
            else
                return queryable.Skip(pageModel.Offset).Take(pageModel.Limit);
        }

        public static async Task<PaginatedResponce<T>> GetPageAsync<T>(this IQueryable<T> queryable, PageModel pageModel)
        {
            var results = await queryable
                .Page(pageModel)
                .ToListAsync();

            var count = await queryable.CountAsync();
            return new PaginatedResponce<T>(pageModel, count, results);
        }
    }
}