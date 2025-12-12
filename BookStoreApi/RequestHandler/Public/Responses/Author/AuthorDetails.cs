using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.RequestHandler.Public.Responses.Author
{
    public class AuthorDetails
    {
        public RAuthor? Author { get; set; }
        public BookAllDataListResponse? Books { get; set; }
    }
}
