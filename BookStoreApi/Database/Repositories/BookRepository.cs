using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Book;
using BookStoreApi.RequestHandler.Admin.Responses.Book;
using BookStoreApi.Services.Models;
using Dapper;
using System.Data;

namespace BookStoreApi.Database.Repositories
{
    public class BookRepository(DapperUtility dapperUtility) : IBookRepository
    {
        public async Task<int> CreateAsync(Book book, List<ImageInfo> imageInfos)
        {
            using var connection = dapperUtility.GetConnection();

            var sql = "Book_Insert";

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
                Images = imagesTable.AsTableValuedParameter("ImageInfoType")
            };

            int insertedId = await connection.ExecuteScalarAsync<int>(sql, parameters, commandType: CommandType.StoredProcedure);
            return insertedId;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<(List<Book> books, BPaginationInfo info)> GetAllAsync(QBookGetAll query)
        {
            throw new NotImplementedException();
        }

        public Task<Book?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Book?> UpdateAsync(Book bookWithId)
        {
            throw new NotImplementedException();
        }
    }
}
