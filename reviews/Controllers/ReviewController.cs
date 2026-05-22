using Microsoft.AspNetCore.Mvc;
using MemoApp.Reviews.Dtos;
using MemoApp.Reviews.Services;

namespace MemoApp.Reviews.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // 가장 가까운 한줄평 1개 (스포일러 방지 - 페이지 이하만)
    [HttpGet("closest")]
    public async Task<IActionResult> GetClosest([FromQuery] ReviewQueryDto query)
    {
        try {
            var result = await _reviewService.GetClosestReview(query);
            if (result == null)
                return NotFound(new { message = "현재 페이지까지 작성된 한줄평이 없습니다." });
            return Ok(result);
        } catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 타임라인 뷰 - 현재 페이지 이전 모든 한줄평 목록
    [HttpGet("timeline")]
    public async Task<IActionResult> GetTimeline([FromQuery] ReviewQueryDto query)
    {
        try {
            var result = await _reviewService.GetTimeline(query);
            return Ok(result);
        } catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}
