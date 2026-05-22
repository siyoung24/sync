using Microsoft.EntityFrameworkCore;
using MemoApp.Books.Dtos;
using MemoApp.Data;
using MemoApp.Data.Entities;

namespace MemoApp.Books.Services;

public class MyBookService : IMyBookService
{
    private readonly AppDbContext _db;
    private readonly IBookService _bookService;

    public MyBookService(AppDbContext db, IBookService bookService)
    {
        _db = db;
        _bookService = bookService;
    }

    // 위시리스트 추가: ISBN13으로 책을 찾고, 없으면 알라딘에서 가져와 Book 생성 → UserBook 생성
    public async Task<MyBookDto> AddBook(int userId, AddMyBookDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Isbn13))
            throw new BadHttpRequestException("ISBN13이 필요합니다.");

        var isbn = dto.Isbn13.Trim();

        // 1. Book upsert (ISBN13 기준 dedup)
        var book = await _db.Books.FirstOrDefaultAsync(b => b.Isbn13 == isbn);
        if (book == null)
        {
            var aladin = await _bookService.LookupAladin(isbn)
                ?? throw new InvalidOperationException("알라딘에서 해당 책을 찾을 수 없습니다.");

            book = new Book
            {
                Title = aladin.Title,
                Author = aladin.Author,
                TotalPages = aladin.TotalPages,
                Isbn13 = aladin.Isbn13,
                Publisher = aladin.Publisher,
                Cover = aladin.Cover
            };
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
        }

        // 2. 이미 위시리스트에 있는지 체크
        var exists = await _db.UserBooks
            .AnyAsync(ub => ub.UserId == userId && ub.BookId == book.Id);
        if (exists)
            throw new InvalidOperationException("이미 위시리스트에 등록된 책입니다.");

        // 3. UserBook 생성
        var userBook = new UserBook
        {
            UserId = userId,
            BookId = book.Id,
            CurrentPage = 0
        };
        _db.UserBooks.Add(userBook);
        await _db.SaveChangesAsync();

        return ToDto(book, userBook);
    }

    public async Task<List<MyBookDto>> GetMyBooks(int userId)
    {
        return await _db.UserBooks
            .Where(ub => ub.UserId == userId)
            .Join(_db.Books, ub => ub.BookId, b => b.Id, (ub, b) => new MyBookDto
            {
                BookId = b.Id,
                Title = b.Title,
                Author = b.Author,
                TotalPages = b.TotalPages,
                Cover = b.Cover,
                Publisher = b.Publisher,
                Isbn13 = b.Isbn13,
                CurrentPage = ub.CurrentPage,
                AddedAt = ub.AddedAt
            })
            .OrderByDescending(x => x.AddedAt)
            .ToListAsync();
    }

    public async Task RemoveBook(int userId, int bookId)
    {
        var userBook = await _db.UserBooks
            .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookId)
            ?? throw new InvalidOperationException("위시리스트에 없는 책입니다.");

        _db.UserBooks.Remove(userBook);
        await _db.SaveChangesAsync();
    }

    public async Task<MyBookDto> UpdatePage(int userId, int bookId, UpdatePageDto dto)
    {
        if (dto.CurrentPage < 0)
            throw new BadHttpRequestException("페이지는 0 이상이어야 합니다.");

        var userBook = await _db.UserBooks
            .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookId)
            ?? throw new InvalidOperationException("위시리스트에 없는 책입니다.");

        var book = await _db.Books.FirstAsync(b => b.Id == bookId);

        if (book.TotalPages > 0 && dto.CurrentPage > book.TotalPages)
            throw new BadHttpRequestException($"페이지는 총 페이지 수({book.TotalPages})를 넘을 수 없습니다.");

        userBook.CurrentPage = dto.CurrentPage;
        await _db.SaveChangesAsync();

        return ToDto(book, userBook);
    }

    private static MyBookDto ToDto(Book book, UserBook userBook) => new()
    {
        BookId = book.Id,
        Title = book.Title,
        Author = book.Author,
        TotalPages = book.TotalPages,
        Cover = book.Cover,
        Publisher = book.Publisher,
        Isbn13 = book.Isbn13,
        CurrentPage = userBook.CurrentPage,
        AddedAt = userBook.AddedAt
    };
}
