namespace Modules.Auth.Application.Services;

using Modules.Auth.Domain.Entities;

public interface ITokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}