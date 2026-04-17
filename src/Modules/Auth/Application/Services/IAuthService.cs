namespace Modules.Auth.Application.Services;

using Modules.Auth.Application.DTOs;

public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto dto);
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto);
}