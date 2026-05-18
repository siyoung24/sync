using Microsoft.AspNetCore.Mvc;
using MemoApp.Books.Dtos;
using MemoApp.Books.Services;

namespace MemoApp.Books.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibraryController : ControllerBase
{
    private readonly IBookService _bookService;

    public LibraryController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // 책 목록 조회 API (쿼리스트링으로 검색어 받음)
    [HttpGet]
    public async Task<IActionResult> GetBookList([FromQuery] BookQueryDto query)
    {
        try {
            var result = await _bookService.GetBookList(query);
            return Ok(result);
        } catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}
