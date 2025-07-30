namespace BookStoreApi.RequestHandler.Admin.Responses.Author
{
    public class AuthorListResponse
    {
        public List<RAuthor>? Authors { get; set; }
        public APaginationInfo? Pagination { get; set; }
    }
}
