using Finance.StockMarket.Application.Common;
using MediatR;

namespace Finance.StockMarket.Application.Features.Portfolio.Queries.GetInvestmentsByStockId;

public record GetInvestmentsByStockIdQuery(Guid StockId, Guid UserId, int Page = 1, int PageSize = 10)
    : IRequest<PagedResult<InvestmentHistoryDTO>>;
