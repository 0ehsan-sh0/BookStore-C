namespace BookStoreApi.RequestHandler.Admin.Responses.Book
{
    public class BookAllDataListResponse
    {
        public List<RBookAllData>? Books { get; set; }
        public BPaginationInfo? Pagination { get; set; }
    }
}
