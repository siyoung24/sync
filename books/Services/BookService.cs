using Microsoft.EntityFrameworkCore;
using MemoApp.Books.Dtos;
using MemoApp.Common.Validators;
using MemoApp.Data;
using MemoApp.Data.Entities;

namespace MemoApp.Books.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _db;

    public BookService(AppDbContext db)
    {
        _db = db;
    }

    // 책 목록 조회 (검색어 있으면 제목으로 필터링)
    public async Task<List<BookDto>> GetBookList(BookQueryDto query)
    {
        IQueryable<Book> books = _db.Books;

        // 검색어 유효한 경우만 필터 적용
        if (InputValidator.IsValidKeyword(query.SearchKeyword))
        {
            var keyword = query.SearchKeyword!.Trim();
            books = books.Where(b => b.Title.Contains(keyword));
        }

        // UI 바인딩용 DTO로 변환해서 반환
        return await books
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                TotalPages = b.TotalPages,
                CreatedAt = b.CreatedAt
            })
            .ToListAsync();
    }
}
