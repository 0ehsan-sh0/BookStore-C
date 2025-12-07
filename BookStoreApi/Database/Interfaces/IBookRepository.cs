using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Book;
using BookStoreApi.RequestHandler.Admin.Responses.Book;
using BookStoreApi.RequestHandler.Public.Responses.Book;
using BookStoreApi.Services.Models;

namespace BookStoreApi.Database.Interfaces
{
    public interface IBookRepository
    {
        Task<(List<BookAllData>? books, BPaginationInfo info)> GetAllAsync(QBookGetAll query);
        Task<(List<BookAllData>? books, BPPaginationInfo info)> GetNewAsync(int pageSize = 20, int pageNumber = 1, bool isRecommended = false);
        Task<BookAllData?> GetByIdAsync(int id);
        Task<Book?> GetByISBNAsync(string isbn);
        Task<int> CreateAsync(Book book, List<ImageInfo> imageInfos, List<int>? translators, List<int> categories, List<int> tags);
        Task<BookAllData?> UpdateAsync(Book bookWithId, List<int>? translators, List<int> categories, List<int> tags);
        Task<bool> DeleteAsync(int id);
        Task<bool> DecreaseStockBulkAsync(List<(int BookId, int Count)> items);
        Task<bool> ToggleIsRecommended(int id);
    }
}
