namespace BookStoreApi.Services.Models
{
    public class ImageInfo
    {
        public string? Height { get; set; }
        public string? Width { get; set; }
        public string StoredFileName { get; set; } = string.Empty;
        public string RelativePath { get; set; } = string.Empty;
        public string? FileSize { get; set; }
        public string? MimeType { get; set; }
    }
}
