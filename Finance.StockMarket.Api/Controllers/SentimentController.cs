using Finance.StockMarket.Application.Contracts.SentimentAnalysis;
using Finance.StockMarket.Application.Contracts.YahooFinance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Finance.StockMarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SentimentController : ControllerBase
    {
        private readonly ISentimentAnalysisService _sentimentService;
        private readonly IYahooFinanceService _yahooFinanceService;

        public SentimentController(ISentimentAnalysisService sentimentService, IYahooFinanceService yahooFinanceService)
        {
            _sentimentService = sentimentService;
            _yahooFinanceService = yahooFinanceService;
        }

        [HttpGet("analyze-yahoo-news/{ticker}")]
        public async Task<IActionResult> AnalyzeYahooFinanceNews(string ticker)
        {
            var newsArticles = await _yahooFinanceService.FetchLatestStockNews(ticker);
            List<object> results = new();

            foreach (var article in newsArticles)
            {
                results.Add(new { Article = article, Sentiment = _sentimentService.PredictSentiment(article) });
            }

            return Ok(results);
        }
    }
}
