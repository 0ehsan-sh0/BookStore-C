namespace BookStoreApi.RequestHandler.User.Responses.Comment
{
    public class RComment
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool Status { get; set; }
        public string ForeignTable { get; set; } = string.Empty;
        public int ForeignId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
