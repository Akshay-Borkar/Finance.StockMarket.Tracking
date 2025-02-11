using Finance.StockMarket.Domain;

namespace Finance.StockMarket.Application.Contracts.Persistence
{
    public interface IInvestmentRepository : IGenericRepository<Investment>
    {
        Task<List<Investment>> GetInvestmentsByStockId(Guid stockId);
        Task<List<Investment>> GetInvestmentsByUserId(Guid userId);
        Task<Investment> GetInvestmentDetails(Guid investmentId);
        Task<List<Investment>> GetInvestmentsByDate(DateTime startDate, DateTime endDate);
    }
}
