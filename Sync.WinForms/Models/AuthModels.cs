namespace Sync.WinForms.Models;

public class LoginRequest
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public class SignupRequest
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string PasswordConfirm { get; set; } = "";
}

public class AuthResponse
{
    public UserDto User { get; set; } = new();
    public string AccessToken { get; set; } = "";
}

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public class ErrorResponse
{
    public string Message { get; set; } = "";
}

public class RegisterRequest
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string PasswordConfirm { get; set; } = "";
}