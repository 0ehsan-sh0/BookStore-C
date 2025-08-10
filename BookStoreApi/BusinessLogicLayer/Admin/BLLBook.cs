using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.Requests.Book;
using BookStoreApi.Services;
using BookStoreApi.Services.Models;

namespace BookStoreApi.BusinessLogicLayer.Admin
{
    public class BLLBook(IBookRepository repo, IAuthorRepository authorRepo)
    {
        public async Task<List<ImageInfo>?> UploadImages(List<IFormFile> images)
        {
            List<ImageInfo> imageInfos = new List<ImageInfo>();

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
                        List<string> relativePaths = new List<string>();
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
        public async Task<(string message, Book? book, int status)> Create(CreateBookRequest createBookRequest)
        {
            var book = createBookRequest.ToBook();

            var isbnExists = await repo.GetByISBNAsync(book.ISBN);
            if (isbnExists is not null)
                return ("کتابی با این شابک قبلا ثبت شده است", null, 400);

            var author = await authorRepo.GetByIdAsync(createBookRequest.AuthorId);
            if (author is null)
                return ("نویسنده ای با این شناسه وجود ندارد", null, 404);

            var imageInfos = await UploadImages(createBookRequest.Images);
            if (imageInfos is null)
                return ("هیچ عکسی آپلود نشد", null, 500);

            int id = await repo.CreateAsync(book, imageInfos);
            book = await repo.GetByIdAsync(id);

            return ("کتاب با موفقیت اضافه شد", book, 201);
        }
    }
}
