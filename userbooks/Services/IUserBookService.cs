using MemoApp.UserBooks.Dtos;

namespace MemoApp.UserBooks.Services;

public enum AddUserBookResultCode
{
    Success,
    LimitReached,    // 3권 초과
    AlreadyExists,   // 이미 책장에 있음
    BookNotFound,    // 존재하지 않는 책
    UserNotFound     // 존재하지 않는 사용자
}

public class AddUserBookResult
{
    public AddUserBookResultCode Code { get; set; }
    public UserBookDto? UserBook { get; set; }
}

public interface IUserBookService
{
    Task<AddUserBookResult> AddToShelf(CreateUserBookDto dto);   // 책장에 추가
    Task<List<UserBookDto>> GetShelf(int userId);                // 내 책장 목록
    Task<bool> RemoveFromShelf(int userId, int bookId);          // 책장에서 제거
}
