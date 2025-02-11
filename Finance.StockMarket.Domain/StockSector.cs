using Finance.StockMarket.Domain.Common;

namespace Finance.StockMarket.Domain;

public class StockSector: BaseEntity
{
    public string StockSectorName { get; set; } = string.Empty;

    public double? SectorPE { get; set; }
}
