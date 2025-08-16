using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Tag;
using BookStoreApi.RequestHandler.Admin.Responses.Tag;

namespace BookStoreApi.Database.Interfaces
{
    public interface ITagRepository
    {
        Task<(List<Tag> tags, TagPaginationInfo info)> GetAllAsync(QTagGetAll query);
        Task<Tag?> GetByIdAsync(int id);
        Task<Tag?> GetByUrlAsync(string Url);
        Task<int> CreateAsync(Tag tag);
        Task<Tag?> UpdateAsync(Tag tagWithId);
        Task<bool> DeleteAsync(int id);
    }
}
