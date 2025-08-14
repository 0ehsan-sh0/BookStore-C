using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Responses.Image;

namespace BookStoreApi.RequestHandler.Admin.Mappers
{
    public static class ImageMapper
    {
        public static RImage ToRImage(this Image image)
        {
            return new RImage
            {
                Id = image.Id,
                Height = image.Height,
                Width = image.Width,
                IsPrimary = image.IsPrimary,
                StoredFileName = image.StoredFileName,
                RelativePath = image.RelativePath,
                FileSize = image.FileSize,
                MimeType = image.MimeType,
                CreatedAt = image.CreatedAt,
                UpdatedAt = image.UpdatedAt,
            };
        }
    }
}
