namespace Modules.Address.Application.Handlers;

using MediatR;
using Modules.Address.Application.Commands;
using Modules.Address.Application.Results;
using Modules.Address.Domain.Entities;
using Modules.Address.Domain.Interfaces;

public class CreateAddressHandler : IRequestHandler<CreateAddressCommand, AddressResult>
{
    private readonly IAddressRepository _repository;

    public CreateAddressHandler(IAddressRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddressResult> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = new Address
        {
            UserId = request.UserId,
            Label = request.Label,
            Street = request.Street,
            City = request.City,
            State = request.State,
            PostalCode = request.PostalCode,
            Country = request.Country
        };

        var created = await _repository.CreateAsync(address);

        var dto = new AddressDto(
            created.Id,
            created.UserId,
            created.Label,
            created.Street,
            created.City,
            created.State,
            created.PostalCode,
            created.Country,
            created.IsDefault
        );

        return AddressResult.Ok(dto);
    }
}