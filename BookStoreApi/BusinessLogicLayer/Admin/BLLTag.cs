using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Tag;
using BookStoreApi.RequestHandler.Admin.Requests.Tag;
using BookStoreApi.RequestHandler.Admin.Responses.Tag;

namespace BookStoreApi.BusinessLogicLayer.Admin
{
    public class BLLTag(ITagRepository repo)
    {
        public async Task<(List<Tag> tags, TagPaginationInfo pagination)> GetAllAsync(QTagGetAll query)
        {
            return await repo.GetAllAsync(query);
        }

        public async Task<Tag?> GetByIdAsync(int id)
        {
            return await repo.GetByIdAsync(id);
        }

        public async Task<(string message, Tag? tag, int status)> Create(CreateTagRequest createTagRequest)
        {
            var tag = createTagRequest.ToTag();

            var urlTag = await repo.GetByUrlAsync(tag.Url);
            if (urlTag is not null) return ("لینک وارد شده تکراری است", null, 400);

            int id = await repo.CreateAsync(tag);
            tag = await repo.GetByIdAsync(id);
            return ("تگ با موفقیت اضافه شد", tag, 201);
        }

        public async Task<(string message, Tag? tag, int status)> Update(int id, UpdateTagRequest UTag)
        {
            var tag = await repo.GetByIdAsync(id);
            if (tag is null)
                return ("تگ مورد نظر یافت نشد", null, 404);


            // 🔹 check if another tag already has this new URL
            var urlTag = await repo.GetByUrlAsync(UTag.Url);
            if (urlTag is not null && urlTag.Id != id) // make sure it's not the same tag
            {
                return ("لینک وارد شده تکراری است", null, 400);
            }

            tag = await repo.UpdateAsync(UTag.ToTag(id));
            return ("تگ با موفقیت بروزرسانی شد", tag, 200);
        }

        public async Task<(string message, int status)> Delete(int id)
        {
            var existingTag = await repo.GetByIdAsync(id);
            if (existingTag is null)
                return ("تگ مورد نظر یافت نشد", 404);

            var tag = await repo.DeleteAsync(id);
            if (!tag) return ("تگ مورد نظر یافت نشد", 404);

            return ("تگ با موفقیت حذف شد", 204);
        }
    }
}
