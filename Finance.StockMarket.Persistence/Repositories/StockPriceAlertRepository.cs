using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Domain;
using Finance.StockMarket.Domain.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Finance.StockMarket.Persistence.Repositories;

public class StockPriceAlertRepository : GenericRepository<StockPriceAlert>, IStockPriceAlertRepository
{
    public StockPriceAlertRepository(FinanceStockMarketDatabaseContext context) : base(context) { }

    public async Task<List<StockPriceAlert>> GetActiveAlertsByTickerAsync(string ticker)
        => await _context.StockPriceAlerts
            .Where(a => a.Ticker == ticker && !a.IsTriggered)
            .ToListAsync();

    public async Task<List<StockPriceAlert>> GetAlertsByUserIdAsync(Guid userId)
        => await _context.StockPriceAlerts
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.DateCreated)
            .ToListAsync();
}
