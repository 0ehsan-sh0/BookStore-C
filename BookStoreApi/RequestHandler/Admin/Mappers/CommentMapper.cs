using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Responses.Comment;

namespace BookStoreApi.RequestHandler.Admin.Mappers
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
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
            };
        }
    }
}
