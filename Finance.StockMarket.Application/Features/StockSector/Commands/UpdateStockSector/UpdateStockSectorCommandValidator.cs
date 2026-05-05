using FluentValidation;

namespace Finance.StockMarket.Application.Features.StockSector.Commands.UpdateStockSector
{
    public class UpdateStockSectorCommandValidator : AbstractValidator<UpdateStockSectorCommand>
    {
        public UpdateStockSectorCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEqual(Guid.Empty).WithMessage("{PropertyName} is required.");

            RuleFor(p => p.StockSectorName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.SectorPE)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.")
                .When(p => p.SectorPE.HasValue);
        }
    }
}
