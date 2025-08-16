namespace BookStoreApi.RequestHandler.Admin.Responses.Comment
{
    public class CommentListResponse
    {
        public List<RComment>? Comments { get; set; }
        public COPaginationInfo? Pagination { get; set; }
    }
}
