namespace Modules.Auth.Infrastructure.Repositories;

using Modules.Auth.Domain.Entities;
using Modules.Auth.Domain.Interfaces;
using Modules.Auth.Infrastructure.Data;

public class UserRepository(AuthDbContext _context) : IUserRepository
{
    public Task<User?> GetByUsernameAsync(string username)
    {
        return Task.FromResult(_context.Users.FirstOrDefault(u => u.Username == username));
    }

    public Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return Task.FromResult(_context.Users.FirstOrDefault(u => u.RefreshToken == refreshToken));
    }

    public Task<User> CreateAsync(User user)
    {
        var maxId = _context.Users.Any() ? _context.Users.Max(u => u.Id) : 0;
        user.Id = maxId + 1;
        _context.Users.Add(user);
        return Task.FromResult(user);
    }

    public Task<User> UpdateAsync(User user)
    {
        var existing = _context.Users.FirstOrDefault(u => u.Id == user.Id);
        if (existing is null)
            return Task.FromResult(user);

        existing.Username = user.Username;
        existing.Email = user.Email;
        existing.Password = user.Password;
        existing.Role = user.Role;
        existing.RefreshToken = user.RefreshToken;
        existing.RefreshTokenExpiry = user.RefreshTokenExpiry;
        existing.UpdatedAt = DateTime.UtcNow;

        return Task.FromResult(existing);
    }

    public Task<bool> ExistsAsync(string username)
    {
        return Task.FromResult(_context.Users.Any(u => u.Username == username));
    }
}