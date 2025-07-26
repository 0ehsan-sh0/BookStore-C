using Dapper.Contrib.Extensions;

namespace BookStoreApi.Database.Models
{
    [Table("BookTags")]

    public class BookTag
    {
        public int BookId { get; set; }
        public int TagId { get; set; }
    }
}
