namespace Modules.Auth.Application.Results;

public record AuthResult(
    bool Success,
    AuthDto? Auth,
    string? Error
)
{
    public static AuthResult Ok(AuthDto auth) => new(true, auth, null);
    public static AuthResult Bad(string error) => new(false, null, error);
}

public record AuthDto(
    int UserId,
    string Username,
    string Email,
    string Role,
    string Token,
    string RefreshToken
);