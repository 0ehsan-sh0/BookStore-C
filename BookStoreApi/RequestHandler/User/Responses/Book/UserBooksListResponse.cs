namespace BookStoreApi.RequestHandler.User.Responses.Book
{
    public class UserBooksListResponse
    {
        public List<RBookAllData>? Books { get; set; }
        public BookPaginationInfo? Pagination { get; set; }
    }
}
