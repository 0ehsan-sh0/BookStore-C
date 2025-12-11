using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.User;
using BookStoreApi.RequestHandler.Admin.Responses.User;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Admin
{
    public interface IBLLUser
    {
        Task<(string message, List<User> users, UPaginationInfo info, int status)> GetAllAsync(QUserGetAll query);
        Task<(string message, User? user, int status)> GetByIdAsync(int id);
        Task<(string message, User? user, int status)> CreateAsync(User user);
        Task<(string message, User? user, int status)> UpdateAsync(User userWithId);
        Task<(string message, int status)> DeleteAsync(int id);
    }
}
