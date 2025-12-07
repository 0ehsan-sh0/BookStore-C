using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.Requests.Comment;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.UserPanel
{
    public class BLLUserComment(
        ICommentRepository repo,
        IUserRepository userRepository,
        IBookRepository bookRepository) : IBLLUserComment
    {
        public async Task<(string message, CommentInfo? comment, int status)> CreateAsync(string mobile, int bookId, CreateCommentRequest request)
        {
            var user = await userRepository.GetByMobileAsync(mobile);
            if (user is null) return ("کاربر یافت نشد", null, 404);

            var book = await bookRepository.GetByIdAsync(bookId);
            if (book is null) return ("کتاب یافت نشد", null, 404);

            CommentInfo commentInfo = new CommentInfo
            {
                UserId = user.Id,
                ForeignId = bookId,
                ForeignTable = "Books",
                Status = false,
                Comment = request.Comment
            };

            var createdCommentId = await repo.CreateAsync(commentInfo);
            var comment = await repo.GetByIdAsync(createdCommentId);

            if (comment is null) return ("خطا در ثبت نظر", null, 500);

            return ("نظر با موفقیت ثبت شد و پس از تایید نمایش داده خواهد شد", comment, 201);
        }
    }
}
