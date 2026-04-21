namespace Modules.Address.Application.Handlers;

using MediatR;
using Modules.Address.Application.Commands;
using Modules.Address.Application.Results;
using Modules.Address.Domain.Interfaces;

public class SetDefaultAddressHandler(IAddressRepository repository) : IRequestHandler<SetDefaultAddressCommand, AddressResult>
{
    private readonly IAddressRepository _repository = repository;

    public async Task<AddressResult> Handle(SetDefaultAddressCommand request, CancellationToken cancellationToken)
    {
        await _repository.SetDefaultAsync(request.AddressId, request.UserId);

        var address = await _repository.GetByIdAsync(request.AddressId);

        if (address is null)
            return AddressResult.Bad("Address not found");

        var dto = new AddressDto(
            address.Id,
            address.UserId,
            address.Label,
            address.Street,
            address.City,
            address.State,
            address.PostalCode,
            address.Country,
            address.IsDefault
        );

        return AddressResult.Ok(dto);
    }
}