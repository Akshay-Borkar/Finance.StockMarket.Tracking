using Finance.StockMarket.Application.Common;
using Finance.StockMarket.Application.Contracts.Persistence;
using MediatR;

namespace Finance.StockMarket.Application.Features.Alerts.GetUserAlerts;

public class GetUserAlertsQueryHandler : IRequestHandler<GetUserAlertsQuery, PagedResult<AlertDTO>>
{
    private readonly IStockPriceAlertRepository _alertRepository;

    public GetUserAlertsQueryHandler(IStockPriceAlertRepository alertRepository)
    {
        _alertRepository = alertRepository;
    }

    public async Task<PagedResult<AlertDTO>> Handle(GetUserAlertsQuery request, CancellationToken cancellationToken)
    {
        var (alerts, totalCount) = await _alertRepository.GetAlertsByUserIdPagedAsync(
            request.UserId, request.Page, request.PageSize);

        var items = alerts.Select(a => new AlertDTO
        {
            Id = a.Id,
            Ticker = a.Ticker,
            Condition = a.Condition.ToString(),
            TargetPrice = a.TargetPrice,
            IsTriggered = a.IsTriggered,
            DateCreated = a.DateCreated
        }).ToList();

        return new PagedResult<AlertDTO>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
        };
    }
}
