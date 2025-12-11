using BookStoreApi.BusinessLogicLayer.Interfaces.Admin;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.User;
using BookStoreApi.RequestHandler.Admin.Responses.User;
using BookStoreApi.Services;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.Admin
{
    public class BLLUser(IUserRepository repo) : IBLLUser
    {
        public async Task<(string message, User? user, int status)> CreateAsync(User user)
        {
            var databaseUser = await repo.GetByMobileAsync(user.Mobile);
            if (databaseUser is not null) return ("کاربری با این شماره تلفن ثبت شده است.", null, 400);

            user = new User()
            {
                Mobile = user.Mobile,
                LastName = user.LastName,
                Name = user.Name,
                Role = user.Role,
                Password = PasswordHasherService.HashPassword(user.Password),
            };

            var result = await repo.AdminCreateAsync(user);
            var createdUser = await repo.GetByIdAsync(result);
            if (createdUser is null) return ("خطا در ثبت کاربر", null, 500);

            return ("کاربر با موفقیت ثبت شد.", createdUser, 201);
        }

        public async Task<(string message, int status)> DeleteAsync(int id)
        {
            var user = await repo.GetByIdAsync(id);
            if (user is null) return ("کاربر یافت نشد", 404);

            var isDeleted = await repo.DeleteAsync(id);
            if (!isDeleted) return ("خطا در حذف کاربر", 500);

            return ("کاربر با موفقیت پاک شد.", 204);
        }

        public async Task<(string message, List<User> users, UPaginationInfo info, int status)> GetAllAsync(QUserGetAll query)
        {
            var (users, pagination) = await repo.GetAllAsync(query);
            return ("اطلاعات با موفقیت بارگزاری شد", users, pagination, 200);
        }

        public async Task<(string message, User? user, int status)> GetByIdAsync(int id)
        {
            var user = await repo.GetByIdAsync(id);
            if (user is null) return ("کاربر یافت نشد", null, 404);

            return ("اطلاعات با موفقیت بارگزاری شد", user, 200);
        }

        public async Task<(string message, User? user, int status)> UpdateAsync(User userWithId)
        {
            var user = await repo.GetByIdAsync(userWithId.Id);
            if (user is null) return ("کاربر یافت نشد", null, 404);

            user = await repo.UpdateAsync(userWithId);

            return ("کاربر با موفقیت بروزرسانی شد", user, 200);
        }
    }
}
