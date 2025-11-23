using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.Responses.WishList;

namespace BookStoreApi.RequestHandler.User.Mappers
{
    public static class WishListMapper
    {
        public static RWishList ToRWishList(this WishList wishList)
        {
            return new RWishList
            {
                UserId = wishList.UserId,
                BookId = wishList.BookId,
            };
        }

        public static WishList ToWishList(this RWishList rWishList)
        {
            return new WishList
            {
                UserId = rWishList.UserId,
                BookId = rWishList.BookId,
            };
        }
    }
}
