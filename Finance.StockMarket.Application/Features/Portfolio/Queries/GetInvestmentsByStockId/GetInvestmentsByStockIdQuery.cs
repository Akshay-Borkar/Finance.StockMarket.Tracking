using MediatR;

namespace Finance.StockMarket.Application.Features.Portfolio.Queries.GetInvestmentsByStockId;

public record GetInvestmentsByStockIdQuery(Guid StockId, Guid UserId) : IRequest<List<InvestmentHistoryDTO>>;
