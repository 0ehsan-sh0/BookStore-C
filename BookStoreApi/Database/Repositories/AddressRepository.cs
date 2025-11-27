using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.QueryObjects.Address;
using BookStoreApi.RequestHandler.User.Responses.Address;
using Dapper;
using System.Data;

namespace BookStoreApi.Database.Repositories
{
    public class AddressRepository(DapperUtility dapperUtility) : IAddressRepository
    {
        public async Task<int> CreateAsync(AddressInfo address)
        {
            var sql = @"
                 INSERT INTO Addresses (Name, LastName, Phone, PostCode, State, City, Address, UserId)
                 VALUES (@Name, @LastName, @Phone, @PostCode, @State, @City, @Address, @UserId);
                 SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new
            {
                address.Name,
                address.LastName,
                address.Phone,
                address.PostCode,
                address.State,
                address.City,
                address.Address,
                address.UserId
            };

            try
            {
                using var connection = dapperUtility.GetConnection();

                int insertedId = await connection.ExecuteScalarAsync<int>(sql, parameters);
                return insertedId;
            }
            catch (Exception e)
            {
                return -1;

            }

        }

        public async Task<(List<AddressInfo> addresses, AddressPaginationInfo info)> GetUserAddressesAsync(int userId, QUserAddress query)
        {
            string sql = "Address_Get_All"; // Stored procedure name
            using var connection = dapperUtility.GetConnection();

            using var multi = await connection.QueryMultipleAsync(
                sql,
                new
                {
                    userId,
                    query.PageNumber,
                    query.PageSize,
                },
                commandType: CommandType.StoredProcedure
            );

            var addresses = (await multi.ReadAsync<AddressInfo>()).ToList();
            var pagination = await multi.ReadFirstOrDefaultAsync<AddressPaginationInfo>();

            return (addresses, pagination!);
        }

        public async Task<AddressInfo?> GetByIdAsync(int id)
        {
            string sql = "Address_Get_One";
            using var connection = dapperUtility.GetConnection();

            var result = await connection.QueryFirstOrDefaultAsync<AddressInfo>(
                sql,
                new { id },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<AddressInfo?> UpdateAsync(AddressInfo addressWithId)
        {
            string sql = @"
                UPDATE A
                SET 
                    Name = @Name,
                    LastName = @LastName,
                    Phone = @Phone,
                    PostCode = @PostCode,
                    State = @State,
                    City = @City,
                    Address = @Address
                FROM Addresses A
                WHERE Id = @Id";

            using var connection = dapperUtility.GetConnection();

            var parameters = new
            {
                addressWithId.Name,
                addressWithId.LastName,
                addressWithId.Phone,
                addressWithId.PostCode,
                addressWithId.State,
                addressWithId.City,
                addressWithId.Address,
                addressWithId.Id
            };

            bool result = await connection.ExecuteAsync(sql, parameters) > 0;

            if (result)
                return await GetByIdAsync(addressWithId.Id); // reuse your GetByIdAsync method

            return null;
        }
    }
}
