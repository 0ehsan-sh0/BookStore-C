using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.User;
using BookStoreApi.RequestHandler.Admin.Responses.User;

namespace BookStoreApi.Database.Interfaces
{
    public interface IUserRepository
    {
        Task<(List<User> users, UPaginationInfo info)> GetAllAsync(QUserGetAll query);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByMobileAsync(string mobile);
        Task<int> CreateAsync(User user);
        Task<int> AdminCreateAsync(User user);
        Task<User?> UpdateAsync(User userWithId);
        Task<User?> UpdateByMobileAsync(User userWithMobile);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateLoggedInAt(string mobile);
    }
}
