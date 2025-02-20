using Finance.StockMarket.Application.Contracts.Logging;
using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Application.Contracts.RedisCache;
using Finance.StockMarket.Domain;
using Finance.StockMarket.Domain.Common;
using Finance.StockMarket.Domain.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Finance.StockMarket.Persistence.Repositories
{
    public class StockSectorRepository : GenericRepository<StockSector>, IStockSectorRepository
    {
        private readonly int _cacheExpiry = TimeSpan.FromMinutes(5).Minutes;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IHttpClientFactory _factory;
        private readonly IAppLogger<StockRepository> _logger;

        public StockSectorRepository(
            FinanceStockMarketDatabaseContext context,
            IRedisCacheService redisCacheService,
            IAppLogger<StockRepository> logger,
            IHttpClientFactory factory) : base(context)
        {
            _logger = logger;
            _factory = factory;
            this._redisCacheService = redisCacheService;
        }

        public async Task<bool> IsUniqueStockSector(string stockSectorName)
        {
            return await _context.StockSectors.AnyAsync(x => x.StockSectorName != stockSectorName);
        }

        public new async Task<StockSector> GetByIdAsync(Guid id)
        {
            string cacheKey = $"StockSector-{id}";
            await FetchAndStoreStockData();
            // Step 1: Try to get the item from Redis cache
            var cachedItem = await _redisCacheService.GetCacheAsync<StockSector>(cacheKey);
            if (cachedItem is not null)
            {
                return cachedItem;
            }

            // Step 2: Simulate database fetch (or real DB call in a production app)
            var stockSector = await base.GetByIdAsync(id);

            // Step 3: Cache the item in Redis
            await _redisCacheService.SetCacheAsync(cacheKey, stockSector, _cacheExpiry);

            return stockSector;
        }

        public async Task FetchAndStoreStockData()
        {
            try
            {
                using var client = _factory.CreateClient();

                string symbol = "CDSL.NS"; // Change to your stock ticker
                //string url1 = $"https://query1.finance.yahoo.com/v7/finance/quote?symbols={symbol}";
                string url = $"https://query1.finance.yahoo.com/v8/finance/chart/{symbol}";

                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0");
                var res = await client.GetAsync(url);
                //var response = await _httpClient.GetAsync(url);
                //var response1 = await _httpClient.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject<StockApiResponse>(res.Content.ReadAsStringAsync().Result);

                if (data?.Chart != null && data.Chart.Result.Count > 0)
                {
                    var stock = data.Chart.Result[0];

                    string cacheKey = $"StockPrice-{symbol}";

                    await _redisCacheService.SetCacheAsync(cacheKey, stock, 5);

                    _logger.LogInformation($"Updated stock data for {symbol} in Redis.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching Yahoo Finance data", ex.Message);
            }
        }
    }
}
