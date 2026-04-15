using Finance.StockMarket.Application.Contracts.Hangfire.StockPriceUpdationJob;
using Finance.StockMarket.Application.Contracts.Logging;
using Finance.StockMarket.Application.Contracts.RedisCache;
using Finance.StockMarket.Application.Contracts.YahooFinance;
using Finance.StockMarket.Domain.Common;

namespace Finance.StockMarket.Infrastructure.HangfireJob
{
    public class StockPriceUpdateJob: IStockPriceUpdateJob
    {
        private readonly int _cacheExpiry = TimeSpan.FromMinutes(5).Minutes;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IStockQuoteService _stockQuoteService;
        private readonly IAppLogger<StockPriceUpdateJob> _logger;
        public StockPriceUpdateJob(
            IRedisCacheService redisCacheService,
            IAppLogger<StockPriceUpdateJob> logger,
            IStockQuoteService stockQuoteService)
        {
            _logger = logger;
            _stockQuoteService = stockQuoteService;
            this._redisCacheService = redisCacheService;
        }

        public async Task UpdateStockPriceAsync()
        {
            var tikers = await _redisCacheService.GetSubscribedTickers();
            foreach (var ticker in tikers)
            {
                await FetchAndStoreStockData(ticker);
            }
        }

        private async Task FetchAndStoreStockData(string ticker)
        {
            try
            {
                var data = await _stockQuoteService.FetchStockQuoteAsync(ticker);

                if (data?.Chart != null && data.Chart.Result.Count > 0)
                {
                    var stock = data.Chart.Result[0];
                    string cacheKey = $"StockPrice-{ticker}";

                    await _redisCacheService.SetCacheAsync(cacheKey, stock, 5);
                    _logger.LogInformation($"Updated stock data for {ticker} in Redis.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching Yahoo Finance data", ex.Message);
            }
        }
    }
}
