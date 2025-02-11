using Finance.StockMarket.Domain;

namespace Finance.StockMarket.Application.Contracts.Persistence
{
    public interface IStockRepository : IGenericRepository<Stock>
    {
        Task<Stock> GetStockByName(string stockName);
        
        Task<List<Stock>> GetStocksBySectorId(int sectorId);

        Task<List<Stock>> GetStocksByUserId(Guid userId);
    }
}
