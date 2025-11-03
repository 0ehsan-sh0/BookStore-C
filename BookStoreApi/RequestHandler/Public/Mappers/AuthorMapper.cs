using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.Responses.Author;

namespace BookStoreApi.RequestHandler.Public.Mappers
{
    public static class AuthorMapper
    {
        public static RAuthor ToPublicAuthor(this Author author)
        {
            return new RAuthor
            {
                Id = author.Id,
                Name = author.Name,
                Description = author.Description,
                CreatedAt = author.CreatedAt,
                UpdatedAt = author.UpdatedAt,
            };
        }
    }
}
