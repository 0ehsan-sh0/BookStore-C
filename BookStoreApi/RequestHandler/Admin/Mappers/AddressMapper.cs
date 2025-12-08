using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Responses.Address;

namespace BookStoreApi.RequestHandler.Admin.Mappers
{
    public static class AddressMapper
    {
        public static RAddress ToRAddress(this AddressInfo address)
        {
            return new RAddress
            {
                Id = address.Id,
                Name = address.Name,
                LastName = address.LastName,
                Phone = address.Phone,
                PostCode = address.PostCode,
                State = address.State,
                City = address.City,
                UserId = address.UserId,
                Address = address.Address,
                CreatedAt = address.CreatedAt,
                UpdatedAt = address.UpdatedAt
            };
        }
    }
}
