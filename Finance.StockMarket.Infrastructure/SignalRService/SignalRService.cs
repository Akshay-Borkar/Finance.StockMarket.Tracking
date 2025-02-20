using Finance.StockMarket.Application.Contracts.SignalRHub;
using Finance.StockMarket.Application.SignalRHub;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Infrastructure.SignalRService
{
    public class SignalRService : ISignalRService
    {
        private readonly IHubContext<StockPriceHub> _hubContext;

        public SignalRService(IHubContext<StockPriceHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendStockPriceUpdate(string ticker, string price)
        {
            await _hubContext.Clients.Group(ticker).SendAsync("ReceiveStockPrice", ticker, price);
        }
    }
}
