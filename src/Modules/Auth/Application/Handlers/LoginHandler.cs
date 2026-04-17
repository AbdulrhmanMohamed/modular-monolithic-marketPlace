namespace Modules.Auth.Application.Handlers;

using MediatR;
using Modules.Auth.Application.Commands;
using Modules.Auth.Application.Results;
using Modules.Auth.Application.Services;
using Modules.Auth.Domain.Interfaces;
using Host.Configuration;

public class LoginHandler(IUserRepository _userRepository, ITokenService _tokenService, JwtSettings _jwtSettings) : IRequestHandler<LoginCommand, AuthResult>
{
    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null)
            return AuthResult.Bad("Invalid username or password");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return AuthResult.Bad("Invalid username or password");

        var refreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);
        await _userRepository.UpdateAsync(user);

        var token = _tokenService.GenerateToken(user);

        var authDto = new AuthDto(
            user.Id,
            user.Username,
            user.Email,
            user.Role,
            token,
            refreshToken
        );

        return AuthResult.Ok(authDto);
    }
}