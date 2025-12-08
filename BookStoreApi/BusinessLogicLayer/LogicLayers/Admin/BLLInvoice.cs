using BookStoreApi.BusinessLogicLayer.Interfaces.Admin;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Invoice;
using BookStoreApi.RequestHandler.Admin.Responses.Invoice;

namespace BookStoreApi.BusinessLogicLayer.LogicLayers.Admin
{
    public class BLLInvoice(IInvoiceRepository repo) : IBLLInvoice
    {
        public async Task<(string message, List<Invoice> invoices, InvoicePaginationInfo info, int status)> GetAllAsync(QInvoiceGetAll query)
        {
            var (invoices, pagination) = await repo.GetAllAsync(query);
            return ("فاکتور ها با موفقیت بارگزاری شدند.", invoices, pagination, 200);
        }
    }
}
