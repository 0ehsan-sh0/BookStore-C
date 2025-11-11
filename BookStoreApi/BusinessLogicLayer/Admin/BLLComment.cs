using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Comment;
using BookStoreApi.RequestHandler.Admin.Responses.Comment;

namespace BookStoreApi.BusinessLogicLayer.Admin
{
    public class BLLComment(ICommentRepository repo)
    {
        public async Task<(List<CommentInfo> comments, COPaginationInfo pagination)> GetAllAsync(QCommentGetAll query)
        {
            return await repo.GetAllAsync(query);
        }

        public async Task<CommentInfo?> GetByIdAsync(int id)
        {
            return await repo.GetByIdAsync(id);
        }

        public async Task<(string message, int status)> Delete(int id)
        {
            var existingEntity = await repo.GetByIdAsync(id);
            if (existingEntity is null)
                return ("نظر مورد نظر یافت نشد", 404);

            var entity = await repo.DeleteAsync(id);
            if (!entity) return ("نظر مورد نظر یافت نشد", 404);

            return ("نظر با موفقیت حذف شد", 204);
        }

        public async Task<(string message, CommentInfo? comment, int status)> SwitchIsConfirmed(int id)
        {
            var existingEntity = await repo.GetByIdAsync(id);
            if (existingEntity is null)
                return ("نظر مورد نظر یافت نشد", null, 404);

            var result = await repo.SwitchIsConfirmedAsync(id);
            if (result) return ("وضعیت نظر با موفقیت تغییر کرد", await repo.GetByIdAsync(id), 201);
            return ("تغییر وضعیت نظر با مشکل مواجه شد", null, 500);
        }
    }
}
