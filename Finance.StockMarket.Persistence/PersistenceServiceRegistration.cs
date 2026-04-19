using System;
using Finance.StockMarket.Application.Contracts.Logging;
using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Domain.DatabaseContext;
using Finance.StockMarket.Infrastructure.Logging;
using Finance.StockMarket.Persistence.Repositories;
using Finance.StockMarket.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.StockMarket.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<FinanceStockMarketDatabaseContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("FinanceStockDatabaseConnectionString"),
                    sql => sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null));
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IStockRepository, StockRepository>(); // <Stock>
            services.AddScoped<IStockSectorRepository, StockSectorRepository>(); // <StockSector>
            services.AddScoped<IInvestmentRepository, InvestmentRepository>(); // <Investment>
            services.AddScoped<IStockPriceAlertRepository, StockPriceAlertRepository>(); // <StockPriceAlert>
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            return services; 
        }
    }
}
