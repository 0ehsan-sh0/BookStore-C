using BookStoreApi.RequestHandler.User.Requests.User;
using BookStoreApi.RequestHandler.User.Responses.Address;
using BookStoreApi.RequestHandler.User.Responses.User;

namespace BookStoreApi.RequestHandler.User.Mappers
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
                }).ToList(),
                Invoices = user.Invoices?.Select(i => i.ToRInvoice()).ToList(),
                WishList = user.WishList?.Select(w => w.ToRBookAllData()).ToList(),
            };
        }

        public static Database.Models.User ToUser(this UpdateUserRequest user, string mobile)
        {
            return new Database.Models.User
            {
                Mobile = mobile,
                Name = user.Name,
                LastName = user.LastName,
            };
        }
    }
}