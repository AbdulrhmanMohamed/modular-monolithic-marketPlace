namespace Modules.Reviews.Application.Handlers;

using MediatR;
using Modules.Reviews.Application.Commands;
using Modules.Reviews.Application.Results;
using Modules.Reviews.Domain.Interfaces;

public class UpdateReviewHandler : IRequestHandler<UpdateReviewCommand, ReviewResult>
{
    private readonly IReviewRepository _repository;

    public UpdateReviewHandler(IReviewRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReviewResult> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _repository.GetByIdAsync(request.ReviewId);

        if (review is null)
            return ReviewResult.Bad("Review not found");

        review.Rating = request.Rating;
        review.Title = request.Title;
        review.Comment = request.Comment;

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