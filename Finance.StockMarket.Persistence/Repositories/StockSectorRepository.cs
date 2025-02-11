using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Domain;
using Finance.StockMarket.Domain.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Persistence.Repositories
{
    public class StockSectorRepository : GenericRepository<StockSector>, IStockSectorRepository
    {
        public StockSectorRepository(FinanceStockMarketDatabaseContext context) : base(context)
        {
        }

        public async Task<bool> IsUniqueStockSector(string stockSectorName)
        {
            return await _context.StockSectors.AnyAsync(x => x.StockSectorName != stockSectorName);
        }
    }
}
