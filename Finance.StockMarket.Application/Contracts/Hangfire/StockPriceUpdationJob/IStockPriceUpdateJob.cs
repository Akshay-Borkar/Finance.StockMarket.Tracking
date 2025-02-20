using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Contracts.Hangfire.StockPriceUpdationJob
{
    public interface IStockPriceUpdateJob
    {
        Task UpdateStockPriceAsync();
    }
}
