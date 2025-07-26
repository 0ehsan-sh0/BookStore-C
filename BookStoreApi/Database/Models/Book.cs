using Dapper.Contrib.Extensions;

namespace BookStoreApi.Database.Models
{
    [Table("Books")]

    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long price { get; set; }
        public short PrintSeries { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string CoverType { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public string Pages { get; set; } = string.Empty;
        public string PublishYear { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public int WriterId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
