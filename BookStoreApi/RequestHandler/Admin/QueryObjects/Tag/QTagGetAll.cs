namespace BookStoreApi.RequestHandler.Admin.QueryObjects.Tag
{
    public class QTagGetAll
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string Search { get; set; } = string.Empty;

    }
}
