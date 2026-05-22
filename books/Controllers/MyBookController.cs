using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MemoApp.Books.Dtos;
using MemoApp.Books.Services;

namespace MemoApp.Books.Controllers;

[ApiController]
[Authorize]
[Route("api/mybooks")]
public class MyBookController : ControllerBase
{
    private readonly IMyBookService _myBookService;

    public MyBookController(IMyBookService myBookService)
    {
        _myBookService = myBookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyBooks()
    {
        try {
            var userId = GetUserId();
            var result = await _myBookService.GetMyBooks(userId);
            return Ok(result);
        } catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddBook(AddMyBookDto dto)
    {
        try {
            var userId = GetUserId();
            var result = await _myBookService.AddBook(userId, dto);
            return Ok(result);
        } catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{bookId:int}")]
    public async Task<IActionResult> RemoveBook(int bookId)
    {
        try {
            var userId = GetUserId();
            await _myBookService.RemoveBook(userId, bookId);
            return NoContent();
        } catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{bookId:int}/page")]
    public async Task<IActionResult> UpdatePage(int bookId, UpdatePageDto dto)
    {
        try {
            var userId = GetUserId();
            var result = await _myBookService.UpdatePage(userId, bookId, dto);
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
