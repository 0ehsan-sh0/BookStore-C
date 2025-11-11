using BookStoreApi.Enums;

namespace BookStoreApi.RequestHandler.User.Responses.User
{
    public class RUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LoggedInAt { get; set; }
    }
}