using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Book;
using BookStoreApi.RequestHandler.Admin.Responses.Book;
using BookStoreApi.Services.Models;

namespace BookStoreApi.Database.Interfaces
{
    public interface IBookRepository
    {
        Task<(List<Book> books, BPaginationInfo info)> GetAllAsync(QBookGetAll query);
        Task<Book?> GetByIdAsync(int id);
        Task<int> CreateAsync(Book book, List<ImageInfo> imageInfos);
        Task<Book?> UpdateAsync(Book bookWithId);
        Task<bool> DeleteAsync(int id);
    }
}
