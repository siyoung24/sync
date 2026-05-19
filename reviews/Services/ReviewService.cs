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

    // 현재 페이지 이하 한줄평 중 가장 가까운 1개 (스포일러 방지)
    public async Task<ReviewDto?> GetClosestReview(ReviewQueryDto query)
    {
        return await BuildBaseQuery(query)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                BookId = r.BookId,
                WriterId = r.WriterId,
                Page = r.Page,
                Content = r.Content,
                CreatedAt = r.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    // 타임라인 - 현재 페이지 이하 한줄평 전부 (가까운 페이지부터)
    public async Task<List<ReviewDto>> GetTimeline(ReviewQueryDto query)
    {
        return await BuildBaseQuery(query)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                BookId = r.BookId,
                WriterId = r.WriterId,
                Page = r.Page,
                Content = r.Content,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();
    }

    // 추천 알고리즘 핵심: 같은 책 & 현재 페이지 이하 → 페이지 가까운 순 → 최신순
    private IQueryable<Review> BuildBaseQuery(ReviewQueryDto query)
    {
        return _db.Reviews
            .Where(r => r.BookId == query.BookId && r.Page <= query.CurrentPage)
            .OrderByDescending(r => r.Page)
            .ThenByDescending(r => r.CreatedAt);
    }
}
