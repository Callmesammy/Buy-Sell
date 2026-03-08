using Application.DTOs.Review;
using FluentValidation;

namespace Application.Validators.Review;

public class CreateReviewValidator : AbstractValidator<CreateReviewRequest>
{
    public CreateReviewValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required")
            .MinimumLength(10).WithMessage("Comment must be at least 10 characters")
            .MaximumLength(2000).WithMessage("Comment cannot exceed 2000 characters");
    }
}
