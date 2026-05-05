using FluentValidation;

namespace Finance.StockMarket.Application.Features.Portfolio.Commands.DeleteStock;

public class DeleteStockCommandValidator : AbstractValidator<DeleteStockCommand>
{
    public DeleteStockCommandValidator()
    {
        RuleFor(x => x.StockId)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} is required.");

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} is required.");
    }
}
