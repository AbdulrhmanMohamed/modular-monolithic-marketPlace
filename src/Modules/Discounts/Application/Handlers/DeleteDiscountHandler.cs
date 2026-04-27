namespace Modules.Discounts.Application.Handlers;

using MediatR;
using Modules.Discounts.Application.Commands;
using Modules.Discounts.Domain.Interfaces;

public class DeleteDiscountHandler : IRequestHandler<DeleteDiscountCommand, bool>
{
    private readonly IDiscountRepository _repository;

    public DeleteDiscountHandler(IDiscountRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.DiscountId);
    }
}