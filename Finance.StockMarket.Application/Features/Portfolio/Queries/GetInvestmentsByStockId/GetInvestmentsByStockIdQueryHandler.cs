using Finance.StockMarket.Application.Contracts.Persistence;
using MediatR;

namespace Finance.StockMarket.Application.Features.Portfolio.Queries.GetInvestmentsByStockId;

public class GetInvestmentsByStockIdQueryHandler
    : IRequestHandler<GetInvestmentsByStockIdQuery, List<InvestmentHistoryDTO>>
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

    public async Task<List<InvestmentHistoryDTO>> Handle(
        GetInvestmentsByStockIdQuery request,
        CancellationToken cancellationToken)
    {
        // Verify the stock belongs to the requesting user
        var stock = await _stockRepository.GetByIdAsync(request.StockId);
        if (stock is null || stock.UserId != request.UserId)
            return [];

        var investments = await _investmentRepository.GetInvestmentsByStockId(request.StockId);

        return investments
            .OrderByDescending(i => i.InvestmentDate)
            .Select(i =>
            {
                double.TryParse(i.InvestedAmount, out double invested);
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
    }
}
