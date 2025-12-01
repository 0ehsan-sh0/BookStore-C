using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.QueryObjects.Address;
using BookStoreApi.RequestHandler.User.Responses.Address;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel
{
    public interface IBLLUserAddress
    {
        Task<(List<AddressInfo>? addresses, AddressPaginationInfo? info)> GetUserAddressesAsync(string mobile, QUserAddress query);
        Task<(string message, AddressInfo? address, int status)> CreateAsync(string mobile, AddressInfo address);
        Task<(string message, AddressInfo? address, int status)> UpdateAsync(string mobile, AddressInfo addressWithId);
        Task<(string message, AddressInfo? address, int status)> GetById(string mobile, int id);
    }
}
