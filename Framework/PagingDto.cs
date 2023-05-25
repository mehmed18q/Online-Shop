namespace Framework
{
    public class Paging
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public class IdPaging<T> : Paging
    {
        public T? Id { get; set; }
    }

    public class IdPaging : Paging
    {
        public int Id { get; set; }
    }

    public class PagingId : Paging
    {
        public long Id { get; set; }
    }
}