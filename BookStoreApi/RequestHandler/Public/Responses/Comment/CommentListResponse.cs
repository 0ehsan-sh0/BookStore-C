namespace BookStoreApi.RequestHandler.Public.Responses.Comment
{
    public class CommentListResponse
    {
        public List<RComment>? Comments { get; set; }
        public CommentPaginationInfo? Pagination { get; set; }
    }
}
