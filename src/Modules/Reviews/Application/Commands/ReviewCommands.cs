namespace Modules.Reviews.Application.Commands;

using MediatR;
using Modules.Reviews.Application.Results;

public record CreateReviewCommand(
    int ProductId,
    int UserId,
    int Rating,
    string Title,
    string? Comment
) : IRequest<ReviewResult>;

public record UpdateReviewCommand(
    int ReviewId,
    int Rating,
    string Title,
    string? Comment
) : IRequest<ReviewResult>;

public record DeleteReviewCommand(
    int ReviewId
) : IRequest<bool>;

public record ApproveReviewCommand(
    int ReviewId
) : IRequest<ReviewResult>;