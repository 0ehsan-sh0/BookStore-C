using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using Dapper;
using System.Data;

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
