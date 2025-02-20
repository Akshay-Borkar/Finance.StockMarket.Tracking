using Finance.StockMarket.Application.Contracts.RedisCache;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.SignalRHub
{
    public class StockPriceHub: Hub
    {
        private readonly IRedisCacheService _redisCacheService;

        public StockPriceHub(IRedisCacheService redisCacheService)
        {
            this._redisCacheService = redisCacheService;
        }
        public async Task SubscribeToStock(string stockTicker)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, stockTicker);
            //var isStockAlreadySubscribed = await _redisCacheService.IsTickerSubscribedAsync(Context.UserIdentifier, stockTicker);
            if (!string.IsNullOrEmpty(stockTicker) && !await _redisCacheService.IsTickerSubscribedAsync(Context.UserIdentifier, stockTicker))
                await _redisCacheService.AddTickerAsync(Context.UserIdentifier, stockTicker);   
        }

        public async Task UnsubscribeFromStock(string stockTicker)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, stockTicker);
        }

        public async Task SendStockUpdate(string stockSymbol, decimal price)
        {
            await Clients.All.SendAsync("ReceiveStockPrice", stockSymbol, price);
        }
    }
}
