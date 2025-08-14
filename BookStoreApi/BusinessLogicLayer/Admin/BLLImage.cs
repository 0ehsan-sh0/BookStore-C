using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Requests.Image;
using BookStoreApi.Services;
using BookStoreApi.Services.Models;

namespace BookStoreApi.BusinessLogicLayer.Admin
{
    public class BLLImage(IImageRepository repo)
    {
        public async Task<List<ImageInfo>?> UploadImages(List<IFormFile> images)
        {
            List<ImageInfo> imageInfos = [];

            foreach (var image in images)
            {
                try
                {
                    var (message, imageInfo, status) = await ImageService.SaveImageAsync(image);
                    if (status == 201)
                    {
                        imageInfos.Add(imageInfo!);
                    }
                    else
                    {
                        throw new Exception(message);
                    }
                }
                catch (Exception)
                {
                    if (imageInfos.Count > 0)
                    {
                        List<string> relativePaths = [];
                        foreach (var imageInfo in imageInfos)
                        {
                            string relativePath = imageInfo.RelativePath.Trim() + imageInfo.StoredFileName.Trim();
                            relativePaths.Add(relativePath);
                        }
                        ImageService.DeleteImagesAsync(relativePaths);
                    }
                    return null;
                }
            }

            return imageInfos;
        }

        public async Task<(string message, List<Image>? image, int status)> Create(CreateImageRequest createImageRequest)
        {
            var foreignExists = await repo.ForeignIdExists(createImageRequest.ForeignId, createImageRequest.ForeignTable);
            if (!foreignExists)
                return ("شناسه خارجی یافت نشد", null, 404);

            var imageInfos = await UploadImages(createImageRequest.Images);
            if (imageInfos is null)
                return ("هیچ عکسی آپلود نشد", null, 500);

            var ids = await repo.CreateAsync(imageInfos, createImageRequest.ForeignTable, createImageRequest.ForeignId);
            if (ids is null)
            {
                List<string> relativePaths = [];
                foreach (var imageInfo in imageInfos)
                {
                    string relativePath = imageInfo.RelativePath.Trim() + imageInfo.StoredFileName.Trim();
                    relativePaths.Add(relativePath);
                }
                ImageService.DeleteImagesAsync(relativePaths);
                return ("مشکلی در اضافه کردن تصاویر پیش آمد", null, 500);
            }

            var images = await repo.GetByIdAsync(ids);

            return ("تصاویر با موفقیت اضافه شدند", images, 201);
        }

        public async Task<(string message, int status)> Delete(int id)
        {
            var image = await repo.GetByIdAsync(id);
            if (image is null)
                return ("تصویر مورد نظر یافت نشد", 404);
            if (image.IsPrimary == true)
                return ("نمیتوانید تصویر اصلی را حذف کنید.تصویر اصلی را تغییر داده سپس اقدام به حذف کنید", 403);

            string path = image.RelativePath.Trim() + image.StoredFileName.Trim();
            var (_, status) = ImageService.DeleteImageAsync(path);

            if (!status)
                return ("در حذف تصویر مشکلی پیش آمد", 500);

            var entity = await repo.DeleteAsync(id);
            if (!entity) return ("تصویر مورد نظر یافت نشد", 404);

            return ("تصویر با موفقیت حذف شد", 204);
        }

        public async Task<(string message, int status)> ChangePrimary(int id)
        {
            var image = await repo.GetByIdAsync(id);
            if (image is null)
                return ("تصویری با این شناسه یافت نشد", 404);
            if (image.IsPrimary)
                return ("تصویر انتخاب شده تصویر اصلی است", 403);

            var status = await repo.ChangePrimary(id);
            return status
                ? ("تصویر اصلی با موفقیت تغییر کرد", 201)
                : ("تغییر تصویر اصلی با مشکل مواجه شد", 500);
        }
    }
}