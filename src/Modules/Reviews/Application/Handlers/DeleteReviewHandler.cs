namespace Modules.Reviews.Application.Handlers;

using MediatR;
using Modules.Reviews.Application.Commands;
using Modules.Reviews.Domain.Interfaces;

public class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand, bool>
{
    private readonly IReviewRepository _repository;

    public DeleteReviewHandler(IReviewRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.ReviewId);
    }
}