namespace Modules.Auth.Application.Handlers;

using MediatR;
using Modules.Auth.Application.Commands;
using Modules.Auth.Application.Results;
using Modules.Auth.Domain.Entities;
using Modules.Auth.Domain.Interfaces;
using Modules.Auth.Application.Services;
using Host.Configuration;

public class RefreshTokenHandler(IUserRepository _userRepo, ITokenService _tokenService, JwtSettings _jwtSettings) : IRequestHandler<RefreshTokenCommand, AuthResult>
{
    public async Task<AuthResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByRefreshTokenAsync(request.RefreshToken);
        if (user is null)
            return AuthResult.Bad("Invalid refresh token");

        if (user.RefreshTokenExpiry < DateTime.UtcNow)
            return AuthResult.Bad("Refresh token expired");

        var newRefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);
        await _userRepo.UpdateAsync(user);

        var token = _tokenService.GenerateToken(user);

        var auth = new AuthDto(
            user.Id,
            user.Username,
            user.Email,
            user.Role,
            token,
            newRefreshToken
        );

        return AuthResult.Ok(auth);
    }
}
}