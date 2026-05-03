using FluentValidation;

namespace Finance.StockMarket.Application.Features.Portfolio.Commands.AddStock
{
    public class AddStockCommandValidator : AbstractValidator<AddStockCommand>
    {
        public AddStockCommandValidator()
        {
            RuleFor(p => p.Ticker)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(20).WithMessage("{PropertyName} must not exceed 20 characters.")
                .Matches(@"^[A-Z0-9.]+$").WithMessage("{PropertyName} must contain only uppercase letters, digits, or dots.");

            RuleFor(p => p.StockName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(p => p.StockSectorId)
                .NotEqual(Guid.Empty).WithMessage("{PropertyName} is required.");

            RuleFor(p => p.UserId)
                .NotEqual(Guid.Empty).WithMessage("{PropertyName} is required.");

            RuleFor(p => p.StockPE)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.")
                .When(p => p.StockPE.HasValue);
        }
    }
}
