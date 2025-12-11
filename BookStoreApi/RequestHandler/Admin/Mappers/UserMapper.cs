using BookStoreApi.RequestHandler.Admin.Requests.User;
using BookStoreApi.RequestHandler.Admin.Responses.User;

namespace BookStoreApi.RequestHandler.Admin.Mappers
{
    public static class UserMapper
    {
        public static RUser ToRUser(this Database.Models.User user)
        {
            return new RUser
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Mobile = user.Mobile,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                LoggedInAt = user.LoggedInAt,
                BuyCount = user.BuyCount,
            };
        }

        public static Database.Models.User ToUser(this CreateUserRequest request)
        {
            return new Database.Models.User
            {
                Name = request.Name,
                LastName = request.LastName,
                Mobile = request.Mobile,
                Password = request.Password,
                Role = request.Role,
            };
        }

        public static Database.Models.User ToUser(this UpdateAdminUserRequest request, int id)
        {
            return new Database.Models.User
            {
                Id = id,
                Name = request.Name,
                LastName = request.LastName,
                Role = request.Role,
            };
        }
    }
}