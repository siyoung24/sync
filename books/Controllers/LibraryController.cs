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

    // 책 목록 조회 API (우리 DB 기준, 쿼리스트링으로 검색어 받음)
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

    // 알라딘 외부 검색 API (DB에 없는 책 찾을 때, 페이지 수 X)
    [HttpGet("search")]
    public async Task<IActionResult> SearchAladin([FromQuery] string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest(new { message = "검색어를 입력해주세요." });

        try {
            var result = await _bookService.SearchAladin(keyword);
            return Ok(result);
        } catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 알라딘 단건 조회 (ISBN13으로 페이지 수까지 받아옴)
    [HttpGet("lookup")]
    public async Task<IActionResult> LookupAladin([FromQuery] string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            return BadRequest(new { message = "ISBN을 입력해주세요." });

        try {
            var result = await _bookService.LookupAladin(isbn);
            if (result == null)
                return NotFound(new { message = "알라딘에서 해당 책을 찾을 수 없습니다." });
            return Ok(result);
        } catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}
