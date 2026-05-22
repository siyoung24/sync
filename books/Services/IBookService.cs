using MemoApp.Books.Dtos;

namespace MemoApp.Books.Services;

public interface IBookService
{
    Task<List<BookDto>> GetBookList(BookQueryDto query);
    Task<List<BookSearchResultDto>> SearchAladin(string keyword);
    Task<BookSearchResultDto?> LookupAladin(string isbn13);
}
