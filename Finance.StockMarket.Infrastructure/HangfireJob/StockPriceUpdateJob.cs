using Finance.StockMarket.Application.Contracts.Hangfire.StockPriceUpdationJob;
using Finance.StockMarket.Application.Contracts.Logging;
using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Application.Contracts.RedisCache;
using Finance.StockMarket.Domain.Common;
using Newtonsoft.Json;

namespace Finance.StockMarket.Infrastructure.HangfireJob
{
    public class StockPriceUpdateJob: IStockPriceUpdateJob
    {
        private readonly int _cacheExpiry = TimeSpan.FromMinutes(5).Minutes;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IHttpClientFactory _factory;
        private readonly IAppLogger<StockPriceUpdateJob> _logger;
        public StockPriceUpdateJob(
            IRedisCacheService redisCacheService,
            IAppLogger<StockPriceUpdateJob> logger,
            IHttpClientFactory factory)
        {
            _logger = logger;
            _factory = factory;
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
                using var client = _factory.CreateClient();

                string url = $"https://query1.finance.yahoo.com/v8/finance/chart/{ticker}";

                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0");
                var res = await client.GetAsync(url);
                var data = JsonConvert.DeserializeObject<StockApiResponse>(res.Content.ReadAsStringAsync().Result);

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
