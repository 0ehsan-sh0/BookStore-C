using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Payment;
using BookStoreApi.RequestHandler.Admin.Responses.Payment;
using Dapper;
using System.Data;
using System.Text.Json;

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

        public async Task<(List<Payment> payments, PaymentPaginationInfo info)> GetAllAsync(QPaymentGetAll query)
        {
            using var connection = dapperUtility.GetConnection();
            await connection.OpenAsync();

            using var multi = await connection.QueryMultipleAsync(
                "Payment_Get_All_JSON",
                new { query.PageNumber, query.PageSize, query.Search },
                commandType: CommandType.StoredProcedure
            );

            // First result: JSON string
            var paymentsJson = await multi.ReadFirstOrDefaultAsync<string>();
            List<Payment> payments = [];
            if (paymentsJson is not null)
                payments = JsonSerializer.Deserialize<List<Payment>>(paymentsJson)!;

            var paginationJson = await multi.ReadFirstOrDefaultAsync<string>();
            var pagination = JsonSerializer.Deserialize<PaymentPaginationInfo>(paginationJson!);

            return (payments, pagination!);
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
