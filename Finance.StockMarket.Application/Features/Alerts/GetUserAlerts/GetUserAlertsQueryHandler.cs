using Finance.StockMarket.Application.Contracts.Persistence;
using MediatR;

namespace Finance.StockMarket.Application.Features.Alerts.GetUserAlerts;

public class GetUserAlertsQueryHandler : IRequestHandler<GetUserAlertsQuery, List<AlertDTO>>
{
    private readonly IStockPriceAlertRepository _alertRepository;

    public GetUserAlertsQueryHandler(IStockPriceAlertRepository alertRepository)
    {
        _alertRepository = alertRepository;
    }

    public async Task<List<AlertDTO>> Handle(GetUserAlertsQuery request, CancellationToken cancellationToken)
    {
        var alerts = await _alertRepository.GetAlertsByUserIdAsync(request.UserId);
        return alerts.Select(a => new AlertDTO
        {
            Id = a.Id,
            Ticker = a.Ticker,
            Condition = a.Condition.ToString(),
            TargetPrice = a.TargetPrice,
            IsTriggered = a.IsTriggered,
            DateCreated = a.DateCreated
        }).ToList();
    }
}
