using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.Requests.Comment;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel
{
    public interface IBLLUserComment
    {
        Task<(string message, CommentInfo? comment, int status)> CreateAsync(string mobile, int bookId, CreateCommentRequest request);
    }
}
