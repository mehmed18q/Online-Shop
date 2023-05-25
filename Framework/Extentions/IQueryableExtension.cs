namespace Framework
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> If<T>(this IQueryable<T> sourceQuery, bool condition, Func<IQueryable<T>, IQueryable<T>> query)
       => condition ? query(sourceQuery) : sourceQuery;

        public static IQueryable<T> Paging<T>(this IQueryable<T> queryable, Paging paging)
        {
            if (paging != null && paging.PageSize > 0 && paging.PageNumber > 0)
                return queryable.Skip(paging.PageSize * (paging.PageNumber - 1)).Take(paging.PageSize);
            else return queryable;
        }
    }
}