using BookStoreApi.RequestHandler.Admin.Responses.Address;
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
                LoggedInAt = user.LoggedInAt
            };
        }

        public static RUserDetail ToRUserDetail(this Database.Models.User user)
        {
            return new RUserDetail
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Mobile = user.Mobile,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                LoggedInAt = user.LoggedInAt,
                Addresses = user.Addresses?.Select(a => new RAddress
                {
                    Id = a.Id,
                    Name = a.Name,
                    LastName = a.LastName,
                    Phone = a.Phone,
                    PostCode = a.PostCode,
                    State = a.State,
                    City = a.City,
                    Address = a.Address,
                    UserId = a.UserId,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                }).ToList()
            };
        }
    }
}