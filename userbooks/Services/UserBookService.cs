using Microsoft.EntityFrameworkCore;
using MemoApp.Data;
using MemoApp.Data.Entities;
using MemoApp.UserBooks.Dtos;

namespace MemoApp.UserBooks.Services;

public class UserBookService : IUserBookService
{
    private const int ShelfLimit = 3;   // 책장 최대 담을 수 있는 권수

    private readonly AppDbContext _db;

    public UserBookService(AppDbContext db)
    {
        _db = db;
    }

    // 책장에 추가 — 3권 초과 / 중복 / 존재하지 않는 책 모두 백엔드에서 검증
    public async Task<AddUserBookResult> AddToShelf(CreateUserBookDto dto)
    {
        // 1) 사용자/책 존재 확인
        if (!await _db.Users.AnyAsync(u => u.Id == dto.UserId))
            return new AddUserBookResult { Code = AddUserBookResultCode.UserNotFound };

        if (!await _db.Books.AnyAsync(b => b.Id == dto.BookId))
            return new AddUserBookResult { Code = AddUserBookResultCode.BookNotFound };

        // 2) 중복 체크 — DB UNIQUE 제약과 이중 방어
        var exists = await _db.UserBooks
            .AnyAsync(ub => ub.UserId == dto.UserId && ub.BookId == dto.BookId);
        if (exists)
            return new AddUserBookResult { Code = AddUserBookResultCode.AlreadyExists };

        // 3) 3권 제한 체크
        var count = await _db.UserBooks.CountAsync(ub => ub.UserId == dto.UserId);
        if (count >= ShelfLimit)
            return new AddUserBookResult { Code = AddUserBookResultCode.LimitReached };

        // 4) 추가
        var entity = new UserBook
        {
            UserId = dto.UserId,
            BookId = dto.BookId,
            CreatedAt = DateTime.UtcNow
        };
        _db.UserBooks.Add(entity);
        await _db.SaveChangesAsync();

        return new AddUserBookResult
        {
            Code = AddUserBookResultCode.Success,
            UserBook = new UserBookDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                BookId = entity.BookId,
                CreatedAt = entity.CreatedAt
            }
        };
    }

    public async Task<List<UserBookDto>> GetShelf(int userId)
    {
        return await _db.UserBooks
            .Where(ub => ub.UserId == userId)
            .OrderByDescending(ub => ub.CreatedAt)
            .Select(ub => new UserBookDto
            {
                Id = ub.Id,
                UserId = ub.UserId,
                BookId = ub.BookId,
                CreatedAt = ub.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<bool> RemoveFromShelf(int userId, int bookId)
    {
        var entity = await _db.UserBooks
            .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookId);
        if (entity == null) return false;

        _db.UserBooks.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
