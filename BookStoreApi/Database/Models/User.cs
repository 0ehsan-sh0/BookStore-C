using Dapper.Contrib.Extensions;

namespace BookStoreApi.Database.Models
{
    [Table("Users")]

    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? EmailVerifiedAt { get; set; }
        public string Password { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LoggedInAt { get; set; }
    }
}

