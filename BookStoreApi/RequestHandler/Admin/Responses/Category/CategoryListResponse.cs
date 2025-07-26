namespace BookStoreApi.RequestHandler.Admin.Responses.Category
{
    public class CategoryListResponse
    {
        public List<RCategory>? Categories { get; set; }
        public CPaginationInfo? Pagination { get; set; }
    }
}
