using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.Enums;
using ZarinPal.Interfaces;
using ZarinPal.Models;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.UserPanel
{
    public class BLLUserPayment(
        IPaymentRepository repo,
        IInvoiceRepository invoiceRepository,
        IBookRepository bookRepository,
        IZarinPal zarinPal
        ) : IBLLUserPayment
    {
    public async Task<(string message, string? paymentUrl, int status)> InitiatePurchaseAsync(int invoiceId, string callbackUrl)
    {
        var invoice = await invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice == null)
            return ("فاکتور پیدا نشد.", null, 404);

        var paymentRequest = new PaymentRequest
        {
            Amount = invoice.FinalTotalPrice,
            CallbackUrl = callbackUrl,
            Description = $"پرداخت فاکتور شماره {invoiceId}",
        };

        try
        {
            var response = await zarinPal.CreateAsync(paymentRequest);

            if (string.IsNullOrEmpty(response.Authority))
                return ("خطا در دریافت کد پرداخت از زرین پال.", null, 500);

            var payment = new Payment
            {
                InvoiceId = invoiceId,
                GatewayId = "ZarinPal",
                Price = invoice.FinalTotalPrice,
                PaymentGateway = "زرین پال",
                Status = PaymentStatus.Initiated,
                TransactionCode = response.Authority,
            };

            int paymentId = await repo.CreateAsync(payment);
            if (paymentId == 0)
                return ("ثبت پرداخت با خطا مواجه شد.", null, 500);

            await invoiceRepository.UpdatePaymentStatusAsync(invoiceId, PaymentStatus.Initiated);

            var paymentUrl = zarinPal.GetRedirectUrl(response.Authority);
            return ("درخواست پرداخت با موفقیت ثبت شد.", paymentUrl, 201);
        }
        catch (Exception ex)
        {
            return ($"خطا در ایجاد درخواست پرداخت: {ex.Message}", null, 500);
        }
    }

    public async Task<(string message, Payment? payment, int status)> VerifyPurchaseAsync(int invoiceId, string authority)
    {
        var invoice = await invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice == null)
            return ("فاکتور پیدا نشد.", null, 404);

        var verificationRequest = new VerificationRequest
        {
            Amount = invoice.FinalTotalPrice,
            Authority = authority
        };

        try
        {
            var response = await zarinPal.VerifyAsync(verificationRequest);

            if (response.Code == 100 || response.Code == 101)
            {
                string refId = response.RefId?.ToString() ?? "";

                // 1. Get invoice books
                var (books, _) = await invoiceRepository.GetBooksOfInvoiceAsync(invoiceId);
                if (books == null || books.Count == 0)
                    return ("کتاب‌های فاکتور پیدا نشد.", null, 404);

                // 2. Create payment record (or update existing one)
                var payment = new Payment
                {
                    InvoiceId = invoiceId,
                    GatewayId = "ZarinPal",
                    Price = invoice.FinalTotalPrice,
                    PaymentGateway = "زرین پال",
                    ResponseCode = response.Code.ToString(),
                    Message = "پرداخت با موفقیت انجام شد",
                    Status = PaymentStatus.Completed,
                    TransactionCode = refId,
                };

                int paymentId = await repo.CreateAsync(payment);
                if (paymentId == 0)
                    return ("ثبت نهایی پرداخت با خطا مواجه شد.", null, 500);

                // 3. Update invoice payment status
                await invoiceRepository.UpdatePaymentStatusAsync(invoiceId, PaymentStatus.Completed);

                // 4. Update stock
                var items = books.Select(b => (b.BookId, b.Count)).ToList();
                bool stockUpdated = await bookRepository.DecreaseStockBulkAsync(items);

                var paymentCreated = await repo.GetByIdAsync(paymentId);
                if (!stockUpdated)
                    return ("خطا در به‌روزرسانی موجودی کتاب‌ها", paymentCreated, 500);

                return ("پرداخت با موفقیت تایید شد.", paymentCreated, 200);
            }
            else
            {
                await invoiceRepository.UpdatePaymentStatusAsync(invoiceId, PaymentStatus.Failed);
                return ("پرداخت ناموفق بود یا قبلا تایید شده است.", null, 400);
            }
        }
        catch (Exception ex)
        {
            return ($"خطا در تایید پرداخت: {ex.Message}", null, 500);
        }
    }
}
}
