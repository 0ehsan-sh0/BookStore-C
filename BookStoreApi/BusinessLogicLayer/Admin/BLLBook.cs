using BookStoreApi.Database.Interfaces;

namespace BookStoreApi.BusinessLogicLayer.Admin
{
    public class BLLBook(IBookRepository repo)
    {
        //public async Task<(string message, Book? category, int status)> Create(CreateBookRequest createBookRequest)
        //{
        //    var book = createBookRequest.ToBook();

        //    int id = await repo.CreateAsync(book);
        //    author = await repo.GetByIdAsync(id);

        //    return ("نویسنده با موفقیت اضافه شد", author, 201);
        //}
    }
}
