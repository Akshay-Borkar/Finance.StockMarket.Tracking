using Finance.StockMarket.Application.Common;
using Finance.StockMarket.Application.Contracts.Persistence;
using MediatR;

namespace Finance.StockMarket.Application.Features.Portfolio.Queries.GetInvestmentsByStockId;

public class GetInvestmentsByStockIdQueryHandler
    : IRequestHandler<GetInvestmentsByStockIdQuery, PagedResult<InvestmentHistoryDTO>>
{
    private readonly IInvestmentRepository _investmentRepository;
    private readonly IStockRepository _stockRepository;

    public GetInvestmentsByStockIdQueryHandler(
        IInvestmentRepository investmentRepository,
        IStockRepository stockRepository)
    {
        _investmentRepository = investmentRepository;
        _stockRepository = stockRepository;
    }

    public async Task<PagedResult<InvestmentHistoryDTO>> Handle(
        GetInvestmentsByStockIdQuery request,
        CancellationToken cancellationToken)
    {
        var stock = await _stockRepository.GetByIdAsync(request.StockId);
        if (stock is null || stock.UserId != request.UserId)
            return new PagedResult<InvestmentHistoryDTO> { Page = request.Page, PageSize = request.PageSize };

        var investments = await _investmentRepository.GetInvestmentsByStockId(request.StockId);

        var ordered = investments.OrderByDescending(i => i.InvestmentDate).ToList();
        var totalCount = ordered.Count;

        var items = ordered
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(i =>
            {
                double invested = (double)i.InvestedAmount;
                double qty = i.BuyingPrice > 0 ? invested / i.BuyingPrice : 0;
                return new InvestmentHistoryDTO
                {
                    Id = i.Id,
                    InvestedAmount = Math.Round(invested, 2),
                    BuyingPrice = Math.Round(i.BuyingPrice, 2),
                    Quantity = Math.Round(qty, 4),
                    InvestmentDate = i.InvestmentDate
                };
            })
            .ToList();

        return new PagedResult<InvestmentHistoryDTO>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
        };
    }
}
