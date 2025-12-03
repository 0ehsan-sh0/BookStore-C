using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Mappers;
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

        public static RBookAllData ToRBookAllData(this BookAllData bookAllData)
        {
            RBookAllData rBookAllData = new()
            {
                Id = bookAllData.Id,
                Name = bookAllData.Name,
                EnglishName = bookAllData.EnglishName ?? "",
                Description = bookAllData.Description ?? "",
                Price = bookAllData.Price,
                AuthorId = bookAllData.AuthorId,
                AuthorName = bookAllData.AuthorName,
                PrintSeries = bookAllData.PrintSeries,
                ISBN = bookAllData.ISBN,
                CoverType = bookAllData.CoverType,
                Format = bookAllData.Format,
                Pages = bookAllData.Pages.ToString(),
                PublishYear = bookAllData.PublishYear.ToString(),
                Publisher = bookAllData.Publisher,
                IsRecommended = bookAllData.IsRecommended,
                Stock = bookAllData.Stock,
                Quantity = bookAllData.Quantity,
                CreatedAt = bookAllData.CreatedAt,
                UpdatedAt = bookAllData.UpdatedAt
            };

            if (bookAllData.Author is not null)
                rBookAllData.Author = bookAllData.Author.ToRAuthor();

            if (bookAllData.Images is not null && bookAllData.Images.Count > 0)
                rBookAllData.Images = bookAllData.Images.Select(a => a.ToRImage()).ToList();

            return rBookAllData;
        }
    }
}
