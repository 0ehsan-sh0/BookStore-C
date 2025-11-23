namespace BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel
{
    public interface IBLLUserPanel
    {
        Task<Database.Models.User?> UpdateAsync(Database.Models.User user);
        Task<(string message, bool? wishListStatus, int status)> ToggleWishListAsync(string mobile, int bookId);
    }
}
