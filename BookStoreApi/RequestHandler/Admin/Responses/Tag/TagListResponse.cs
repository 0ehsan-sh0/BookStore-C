namespace BookStoreApi.RequestHandler.Admin.Responses.Tag
{
    public class TagListResponse
    {
        public List<RTag>? Tags { get; set; }
        public TagPaginationInfo? Pagination { get; set; }
    }
}
