using MediatR;

namespace Finance.StockMarket.Application.Features.Alerts.DeleteAlert;

public record DeleteAlertCommand(Guid AlertId, Guid UserId) : IRequest;
