namespace BookStoreApi.RequestHandler.Admin.Responses.Category
{
    public class RCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Url { get; set; }
        public int? MainCategoryId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
