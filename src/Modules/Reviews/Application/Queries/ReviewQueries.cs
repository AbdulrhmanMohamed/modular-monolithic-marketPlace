namespace Modules.Reviews.Application.Queries;

using MediatR;
using Modules.Reviews.Application.Results;

public record GetProductReviewsQuery(
    int ProductId
) : IRequest<ReviewListResult>;

public record GetReviewByIdQuery(
    int ReviewId
) : IRequest<ReviewResult>;