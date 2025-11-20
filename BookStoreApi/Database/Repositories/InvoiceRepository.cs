using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.Enums;
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
            var booksWithCount = await multi.ReadAsync<dynamic>();
            var booksList = new List<Book>();
            foreach (var b in booksWithCount)
            {
                booksList.Add(new Book
                {
                    Id = b.Id,
                    Name = b.Name,
                    EnglishName = b.EnglishName,
                    Description = b.Description,
                    Price = b.Price,
                    PrintSeries = b.PrintSeries,
                    ISBN = b.ISBN,
                    CoverType = b.CoverType,
                    Format = b.Format,
                    Pages = b.Pages,
                    PublishYear = b.PublishYear,
                    Publisher = b.Publisher,
                    Stock = b.Stock,
                    AuthorId = b.AuthorId,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt,
                    // You can add a custom property for purchased count if needed
                    // PurchasedCount = b.PurchasedCount
                });
            }
            invoice.Books = booksList;

            // 4. User
            invoice.User = await multi.ReadFirstOrDefaultAsync<User>();

            return invoice;
        }


        public Task<(List<Invoice> invoices, InvoicePaginationInfo info)> GetUserInvoicesAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<(List<Invoice> invoices, InvoicePaginationInfo info)> GetUserInvoicesAsync(string mobile)
        {
            throw new NotImplementedException();
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
    }
}
