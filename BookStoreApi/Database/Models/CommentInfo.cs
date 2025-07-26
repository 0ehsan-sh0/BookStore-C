using Dapper.Contrib.Extensions;

namespace BookStoreApi.Database.Models
{
    [Table("Comments")]

    public class CommentInfo
    {
        [Key]
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool? Status { get; set; }
        public string ForeignTable { get; set; } = string.Empty;
        public int ForeignId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
