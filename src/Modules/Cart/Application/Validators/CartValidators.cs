namespace Modules.Cart.Application.Validators;

using FluentValidation;
using Modules.Cart.Application.Commands;

public class AddToCartValidator : AbstractValidator<AddToCartCommand>
{
    public AddToCartValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("Valid user ID is required");

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Valid product ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be at least 1")
            .LessThanOrEqualTo(99).WithMessage("Quantity cannot exceed 99");
    }
}

public class UpdateCartItemValidator : AbstractValidator<UpdateCartItemCommand>
{
    public UpdateCartItemValidator()
    {
        RuleFor(x => x.CartItemId)
            .GreaterThan(0).WithMessage("Valid cart item ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be at least 1");
    }
}