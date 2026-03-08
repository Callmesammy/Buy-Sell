using Application.DTOs.Product;
using Application.Interfaces;
using FluentValidation;

namespace Application.Validators.Product;

/// <summary>
/// Validator for CreateProductRequest.
/// </summary>
public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Product title is required.")
            .MaximumLength(256)
            .WithMessage("Product title must not exceed 256 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Product description is required.")
            .MaximumLength(4000)
            .WithMessage("Product description must not exceed 4000 characters.");

        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("Product price is required.")
            .GreaterThan(0)
            .WithMessage("Product price must be greater than 0.")
            .Must(BeValidDecimal)
            .WithMessage("Product price must have maximum 2 decimal places.");

        RuleFor(x => x.Stock)
            .NotEmpty()
            .WithMessage("Product stock is required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Product stock must be 0 or greater.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Product category is required.")
            .MustAsync(BeCategoryExists)
            .WithMessage("The selected category does not exist.");
    }

    private static bool BeValidDecimal(decimal price)
    {
        // Check if price has more than 2 decimal places
        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(price)[3])[2];
        return decimalPlaces <= 2;
    }

    private async Task<bool> BeCategoryExists(Guid categoryId, CancellationToken ct)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, ct);
        return category != null;
    }
}
