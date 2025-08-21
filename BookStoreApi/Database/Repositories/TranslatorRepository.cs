using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Translator;
using BookStoreApi.RequestHandler.Admin.Responses.Translator;
using Dapper;
using System.Data;

namespace BookStoreApi.Database.Repositories
{
    public class TranslatorRepository(DapperUtility dapperUtility) : ITranslatorRepository
    {
        public async Task<int> CreateAsync(Translator translator)
        {
            using var connection = dapperUtility.GetConnection();

            var sql = @"
            INSERT INTO Translators (Name, Description)
            VALUES (@Name, @Description);
            SELECT CAST(SCOPE_IDENTITY() as int);"
            ;
            var parameters = new
            {
                translator.Name,
                Description = string.IsNullOrWhiteSpace(translator.Description) ? null : translator.Description,
            };

            int insertedId = await connection.ExecuteScalarAsync<int>(sql, parameters);
            return insertedId;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = "Update T set DeletedAt = GETDATE() FROM Translators T WHERE Id = @id";
            using var connection = dapperUtility.GetConnection();
            int result = await connection.ExecuteAsync(sql, new { id });
            if (result == 1) return true;
            return false;
        }

        public async Task<(List<Translator> translators, TPaginationInfo info)> GetAllAsync(QTranslatorGetAll query)
        {
            string sql = "Translator_Get_All";
            using var connection = dapperUtility.GetConnection();

            using var multi = await connection.QueryMultipleAsync(
                sql,
                new { query.PageNumber, query.PageSize, Search = query.Search },
                commandType: CommandType.StoredProcedure);

            var translators = (await multi.ReadAsync<Translator>()).ToList();
            var pagination = await multi.ReadFirstOrDefaultAsync<TPaginationInfo>();

            return (translators, pagination!);
        }

        public async Task<Translator?> GetByIdAsync(int id)
        {
            string sql = "Translator_Get_One";
            using var connection = dapperUtility.GetConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Translator>(sql, new { id }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Translator?> UpdateAsync(Translator translatorWithId)
        {
            string sql = @"Update T
                           set Name = @name,Description = @Description
                           FROM Translators T
                           WHERE Id = @Id and DeletedAt IS NULL";
            using var connection = dapperUtility.GetConnection();
            var parameters = new
            {
                translatorWithId.Name,
                Description = string.IsNullOrWhiteSpace(translatorWithId.Description) ? null : translatorWithId.Description,
                translatorWithId.Id,
            };
            bool result = await connection.ExecuteAsync(sql, parameters) >= 0;
            if (result) return await GetByIdAsync(translatorWithId.Id);
            return null;
        }
    }
}
