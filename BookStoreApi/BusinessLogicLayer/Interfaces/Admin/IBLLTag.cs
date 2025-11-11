using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Tag;
using BookStoreApi.RequestHandler.Admin.Requests.Tag;
using BookStoreApi.RequestHandler.Admin.Responses.Tag;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Admin
{
    public interface IBLLTag
    {
        Task<(List<Tag> tags, TagPaginationInfo pagination)> GetAllAsync(QTagGetAll query);
        Task<Tag?> GetByIdAsync(int id);
        Task<(string message, Tag? tag, int status)> Create(CreateTagRequest createTagRequest);
        Task<(string message, Tag? tag, int status)> Update(int id, UpdateTagRequest UTag);
        Task<(string message, int status)> Delete(int id);
    }
}