using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.Responses.Book;

namespace BookStoreApi.RequestHandler.User.Mappers
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
                IsRecommended = book.IsRecommended,
                Stock = book.Stock,
                AuthorId = book.AuthorId,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt,
                Author = book.Author?.ToRAuthor()
            };
        }
    }
}
