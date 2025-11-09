
using BookStoreApi.Enums;

namespace BookStoreApi.Database.Models
{

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LoggedInAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<AddressInfo>? Addresses { get; set; }
    }
}

