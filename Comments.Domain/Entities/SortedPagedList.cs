namespace Comments.Domain.Entities
{
    public class SortedPagedList<T> : PagedList<T>
    {
        public string SortOrder { get; set; }
        public string SortBy { get; set; }
    }
}
