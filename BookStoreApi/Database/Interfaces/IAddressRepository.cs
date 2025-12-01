using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.QueryObjects.Address;
using BookStoreApi.RequestHandler.User.Responses.Address;

namespace BookStoreApi.Database.Interfaces
{
    public interface IAddressRepository
    {
        Task<(List<AddressInfo> addresses, AddressPaginationInfo info)> GetUserAddressesAsync(int userId, QUserAddress query);
        Task<AddressInfo?> GetByIdAsync(int id);
        Task<int> CreateAsync(AddressInfo address);
        Task<bool> UpdateAsync(AddressInfo addressWithId);
    }
}
