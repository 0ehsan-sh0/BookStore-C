using BookStoreApi.Services.Models;
using Microsoft.AspNetCore.Http;

namespace BookStoreApi.Services.Interfaces
{
    public interface IImageService
    {
        Task<(string message, ImageInfo? information, int status)> SaveImageAsync(IFormFile file, string subFolder = "");
        (string message, int deletedCount) DeleteImages(List<string> relativePaths);
        (string message, bool status) DeleteImage(string relativePath);
    }
}
