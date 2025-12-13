using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.RequestHandler.Public.Responses.Tag
{
    public class TagDetails
    {
        public RTag? Tag { get; set; }
        public BookAllDataListResponse? Books { get; set; }
    }
}
