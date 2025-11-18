using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.Responses.Invoice;
using Dapper;
using System.Data;

namespace BookStoreApi.Database.Repositories
{
    public class InvoiceRepository(DapperUtility dapperUtility) : IInvoiceRepository
    {
        public async Task<int> CreateAsync(int userId, List<int> books, List<int> counts)
        {
            var sql = "Invoice_Insert"; // SP name

            // Convert books and counts to DataTables
            var booksTable = DataTables.IntListTable(books);
            var countsTable = DataTables.IntListTable(counts);

            // Parameters for the stored procedure
            var parameters = new
            {
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

        public Task<(List<Invoice> invoices, InvoicePaginationInfo info)> GetUserInvoicesAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<(List<Invoice> invoices, InvoicePaginationInfo info)> GetUserInvoicesAsync(string mobile)
        {
            throw new NotImplementedException();
        }
    }
}
