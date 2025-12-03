namespace BookStoreApi.Database.Models
{
    public class BookAllData
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
        public bool IsRecommended { get; set; }
        public int Stock { get; set; }
        public int? Quantity { get; set; }

        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<Translator>? Translators { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Tag>? Tags { get; set; }
        public List<Image>? Images { get; set; }
        public List<Invoice>? Invoices { get; set; }
        public Author? Author { get; set; }
    }
}
