using BookStoreApi.BusinessLogicLayer.Interfaces;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.Services;
using BookStoreApi.Services.Models;

namespace BookStoreApi.BusinessLogicLayer
{
    public class BLLAuth(IUserRepository userRepository) : IBLLAuth
    {
        public async Task<bool> UserExist(string mobile)
        {
            var user = await userRepository.GetByMobileAsync(mobile);
            if (user is null) return false;
            return true;
        }
        public async Task<User?> RegisterAsync(User user)
        {
            var databaseUser = await userRepository.GetByMobileAsync(user.Mobile);
            if (databaseUser is not null) return null;

            user = new User()
            {
                Mobile = user.Mobile,
                Password = PasswordHasherService.HashPassword(user.Password),
            };

            var result = await userRepository.CreateAsync(user);
            var createdUser = await userRepository.GetByIdAsync(result);
            return createdUser;
        }
        public async Task<User?> LoginAsync(string mobile, string code, bool isCode)
        {
            var user = await userRepository.GetByMobileAsync(mobile);
            if (user is null) return null;

            VerificationStore.Codes.TryGetValue(mobile, out var storedCode);

            if (storedCode is null || storedCode != code) return null;

            // Verification successful
            VerificationStore.Codes.Remove(mobile); // remove after success

            await userRepository.UpdateLoggedInAt(user.Mobile);

            return user;
        }

        public async Task<User?> LoginAsync(string mobile, string password)
        {
            var user = await userRepository.GetByMobileAsync(mobile);

            if (user is null || !PasswordHasherService.VerifyPassword(password, user.Password))
                return null;

            await userRepository.UpdateLoggedInAt(user.Mobile);

            return user;
        }

        public async Task<User?> GetUserByMobileAsync(string mobile)
        {
            var user = await userRepository.GetByMobileAsync(mobile);
            return user;
        }
    }
}