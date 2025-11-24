using BookStoreApi.RequestHandler.User.Requests.User;
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
                Addresses = user.Addresses?.Select(a => a.ToRAddress()).ToList(),
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