using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.QueryObjects.Book;
using BookStoreApi.RequestHandler.User.Responses.Book;
using Dapper;
using System.Data;
using System.Text.Json;

namespace BookStoreApi.Database.Repositories
{
    public class WishListRepository(DapperUtility dapperUtility) : IWishListRepository
    {
        public async Task<WishList?> CreateAsync(WishList wishList)
        {
            const string sql = @"
                INSERT INTO WishList (UserId, BookId)
                VALUES (@UserId, @BookId);

                SELECT UserId, BookId
                FROM WishList
                WHERE UserId = @UserId AND BookId = @BookId;
              ";

            using var connection = dapperUtility.GetConnection();

            return await connection.QueryFirstOrDefaultAsync<WishList>(
                sql,
                new { wishList.UserId, wishList.BookId }
            );
        }

        public async Task<bool> DeleteAsync(WishList wishList)
        {
            const string sql = @"
                DELETE FROM WishList
                WHERE UserId = @UserId AND BookId = @BookId;
            ";

            using var connection = dapperUtility.GetConnection();

            int rows = await connection.ExecuteAsync(sql, new
            {
                wishList.UserId,
                wishList.BookId
            });

            return rows > 0;
        }

        public async Task<(List<BookAllData>? books, BookPaginationInfo info)> GetUserWishListAsync(int id, QUserWishList query)
        {
            using var connection = dapperUtility.GetConnection();
            await connection.OpenAsync();

            using var multi = await connection.QueryMultipleAsync(
                "Get_User_WishList",
                new { query.PageNumber, query.PageSize, Id = id },
                commandType: CommandType.StoredProcedure
            );

            // First result: JSON string
            var booksJson = await multi.ReadFirstOrDefaultAsync<string>();
            List<BookAllData> books = [];
            if (booksJson is not null)
                books = JsonSerializer.Deserialize<List<BookAllData>>(booksJson)!;

            var paginationJson = await multi.ReadFirstOrDefaultAsync<string>();
            var pagination = JsonSerializer.Deserialize<BookPaginationInfo>(paginationJson!);

            return (books, pagination!);
        }

        public async Task<WishList?> GetWishListAsync(WishList wishList)
        {
            string sql = "WishList_Get_One";
            using var connection = dapperUtility.GetConnection();
            var result = await connection.QueryFirstOrDefaultAsync<WishList>(
                sql,
                new { userId = wishList.UserId, bookId = wishList.BookId },
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<bool> ToggleAsync(WishList wishList)
        {
            // Check if it already exists
            var exists = await GetWishListAsync(wishList);

            if (exists != null)
            {
                // Remove it
                await DeleteAsync(wishList);
                return false; // Removed from wishlist
            }

            // Add it
            await CreateAsync(wishList);
            return true; // Added to wishlist
        }
    }
}
