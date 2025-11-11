namespace BookStoreApi.RequestHandler.Admin.Responses.User
{
    public class UserListResponse
    {
        public List<RUser>? Users { get; set; }
        public UPaginationInfo? Pagination { get; set; }
    }
}
