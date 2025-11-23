using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.UserPanel
{
    public class BLLUserPanel(
        IUserRepository userRepository,
        IWishListRepository wishListRepository,
        IBookRepository bookRepository) : IBLLUserPanel
    {
        public async Task<User?> UpdateAsync(User user)
        {
            return await userRepository.UpdateByMobileAsync(user);
        }

        public async Task<(string message, bool? wishListStatus, int status)> ToggleWishListAsync(string mobile, int bookId)
        {
            var user = await userRepository.GetByMobileAsync(mobile);
            if (user == null) return ("کاربر یافت نشد.", null, 404);

            var book = await bookRepository.GetByIdAsync(bookId);
            if (book == null) return ("کتاب یافت نشد.", null, 404);

            var wishlistStatus = await wishListRepository.ToggleAsync(new WishList
            {
                UserId = user.Id,
                BookId = book.Id,
            });

            return ("وضعیت با موفقیت تغییر کرد", wishlistStatus, 201);
        }
    }
}
