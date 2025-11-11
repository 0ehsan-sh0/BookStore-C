using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Requests.Image;
using BookStoreApi.Services.Models;

namespace BookStoreApi.BusinessLogicLayer.Interfaces.Admin
{
    public interface IBLLImage
    {
        Task<(string message, List<Image>? image, int status)> Create(CreateImageRequest createImageRequest);
        Task<(string message, int status)> Delete(int id);
        Task<(string message, int status)> ChangePrimary(int id);
    }
}