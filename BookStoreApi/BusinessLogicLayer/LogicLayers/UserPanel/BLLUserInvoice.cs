using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.User.QueryObjects.Invoice;
using BookStoreApi.RequestHandler.User.Requests.Invoice;
using BookStoreApi.RequestHandler.User.Responses.Invoice;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.UserPanel
{
    public class BLLUserInvoice(
        IInvoiceRepository invoiceRepository,
        IBookRepository bookRepository,
        IUserRepository userRepository,
        IAddressRepository addressRepository) : IBLLUserInvoice
    {
        public async Task<(string message, Invoice? invoice, int status)> CreateAsync(string userMobile, CreateInvoiceRequest request)
        {
            // 1. Validate input
            if (request.Books == null || request.Counts == null || request.Books.Count != request.Counts.Count)
                return ("لیست کتاب‌ها یا تعدادها نامعتبر است.", null, 400);

            // 2. Check for address
            var address = await addressRepository.GetByIdAsync(request.AddressId);
            if (address == null)
                return ("آدرس پیدا نشد.", null, 404);

            // 3. Check each book exists and stock is enough
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

            // 4. Get user ID
            var user = await userRepository.GetByMobileAsync(userMobile);
            if (user == null)
                return ("کاربر پیدا نشد.", null, 404);

            // 5. Verify address belongs to user
            if (user.Id != address.UserId)
                return ("آدرس پیدا نشد", null, 404);

            // 6. Create invoice
            int newInvoiceId = await invoiceRepository.CreateAsync(user.Id, address.Id, request.Books, request.Counts);
            var invoice = await invoiceRepository.GetByIdAsync(newInvoiceId);

            if (invoice is null)
                return ("ایجاد فاکتور با مشکل مواجه شد.", null, 500);

            return ("فاکتور با موفقیت ایجاد شد.", invoice, 201);
        }

        public async Task<(List<Invoice>? invoices, InvoicePaginationInfo pagination)> GetUserInvoicesAsync(string mobile, QUserInvoices query)
        {
            return await invoiceRepository.GetUserInvoicesAsync(mobile, query);
        }

        public async Task<(string message, Invoice? invoice, int status)> GetByIdAsync(string mobile, int invoiceId)
        {
            var user = await userRepository.GetByMobileAsync(mobile);
            if (user == null)
                return ("کاربر پیدا نشد.", null, 404);

            var invoice = await invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice is null || invoice.UserId != user.Id)
                return ("فاکتور پیدا نشد.", null, 404);


            return ("فاکتور با موفقیت بارگذاری شد.", invoice, 200);
        }
    }
}
