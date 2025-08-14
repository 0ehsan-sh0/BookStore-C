using BookStoreApi.RequestHandler.Admin.Responses.Author;
using BookStoreApi.RequestHandler.Admin.Responses.Category;
using BookStoreApi.RequestHandler.Admin.Responses.Image;
using BookStoreApi.RequestHandler.Admin.Responses.Translator;

namespace BookStoreApi.RequestHandler.Admin.Responses.Book
{
    public class RBookAllData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long Price { get; set; }
        public short PrintSeries { get; set; } // سری چاپ
        public string ISBN { get; set; } = string.Empty; // شابک
        public string CoverType { get; set; } = string.Empty; // نوع جلد
        public string Format { get; set; } = string.Empty; // قطع
        public string Pages { get; set; } = string.Empty;
        public string PublishYear { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<RTranslator>? Translators { get; set; }
        public List<RCategory>? Categories { get; set; }
        public List<RImage>? Images { get; set; }
        public RAuthor? Author { get; set; }
    }
}
