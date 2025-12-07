using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.Responses.Book;
using BookStoreApi.RequestHandler.Public.Responses.Comment;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Public
{
    public interface IBLLBookPublic
    {
        Task<(List<BookAllData>? books, BPPaginationInfo info)> GetNewAsync(int pageSize = 20, int pageNumber = 1, bool isRecommended = false);
        Task<BookAllData?> GetByIdAsync(int id);
        Task<(string message, List<CommentInfo>? comments, CommentPaginationInfo? info, int status)>
            GetBookComments(int bookId, int pageNumber, int pageSize);
    }
}