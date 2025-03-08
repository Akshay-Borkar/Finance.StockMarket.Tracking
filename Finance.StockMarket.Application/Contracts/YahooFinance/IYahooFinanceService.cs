using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Contracts.YahooFinance
{
    public interface IYahooFinanceService
    {
        Task<List<string>> FetchLatestStockNews(string ticker);
    }
}
