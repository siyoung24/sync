using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MemoApp.Reviews.Dtos;
using MemoApp.Reviews.Services;

namespace MemoApp.Reviews.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // 한줄평 작성
    [HttpPost]
    public async Task<IActionResult> Create(CreateReviewDto dto)
    {
        try {
            var userId = GetUserId();
            var result = await _reviewService.CreateReview(userId, dto);
            return Ok(result);
        } catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 현재 페이지 이하 + 가까운 순 5개 (요구사항 핵심)
    [HttpGet("near")]
    public async Task<IActionResult> GetNearby([FromQuery] ReviewQueryDto query, [FromQuery] int take = 5)
    {
        try {
            var result = await _reviewService.GetNearbyReviews(query, take);
            return Ok(result);
        } catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

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

    // 내가 쓴 한줄평 모아보기
    [HttpGet("mine")]
    public async Task<IActionResult> GetMine()
    {
        try {
            var userId = GetUserId();
            var result = await _reviewService.GetMyReviews(userId);
            return Ok(result);
        } catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    private int GetUserId()
    {
        var sub = User.FindFirst("sub")?.Value
            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("인증 토큰에 사용자 정보가 없습니다.");
        return int.Parse(sub);
    }
}
