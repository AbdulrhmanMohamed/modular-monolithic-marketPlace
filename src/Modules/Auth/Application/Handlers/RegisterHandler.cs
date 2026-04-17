namespace Modules.Auth.Application.Handlers;

using MediatR;
using Modules.Auth.Application.Commands;
using Modules.Auth.Application.Results;
using Modules.Auth.Application.Services;
using Modules.Auth.Domain.Entities;
using Modules.Auth.Domain.Interfaces;
using Host.Configuration;

public class RegisterHandler(IUserRepository _userRepository, ITokenService _tokenService, JwtSettings _jwtSettings) : IRequestHandler<RegisterCommand, AuthResult>
{
    public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsAsync(request.Username))
            return AuthResult.Bad("Username is already taken");
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var refreshToken = _tokenService.GenerateRefreshToken();
        
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            Password = passwordHash,
            Role = "User",
            RefreshToken = refreshToken,
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            CreatedAt = DateTime.UtcNow
        };
        
        var created = await _userRepository.CreateAsync(user);
        var token = _tokenService.GenerateToken(created);
        
        var authDto = new AuthDto(
            created.Id,
            created.Username,
            created.Email,
            created.Role,
            token,
            refreshToken
        );
        
        return AuthResult.Ok(authDto);
    }
}