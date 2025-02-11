using Finance.StockMarket.Domain.Common;
using Finance.StockMarket.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Finance.StockMarket.Domain.DatabaseContext
{
    public class FinanceStockMarketDatabaseContext: DbContext
    {
        public FinanceStockMarketDatabaseContext(DbContextOptions<FinanceStockMarketDatabaseContext> options) : base(options)
        {
            
        }

        public DbSet<StockSector> StockSectors { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Investment> Investments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // This is to Seed all Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceStockMarketDatabaseContext).Assembly);
            // This is to Seed individual Configurations
            //modelBuilder.ApplyConfiguration(new StockSectorConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified)) {
                entry.Entity.DateModified = DateTime.Now;
                if (entry.State == EntityState.Added) {
                    entry.Entity.DateCreated = DateTime.Now;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
