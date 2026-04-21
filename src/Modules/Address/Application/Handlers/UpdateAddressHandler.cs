namespace Modules.Address.Application.Handlers;

using MediatR;
using Modules.Address.Application.Commands;
using Modules.Address.Application.Results;
using Modules.Address.Domain.Interfaces;

public class UpdateAddressHandler(IAddressRepository repository) : IRequestHandler<UpdateAddressCommand, AddressResult>
{
    private readonly IAddressRepository _repository = repository;

    public async Task<AddressResult> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _repository.GetByIdAsync(request.AddressId);

        if (address is null)
            return AddressResult.Bad("Address not found");

        address.Label = request.Label;
        address.Street = request.Street;
        address.City = request.City;
        address.State = request.State;
        address.PostalCode = request.PostalCode;
        address.Country = request.Country;

        var updated = await _repository.UpdateAsync(address);

        var dto = new AddressDto(
            updated!.Id,
            updated.UserId,
            updated.Label,
            updated.Street,
            updated.City,
            updated.State,
            updated.PostalCode,
            updated.Country,
            updated.IsDefault
        );

        return AddressResult.Ok(dto);
    }
}