using Finance.StockMarket.Application.Contracts.Email;
using Finance.StockMarket.Application.Contracts.Logging;
using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Application.Contracts.RedisCache;
using Finance.StockMarket.Application.Contracts.SignalRHub;
using Finance.StockMarket.Application.Contracts.YahooFinance;
using Finance.StockMarket.Application.Models.Email;
using Finance.StockMarket.Domain;
using Finance.StockMarket.Domain.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Finance.StockMarket.Infrastructure.BackgroundJob
{
    public class StockPriceBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public StockPriceBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var redisCacheService = scope.ServiceProvider.GetRequiredService<IRedisCacheService>();
                    var signalRService = scope.ServiceProvider.GetRequiredService<ISignalRService>();
                    var stockQuoteService = scope.ServiceProvider.GetRequiredService<IStockQuoteService>();
                    var alertRepository = scope.ServiceProvider.GetRequiredService<IStockPriceAlertRepository>();
                    var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                    var subscribedStocks = await redisCacheService.GetSubscribedTickers();
                    if (!subscribedStocks.Any())
                    {
                        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                        continue;
                    }

                    var connectionMultiplexer = scope.ServiceProvider.GetService<IConnectionMultiplexer>();

                    foreach (var ticker in subscribedStocks)
                    {
                        try
                        {
                            decimal price = 0;

                            if (connectionMultiplexer is not null && connectionMultiplexer.IsConnected)
                            {
                                var db = connectionMultiplexer.GetDatabase();
                                var stockPriceJson = await db.StringGetAsync($"StockPrice-{ticker}");
                                if (stockPriceJson.HasValue)
                                {
                                    var chartResult = JsonConvert.DeserializeObject<ChartResult>(stockPriceJson);
                                    price = chartResult?.Meta?.RegularMarketPrice ?? 0;
                                }
                            }

                            if (price == 0)
                            {
                                var quote = await stockQuoteService.FetchStockQuoteAsync(ticker);
                                price = quote?.Chart?.Result?.FirstOrDefault()?.Meta?.RegularMarketPrice ?? 0;
                            }

                            if (price > 0)
                            {
                                await signalRService.SendStockPriceUpdate(ticker, price.ToString("F2"));
                                await CheckAndTriggerAlertsAsync(ticker, price, alertRepository, emailSender);
                            }
                        }
                        catch { /* skip this ticker if fetch fails */ }
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var logger = scope.ServiceProvider.GetService<IAppLogger<StockPriceBackgroundService>>();
                    logger?.LogError($"Error in StockPriceBackgroundService: {ex.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
            }
        }

        private static async Task CheckAndTriggerAlertsAsync(
            string ticker,
            decimal currentPrice,
            IStockPriceAlertRepository alertRepository,
            IEmailSender emailSender)
        {
            var activeAlerts = await alertRepository.GetActiveAlertsByTickerAsync(ticker);
            foreach (var alert in activeAlerts)
            {
                bool triggered = alert.Condition == AlertCondition.Above
                    ? currentPrice >= alert.TargetPrice
                    : currentPrice <= alert.TargetPrice;

                if (!triggered) continue;

                alert.IsTriggered = true;
                await alertRepository.UpdateAsync(alert);

                var direction = alert.Condition == AlertCondition.Above ? "above" : "below";
                await emailSender.SendEmail(new EmailMessage
                {
                    To = alert.UserEmail,
                    Subject = $"Price Alert Triggered: {alert.Ticker}",
                    Content = $"""
                        <h2>Price Alert Triggered</h2>
                        <p>Your alert for <strong>{alert.Ticker}</strong> has been triggered.</p>
                        <p>The price is now <strong>{currentPrice:F2}</strong>, which is {direction} your target of <strong>{alert.TargetPrice:F2}</strong>.</p>
                        <p>Log in to your StockMarket dashboard to review your portfolio.</p>
                        """,
                });
            }
        }
    }
}
