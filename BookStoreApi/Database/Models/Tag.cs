using Dapper.Contrib.Extensions;

namespace BookStoreApi.Database.Models
{
    [Table("Tags")]

    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
