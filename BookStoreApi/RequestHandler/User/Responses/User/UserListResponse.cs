namespace BookStoreApi.RequestHandler.User.Responses.User
{
    public class UserListResponse
    {
        public List<RUser>? Users { get; set; }
        public UPaginationInfo? Pagination { get; set; }
    }
}
