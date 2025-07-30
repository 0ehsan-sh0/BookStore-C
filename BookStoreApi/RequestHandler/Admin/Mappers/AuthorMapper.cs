using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Requests.Author;
using BookStoreApi.RequestHandler.Admin.Responses.Author;

namespace BookStoreApi.RequestHandler.Admin.Mappers
{
    public static class AuthorMapper
    {
        public static RAuthor ToRAuthor(this Author author)
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

        public static Author ToAuthor(this CreateAuthorRequest Author)
        {
            return new Author
            {
                Name = Author.Name,
                Description = Author.Description,
            };
        }

        public static Author ToAuthor(this UpdateAuthorRequest Author, int id)
        {
            return new Author
            {
                Id = id,
                Name = Author.Name,
                Description = Author.Description,
            };
        }
    }
}
