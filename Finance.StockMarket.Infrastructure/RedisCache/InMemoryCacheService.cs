using Finance.StockMarket.Application.Contracts.RedisCache;
using Microsoft.Extensions.Caching.Memory;

namespace Finance.StockMarket.Infrastructure.RedisCache;

public class InMemoryCacheService : IRedisCacheService
{
    private readonly IMemoryCache _cache;
    private readonly HashSet<string> _subscribedTickers = [];

    public InMemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task SetCacheAsync<T>(string key, T data, int expirationTime)
    {
        _cache.Set(key, data, TimeSpan.FromMinutes(expirationTime));
        return Task.CompletedTask;
    }

    public Task<T?> GetCacheAsync<T>(string key)
    {
        _cache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    public Task RemoveCacheAsync(string key)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }

    public Task<List<string>> GetSubscribedTickers() =>
        Task.FromResult(_subscribedTickers.ToList());

    public Task AddTickerAsync(string userId, string ticker)
    {
        _subscribedTickers.Add(ticker);
        return Task.CompletedTask;
    }

    public Task<bool> IsTickerSubscribedAsync(string userId, string ticker) =>
        Task.FromResult(_subscribedTickers.Contains(ticker));

    public void StoreSentiment(string key, string sentiment) =>
        _cache.Set(key, sentiment, TimeSpan.FromMinutes(30));

    public string? GetSentiment(string key) =>
        _cache.TryGetValue(key, out string? v) ? v : null;
}
