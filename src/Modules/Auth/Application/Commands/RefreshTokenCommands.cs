namespace Modules.Auth.Application.Commands;

using MediatR;
using Modules.Auth.Application.Results;

public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResult>
{

}