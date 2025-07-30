using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Author;
using BookStoreApi.RequestHandler.Admin.Requests.Author;
using BookStoreApi.RequestHandler.Admin.Responses.Author;

namespace BookStoreApi.BusinessLogicLayer.Admin
{
    public class BLLAuthor(IAuthorRepository repo)
    {
        public async Task<(List<Author> categories, APaginationInfo pagination)> GetAllAsync(QAuthorGetAll query)
        {
            return await repo.GetAllAsync(query);
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await repo.GetByIdAsync(id);
        }

        public async Task<(string message, Author? category, int status)> Create(CreateAuthorRequest createAuthorRequest)
        {
            var author = createAuthorRequest.ToAuthor();

            int id = await repo.CreateAsync(author);
            author = await repo.GetByIdAsync(id);

            return ("نویسنده با موفقیت اضافه شد", author, 201);
        }

        public async Task<(string message, Author? category, int status)> Update(int id, UpdateAuthorRequest UAuthor)
        {
            var author = await repo.GetByIdAsync(id);
            if (author is null)
                return ("نویسنده مورد نظر یافت نشد", null, 404);

            author = await repo.UpdateAsync(UAuthor.ToAuthor(id));

            return ("نویسنده با موفقیت بروزرسانی شد", author, 200);
        }

        public async Task<(string message, int status)> Delete(int id)
        {
            var existingEntity = await repo.GetByIdAsync(id);
            if (existingEntity is null)
                return ("نویسنده مورد نظر یافت نشد", 404);

            var entity = await repo.DeleteAsync(id);
            if (!entity) return ("نویسنده مورد نظر یافت نشد", 404);

            return ("نویسنده با موفقیت حذف شد", 204);
        }
    }
}
