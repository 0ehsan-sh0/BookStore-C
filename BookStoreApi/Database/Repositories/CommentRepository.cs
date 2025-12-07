using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Comment;
using BookStoreApi.RequestHandler.Admin.Responses.Comment;
using BookStoreApi.RequestHandler.Public.Responses.Comment;
using Dapper;
using System.Data;
using System.Text.Json;

namespace BookStoreApi.Database.Repositories
{
    public class CommentRepository(DapperUtility dapperUtility) : ICommentRepository
    {
        public async Task<int> CreateAsync(CommentInfo comment)
        {
            using var connection = dapperUtility.GetConnection();

            var sql = @"
            INSERT INTO Comments (Comment, Status, ForeignTable, ForeignId, UserId)
            VALUES (@Comment, @Status, @ForeignTable, @ForeignId, @UserId);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new
            {
                Comment = string.IsNullOrWhiteSpace(comment.Comment) ? null : comment.Comment,
                Status = false,
                comment.ForeignTable,
                comment.ForeignId,
                comment.UserId
            };

            int insertedId = await connection.ExecuteScalarAsync<int>(sql, parameters);
            return insertedId;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = "DELETE FROM Comments WHERE Id = @id";
            using var connection = dapperUtility.GetConnection();
            int result = await connection.ExecuteAsync(sql, new { id });
            if (result == 1) return true;
            return false;
        }

        public async Task<(List<CommentInfo> comments, COPaginationInfo info)> GetAllAsync(QCommentGetAll query)
        {
            string sql = "Comment_Get_All";
            using var connection = dapperUtility.GetConnection();

            using var multi = await connection.QueryMultipleAsync(
                sql,
                new { query.PageNumber, query.PageSize, query.Search },
                commandType: CommandType.StoredProcedure);

            var comments = (await multi.ReadAsync<CommentInfo>()).ToList();
            var pagination = await multi.ReadFirstOrDefaultAsync<COPaginationInfo>();

            return (comments, pagination!);
        }

        public async Task<(List<CommentInfo> comments, CommentPaginationInfo info)> GetBookCommentsAsync(int bookId, int pageNumber, int pageSize)
        {
            using var connection = dapperUtility.GetConnection();

            using var multi = await connection.QueryMultipleAsync(
                "Comment_Get_Book_JSON",
                new { BookId = bookId, PageNumber = pageNumber, PageSize = pageSize },
                commandType: CommandType.StoredProcedure
            );

            var commentsJson = await multi.ReadFirstOrDefaultAsync<string>();
            List<CommentInfo> comments = new();

            if (commentsJson != null)
                comments = JsonSerializer.Deserialize<List<CommentInfo>>(commentsJson)!;

            var paginationJson = await multi.ReadFirstOrDefaultAsync<string>();
            var pagination = JsonSerializer.Deserialize<CommentPaginationInfo>(paginationJson!)!;

            return (comments, pagination);
        }

        public async Task<CommentInfo?> GetByIdAsync(int id)
        {
            string sql = "Comment_Get_One";
            using var connection = dapperUtility.GetConnection();
            var result = await connection.QueryFirstOrDefaultAsync<CommentInfo>(sql, new { id }, commandType: CommandType.StoredProcedure);
            return result;

        }

        public async Task<bool> SwitchIsConfirmedAsync(int id)
        {
            var comment = await GetByIdAsync(id);
            if (comment is null) return false;

            string newStatus = (!comment.Status) ? "1" : "0";
            string sql = $"Update C set Status = {newStatus} FROM Comments C WHERE Id = @id";
            using var connection = dapperUtility.GetConnection();
            int result = await connection.ExecuteAsync(sql, new { id });
            if (result == 1) return true;
            return false;
        }
    }
}
