using Finance.StockMarket.Application.Contracts.YahooFinance;
using Finance.StockMarket.Domain.Common;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Finance.StockMarket.Infrastructure.YahooFinance
{
    public class StockQuoteService : IStockQuoteService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string QuoteUrl = "https://query1.finance.yahoo.com/v8/finance/chart/{0}";

        public StockQuoteService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<StockApiResponse?> FetchStockQuoteAsync(string ticker)
        {
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0");

            var response = await client.GetAsync(string.Format(QuoteUrl, ticker));
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StockApiResponse>(content);
        }
    }
}
