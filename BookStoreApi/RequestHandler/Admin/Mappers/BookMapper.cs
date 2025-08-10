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
