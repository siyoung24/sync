using MemoApp.Books.Dtos;

namespace MemoApp.Books.Services;

public interface IMyBookService
{
    Task<MyBookDto> AddBook(int userId, AddMyBookDto dto);
    Task<List<MyBookDto>> GetMyBooks(int userId);
    Task RemoveBook(int userId, int bookId);
    Task<MyBookDto> UpdatePage(int userId, int bookId, UpdatePageDto dto);
}
