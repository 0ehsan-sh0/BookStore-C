using BookStoreApi.RequestHandler.Public.Responses.User;

namespace BookStoreApi.RequestHandler.Public.Mappers
{
    public static class UserMapper
    {
        public static RUser ToPublicUser(this Database.Models.User user)
        {
            return new RUser
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Role = user.Role,
            };
        }
    }
}
