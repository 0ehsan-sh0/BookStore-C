using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.RequestHandler.Public.Responses.Category
{
    public class CategoryDetails
    {
        public RCategory? Category { get; set; }
        public BookAllDataListResponse? Books { get; set; }
    }
}
