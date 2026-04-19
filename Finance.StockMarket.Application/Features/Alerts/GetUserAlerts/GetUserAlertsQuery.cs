using MediatR;

namespace Finance.StockMarket.Application.Features.Alerts.GetUserAlerts;

public record GetUserAlertsQuery(Guid UserId) : IRequest<List<AlertDTO>>;
