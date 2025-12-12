using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.Responses.Comment;

namespace BookStoreApi.RequestHandler.User.Mappers
{
    public static class CommentMapper
    {
        public static RComment ToRComment(this CommentInfo comment)
        {
            return new RComment
            {
                Id = comment.Id,
                Comment = comment.Comment,
                Status = comment.Status,
                ForeignTable = comment.ForeignTable,
                ForeignId = comment.ForeignId,
                UserId = comment.UserId,
                CreatedAt = comment.CreatedAt,
            };
        }
    }
}
