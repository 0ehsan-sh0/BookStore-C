using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Book;
using BookStoreApi.RequestHandler.Admin.Requests.Book;
using BookStoreApi.RequestHandler.Admin.Responses.Book;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Admin
{
    public interface IBLLBook
    {
        Task<(string message, BookAllData? book, int status)> Create(CreateBookRequest createBookRequest);
        Task<(string message, BookAllData? book, int status)> Update(UpdateBookRequest updateBookRequest, int id);
        Task<(string message, int status)> Delete(int id);
        Task<BookAllData?> GetByIdAsync(int id);
        Task<(List<BookAllData>? books, BPaginationInfo pagination)> GetAllAsync(QBookGetAll query);
    }
}