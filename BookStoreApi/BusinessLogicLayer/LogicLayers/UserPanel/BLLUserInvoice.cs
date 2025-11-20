using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.Requests.Invoice;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.UserPanel
{
    public class BLLUserInvoice(
        IInvoiceRepository invoiceRepository,
        IBookRepository bookRepository,
        IUserRepository userRepository) : IBLLUserInvoice
    {
        public async Task<(string message, Invoice? invoice, int status)> CreateAsync(string userMobile, CreateInvoiceRequest request)
        {
            // 1. Validate input
            if (request.Books == null || request.Counts == null || request.Books.Count != request.Counts.Count)
                return ("لیست کتاب‌ها یا تعدادها نامعتبر است.", null, 400);

            // 2. Check each book exists and stock is enough
            for (int i = 0; i < request.Books.Count; i++)
            {
                int bookId = request.Books[i];
                int requestedCount = request.Counts[i];

                var book = await bookRepository.GetByIdAsync(bookId);
                if (book == null)
                    return ($"کتاب با شناسه {bookId} پیدا نشد.", null, 404);

                if (book.Stock < requestedCount)
                    return ($"کتاب {book.Name} موجودی کافی ندارد.", null, 409);
            }

            // 3. Get user ID
            var user = await userRepository.GetByMobileAsync(userMobile);
            if (user == null)
                return ("کاربر پیدا نشد.", null, 404);

            // 4. Create invoice
            int newInvoiceId = await invoiceRepository.CreateAsync(user.Id, request.Books, request.Counts);
            var invoice = await invoiceRepository.GetByIdAsync(newInvoiceId);

            if (invoice is null)
                return ("ایجاد فاکتور با مشکل مواجه شد.", null, 500);

            return ("فاکتور با موفقیت ایجاد شد.", invoice, 201);
        }

        public async Task<(string message, Invoice? invoice, int status)> GetByIdAsync(int invoiceId)
        {
            var invoice = await invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice is null)
                return ("فاکتور پیدا نشد.", null, 404);

            return ("فاکتور با موفقیت بارگذاری شد.", invoice, 200);
        }
    }
}
