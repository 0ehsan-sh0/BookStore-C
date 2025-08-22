using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Category;
using BookStoreApi.RequestHandler.Admin.Responses.Category;
using Dapper;
using System.Data;

namespace BookStoreApi.Database.Repositories
{
    public class CategoryRepository(DapperUtility dapperUtility) : ICategoryRepository
    {
        public async Task<int> CreateAsync(Category category)
        {
            using var connection = dapperUtility.GetConnection();

            var sql = @"
            INSERT INTO Categories (Name, Url, MainCategoryId)
            VALUES (@Name, @Url, @MainCategoryId);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new
            {
                category.Name,
                category.Url,
                category.MainCategoryId
            };

            int insertedId = await connection.ExecuteScalarAsync<int>(sql, parameters);
            return insertedId;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = "Update C set DeletedAt = GETDATE() FROM Categories C WHERE Id = @id";
            using var connection = dapperUtility.GetConnection();
            int result = await connection.ExecuteAsync(sql, new { id });
            if (result == 1) return true;
            return false;
        }

        public async Task<(List<Category> categories, CPaginationInfo info)> GetAllAsync(QCategoryGetAll query)
        {
            string sql = "Category_Get_All";
            using var connection = dapperUtility.GetConnection();

            using var multi = await connection.QueryMultipleAsync(
                sql,
                new { query.PageNumber, query.PageSize, query.Search },
                commandType: CommandType.StoredProcedure);

            var categories = (await multi.ReadAsync<Category>()).ToList();
            var pagination = await multi.ReadFirstOrDefaultAsync<CPaginationInfo>();

            return (categories, pagination!);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            string sql = "Category_Get_One";
            using var connection = dapperUtility.GetConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Category>(sql, new { id }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Category?> GetByUrlAsync(string Url)
        {
            string sql = "Category_Get_By_Url";
            using var connection = dapperUtility.GetConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Category>(sql, new { Url }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Category?> UpdateAsync(Category c)
        {
            string sql = @"Update C
                           set Name = @name,Url = @Url,MainCategoryId = @MainCategoryId
                           FROM Categories C
                           WHERE Id = @Id and DeletedAt IS NULL";
            using var connection = dapperUtility.GetConnection();
            var parameters = new
            {
                c.Name,
                c.Url,
                c.MainCategoryId,
                c.Id
            };
            bool result = await connection.ExecuteAsync(sql, parameters) >= 0;
            if (result) return await GetByIdAsync(c.Id);
            return null;
        }
    }
}
