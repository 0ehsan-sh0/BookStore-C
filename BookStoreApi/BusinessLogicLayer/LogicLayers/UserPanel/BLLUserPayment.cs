using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.Enums;
using ZarinPal;
using ZarinPal.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.UserPanel
{
    public class BLLUserPayment(
        IPaymentRepository repo,
        IInvoiceRepository invoiceRepository,
        IBookRepository bookRepository,
        IConfiguration configuration
        ) : IBLLUserPayment
    {
        private readonly ZarinPal.ZarinPal _zarinPal = new(new Config
        {
            MerchantId = configuration["ZarinPal:MerchantId"]!,
            Sandbox = bool.Parse(configuration["ZarinPal:Sandbox"] ?? "false")
        });

    public async Task<(string message, string? paymentUrl, int status)> InitiatePurchaseAsync(int invoiceId, string callbackUrl)
    {
        var invoice = await invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice == null)
            return ("فاکتور پیدا نشد.", null, 404);

        var paymentRequest = new PaymentRequest
        {
            Amount = (int)invoice.FinalTotalPrice, // Amount in Toman/Rial depending on SDK implementation (README says Rials, but usually ZarinPal is Toman or configurable)
            CallbackUrl = callbackUrl,
            Description = $"پرداخت فاکتور شماره {invoiceId}",
        };

        try
        {
            var response = await _zarinPal.Payments.CreateAsync(paymentRequest);
            var authority = response.GetProperty("data").GetProperty("authority").GetString();

            if (string.IsNullOrEmpty(authority))
                return ("خطا در دریافت کد پرداخت از زرین پال.", null, 500);

            var payment = new Payment
            {
                InvoiceId = invoiceId,
                GatewayId = "ZarinPal",
                Price = invoice.FinalTotalPrice,
                PaymentGateway = "زرین پال",
                Status = PaymentStatus.Initiated,
                TransactionCode = authority,
            };

            int paymentId = await repo.CreateAsync(payment);
            if (paymentId == 0)
                return ("ثبت پرداخت با خطا مواجه شد.", null, 500);

            await invoiceRepository.UpdatePaymentStatusAsync(invoiceId, PaymentStatus.Initiated);

            var paymentUrl = _zarinPal.GetBaseUrl() + "/pg/StartPay/" + authority;
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
            Amount = (int)invoice.FinalTotalPrice,
            Authority = authority
        };

        try
        {
            var response = await _zarinPal.Verifications.VerifyAsync(verificationRequest);
            var code = response.GetProperty("data").GetProperty("code").GetInt32();

            if (code == 100 || code == 101)
            {
                // Success
                // Success
                string refId;
                var refIdProp = response.GetProperty("data").GetProperty("ref_id");
                if (refIdProp.ValueKind == JsonValueKind.Number)
                {
                    refId = refIdProp.GetInt64().ToString();
                }
                else
                {
                    refId = refIdProp.GetString() ?? "";
                }

                // 1. Get invoice books
                var (books, _) = await invoiceRepository.GetBooksOfInvoiceAsync(invoiceId);
                if (books == null || books.Count == 0)
                    return ("کتاب‌های فاکتور پیدا نشد.", null, 404);

                // 2. Create payment record (or update existing one)
                // In this flow, we should find the initiated payment and update it.
                // For simplicity, let's create a completion record or update status.
                // Assuming we want to update the existing initiated payment:
                // But IPaymentRepository only has Create and GetById.
                // Let's create a new payment record with Success status.

                var payment = new Payment
                {
                    InvoiceId = invoiceId,
                    GatewayId = "ZarinPal",
                    Price = invoice.FinalTotalPrice,
                    PaymentGateway = "زرین پال",
                    ResponseCode = code.ToString(),
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
