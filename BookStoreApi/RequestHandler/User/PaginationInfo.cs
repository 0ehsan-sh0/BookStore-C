namespace BookStoreApi.RequestHandler.User.Responses
{
    public class PaginationInfo
    {
        public int? TotalCount { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? TotalPages { get; set; }
    }
}
