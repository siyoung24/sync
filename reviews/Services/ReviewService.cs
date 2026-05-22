using Microsoft.EntityFrameworkCore;
using MemoApp.Data;
using MemoApp.Data.Entities;
using MemoApp.Reviews.Dtos;

namespace MemoApp.Reviews.Services;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _db;

    public ReviewService(AppDbContext db)
    {
        _db = db;
    }

    // 한줄평 작성: 위시리스트 확인 → 페이지 검증 → 저장 → CurrentPage 자동 max 갱신
    public async Task<ReviewDto> CreateReview(int userId, CreateReviewDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Content))
            throw new BadHttpRequestException("한줄평 내용을 입력해주세요.");

        if (dto.Page <= 0)
            throw new BadHttpRequestException("페이지는 1 이상이어야 합니다.");

        var userBook = await _db.UserBooks
            .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == dto.BookId)
            ?? throw new InvalidOperationException("위시리스트에 등록된 책에만 한줄평을 작성할 수 있습니다.");

        var book = await _db.Books.FirstAsync(b => b.Id == dto.BookId);

        // 스포 방지: 현재 진행한 페이지 이하만 작성 가능
        if (dto.Page > userBook.CurrentPage)
            throw new BadHttpRequestException($"현재 페이지({userBook.CurrentPage})보다 뒤의 페이지에는 작성할 수 없습니다. 먼저 진행도를 갱신해주세요.");

        // TotalPages 알면 상한 검증, 모르면(0) 통과
        if (book.TotalPages > 0 && dto.Page > book.TotalPages)
            throw new BadHttpRequestException($"페이지는 총 페이지 수({book.TotalPages})를 넘을 수 없습니다.");

        var review = new Review
        {
            BookId = dto.BookId,
            WriterId = userId,
            Page = dto.Page,
            Content = dto.Content.Trim()
        };
        _db.Reviews.Add(review);

        await _db.SaveChangesAsync();

        return ToDto(review);
    }

    // 현재 페이지 이하 + 가까운 페이지 순 N개 (스포 방지 핵심)
    public async Task<List<ReviewDto>> GetNearbyReviews(ReviewQueryDto query, int take)
    {
        if (take <= 0) take = 5;

        return await BuildBaseQuery(query)
            .Take(take)
            .Select(r => new ReviewDto
            {
                Id = r.Id, BookId = r.BookId, WriterId = r.WriterId,
                Page = r.Page, Content = r.Content, CreatedAt = r.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ReviewDto?> GetClosestReview(ReviewQueryDto query)
    {
        return await BuildBaseQuery(query)
            .Select(r => new ReviewDto
            {
                Id = r.Id, BookId = r.BookId, WriterId = r.WriterId,
                Page = r.Page, Content = r.Content, CreatedAt = r.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<ReviewDto>> GetTimeline(ReviewQueryDto query)
    {
        return await BuildBaseQuery(query)
            .Select(r => new ReviewDto
            {
                Id = r.Id, BookId = r.BookId, WriterId = r.WriterId,
                Page = r.Page, Content = r.Content, CreatedAt = r.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<MyReviewDto>> GetMyReviews(int userId)
    {
        return await _db.Reviews
            .Where(r => r.WriterId == userId)
            .Join(_db.Books, r => r.BookId, b => b.Id, (r, b) => new MyReviewDto
            {
                Id = r.Id,
                BookId = b.Id,
                BookTitle = b.Title,
                Cover = b.Cover,
                Page = r.Page,
                Content = r.Content,
                CreatedAt = r.CreatedAt
            })
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    // 같은 책 & 현재 페이지 이하 → 페이지 가까운 순 → 최신순
    private IQueryable<Review> BuildBaseQuery(ReviewQueryDto query)
    {
        return _db.Reviews
            .Where(r => r.BookId == query.BookId && r.Page <= query.CurrentPage)
            .OrderByDescending(r => r.Page)
            .ThenByDescending(r => r.CreatedAt);
    }

    private static ReviewDto ToDto(Review r) => new()
    {
        Id = r.Id,
        BookId = r.BookId,
        WriterId = r.WriterId,
        Page = r.Page,
        Content = r.Content,
        CreatedAt = r.CreatedAt
    };
}
