using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Public.Responses.Book;

namespace BookStoreApi.RequestHandler.Public.Mappers
{
    public static class BookMapper
    {
        public static RBookAllData ToPublicBookAllData(this BookAllData bookAllData)
        {
            RBookAllData rBookAllData = new()
            {
                Id = bookAllData.Id,
                Name = bookAllData.Name,
                EnglishName = bookAllData.EnglishName ?? "",
                Description = bookAllData.Description ?? "",
                Price = bookAllData.Price,
                PrintSeries = bookAllData.PrintSeries,
                ISBN = bookAllData.ISBN,
                CoverType = bookAllData.CoverType,
                Format = bookAllData.Format,
                Pages = bookAllData.Pages.ToString(),
                PublishYear = bookAllData.PublishYear.ToString(),
                Publisher = bookAllData.Publisher,
                CreatedAt = bookAllData.CreatedAt,
                UpdatedAt = bookAllData.UpdatedAt
            };

            if (bookAllData.Translators is not null && bookAllData.Translators.Count > 0)
                rBookAllData.Translators = bookAllData.Translators.Select(a => a.ToPublicTranslator()).ToList();


            if (bookAllData.Categories is not null && bookAllData.Categories.Count > 0)
                rBookAllData.Categories = bookAllData.Categories.Select(a => a.ToPublicCategory()).ToList();

            if (bookAllData.Tags is not null && bookAllData.Tags.Count > 0)
                rBookAllData.Tags = bookAllData.Tags.Select(a => a.ToPublicTag()).ToList();

            if (bookAllData.Images is not null && bookAllData.Images.Count > 0)
                rBookAllData.Images = bookAllData.Images.Select(a => a.ToPublicImage()).ToList();

            if (bookAllData.Author is not null)
                rBookAllData.Author = bookAllData.Author.ToPublicAuthor();

            return rBookAllData;
        }
    }
}
