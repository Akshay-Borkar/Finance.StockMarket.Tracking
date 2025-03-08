using Finance.StockMarket.Application.Contracts.YahooFinance;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Finance.StockMarket.Infrastructure.YahooFinance
{
    public class YahooFinanceService: IYahooFinanceService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://feeds.finance.yahoo.com/rss/2.0/headline?s={0}&region=US&lang=en-US";
        public YahooFinanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> FetchLatestStockNews(string tickerSymbol)
        {
            var requestUrl = string.Format(BASE_URL, tickerSymbol);
            var response = await _httpClient.GetStringAsync(requestUrl);

            var xml = XDocument.Parse(response);
            var newsTitles = xml.Descendants("item")
                .Select(item => item.Element("title")?.Value)
                .Where(title => !string.IsNullOrEmpty(title))
                .ToList();

            return newsTitles;
        }
    }
}
