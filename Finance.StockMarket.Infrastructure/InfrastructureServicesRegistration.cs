using Finance.StockMarket.Application.Contracts.Email;
using Finance.StockMarket.Application.Contracts.Hangfire.BackgroundJobService;
using Finance.StockMarket.Application.Contracts.Hangfire.StockPriceUpdationJob;
using Finance.StockMarket.Application.Contracts.Logging;
using Finance.StockMarket.Application.Contracts.RedisCache;
using Finance.StockMarket.Application.Models.Email;
using Finance.StockMarket.Infrastructure.HangfireJob;
using Finance.StockMarket.Infrastructure.EmailService;
using Finance.StockMarket.Infrastructure.Logging;
using Finance.StockMarket.Infrastructure.RedisCache;
using Hangfire;
using Hangfire.InMemory;
using Hangfire.Redis.StackExchange;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Finance.StockMarket.Application.Contracts.SignalRHub;
using Finance.StockMarket.Infrastructure.BackgroundJob;
using Finance.StockMarket.Application.Contracts.YahooFinance;
using Finance.StockMarket.Infrastructure.YahooFinance;
using Finance.StockMarket.Application.Contracts.SentimentAnalysis;
using Finance.StockMarket.Infrastructure.SentimentAnalysis;

namespace Finance.StockMarket.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";

        IConnectionMultiplexer? connectionMultiplexer = null;
        try
        {
            var redisConfig = ConfigurationOptions.Parse(redisConnectionString);
            redisConfig.ConnectTimeout = 2000;
            redisConfig.AbortOnConnectFail = false;
            connectionMultiplexer = ConnectionMultiplexer.Connect(redisConfig);
        }
        catch { /* Redis unavailable — fall back to in-memory */ }

        bool redisAvailable = connectionMultiplexer is not null && connectionMultiplexer.IsConnected;

        #region Configure Hangfire (Redis or InMemory)
        if (redisAvailable)
        {
            services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer!);
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseRedisStorage(connectionMultiplexer!, new RedisStorageOptions()));
            services.AddScoped<IRedisCacheService, RedisCacheService>();
        }
        else
        {
            services.AddMemoryCache();
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseInMemoryStorage());
            services.AddScoped<IRedisCacheService, InMemoryCacheService>();
        }

        services.AddHangfireServer();
        services.AddScoped<IBackgroundJobService, BackgroundJobService>();
        services.AddTransient<IStockPriceUpdateJob, StockPriceUpdateJob>();
        services.AddScoped<JobSchedulerService>();
        #endregion

        // Add SignalR
        services.AddSignalR(options => {
            options.EnableDetailedErrors = true;
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
            options.KeepAliveInterval = TimeSpan.FromSeconds(15);
        });

        services.Configure<EmailSettings>(configuration.GetRequiredSection("EmailSettings"));
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        services.AddScoped<ISignalRService, SignalRService.SignalRService>();
        services.AddHostedService<StockPriceBackgroundService>();
        services.AddTransient<IStockQuoteService, StockQuoteService>();
        services.AddHttpClient<IYahooFinanceService, YahooFinanceService>();
        services.AddScoped<ISentimentAnalysisService, SentimentAnalysisService>();
        return services;
    }
}
