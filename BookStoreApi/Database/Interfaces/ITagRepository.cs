using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Tag;
using BookStoreApi.RequestHandler.Admin.Responses.Tag;
using BookStoreApi.RequestHandler.Public.QueryObjects.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.Database.Interfaces
{
    public interface ITagRepository
    {
        Task<(List<Tag> tags, TagPaginationInfo info)> GetAllAsync(QTagGetAll query);
        Task<Tag?> GetByIdAsync(int id);
        Task<(Tag? tag, BPPaginationInfo info)> GetByUrlAsync(string url, QTagBooks query);
        Task<Tag?> GetByUrlAsync(string Url);
        Task<int> CreateAsync(Tag tag);
        Task<Tag?> UpdateAsync(Tag tagWithId);
        Task<bool> DeleteAsync(int id);
    }
}
