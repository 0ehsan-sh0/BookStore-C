using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Book;
using BookStoreApi.RequestHandler.Admin.Responses.Book;
using BookStoreApi.Services.Models;

namespace BookStoreApi.Database.Interfaces
{
    public interface IBookRepository
    {
        Task<(List<Book> books, BPaginationInfo info)> GetAllAsync(QBookGetAll query);
        Task<BookAllData?> GetByIdAsync(int id);
        Task<Book?> GetByISBNAsync(string isbn);
        Task<int> CreateAsync(Book book, List<ImageInfo> imageInfos, List<int>? translators, List<int> categories);
        Task<BookAllData?> UpdateAsync(Book bookWithId, List<int>? translators, List<int> categories);
        Task<bool> DeleteAsync(int id);
    }
}
