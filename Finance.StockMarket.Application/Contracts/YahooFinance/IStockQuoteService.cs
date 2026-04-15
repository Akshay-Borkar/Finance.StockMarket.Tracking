using Finance.StockMarket.Domain.Common;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Contracts.YahooFinance
{
    public interface IStockQuoteService
    {
        Task<StockApiResponse?> FetchStockQuoteAsync(string ticker);
    }
}
