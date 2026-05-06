using MemoApp.Dtos.Auth;

namespace MemoApp.Services;

public interface IAuthService
{
    Task<AuthResponse> Register(SignupDto dto);
    Task<AuthResponse> Login(LoginDto dto);
}