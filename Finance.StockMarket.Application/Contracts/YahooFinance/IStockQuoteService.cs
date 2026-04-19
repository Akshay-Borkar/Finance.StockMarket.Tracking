using Finance.StockMarket.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Contracts.YahooFinance
{
    public interface IStockQuoteService
    {
        Task<StockApiResponse?> FetchStockQuoteAsync(string ticker);
        Task<List<OhlcvBar>> FetchOhlcvAsync(string ticker, string interval, string range);
    }
}
