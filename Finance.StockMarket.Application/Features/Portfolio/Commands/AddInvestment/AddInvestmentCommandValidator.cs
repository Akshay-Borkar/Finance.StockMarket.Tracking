using FluentValidation;

namespace Finance.StockMarket.Application.Features.Portfolio.Commands.AddInvestment
{
    public class AddInvestmentCommandValidator : AbstractValidator<AddInvestmentCommand>
    {
        public AddInvestmentCommandValidator()
        {
            RuleFor(p => p.StockId)
                .NotEqual(Guid.Empty).WithMessage("{PropertyName} is required.");

            RuleFor(p => p.InvestedAmount)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

            RuleFor(p => p.BuyingPrice)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

            RuleFor(p => p.InvestmentDate)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("{PropertyName} cannot be in the future.");
        }
    }
}
