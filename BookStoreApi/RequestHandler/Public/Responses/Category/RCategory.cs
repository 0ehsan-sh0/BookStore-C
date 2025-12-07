namespace BookStoreApi.RequestHandler.Public.Responses.Category
{
    public class RCategory
    {
        public string Name { get; set; } = string.Empty;
        public string? Url { get; set; }
        public int? MainCategoryId { get; set; }
        public List<RCategory>? SubCategories { get; set; }
    }
}
