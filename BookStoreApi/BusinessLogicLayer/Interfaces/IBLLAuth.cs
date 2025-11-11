using BookStoreApi.Database.Models;

namespace BookStoreApi.BusinessLogicLayer.Interfaces
{
    public interface IBLLAuth
    {
        Task<bool> UserExist(string mobile);
        Task<User?> RegisterAsync(User user);
        Task<User?> LoginAsync(string mobile, string code, bool isCode);
        Task<User?> LoginAsync(string mobile, string password);
        Task<User?> GetUserByMobileAsync(string mobile);
    }
}