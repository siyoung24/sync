using Microsoft.AspNetCore.Mvc;
using MemoApp.UserBooks.Dtos;
using MemoApp.UserBooks.Services;

namespace MemoApp.UserBooks.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserBookController : ControllerBase
{
    private readonly IUserBookService _userBookService;

    public UserBookController(IUserBookService userBookService)
    {
        _userBookService = userBookService;
    }

    // POST /api/userbook  — 책장에 책 추가
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserBookDto dto)
    {
        try
        {
            var result = await _userBookService.AddToShelf(dto);

            return result.Code switch
            {
                AddUserBookResultCode.Success       => Ok(result.UserBook),
                AddUserBookResultCode.AlreadyExists => Conflict(new { message = "이미 책장에 담긴 책입니다." }),
                AddUserBookResultCode.LimitReached  => BadRequest(new { message = "책장에는 최대 3권까지 담을 수 있습니다." }),
                AddUserBookResultCode.BookNotFound  => NotFound(new { message = "존재하지 않는 책입니다." }),
                AddUserBookResultCode.UserNotFound  => NotFound(new { message = "존재하지 않는 사용자입니다." }),
                _ => StatusCode(500)
            };
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET /api/userbook?userId=1  — 내 책장 목록
    [HttpGet]
    public async Task<IActionResult> GetShelf([FromQuery] int userId)
    {
        try
        {
            var list = await _userBookService.GetShelf(userId);
            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE /api/userbook?userId=1&bookId=2  — 책장에서 빼기
    [HttpDelete]
    public async Task<IActionResult> Remove([FromQuery] int userId, [FromQuery] int bookId)
    {
        try
        {
            var ok = await _userBookService.RemoveFromShelf(userId, bookId);
            if (!ok) return NotFound(new { message = "책장에 없는 책입니다." });
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
