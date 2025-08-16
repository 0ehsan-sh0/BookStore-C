using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Requests.Tag;
using BookStoreApi.RequestHandler.Admin.Responses.Tag;

namespace BookStoreApi.RequestHandler.Admin.Mappers
{
    public static class TagMapper
    {
        public static RTag ToRTag(this Tag tag)
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

        public static Tag ToTag(this CreateTagRequest tag)
        {
            return new Tag
            {
                Name = tag.Name,
                Url = tag.Url,
            };
        }

        public static Tag ToTag(this UpdateTagRequest tag, int id)
        {
            return new Tag
            {
                Id = id,
                Name = tag.Name,
                Url = tag.Url,
            };
        }
    }
}
