using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.Enums;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.UserPanel
{
    public class BLLUserPayment(
        IPaymentRepository repo,
        IInvoiceRepository invoiceRepository,
        IBookRepository bookRepository
        ) : IBLLUserPayment
    {
        public async Task<(string message, Payment? payment, int status)> PurchaseAsync(int invoiceId)
        {
            var invoice = await invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice == null)
                return ("فاکتور پیدا نشد.", null, 404);
            // 1. Get invoice books (you need a repo method for this)
            var (books, _) = await invoiceRepository.GetBooksOfInvoiceAsync(invoiceId);
            if (books == null || books.Count == 0)
                return ("کتاب‌های فاکتور پیدا نشد.", null, 404);

            // 2. Create fake payment data
            var random = new Random();
            string txCode = random.Next(1, 100000000).ToString();

            var payment = new Payment
            {
                InvoiceId = invoiceId,
                GatewayId = "TEST-GATEWAY",
                Price = invoice.FinalTotalPrice,
                PaymentGateway = "زرین پال",
                ResponseCode = "00",
                Message = "پرداخت با موفقیت انجام شد",
                Status = PaymentStatus.Completed,
                TransactionCode = txCode,
            };

            // 3. Insert payment
            int paymentId = await repo.CreateAsync(payment);
            if (paymentId == 0)
                return ("ثبت پرداخت با خطا مواجه شد.", null, 500);

            // 4. Update invoice payment status
            await invoiceRepository.UpdatePaymentStatusAsync(invoiceId, PaymentStatus.Completed);

            // 5. Convert invoice books to TVP list (FOR BULK UPDATE)
            var items = books
                .Select(b => (b.BookId, b.Count))
                .ToList();

            // 6. Bulk decrease book stock
            bool stockUpdated = await bookRepository.DecreaseStockBulkAsync(items);

            // 7. Get created payment
            var paymentCreated = await repo.GetByIdAsync(paymentId);

            if (!stockUpdated)
                return ("خطا در به‌روزرسانی موجودی کتاب‌ها", paymentCreated, 500);

            return ("پرداخت با موفقیت ثبت شد.", paymentCreated, 201);
        }

    }
}
