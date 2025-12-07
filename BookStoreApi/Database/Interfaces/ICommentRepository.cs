using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Comment;
using BookStoreApi.RequestHandler.Admin.Responses.Comment;
using BookStoreApi.RequestHandler.Public.Responses.Comment;

namespace BookStoreApi.Database.Interfaces
{
    public interface ICommentRepository
    {
        Task<(List<CommentInfo> comments, COPaginationInfo info)> GetAllAsync(QCommentGetAll query);
        Task<CommentInfo?> GetByIdAsync(int id);
        Task<int> CreateAsync(CommentInfo comment);
        Task<bool> DeleteAsync(int id);
        Task<bool> SwitchIsConfirmedAsync(int id);
        Task<(List<CommentInfo> comments, CommentPaginationInfo info)> GetBookCommentsAsync(int bookId, int pageNumber, int pageSize);
    }
}
