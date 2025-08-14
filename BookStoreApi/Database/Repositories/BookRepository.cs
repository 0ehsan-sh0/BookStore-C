using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Book;
using BookStoreApi.RequestHandler.Admin.Responses.Book;
using BookStoreApi.Services;
using BookStoreApi.Services.Models;
using Dapper;
using System.Data;

namespace BookStoreApi.Database.Repositories
{
    public class BookRepository(DapperUtility dapperUtility) : IBookRepository
    {
        public async Task<int> CreateAsync(Book book, List<ImageInfo> imageInfos, List<int>? translators, List<int> categories)
        {
            var sql = "Book_Insert";

            // making images table
            var imagesTable = new DataTable();
            imagesTable.Columns.Add("Width", typeof(int));
            imagesTable.Columns.Add("Height", typeof(int));
            imagesTable.Columns.Add("IsPrimary", typeof(bool));
            imagesTable.Columns.Add("StoredFileName", typeof(string));
            imagesTable.Columns.Add("RelativePath", typeof(string));
            imagesTable.Columns.Add("FileSize", typeof(long));
            imagesTable.Columns.Add("MimeType", typeof(string));
            for (int i = 0; i < imageInfos.Count; i++)
            {
                var image = imageInfos[i];
                bool isPrimary = (i == 0);  // First image is primary

                imagesTable.Rows.Add(
                    image.Width,
                    image.Height,
                    isPrimary,
                    image.StoredFileName,
                    image.RelativePath,
                    image.FileSize,
                    image.MimeType
                );
            }

            //translator ids become a table here
            DataTable translatorIds = new();
            translatorIds.Columns.Add("Id", typeof(int));
            if (translators is not null)
            {
                foreach (var id in translators)
                {
                    translatorIds.Rows.Add(id);
                }
            }

            //Categoriy ids become a table here
            DataTable categoryIds = new();
            categoryIds.Columns.Add("Id", typeof(int));
            foreach (var id in categories)
            {
                categoryIds.Rows.Add(id);
            }

            // making the parameters for Store procejure
            var parameters = new
            {
                book.Name,
                EnglishName = string.IsNullOrWhiteSpace(book.EnglishName) ? null : book.EnglishName,
                Description = string.IsNullOrWhiteSpace(book.Description) ? null : book.Description,
                book.Price,
                book.PrintSeries,
                book.ISBN,
                CoverType = string.IsNullOrWhiteSpace(book.CoverType) ? null : book.CoverType,
                Format = string.IsNullOrWhiteSpace(book.Format) ? null : book.Format,
                book.Pages,
                book.PublishYear,
                Publisher = string.IsNullOrWhiteSpace(book.Publisher) ? null : book.Publisher,
                book.AuthorId,
                Images = imagesTable.AsTableValuedParameter("ImageInfoType"),
                TranslatorIds = translatorIds.AsTableValuedParameter("IntList"),
                CategoryIds = categoryIds.AsTableValuedParameter("IntList"),
            };

            // Database call and inserting the book and relationships
            try
            {
                using var connection = dapperUtility.GetConnection();
                int insertedId = await connection.ExecuteScalarAsync<int>(sql, parameters, commandType: CommandType.StoredProcedure);
                return insertedId;
            }
            catch (Exception)
            {
                // if anything goes wrong the images must be deleted
                if (imageInfos.Count > 0)
                {
                    List<string> relativePaths = [];
                    foreach (var imageInfo in imageInfos)
                    {
                        string relativePath = imageInfo.RelativePath.Trim() + imageInfo.StoredFileName.Trim();
                        relativePaths.Add(relativePath);
                    }
                    ImageService.DeleteImagesAsync(relativePaths);
                }
                return 0;
            }
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<(List<Book> books, BPaginationInfo info)> GetAllAsync(QBookGetAll query)
        {
            throw new NotImplementedException();
        }

        public async Task<BookAllData?> GetByIdAsync(int id)
        {
            using var connection = dapperUtility.GetConnection();
            await connection.OpenAsync();

            using var multi = await connection.QueryMultipleAsync(
                "Book_Get_One",
                new { id },
                commandType: CommandType.StoredProcedure
            );

            BookAllData data = new();

            // First result: Book info
            var book = await multi.ReadFirstOrDefaultAsync<Book>();
            if (book == null) return null;

            data.Id = book.Id;
            data.Name = book.Name;
            data.EnglishName = book.EnglishName;
            data.Description = book.Description;
            data.Price = book.Price;
            data.PrintSeries = book.PrintSeries;
            data.ISBN = book.ISBN;
            data.CoverType = book.CoverType;
            data.Format = book.Format;
            data.Pages = book.Pages;
            data.PublishYear = book.PublishYear;
            data.Publisher = book.Publisher;
            data.AuthorId = book.AuthorId;
            data.CreatedAt = book.CreatedAt;
            data.UpdatedAt = book.UpdatedAt;

            // Second result: Author info
            data.Author = await multi.ReadFirstOrDefaultAsync<Author>();

            // Third result: Translators
            data.Translators = (await multi.ReadAsync<Translator>()).ToList();

            // Fourth result: Categories
            data.Categories = (await multi.ReadAsync<Category>()).ToList();

            // Fifth result: Images
            data.Images = (await multi.ReadAsync<Image>()).ToList();

            return data;
        }

        public async Task<Book?> GetByISBNAsync(string isbn)
        {
            string sql = "Book_Get_By_ISBN";
            using var connection = dapperUtility.GetConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Book>(sql, new { isbn }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<BookAllData?> UpdateAsync(Book bookWithId, List<int>? translators, List<int> categories)
        {
            var sql = "Book_Update";

            //translator ids become a table here
            DataTable translatorIds = new();
            translatorIds.Columns.Add("Id", typeof(int));
            if (translators is not null)
            {
                foreach (var id in translators)
                {
                    translatorIds.Rows.Add(id);
                }
            }

            //Categoriy ids become a table here
            DataTable categoryIds = new();
            categoryIds.Columns.Add("Id", typeof(int));
            foreach (var id in categories)
            {
                categoryIds.Rows.Add(id);
            }

            // making the parameters for Store procejure
            var parameters = new
            {
                bookWithId.Id,
                bookWithId.Name,
                EnglishName = string.IsNullOrWhiteSpace(bookWithId.EnglishName) ? null : bookWithId.EnglishName,
                Description = string.IsNullOrWhiteSpace(bookWithId.Description) ? null : bookWithId.Description,
                bookWithId.Price,
                bookWithId.PrintSeries,
                bookWithId.ISBN,
                CoverType = string.IsNullOrWhiteSpace(bookWithId.CoverType) ? null : bookWithId.CoverType,
                Format = string.IsNullOrWhiteSpace(bookWithId.Format) ? null : bookWithId.Format,
                bookWithId.Pages,
                bookWithId.PublishYear,
                Publisher = string.IsNullOrWhiteSpace(bookWithId.Publisher) ? null : bookWithId.Publisher,
                bookWithId.AuthorId,
                TranslatorIds = translatorIds.AsTableValuedParameter("IntList"),
                CategoryIds = categoryIds.AsTableValuedParameter("IntList"),
            };

            // Database call and inserting the book and relationships
            try
            {
                using var connection = dapperUtility.GetConnection();
                int result = await connection.ExecuteScalarAsync<int>(sql, parameters, commandType: CommandType.StoredProcedure);
                if (result == 1)
                    return await GetByIdAsync(bookWithId.Id);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
