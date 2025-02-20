using Finance.StockMarket.Application.Contracts.RedisCache;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Finance.StockMarket.Infrastructure.RedisCache;

public class RedisCacheService : IRedisCacheService
{
    private readonly IConnectionMultiplexer _cache;
    private readonly JsonSerializerSettings _serializerSettings;
    private readonly IDatabase _db;

    public RedisCacheService(IConnectionMultiplexer cache)
    {
        _cache = cache;
        _serializerSettings = new JsonSerializerSettings 
        { 
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore, // Ignore missing fields
            DateFormatHandling = DateFormatHandling.IsoDateFormat, // Format dates correctly
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        _db = _cache.GetDatabase();
    }

    public async Task<T?> GetCacheAsync<T>(string key)
    {
        var jsonData = await _db.StringGetAsync(key);
        return jsonData.HasValue ? JsonConvert.DeserializeObject<T>(jsonData, _serializerSettings) : default;
    }

    public async Task RemoveCacheAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }

    public async Task SetCacheAsync<T>(string key, T data, int expirationTime)
    {
        //var options = new DistributedCacheEntryOptions
        //{
        //    SlidingExpiration = TimeSpan.FromMinutes(3), // Extends expiration on each access
        //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationTime)
        //};

        var jsonData = JsonConvert.SerializeObject(data, _serializerSettings);

        await _db.StringSetAsync(key, jsonData, TimeSpan.FromMinutes(expirationTime));
    }

    public async Task<List<string>> GetSubscribedTickers()
    {
        string key = $"user:{"testUser"}:tickers";
        var tickers = await _db.SetMembersAsync(key);
        return tickers.Select(t => t.ToString()).ToList();
    }

    public async Task AddTickerAsync(string userId, string ticker)
    {
        string key = $"user:{userId ?? "testUser"}:tickers";
        await _db.SetAddAsync(key, ticker);
    }

    public async Task<bool> IsTickerSubscribedAsync(string userId, string ticker)
    {
        string key = $"user:{userId  ?? "testUser"}:tickers";
        return await _db.SetContainsAsync(key, ticker);
    }

}
