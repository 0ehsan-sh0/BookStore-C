using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.QueryObjects.Book;
using BookStoreApi.RequestHandler.User.Responses.Book;

namespace BookStoreApi.Database.Interfaces
{
    public interface IWishListRepository
    {
        Task<bool> ToggleAsync(WishList wishList);
        Task<WishList?> GetWishListAsync(WishList wishList);
        Task<(List<BookAllData>? books, BookPaginationInfo info)> GetUserWishListAsync(int id, QUserWishList query);
        Task<WishList?> CreateAsync(WishList wishList);
        Task<bool> DeleteAsync(WishList wishList);
    }
}
