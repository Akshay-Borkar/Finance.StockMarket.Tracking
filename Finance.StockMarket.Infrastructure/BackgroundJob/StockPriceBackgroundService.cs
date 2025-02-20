using Finance.StockMarket.Application.Contracts.Logging;
using Finance.StockMarket.Application.Contracts.RedisCache;
using Finance.StockMarket.Application.Contracts.SignalRHub;
using Finance.StockMarket.Domain.Common;
using Finance.StockMarket.Infrastructure.RedisCache;
using Finance.StockMarket.Infrastructure.SignalRService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Infrastructure.BackgroundJob
{
    public class StockPriceBackgroundService: BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnectionMultiplexer _redis;
        private readonly IAppLogger<StockPriceBackgroundService> _logger;

        public StockPriceBackgroundService(IServiceProvider serviceProvider, IConnectionMultiplexer redis)
        {
            this._serviceProvider = serviceProvider;
            _redis = redis;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var db = _redis.GetDatabase();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var redisCacheService = scope.ServiceProvider.GetRequiredService<IRedisCacheService>();
                    var subscribedStocks = await redisCacheService.GetSubscribedTickers();
                    if (!subscribedStocks.Any())
                    {
                        // Wait for 1 minute before fetching again
                        return;
                    }
                    // Assuming Hangfire stores stock prices under "stock:{ticker}"
                    string[] stockTickers = { "CDSL.NS" };
                    var signalRService = scope.ServiceProvider.GetRequiredService<ISignalRService>();
                    foreach (var ticker in subscribedStocks)
                    {
                        var stockPriceJson = await db.StringGetAsync($"StockPrice-{ticker}");
                        // Fetch stock price from Redis cache
                        ChartResult stockPrice = JsonConvert.DeserializeObject<ChartResult>(stockPriceJson);

                        if (stockPrice?.Meta.RegularMarketPrice > 0)
                        {
                            // Send stock price update via SignalR
                            await signalRService.SendStockPriceUpdate(ticker, stockPrice.Meta.RegularMarketPrice.ToString());
                        }
                    }

                    // Wait for 1 minute before fetching again
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var logger = scope.ServiceProvider.GetRequiredService<IAppLogger<StockPriceBackgroundService>>();
                    logger.LogError($"Error in StockPriceBackgroundService: {ex.Message}");
                }
            }
        }
    }
}
