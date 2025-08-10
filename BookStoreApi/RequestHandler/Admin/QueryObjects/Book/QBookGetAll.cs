namespace BookStoreApi.RequestHandler.Admin.QueryObjects.Book
{
    public class QBookGetAll
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
