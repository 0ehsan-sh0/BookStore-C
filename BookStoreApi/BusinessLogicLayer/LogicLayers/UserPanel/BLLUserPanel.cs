using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.Database.Interfaces;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.UserPanel
{
    public class BLLUserPanel(IUserRepository userRepository) : IBLLUserPanel
    {
        public async Task<Database.Models.User?> UpdateAsync(Database.Models.User user)
        {
            return await userRepository.UpdateByMobileAsync(user);
        }
    }
}
