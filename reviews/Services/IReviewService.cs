using MemoApp.Reviews.Dtos;

namespace MemoApp.Reviews.Services;

public interface IReviewService
{
    // 현재 페이지 기준 가장 가까운 한줄평 1개 (PRD: "가장 가까운 페이지의 데이터 우선 제공")
    Task<ReviewDto?> GetClosestReview(ReviewQueryDto query);

    // 타임라인 뷰 - 현재 페이지 이전 모든 한줄평 (가까운 페이지부터 정렬)
    Task<List<ReviewDto>> GetTimeline(ReviewQueryDto query);
}
