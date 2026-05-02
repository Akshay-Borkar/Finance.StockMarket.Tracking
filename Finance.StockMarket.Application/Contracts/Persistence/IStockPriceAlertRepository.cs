using Finance.StockMarket.Domain;

namespace Finance.StockMarket.Application.Contracts.Persistence;

public interface IStockPriceAlertRepository : IGenericRepository<StockPriceAlert>
{
    Task<List<StockPriceAlert>> GetActiveAlertsByTickerAsync(string ticker);
    Task<List<StockPriceAlert>> GetAlertsByUserIdAsync(Guid userId);
    Task<(List<StockPriceAlert> Items, int TotalCount)> GetAlertsByUserIdPagedAsync(Guid userId, int page, int pageSize);
}
