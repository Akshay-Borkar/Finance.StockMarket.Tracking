using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Domain;
using Finance.StockMarket.Domain.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Finance.StockMarket.Persistence.Repositories
{
    public class InvestmentRepository : GenericRepository<Investment>, IInvestmentRepository
    {
        public InvestmentRepository(FinanceStockMarketDatabaseContext context) : base(context)
        {
        }

        public async Task<Investment> GetInvestmentDetails(int investmentId)
        {
            var investment = await _context.Investments.Include(x => x.StockDetails).FirstOrDefaultAsync(x => x.Id == investmentId);
            return investment;
        }

        public async Task<List<Investment>> GetInvestmentsByDate(DateTime startDate, DateTime endDate)
        {
            var investments = await _context.Investments.Include(x => x.StockDetails).Where(x => x.InvestmentDate >= startDate && x.InvestmentDate <= endDate).ToListAsync();
            return investments;
        }

        public Task<List<Investment>> GetInvestmentsByStockId(int stockId)
        {
            var investments = _context.Investments.Include(x => x.StockDetails).Where(x => x.StockDetails.Id == stockId).ToListAsync();
            return investments;
        }

        public async Task<List<Investment>> GetInvestmentsByUserId(Guid userId)
        {
            var investments = await _context.Investments
                                .Include(x => x.StockDetails)
                                .Where(x => x.StockDetails.UserId == userId).ToListAsync();
            return investments;
        }
    }
}
