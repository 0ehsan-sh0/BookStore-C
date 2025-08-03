namespace BookStoreApi.Database.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string? Height { get; set; }
        public string? Width { get; set; }
        public bool IsPrimary { get; set; } = false;
        public string StoredFileName { get; set; } = string.Empty;
        public string RelativePath { get; set; } = string.Empty;
        public string? FileSize { get; set; }
        public string? MimeType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ForeignId { get; set; }
        public string ForeignTable { get; set; } = string.Empty;
    }
}
