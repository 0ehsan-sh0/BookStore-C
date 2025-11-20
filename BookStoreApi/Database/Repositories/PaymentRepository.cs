using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using Dapper;
using System.Data;

namespace BookStoreApi.Database.Repositories
{
    public class PaymentRepository(DapperUtility dapperUtility) : IPaymentRepository
    {
        public async Task<int> CreateAsync(Payment payment)
        {
            var sql = @"INSERT INTO Payments 
                        (InvoiceId, GatewayId, PaymentGateway, ResponseCode, Message,Price, Status,
                         TransactionCode)
                        VALUES (@InvoiceId, @GatewayId, @PaymentGateway, @ResponseCode, 
                                @Message,@Price, @Status, @TransactionCode);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var cn = dapperUtility.GetConnection();
            return await cn.ExecuteScalarAsync<int>(sql, payment);
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            using var cn = dapperUtility.GetConnection();
            return await cn.QueryFirstOrDefaultAsync<Payment>(
                "Payment_Get_One",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }
    }
}
