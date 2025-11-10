using BookStoreApi.Database.Models;
using BookStoreApi.Enums;

namespace BookStoreApi.RequestHandler.Auth.Responses
{
    public class MeResponse
    {
        public int Id { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public static MeResponse FromEntity(User user)
        {
            return new MeResponse
            {
                Id = user.Id,
                Mobile = user.Mobile,
                Role = user.Role,
            };
        }
    }
}