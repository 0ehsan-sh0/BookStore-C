using Dapper.Contrib.Extensions;

namespace BookStoreApi.Database.Models
{
    [Table("BookCategories")]
    public class BookCategory
    {
        public int BookId { get; set; }
        public int CategoryId { get; set; }
    }
}
