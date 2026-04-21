namespace Modules.Address.Application.Handlers;

using MediatR;
using Modules.Address.Application.Commands;
using Modules.Address.Domain.Interfaces;

public class DeleteAddressHandler : IRequestHandler<DeleteAddressCommand, bool>
{
    private readonly IAddressRepository _repository;

    public DeleteAddressHandler(IAddressRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.AddressId);
    }
}