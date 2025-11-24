using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.QueryObjects.Address;
using BookStoreApi.RequestHandler.User.Responses.Address;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.UserPanel
{
    public class BLLUserAddress(IAddressRepository repo, IUserRepository userRepository) : IBLLUserAddress
    {
        public async Task<(string message, AddressInfo? address, int status)> CreateAsync(string mobile, AddressInfo address)
        {
            var user = await userRepository.GetByMobileAsync(mobile);
            if (user == null) return ("کاربر پیدا نشد.", null, 404);

            address.UserId = user.Id;
            var addressId = await repo.CreateAsync(address);

            var createdAddress = await repo.GetByIdAsync(addressId);

            return ("آدرس با موفقیت ایجاد شد.", createdAddress, 201);
        }

        public async Task<(List<AddressInfo>? addresses, AddressPaginationInfo? info)> GetUserAddressesAsync(string mobile, QUserAddress query)
        {
            var user = await userRepository.GetByMobileAsync(mobile);
            if (user == null) return (null, null);

            return await repo.GetUserAddressesAsync(user.Id, query);
        }

        public async Task<(string message, AddressInfo? address, int status)> UpdateAsync(string mobile, AddressInfo addressWithId)
        {
            var user = await userRepository.GetByMobileAsync(mobile);
            if (user == null) return ("کاربر پیدا نشد.", null, 404);

            var existingAddress = await repo.GetByIdAsync(addressWithId.Id);
            if (existingAddress == null || existingAddress.UserId != user.Id)
                return ("آدرس پیدا نشد.", null, 404);

            var updatedAddress = await repo.UpdateAsync(addressWithId);
            return ("آدرس با موفقیت به‌روزرسانی شد.", updatedAddress, 200);
        }
    }
}
