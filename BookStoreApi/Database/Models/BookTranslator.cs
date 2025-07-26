using Dapper.Contrib.Extensions;

namespace BookStoreApi.Database.Models
{
    [Table("BookTranslators")]

    public class BookTranslator
    {
        public int BookId { get; set; }
        public int TranslatorId { get; set; }
    }
}
