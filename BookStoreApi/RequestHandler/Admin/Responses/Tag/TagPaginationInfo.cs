namespace BookStoreApi.RequestHandler.Admin.Responses.Tag
{
    public class TagPaginationInfo
    {
        public int? TotalCount { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? TotalPages { get; set; }
    }
}
