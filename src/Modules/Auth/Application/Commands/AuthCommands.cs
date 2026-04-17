namespace Modules.Auth.Application.Commands;

using MediatR;
using Modules.Auth.Application.Results;

public record RegisterCommand(
    string Username,
    string Email,
    string Password
) : IRequest<AuthResult>;

public record LoginCommand(
    string Username,
    string Password
) : IRequest<AuthResult>;