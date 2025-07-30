namespace BookStoreApi.RequestHandler.Admin.QueryObjects.Author
{
    public class QAuthorGetAll
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
