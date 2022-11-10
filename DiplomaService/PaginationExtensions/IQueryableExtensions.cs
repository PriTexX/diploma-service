namespace DiplomaService.PaginationExtensions;

public static class IQueryableExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int currentPage, int pageSize) where T : class
    {
        return await PagedList<T>.ToPagedListAsync(query, currentPage, pageSize);
    }
}