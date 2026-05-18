using MemoApp.Books.Dtos;

namespace MemoApp.Books.Services;

public interface IBookService
{
    Task<List<BookDto>> GetBookList(BookQueryDto query);
}
