namespace BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel
{
    public interface IBLLUserPanel
    {
        Task<Database.Models.User?> UpdateAsync(Database.Models.User user);
    }
}
