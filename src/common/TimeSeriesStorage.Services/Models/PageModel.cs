namespace TimeSeriesStorage.Services.Models;

public class PageModel
{
    public int Limit { get; init; }

    public int Page { get; init; }

    public int Offset => Limit * (Page - 1);

    public PageModel()
    {
        (Limit, Page) = (999999, 0);
    }

    public PageModel(int limit, int page = 0)
    {
        (Limit, Page) = (limit, page);
    }
}