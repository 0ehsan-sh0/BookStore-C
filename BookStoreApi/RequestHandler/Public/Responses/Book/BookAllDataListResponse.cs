namespace BookStoreApi.RequestHandler.Public.Responses.Book
{
    public class BookAllDataListResponse
    {
        public List<RBookAllData>? Books { get; set; }
        public BPPaginationInfo? Pagination { get; set; }
    }
}
