namespace Modules.Auth.Application.Services;

using Modules.Auth.Application.DTOs;
using Modules.Auth.Domain.Entities;
using Modules.Auth.Domain.Interfaces;
using Microsoft.Extensions.Logging;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, ITokenService tokenService, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto dto)
    {
        _logger.LogInformation("Registering user: {Username}", dto.Username);

        if (await _userRepository.ExistsAsync(dto.Username))
        {
            _logger.LogWarning("User already exists: {Username}", dto.Username);
            return null;
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            Password = hashedPassword,
            Role = "User",
            RefreshToken = refreshToken,
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        var created = await _userRepository.CreateAsync(user);
        var token = _tokenService.GenerateToken(created);

        return new AuthResponseDto
        {
            UserId = created.Id,
            Username = created.Username,
            Email = created.Email,
            Role = created.Role,
            Token = token,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        _logger.LogInformation("Logging in user: {Username}", dto.Username);

        var user = await _userRepository.GetByUsernameAsync(dto.Username);
        if (user is null)
        {
            _logger.LogWarning("User not found: {Username}", dto.Username);
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            _logger.LogWarning("Invalid password for user: {Username}", dto.Username);
            return null;
        }

        var refreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userRepository.UpdateAsync(user);

        var token = _tokenService.GenerateToken(user);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            Token = token,
            RefreshToken = refreshToken
        };
    }
}