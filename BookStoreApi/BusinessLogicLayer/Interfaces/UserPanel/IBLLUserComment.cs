using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.QueryObjects.Comment;
using BookStoreApi.RequestHandler.User.Requests.Comment;
using BookStoreApi.RequestHandler.User.Responses.Comment;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel
{
    public interface IBLLUserComment
    {
        Task<(string message, List<CommentInfo>? comments, UserCommentPaginationInfo? pagination, int status)> GetUserCommentsAsync(string mobile, QUserComments query);
        Task<(string message, CommentInfo? comment, int status)> CreateAsync(string mobile, int bookId, CreateCommentRequest request);
    }
}
