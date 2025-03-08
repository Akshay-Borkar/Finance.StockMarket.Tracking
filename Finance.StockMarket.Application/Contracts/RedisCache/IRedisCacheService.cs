namespace Finance.StockMarket.Application.Contracts.RedisCache
{
    public interface IRedisCacheService
    {
        Task SetCacheAsync<T>(string key, T data, int expirationTime);
        Task<T?> GetCacheAsync<T>(string key);
        Task RemoveCacheAsync(string key);

        Task<List<string>> GetSubscribedTickers();

        Task AddTickerAsync(string userId, string ticker);

        Task<bool> IsTickerSubscribedAsync(string userId, string ticker);

        #region Sentiment Analysis
        void StoreSentiment(string key, string sentiment);
        string? GetSentiment(string key);
        #endregion

    }
}
