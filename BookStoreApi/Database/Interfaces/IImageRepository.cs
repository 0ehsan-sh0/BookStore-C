using BookStoreApi.Database.Models;
using BookStoreApi.Services.Models;

namespace BookStoreApi.Database.Interfaces
{
    public interface IImageRepository
    {
        Task<Image?> GetByIdAsync(int id);
        Task<List<int>?> CreateAsync(List<ImageInfo> imageInfos, string foreignTable, int foreignId);
        Task<bool> DeleteAsync(int id);
        Task<List<Image>?> GetByIdAsync(List<int> ids);
        Task<bool> ChangePrimary(int id);
        Task<bool> ForeignIdExists(int id, string table);

    }
}
