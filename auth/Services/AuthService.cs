using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MemoApp.Data;
using MemoApp.Data.Entities;
using MemoApp.Dtos.Auth;
using BC = BCrypt.Net.BCrypt;

namespace MemoApp.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<AuthResponse> Register(SignupDto dto)
    {
        if (dto.Password != dto.PasswordConfirm)
            throw new BadHttpRequestException("비밀번호 확인이 일치하지 않습니다.");

        var existing = await _db.Users.AnyAsync(u => u.Email == dto.Email);
        if (existing)
            throw new InvalidOperationException("이미 가입된 이메일입니다.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BC.HashPassword(dto.Password, 10)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return BuildAuthResponse(user);
    }

    public async Task<AuthResponse> Login(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        
        if (user == null || !BC.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("이메일 또는 비밀번호가 올바르지 않습니다.");

        return BuildAuthResponse(user);
    }

    private AuthResponse BuildAuthResponse(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"] ?? "default_secret_key_32_chars_long_!!");
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim("sub", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return new AuthResponse
        {
            User = new UserDto { Id = user.Id, Name = user.Name, Email = user.Email, CreatedAt = user.CreatedAt },
            AccessToken = tokenHandler.WriteToken(token)
        };
    }
}