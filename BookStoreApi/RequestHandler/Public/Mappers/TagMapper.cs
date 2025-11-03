using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.Responses.Tag;

namespace BookStoreApi.RequestHandler.Public.Mappers
{
    public static class TagMapper
    {
        public static RTag ToPublicTag(this Tag tag)
        {
            return new RTag
            {
                Id = tag.Id,
                Name = tag.Name,
                Url = tag.Url,
                CreatedAt = tag.CreatedAt,
                UpdatedAt = tag.UpdatedAt,
            };
        }
    }
}
