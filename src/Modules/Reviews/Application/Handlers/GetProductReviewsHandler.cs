namespace Modules.Reviews.Application.Handlers;

using MediatR;
using Modules.Reviews.Application.Queries;
using Modules.Reviews.Application.Results;
using Modules.Reviews.Domain.Interfaces;

public class GetProductReviewsHandler : IRequestHandler<GetProductReviewsQuery, ReviewListResult>
{
    private readonly IReviewRepository _repository;

    public GetProductReviewsHandler(IReviewRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReviewListResult> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
    {
        var reviews = await _repository.GetByProductIdAsync(request.ProductId);

        var dtos = reviews.Select(r => new ReviewDto(
            r.Id,
            r.ProductId,
            r.UserId,
            r.Rating,
            r.Title,
            r.Comment,
            r.IsApproved,
            r.CreatedAt
        )).ToList();

        return ReviewListResult.Ok(dtos);
    }
}