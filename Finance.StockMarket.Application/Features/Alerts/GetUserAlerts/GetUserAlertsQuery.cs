using Finance.StockMarket.Application.Common;
using MediatR;

namespace Finance.StockMarket.Application.Features.Alerts.GetUserAlerts;

public record GetUserAlertsQuery(Guid UserId, int Page = 1, int PageSize = 10) : IRequest<PagedResult<AlertDTO>>;
