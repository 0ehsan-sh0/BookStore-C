using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Requests.Book;
using BookStoreApi.RequestHandler.Admin.Responses.Book;

namespace BookStoreApi.RequestHandler.Admin.Mappers
{
    public static class BookMapper
    {
        public static RBook ToRBook(this Book book)
        {
            return new RBook
            {
                Id = book.Id,
                Name = book.Name,
                EnglishName = book.EnglishName,
                Description = book.Description,
                Price = book.Price,
                PrintSeries = book.PrintSeries,
                ISBN = book.ISBN,
                CoverType = book.CoverType,
                Format = book.Format,
                Pages = book.Pages,
                PublishYear = book.PublishYear,
                Publisher = book.Publisher,
                AuthorId = book.AuthorId,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt,
            };
        }

        public static RBookAllData ToRBookAllData(this BookAllData bookAllData)
        {
            RBookAllData rBookAllData = new()
            {
                Id = bookAllData.Id,
                Name = bookAllData.Name,
                EnglishName = bookAllData.EnglishName ?? "",
                Description = bookAllData.Description ?? "",
                Price = bookAllData.Price,
                PrintSeries = bookAllData.PrintSeries,
                ISBN = bookAllData.ISBN.ToString(),
                CoverType = bookAllData.CoverType,
                Format = bookAllData.Format,
                Pages = bookAllData.Pages.ToString(),
                PublishYear = bookAllData.PublishYear.ToString(),
                Publisher = bookAllData.Publisher,
                CreatedAt = bookAllData.CreatedAt,
                UpdatedAt = bookAllData.UpdatedAt
            };

            if (bookAllData.Translators is not null && bookAllData.Translators.Count > 0)
                rBookAllData.Translators = bookAllData.Translators.Select(a => a.ToRTranslator()).ToList();


            if (bookAllData.Categories is not null && bookAllData.Categories.Count > 0)
                rBookAllData.Categories = bookAllData.Categories.Select(a => a.ToRCategory()).ToList();

            if (bookAllData.Tags is not null && bookAllData.Tags.Count > 0)
                rBookAllData.Tags = bookAllData.Tags.Select(a => a.ToRTag()).ToList();

            if (bookAllData.Images is not null && bookAllData.Images.Count > 0)
                rBookAllData.Images = bookAllData.Images.Select(a => a.ToRImage()).ToList();

            if (bookAllData.Author is not null)
                rBookAllData.Author = bookAllData.Author.ToRAuthor();

            return rBookAllData;
        }

        public static Book ToBook(this CreateBookRequest book)
        {
            return new Book
            {
                Name = book.Name,
                EnglishName = book.EnglishName ?? "",
                Description = book.Description ?? "",
                Price = book.Price,
                PrintSeries = book.PrintSeries,
                ISBN = book.ISBN.ToString(),
                CoverType = book.CoverType,
                Format = book.Format,
                Pages = book.Pages.ToString(),
                PublishYear = book.PublishYear.ToString(),
                Publisher = book.Publisher,
                AuthorId = book.AuthorId,

            };
        }

        public static Book ToBook(this UpdateBookRequest book, int id)
        {
            return new Book
            {
                Id = id,
                Name = book.Name,
                EnglishName = book.EnglishName ?? "",
                Description = book.Description ?? "",
                Price = book.Price,
                PrintSeries = book.PrintSeries,
                ISBN = book.ISBN.ToString(),
                CoverType = book.CoverType,
                Format = book.Format,
                Pages = book.Pages.ToString(),
                PublishYear = book.PublishYear.ToString(),
                Publisher = book.Publisher,
                AuthorId = book.AuthorId,

            };
        }

    }
}
