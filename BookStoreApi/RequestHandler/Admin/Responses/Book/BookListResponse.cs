namespace BookStoreApi.RequestHandler.Admin.Responses.Book
{
    public class BookListResponse
    {
        public List<RBook>? books { get; set; }
        public BPaginationInfo? Pagination { get; set; }
    }
}
