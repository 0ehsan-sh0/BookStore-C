using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Comment;
using BookStoreApi.RequestHandler.Admin.Responses.Comment;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Admin
{
    public interface IBLLComment
    {
        Task<(List<CommentInfo> comments, COPaginationInfo pagination)> GetAllAsync(QCommentGetAll query);
        Task<CommentInfo?> GetByIdAsync(int id);
        Task<(string message, int status)> Delete(int id);
        Task<(string message, CommentInfo? comment, int status)> SwitchIsConfirmed(int id);
    }
}