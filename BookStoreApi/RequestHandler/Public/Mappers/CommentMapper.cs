using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.Responses.Comment;

namespace BookStoreApi.RequestHandler.Public.Mappers
{
    public static class CommentMapper
    {
        public static RComment ToPublicComment(this CommentInfo comment)
        {
            return new RComment
            {
                Comment = comment.Comment,
                Status = comment.Status,
                UserId = comment.UserId,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,

                User = comment.User?.ToPublicUser(),
            };
        }
    }
}
