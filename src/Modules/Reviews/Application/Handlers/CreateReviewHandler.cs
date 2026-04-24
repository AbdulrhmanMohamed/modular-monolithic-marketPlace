namespace Modules.Reviews.Application.Handlers;

using MediatR;
using Modules.Reviews.Application.Commands;
using Modules.Reviews.Application.Results;
using Modules.Reviews.Domain.Entities;
using Modules.Reviews.Domain.Interfaces;

public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, ReviewResult>
{
    private readonly IReviewRepository _repository;

    public CreateReviewHandler(IReviewRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReviewResult> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        if (request.Rating < 1 || request.Rating > 5)
            return ReviewResult.Bad("Rating must be between 1 and 5");

        var review = new Review
        {
            ProductId = request.ProductId,
            UserId = request.UserId,
            Rating = request.Rating,
            Title = request.Title,
            Comment = request.Comment,
            IsApproved = false
        };

        var created = await _repository.CreateAsync(review);

        var dto = new ReviewDto(
            created.Id,
            created.ProductId,
            created.UserId,
            created.Rating,
            created.Title,
            created.Comment,
            created.IsApproved,
            created.CreatedAt
        );

        return ReviewResult.Ok(dto);
    }
}