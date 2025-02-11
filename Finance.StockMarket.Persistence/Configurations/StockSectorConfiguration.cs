using Finance.StockMarket.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Persistence.Configurations
{
    public class StockSectorConfiguration : IEntityTypeConfiguration<StockSector>
    {
        public void Configure(EntityTypeBuilder<StockSector> builder)
        {
            builder.HasData(
                new StockSector
                {
                    Id = 1,
                    StockSectorName = "IT",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                },
                new StockSector
                {
                    Id = 2,
                    StockSectorName = "Health",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                }
            );

            builder.Property(p => p.StockSectorName).IsRequired().HasMaxLength(50);
        }
    }
}
