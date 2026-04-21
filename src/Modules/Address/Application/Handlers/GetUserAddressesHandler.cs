namespace Modules.Address.Application.Handlers;



public class GetUserAddressesHandler(IAddressRepository repository) : IRequestHandler<GetUserAddressesQuery, AddressListResult>
{
    private readonly IAddressRepository _repository = repository;

    public async Task<AddressListResult> Handle(GetUserAddressesQuery request, CancellationToken cancellationToken)
    {
        var addresses = await _repository.GetByUserIdAsync(request.UserId);

        var dtos = addresses.Select(a => new AddressDto(
            a.Id,
            a.UserId,
            a.Label,
            a.Street,
            a.City,
            a.State,
            a.PostalCode,
            a.Country,
            a.IsDefault
        )).ToList();

        return AddressListResult.Ok(dtos);
    }
}