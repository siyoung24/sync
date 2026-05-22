using Microsoft.AspNetCore.Mvc;
using MemoApp.Dtos.Auth;
using MemoApp.Services;

namespace MemoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(SignupDto dto)
    {
        try {
            var result = await _authService.Register(dto);
            return Ok(result);
        } catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try {
            var result = await _authService.Login(dto);
            return Ok(result);
        } catch (UnauthorizedAccessException ex) {
            return Unauthorized(new { message = ex.Message });
        } catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}