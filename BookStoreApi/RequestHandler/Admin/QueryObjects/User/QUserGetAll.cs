namespace BookStoreApi.RequestHandler.Admin.QueryObjects.User
{
    public class QUserGetAll : QueryObject
    {
        public int? Role { get; set; }  // Filter by user role (1 for User, 2 for Admin)
    }
}