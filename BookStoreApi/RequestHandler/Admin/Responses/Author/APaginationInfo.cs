namespace BookStoreApi.RequestHandler.Admin.Responses.Author
{
    public class APaginationInfo
    {
        public int? TotalCount { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? TotalPages { get; set; }
    }
}
