using FluentValidation;

namespace Finance.StockMarket.Application.Features.Alerts.DeleteAlert;

public class DeleteAlertCommandValidator : AbstractValidator<DeleteAlertCommand>
{
    public DeleteAlertCommandValidator()
    {
        RuleFor(x => x.AlertId)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} is required.");

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} is required.");
    }
}
