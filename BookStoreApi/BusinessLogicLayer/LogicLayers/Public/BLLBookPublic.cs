using BookStoreApi.BusinessLogicLayer.Interfaces.Public;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.Responses.Book;
using BookStoreApi.RequestHandler.Public.Responses.Comment;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.Public
{
    public class BLLBookPublic(IBookRepository repo, ICommentRepository commentRepository) : IBLLBookPublic
    {
        public async Task<(List<BookAllData>? books, BPPaginationInfo info)> GetNewAsync(int pageSize = 20, int pageNumber = 1, bool isRecommended = false)
        {
            return await repo.GetNewAsync(pageSize, pageNumber, isRecommended);
        }

        public async Task<BookAllData?> GetByIdAsync(int id)
        {
            return await repo.GetByIdAsync(id);
        }

        public async Task<(string message, List<CommentInfo>? comments, CommentPaginationInfo? info, int status)>
            GetBookComments(int bookId, int pageNumber, int pageSize)
        {
            var book = await repo.GetByIdAsync(bookId);
            if (book is null) return ("کتاب پیدا نشد", null, null, 404);

            var (comments, pagination) = await commentRepository.GetBookCommentsAsync(bookId, pageNumber, pageSize);

            return ("موفق", comments, pagination, 200);
        }
    }
}
