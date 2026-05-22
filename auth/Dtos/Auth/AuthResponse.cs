namespace MemoApp.Dtos.Auth;

public class AuthResponse
{
    public UserDto User { get; set; } = null!;
    public string AccessToken { get; set; } = string.Empty;
}

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}