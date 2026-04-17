namespace Modules.Auth.Domain.Interfaces;

using Modules.Auth.Domain.Entities;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByRefreshTokenAsync(string refreshToken);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<bool> ExistsAsync(string username);
}