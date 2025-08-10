namespace BookStoreApi.RequestHandler.Admin.Responses.Book
{
    public class RBook
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
        public int AuthorId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
