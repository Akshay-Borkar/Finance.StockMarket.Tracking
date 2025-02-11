using Finance.StockMarket.Application.Contracts.Email;
using Finance.StockMarket.Application.Contracts.Logging;
using Finance.StockMarket.Application.Models.Email;
using Finance.StockMarket.Infrastructure.EmailService;
using Finance.StockMarket.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.StockMarket.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<FinanceStockMarketDbContext>(options =>
            //{
            //    options.UseSqlServer(configuration.GetConnectionString("FinanceStockMarket"));
            //});
            //services.AddScoped<IStockMarketRepository, StockMarketRepository>();
            services.Configure<EmailSettings>(configuration.GetRequiredSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>(); // Add EmailSender as a transient service
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            return services;
        }
    }
}
