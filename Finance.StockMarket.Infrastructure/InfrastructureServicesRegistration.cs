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
using Hangfire.Redis.StackExchange;
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
        //services.AddDbContext<FinanceStockMarketDbContext>(options =>
        //{
        //    options.UseSqlServer(configuration.GetConnectionString("FinanceStockMarket"));
        //});

        var redisConnectionString = configuration.GetConnectionString("Redis");
        var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer); // Add Redis as a singleton service>
        //services.AddStackExchangeRedisCache(options =>
        //{
        //    options.Configuration = configuration.GetConnectionString("Redis");
        //    options.InstanceName = "StockMarket_"; // Optional prefix for cache keys
        //});

        #region Configure Hangfire with Redis as storage
        // 🔹 Configure Hangfire with Redis as storage
        services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseRedisStorage(connectionMultiplexer, new RedisStorageOptions());
        });


        services.AddHangfireServer();

        // Add Hangfire services
        services.AddScoped<IBackgroundJobService, BackgroundJobService>();
        services.AddScoped<IStockPriceUpdateJob, StockPriceUpdateJob>();
        services.AddScoped<JobSchedulerService>(); // Add JobSchedulerService as a scoped service>
        #endregion
        
        // Add SignalR
        services.AddSignalR(options => {
            options.EnableDetailedErrors = true;
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
            options.KeepAliveInterval = TimeSpan.FromSeconds(15);
        });
        
        services.Configure<EmailSettings>(configuration.GetRequiredSection("EmailSettings"));
        services.AddTransient<IEmailSender, EmailSender>(); // Add EmailSender as a transient service
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        services.AddScoped<IRedisCacheService, RedisCacheService>();  //Add RedisCacheService as a scoped service>
        // Register Services following Clean Architecture
        services.AddScoped<ISignalRService, SignalRService.SignalRService>();
        services.AddHostedService<StockPriceBackgroundService>();
        services.AddHttpClient<IYahooFinanceService, YahooFinanceService>();
        services.AddScoped<ISentimentAnalysisService, SentimentAnalysisService>();
        return services;
    }
}
