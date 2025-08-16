using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Book;
using BookStoreApi.RequestHandler.Admin.Responses.Book;
using BookStoreApi.Services.Models;

namespace BookStoreApi.Database.Interfaces
{
    public interface IBookRepository
    {
        Task<(List<BookAllData>? books, BPaginationInfo info)> GetAllAsync(QBookGetAll query);
        Task<BookAllData?> GetByIdAsync(int id);
        Task<Book?> GetByISBNAsync(string isbn);
        Task<int> CreateAsync(Book book, List<ImageInfo> imageInfos, List<int>? translators, List<int> categories, List<int> tags);
        Task<BookAllData?> UpdateAsync(Book bookWithId, List<int>? translators, List<int> categories, List<int> tags);
        Task<bool> DeleteAsync(int id);
    }
}
