namespace BookStoreApi.RequestHandler.User.Responses.Comment
{
    public class CommentListResponse
    {
        public List<RComment>? Comments { get; set; }
        public UserCommentPaginationInfo? Pagination { get; set; }
    }
}
