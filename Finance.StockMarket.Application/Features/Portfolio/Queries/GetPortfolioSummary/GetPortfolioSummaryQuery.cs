using MediatR;

namespace Finance.StockMarket.Application.Features.Portfolio.Queries.GetPortfolioSummary
{
    public record GetPortfolioSummaryQuery(Guid UserId) : IRequest<PortfolioSummaryDTO>;
}
