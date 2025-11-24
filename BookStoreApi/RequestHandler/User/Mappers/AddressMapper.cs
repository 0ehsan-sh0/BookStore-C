using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.Requests.Address;
using BookStoreApi.RequestHandler.User.Responses.Address;

namespace BookStoreApi.RequestHandler.User.Mappers
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

        public static AddressInfo ToAddressInfo(this CreateAddressRequest req)
        {
            return new AddressInfo
            {
                Name = req.Name,
                LastName = req.LastName,
                Phone = req.Phone,
                PostCode = req.PostCode,
                State = req.State,
                City = req.City,
                Address = req.Address,
            };
        }

        public static AddressInfo ToAddressInfo(this UpdateAddressRequest req, int id)
        {
            return new AddressInfo
            {
                Id = id,
                Name = req.Name,
                LastName = req.LastName,
                Phone = req.Phone,
                PostCode = req.PostCode,
                State = req.State,
                City = req.City,
                Address = req.Address,
            };
        }
    }
}
