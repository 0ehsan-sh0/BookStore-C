using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.QueryObjects.Book;
using BookStoreApi.RequestHandler.User.Responses.Book;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel
{
    public interface IBLLUserPanel
    {
        Task<Database.Models.User?> UpdateAsync(Database.Models.User user);
        Task<(string message, bool? wishListStatus, int status)> ToggleWishListAsync(string mobile, int bookId);
        Task<(string message, List<BookAllData>? books, BookPaginationInfo? info, int status)> GetUserWithList(string mobile, QUserWishList query);
    }
}
