using BookStoreApi.RequestHandler.User.Requests.Invoice;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel
{
    public interface IBLLUserInvoice
    {
        Task<(string message, int createdId, int status)> CreateAsync(string userMobile, CreateInvoiceRequest request);
    }
}
