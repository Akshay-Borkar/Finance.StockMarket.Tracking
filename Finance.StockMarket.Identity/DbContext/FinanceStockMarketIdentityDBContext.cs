using Finance.StockMarket.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Identity.DbContext
{
    public class FinanceStockMarketIdentityDBContext : IdentityDbContext<ApplicationUser>
    {
        public FinanceStockMarketIdentityDBContext(DbContextOptions<FinanceStockMarketIdentityDBContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(FinanceStockMarketIdentityDBContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure warnings to throw if there are pending model changes.
            optionsBuilder.ConfigureWarnings(warningConfigurer =>
            {
                warningConfigurer.Log(RelationalEventId.PendingModelChangesWarning);
            });
            base.OnConfiguring(optionsBuilder);
        }
    }
}
