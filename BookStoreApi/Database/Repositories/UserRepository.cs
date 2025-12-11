using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.Enums;
using BookStoreApi.RequestHandler.Admin.QueryObjects.User;
using BookStoreApi.RequestHandler.Admin.Responses.User;
using Dapper;
using System.Data;
using System.Text.Json;

namespace BookStoreApi.Database.Repositories
{
    public class UserRepository(DapperUtility dapperUtility) : IUserRepository
    {
        public async Task<(List<User> users, UPaginationInfo info)> GetAllAsync(QUserGetAll query)
        {
            string sql = "User_Get_All";
            using var connection = dapperUtility.GetConnection();

            using var multi = await connection.QueryMultipleAsync(
                sql,
                new { query.PageNumber, query.PageSize, query.Search, query.Role },
                commandType: CommandType.StoredProcedure);

            var users = (await multi.ReadAsync<User>()).ToList();
            var pagination = await multi.ReadFirstOrDefaultAsync<UPaginationInfo>();

            return (users, pagination!);
        }

        public async Task<int> CreateAsync(User user)
        {
            using var connection = dapperUtility.GetConnection();

            var sql = @"
            INSERT INTO Users (Mobile, Password, Role, LoggedInAt)
            VALUES (@Mobile, @Password, @Role, @LoggedInAt);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new
            {
                user.Mobile,
                user.Password,
                Role = UserRole.User,
                LoggedInAt = DateTime.Now,
            };

            int insertedId = await connection.ExecuteScalarAsync<int>(sql, parameters);
            return insertedId;
        }

        public async Task<int> AdminCreateAsync(User user)
        {
            using var connection = dapperUtility.GetConnection();

            var sql = @"
            INSERT INTO Users (Mobile,Name,LastName, Password, Role)
            VALUES (@Mobile,@Name,@LastName, @Password, @Role);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new
            {
                user.Mobile,
                user.Password,
                user.Name,
                user.LastName,
                user.Role,
            };
            int insertedId = await connection.ExecuteScalarAsync<int>(sql, parameters);
            return insertedId;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = "Update U set DeletedAt = GETDATE() FROM Users U WHERE Id = @id";
            using var connection = dapperUtility.GetConnection();
            int result = await connection.ExecuteAsync(sql, new { id });
            if (result == 1) return true;
            return false;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var connection = dapperUtility.GetConnection();
            await connection.OpenAsync();

            using var multi = await connection.QueryMultipleAsync(
                "User_Get_One",
                new { id },
                commandType: CommandType.StoredProcedure
            );

            // First result: JSON string
            var userJson = await multi.ReadFirstOrDefaultAsync<string>();
            User user = new();
            if (userJson is not null)
                user = JsonSerializer.Deserialize<User>(userJson)!;

            return user;
        }

        public async Task<User?> GetByMobileAsync(string mobile)
        {
            using var connection = dapperUtility.GetConnection();
            await connection.OpenAsync();

            using var multi = await connection.QueryMultipleAsync(
                "User_Get_By_Mobile",
                new { mobile },
                commandType: CommandType.StoredProcedure
            );

            // First result: JSON string
            var userJson = await multi.ReadFirstOrDefaultAsync<string>();
            User user = new();
            if (userJson is not null)
            {
                user = JsonSerializer.Deserialize<User>(userJson)!;
                return user;
            }

            return null;
        }

        public async Task<User?> UpdateAsync(User userWithId)
        {
            string sql = @"Update U
                           set Name = @Name,LastName = @LastName,Role = @Role
                           FROM Users U
                           WHERE Id = @Id and DeletedAt IS NULL";
            using var connection = dapperUtility.GetConnection();
            var parameters = new
            {
                userWithId.Name,
                userWithId.LastName,
                userWithId.Id,
                userWithId.Role
            };
            bool result = await connection.ExecuteAsync(sql, parameters) >= 0;
            if (result) return await GetByIdAsync(userWithId.Id);
            return null;
        }

        public async Task<User?> UpdateByMobileAsync(User userWithMobile)
        {
            string sql = @"Update U
                           set Name = @Name,LastName = @LastName
                           FROM Users U
                           WHERE Mobile = @Mobile and DeletedAt IS NULL";
            using var connection = dapperUtility.GetConnection();
            var parameters = new
            {
                userWithMobile.Name,
                userWithMobile.LastName,
                userWithMobile.Mobile,
            };
            bool result = await connection.ExecuteAsync(sql, parameters) >= 0;
            if (result) return await GetByMobileAsync(userWithMobile.Mobile);
            return null;
        }

        public async Task<bool> UpdateLoggedInAt(string mobile)
        {
            string sql = @"Update U
                           set LoggedInAt = @LoggedInAt
                           FROM Users U
                           WHERE Mobile = @mobile and DeletedAt IS NULL";
            using var connection = dapperUtility.GetConnection();
            var parameters = new
            {
                LoggedInAt = DateTime.Now,
                mobile,
            };

            bool result = await connection.ExecuteAsync(sql, parameters) >= 0;
            return result;
        }


    }
}
