using BookStoreApi.Enums;

namespace BookStoreApi.RequestHandler.Public.Responses.User
{
    public class RUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
