using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Author;
using BookStoreApi.RequestHandler.Admin.Responses.Author;
using Dapper;
using System.Data;

namespace BookStoreApi.Database.Repositories
{
    public class AuthorRepository(DapperUtility dapperUtility) : IAuthorRepository
    {
        public async Task<int> CreateAsync(Author author)
        {
            using var connection = dapperUtility.GetConnection();

            var sql = @"
            INSERT INTO Authors (Name, Description)
            VALUES (@Name, @Description);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new
            {
                author.Name,
                Description = string.IsNullOrWhiteSpace(author.Description) ? null : author.Description,
            };

            int insertedId = await connection.ExecuteScalarAsync<int>(sql, parameters);
            return insertedId;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = "Update A set DeletedAt = GETDATE() FROM Authors A WHERE Id = @id";
            using var connection = dapperUtility.GetConnection();
            int result = await connection.ExecuteAsync(sql, new { id });
            if (result == 1) return true;
            return false;
        }

        public async Task<(List<Author> authors, APaginationInfo info)> GetAllAsync(QAuthorGetAll query)
        {
            string sql = "Author_Get_All";
            using var connection = dapperUtility.GetConnection();

            using var multi = await connection.QueryMultipleAsync(
                sql,
                new { query.PageNumber, query.PageSize, Search = query.Search },
                commandType: CommandType.StoredProcedure);

            var authors = (await multi.ReadAsync<Author>()).ToList();
            var pagination = await multi.ReadFirstOrDefaultAsync<APaginationInfo>();

            return (authors, pagination!);
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            string sql = "Author_Get_One";
            using var connection = dapperUtility.GetConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Author>(sql, new { id }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Author?> UpdateAsync(Author authorWithId)
        {
            string sql = @"Update A
                           set Name = @name,Description = @Description
                           FROM Authors A
                           WHERE Id = @Id and DeletedAt IS NULL";
            using var connection = dapperUtility.GetConnection();
            var parameters = new
            {
                authorWithId.Name,
                Description = string.IsNullOrWhiteSpace(authorWithId.Description) ? null : authorWithId.Description,
                authorWithId.Id,
            };
            bool result = await connection.ExecuteAsync(sql, parameters) >= 0;
            if (result) return await GetByIdAsync(authorWithId.Id);
            return null;
        }
    }
}
