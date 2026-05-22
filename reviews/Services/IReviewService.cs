using MemoApp.Reviews.Dtos;

namespace MemoApp.Reviews.Services;

public interface IReviewService
{
    // 한줄평 작성 (페이지 검증 + UserBook.CurrentPage 자동 max 갱신)
    Task<ReviewDto> CreateReview(int userId, CreateReviewDto dto);

    // 현재 페이지 이하 + 가까운 페이지 순 N개 (요구사항: 5개)
    Task<List<ReviewDto>> GetNearbyReviews(ReviewQueryDto query, int take);

    // 현재 페이지 기준 가장 가까운 한줄평 1개
    Task<ReviewDto?> GetClosestReview(ReviewQueryDto query);

    // 타임라인 - 현재 페이지 이하 모든 한줄평
    Task<List<ReviewDto>> GetTimeline(ReviewQueryDto query);

    // 내가 쓴 한줄평 모아보기
    Task<List<MyReviewDto>> GetMyReviews(int userId);
}
