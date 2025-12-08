using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.Enums;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Invoice;
using BookStoreApi.RequestHandler.User.QueryObjects.Invoice;
using BookStoreApi.RequestHandler.User.Responses.Invoice;
using Dapper;
using System.Data;
using System.Text.Json;

namespace BookStoreApi.Database.Repositories
{
    public class InvoiceRepository(DapperUtility dapperUtility) : IInvoiceRepository
    {
        public async Task<int> CreateAsync(int userId, int addressId, List<int> books, List<int> counts)
        {
            var sql = "Invoice_Insert"; // SP name

            // Convert books and counts to DataTables
            var booksTable = DataTables.IntListTable(books);
            var countsTable = DataTables.IntListTable(counts);

            // Parameters for the stored procedure
            var parameters = new
            {
                AddressId = addressId,
                UserId = userId,
                Books = booksTable.AsTableValuedParameter("IntList"),
                Counts = countsTable.AsTableValuedParameter("IntList")
            };

            try
            {
                using var connection = dapperUtility.GetConnection();
                await connection.OpenAsync();

                // SP returns the InvoiceId as a SELECT result
                int newInvoiceId = await connection.ExecuteScalarAsync<int>(
                    sql,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return newInvoiceId;
            }
            catch
            {
                // Optional: log the exception
                return 0; // or throw; depending on your error handling strategy
            }
        }

        public async Task<bool> UpdatePaymentStatusAsync(int invoiceId, PaymentStatus status)
        {
            var sql = @"UPDATE Invoices SET PaymentStatus = @Status
                WHERE Id = @InvoiceId";

            using var cn = dapperUtility.GetConnection();
            int rows = await cn.ExecuteAsync(sql, new { InvoiceId = invoiceId, Status = status });
            return rows > 0;
        }

        public async Task<Invoice?> GetByIdAsync(int id)
        {
            using var connection = dapperUtility.GetConnection();
            await connection.OpenAsync();

            using var multi = await connection.QueryMultipleAsync(
                "Invoice_Get_One",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );

            // 1. Invoice main info
            var invoice = await multi.ReadFirstOrDefaultAsync<Invoice>();
            if (invoice == null) return null;

            // 2. Payments
            invoice.Payments = (await multi.ReadAsync<Payment>()).ToList();

            // 3. Books (include PurchasedCount from InvoiceBooks)
            invoice.Books = (await multi.ReadAsync<BookAllData>()).ToList();

            // 4. User
            invoice.User = await multi.ReadFirstOrDefaultAsync<User>();

            // 5. Address
            invoice.Address = await multi.ReadFirstOrDefaultAsync<AddressInfo>();

            return invoice;
        }


        public async Task<(List<Invoice> invoices, InvoicePaginationInfo info)> GetUserInvoicesAsync(int id, QUserInvoices query)
        {
            using var connection = dapperUtility.GetConnection();
            await connection.OpenAsync();

            using var multi = await connection.QueryMultipleAsync(
                "Get_User_Invoices_By_Id",
                new { id, query.PageNumber, query.PageSize },
                commandType: CommandType.StoredProcedure
            );

            // First result: JSON string
            var invoicesJson = await multi.ReadFirstOrDefaultAsync<string>();
            List<Invoice> invoices = [];
            if (invoicesJson is not null)
                invoices = JsonSerializer.Deserialize<List<Invoice>>(invoicesJson)!;

            var paginationJson = await multi.ReadFirstOrDefaultAsync<string>();
            var pagination = JsonSerializer.Deserialize<InvoicePaginationInfo>(paginationJson!);

            return (invoices, pagination!);
        }

        public async Task<(List<Invoice> invoices, InvoicePaginationInfo info)> GetUserInvoicesAsync(string mobile, QUserInvoices query)
        {
            using var connection = dapperUtility.GetConnection();
            await connection.OpenAsync();

            using var multi = await connection.QueryMultipleAsync(
                "Get_User_Invoices_By_Mobile",
                new { mobile, query.PageNumber, query.PageSize },
                commandType: CommandType.StoredProcedure
            );

            // First result: JSON string
            var invoicesJson = await multi.ReadFirstOrDefaultAsync<string>();
            List<Invoice> invoices = [];
            if (invoicesJson is not null)
                invoices = JsonSerializer.Deserialize<List<Invoice>>(invoicesJson)!;

            var paginationJson = await multi.ReadFirstOrDefaultAsync<string>();
            var pagination = JsonSerializer.Deserialize<InvoicePaginationInfo>(paginationJson!);

            return (invoices, pagination!);
        }

        public async Task<(List<InvoiceBooks> books, bool isValid)> GetBooksOfInvoiceAsync(int invoiceId)
        {
            const string sql = @"
                SELECT 
                    InvoiceId,
                    BookId,
                    Count,
                    Price
                FROM InvoiceBooks
                WHERE InvoiceId = @InvoiceId";

            try
            {
                using var cn = dapperUtility.GetConnection();

                var result = await cn.QueryAsync<InvoiceBooks>(
                    sql,
                    new { InvoiceId = invoiceId }
                );

                return (result.ToList(), true);
            }
            catch
            {
                return (new List<InvoiceBooks>(), false);
            }
        }

        public async Task<(List<Invoice> invoices, RequestHandler.Admin.Responses.Invoice.InvoicePaginationInfo info)> GetAllAsync(QInvoiceGetAll query)
        {
            using var connection = dapperUtility.GetConnection();
            await connection.OpenAsync();

            using var multi = await connection.QueryMultipleAsync(
                "Invoice_Get_All",
                new { query.PageNumber, query.PageSize, query.Search },
                commandType: CommandType.StoredProcedure
            );

            // First result: JSON string
            var invoicesJson = await multi.ReadFirstOrDefaultAsync<string>();
            List<Invoice> invoices = [];
            if (invoicesJson is not null)
                invoices = JsonSerializer.Deserialize<List<Invoice>>(invoicesJson)!;

            var paginationJson = await multi.ReadFirstOrDefaultAsync<string>();
            var pagination = JsonSerializer.Deserialize<RequestHandler.Admin.Responses.Invoice.InvoicePaginationInfo>(paginationJson!);

            return (invoices, pagination!);
        }
    }
}
