using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Domain;
using Finance.StockMarket.Domain.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Finance.StockMarket.Persistence.Repositories
{
    public class StockRepository : GenericRepository<Stock>, IStockRepository
    {
        public StockRepository(FinanceStockMarketDatabaseContext context) : base(context)
        {
        }

        public async Task<Stock> GetStockByName(string stockName)
        {
            var stock = await _context.Stocks.
                Include(x => x.StockSector).FirstOrDefaultAsync(x => x.StockName == stockName);
            return stock;
        }

        public async Task<List<Stock>> GetStocksBySectorId(Guid sectorId)
        {
            var stocks = await _context.Stocks
                .Include(x => x.StockSector)
                .Where(x => x.StockSector.Id == sectorId)
                .ToListAsync();
            return stocks;
        }

        public async Task<List<Stock>> GetStocksByUserId(Guid userId)
        {
            var stocks = await _context.Stocks.Include(x => x.StockSector).Where(x => x.UserId == userId).ToListAsync();
            return stocks;
        }
    }
}
