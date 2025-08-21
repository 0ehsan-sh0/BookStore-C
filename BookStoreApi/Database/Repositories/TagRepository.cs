using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Tag;
using BookStoreApi.RequestHandler.Admin.Responses.Tag;
using Dapper;
using System.Data;

namespace BookStoreApi.Database.Repositories
{
    public class TagRepository(DapperUtility dapperUtility) : ITagRepository
    {
        public async Task<int> CreateAsync(Tag tag)
        {
            using var connection = dapperUtility.GetConnection();

            var sql = @"
            INSERT INTO Tags (Name, Url)
            VALUES (@Name, @Url);
            SELECT CAST(SCOPE_IDENTITY() as int);"
            ;
            var parameters = new
            {
                Name = tag.Name.Trim(),
                Url = tag.Url.Trim()
            };

            int insertedId = await connection.ExecuteScalarAsync<int>(sql, parameters);
            return insertedId;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = "Update T set DeletedAt = GETDATE() FROM Tags T WHERE Id = @id";
            using var connection = dapperUtility.GetConnection();
            int result = await connection.ExecuteAsync(sql, new { id });
            if (result == 1) return true;
            return false;
        }

        public async Task<(List<Tag> tags, TagPaginationInfo info)> GetAllAsync(QTagGetAll query)
        {
            string sql = "Tag_Get_All";
            using var connection = dapperUtility.GetConnection();

            using var multi = await connection.QueryMultipleAsync(
                sql,
                new { query.PageNumber, query.PageSize },
                commandType: CommandType.StoredProcedure);

            var tags = (await multi.ReadAsync<Tag>()).ToList();
            var pagination = await multi.ReadFirstOrDefaultAsync<TagPaginationInfo>();

            return (tags, pagination!);
        }

        public async Task<Tag?> GetByIdAsync(int id)
        {
            string sql = "Tag_Get_One";
            using var connection = dapperUtility.GetConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Tag>(sql, new { id }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Tag?> GetByUrlAsync(string Url)
        {
            string sql = "Tag_Get_By_Url";
            using var connection = dapperUtility.GetConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Tag>(sql, new { Url }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Tag?> UpdateAsync(Tag tagWithId)
        {
            string sql = @"Update T
                           set Name = @name,Url = @Url
                           FROM Tags T
                           WHERE Id = @Id and DeletedAt IS NULL";
            using var connection = dapperUtility.GetConnection();
            var parameters = new
            {
                Name = tagWithId.Name.Trim(),
                Url = tagWithId.Url.Trim(),
                tagWithId.Id,
            };
            bool result = await connection.ExecuteAsync(sql, parameters) >= 0;
            if (result) return await GetByIdAsync(tagWithId.Id);
            return null;
        }
    }
}
