namespace Modules.Reviews.Application.Handlers;

using MediatR;
using Modules.Reviews.Application.Commands;
using Modules.Reviews.Application.Results;
using Modules.Reviews.Domain.Interfaces;

public class ApproveReviewHandler : IRequestHandler<ApproveReviewCommand, ReviewResult>
{
    private readonly IReviewRepository _repository;

    public ApproveReviewHandler(IReviewRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReviewResult> Handle(ApproveReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _repository.GetByIdAsync(request.ReviewId);

        if (review is null)
            return ReviewResult.Bad("Review not found");

        review.IsApproved = true;

        var updated = await _repository.UpdateAsync(review);

        var dto = new ReviewDto(
            updated!.Id,
            updated.ProductId,
            updated.UserId,
            updated.Rating,
            updated.Title,
            updated.Comment,
            updated.IsApproved,
            updated.CreatedAt
        );

        return ReviewResult.Ok(dto);
    }
}