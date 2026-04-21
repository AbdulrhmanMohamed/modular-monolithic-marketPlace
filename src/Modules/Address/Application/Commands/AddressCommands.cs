namespace Modules.Address.Application.Commands;

using MediatR;
using Modules.Address.Application.Results;

public record CreateAddressCommand(
    int UserId,
    string Label,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country
) : IRequest<AddressResult>;

public record UpdateAddressCommand(
    int AddressId,
    string Label,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country
) : IRequest<AddressResult>;

public record DeleteAddressCommand(
    int AddressId
) : IRequest<bool>;

public record SetDefaultAddressCommand(
    int AddressId,
    int UserId
) : IRequest<AddressResult>;