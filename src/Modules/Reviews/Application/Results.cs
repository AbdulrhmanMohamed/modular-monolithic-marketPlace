namespace Modules.Reviews.Application.Results;

public record ReviewResult(
    bool Success,
    ReviewDto? Review,
    string? Error
)
{
    public static ReviewResult Ok(ReviewDto review) => new(true, review, null);
    public static ReviewResult Bad(string error) => new(false, null, error);
};

public record ReviewListResult(
    bool Success,
    List<ReviewDto> Reviews,
    string? Error
)
{
    public static ReviewListResult Ok(List<ReviewDto> reviews) => new(true, reviews, null);
    public static ReviewListResult Bad(string error) => new(false, new List<ReviewDto>(), error);
};

public record ReviewDto(
    int Id,
    int ProductId,
    int UserId,
    int Rating,
    string Title,
    string? Comment,
    bool IsApproved,
    DateTime CreatedAt
);