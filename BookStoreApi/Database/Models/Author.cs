using Dapper.Contrib.Extensions;

namespace BookStoreApi.Database.Models
{
    [Table("Authors")]
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
