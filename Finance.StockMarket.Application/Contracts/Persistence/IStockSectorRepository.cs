using Finance.StockMarket.Domain;

namespace Finance.StockMarket.Application.Contracts.Persistence
{
    public interface IStockSectorRepository : IGenericRepository<StockSector>
    {
        Task<bool> IsUniqueStockSector(string stockSectorName);
    }
}
