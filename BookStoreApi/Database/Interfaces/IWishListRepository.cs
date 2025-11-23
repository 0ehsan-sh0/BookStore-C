using BookStoreApi.Database.Models;

namespace BookStoreApi.Database.Interfaces
{
    public interface IWishListRepository
    {
        Task<bool> ToggleAsync(WishList wishList);
        Task<WishList?> GetWishListAsync(WishList wishList);
        Task<WishList?> CreateAsync(WishList wishList);
        Task<bool> DeleteAsync(WishList wishList);
    }
}
