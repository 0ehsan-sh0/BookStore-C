using BookStoreApi.RequestHandler.Public.Responses.User;

namespace BookStoreApi.RequestHandler.Public.Responses.Comment
{
    public class RComment
    {
        public string Comment { get; set; } = string.Empty;
        public bool Status { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public RUser? User { get; set; }
    }
}
