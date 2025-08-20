using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Models;
using BookStoreApi.RequestHandler.Admin.Mappers;
using BookStoreApi.RequestHandler.Admin.QueryObjects.Book;
using BookStoreApi.RequestHandler.Admin.Requests.Book;
using BookStoreApi.RequestHandler.Admin.Responses.Book;
using BookStoreApi.Services;
using ImageInfo = BookStoreApi.Services.Models.ImageInfo;

namespace BookStoreApi.BusinessLogicLayer.Admin
{
    public class BLLBook(IBookRepository repo, IAuthorRepository authorRepo, ITranslatorRepository translatorRepo, ICategoryRepository categoryRepo, ITagRepository tagRepo)
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
        public async Task<(string message, BookAllData? book, int status)> Create(CreateBookRequest createBookRequest)
        {
            var book = createBookRequest.ToBook();

            var isbnExists = await repo.GetByISBNAsync(book.ISBN);
            if (isbnExists is not null)
                return ("کتابی با این شابک قبلا ثبت شده است", null, 400);

            var author = await authorRepo.GetByIdAsync(createBookRequest.AuthorId);
            if (author is null)
                return ("نویسنده ای با این شناسه وجود ندارد", null, 404);

            if (createBookRequest.Translators is not null && createBookRequest.Translators.Count > 0)
            {
                foreach (var translatorId in createBookRequest.Translators)
                {
                    var translator = await translatorRepo.GetByIdAsync(translatorId);
                    if (translator is null)
                        return ($"مترجمی با شناسه {translatorId} وجود ندارد", null, 404);
                }
            }

            if (createBookRequest.Categories.Count > 0)
            {
                foreach (var categoryId in createBookRequest.Categories)
                {
                    var category = await categoryRepo.GetByIdAsync(categoryId);
                    if (category is null)
                        return ($"دسته بندی با شناسه {categoryId} وجود ندارد", null, 404);
                }
            }
            else return ("لطفا حداقل یک دسته بندی وارد کنید", null, 400);

            if (createBookRequest.Tags.Count > 0)
            {
                foreach (var tagId in createBookRequest.Tags)
                {
                    var tag = await tagRepo.GetByIdAsync(tagId);
                    if (tag is null)
                        return ($"دسته بندی با شناسه {tagId} وجود ندارد", null, 404);
                }
            }
            else return ("لطفا حداقل یک دسته بندی وارد کنید", null, 400);

            var imageInfos = await UploadImages(createBookRequest.Images);
            if (imageInfos is null)
                return ("هیچ عکسی آپلود نشد", null, 500);

            int id = await repo.CreateAsync(book, imageInfos, createBookRequest.Translators, createBookRequest.Categories, createBookRequest.Tags);
            var bookdata = await repo.GetByIdAsync(id);

            if (bookdata is null)
                return ("ایجاد کتاب با مشکل مواجه شد", null, 500);

            return ("کتاب با موفقیت اضافه شد", bookdata, 201);
        }

        public async Task<(string message, BookAllData? book, int status)> Update(UpdateBookRequest updateBookRequest, int id)
        {
            var book = updateBookRequest.ToBook(id);

            var bookExist = await repo.GetByIdAsync(id);
            if (bookExist is null)
                return ("کتابی با این شناسه وجود ندارد", null, 404);

            var isbnExists = await repo.GetByISBNAsync(book.ISBN);
            if (isbnExists is not null && isbnExists.ISBN != bookExist.ISBN)
                return ("کتابی با این شابک قبلا ثبت شده است", null, 400);

            var author = await authorRepo.GetByIdAsync(updateBookRequest.AuthorId);
            if (author is null)
                return ("نویسنده ای با این شناسه وجود ندارد", null, 404);

            if (updateBookRequest.Translators is not null && updateBookRequest.Translators.Count > 0)
            {
                foreach (var translatorId in updateBookRequest.Translators)
                {
                    var translator = await translatorRepo.GetByIdAsync(translatorId);
                    if (translator is null)
                        return ($"مترجمی با شناسه {translatorId} وجود ندارد", null, 404);
                }
            }

            if (updateBookRequest.Categories.Count > 0)
            {
                foreach (var categoryId in updateBookRequest.Categories)
                {
                    var category = await categoryRepo.GetByIdAsync(categoryId);
                    if (category is null)
                        return ($"دسته بندی با شناسه {categoryId} وجود ندارد", null, 404);
                }
            }
            else return ("لطفا حداقل یک دسته بندی وارد کنید", null, 400);

            if (updateBookRequest.Tags.Count > 0)
            {
                foreach (var tagId in updateBookRequest.Tags)
                {
                    var tag = await tagRepo.GetByIdAsync(tagId);
                    if (tag is null)
                        return ($"تگ با شناسه {tagId} وجود ندارد", null, 404);
                }
            }
            else return ("لطفا حداقل یک دسته بندی وارد کنید", null, 400);

            var bookAllData = await repo.UpdateAsync(book, updateBookRequest.Translators, updateBookRequest.Categories, updateBookRequest.Tags);

            if (bookAllData is null)
                return ("بروزرسانی کتاب با مشکل مواجه شد", null, 500);

            return ("کتاب با موفقیت بروزرسانی شد", bookAllData, 201);


        }

        public async Task<(List<BookAllData>? books, BPaginationInfo info)> GetAllAsync(QBookGetAll query)
        {
            var (books, info) = await repo.GetAllAsync(query);
            return (books, info);
        }

        public async Task<BookAllData?> GetByIdAsync(int id)
        {
            return await repo.GetByIdAsync(id);
        }

        public async Task<(string message, int status)> Delete(int id)
        {
            var existingEntity = await repo.GetByIdAsync(id);
            if (existingEntity is null)
                return ("کتاب مورد نظر یافت نشد", 404);

            var entity = await repo.DeleteAsync(id);
            if (!entity) return ("کتاب مورد نظر یافت نشد", 404);

            return ("کتاب با موفقیت حذف شد", 204);
        }
    }
}
