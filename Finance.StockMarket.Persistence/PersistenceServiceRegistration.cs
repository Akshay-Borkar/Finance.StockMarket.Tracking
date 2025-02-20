using Finance.StockMarket.Application.Contracts.Logging;
using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Domain.DatabaseContext;
using Finance.StockMarket.Infrastructure.Logging;
using Finance.StockMarket.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<FinanceStockMarketDatabaseContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("FinanceStockDatabaseConnectionString"));
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IStockRepository, StockRepository>(); // <Stock>
            services.AddScoped<IStockSectorRepository, StockSectorRepository>(); // <StockSector>
            services.AddScoped<IInvestmentRepository, InvestmentRepository>(); // <Investment>
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            return services; 
        }
    }
}
