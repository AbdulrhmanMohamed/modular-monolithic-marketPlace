namespace Modules.Address.Application.Queries;

using MediatR;
using Modules.Address.Application.Results;

public record GetUserAddressesQuery(
    int UserId
) : IRequest<AddressListResult>;

public record GetAddressByIdQuery(
    int AddressId
) : IRequest<AddressResult>;