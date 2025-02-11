using Finance.StockMarket.Domain.Common;

namespace Finance.StockMarket.Domain;

public class Stock: BaseEntity
{
    public string Ticker { get; set; } = string.Empty;

    public string StockName { get; set; } = string.Empty;

    public string CurrentPrice { get; set; } = string.Empty;

    public string MarketCap { get; set; } = string.Empty;

    public double? StockPE { get; set; }

    public Guid UserId { get; set; }

    public StockSector StockSector { get; set; }
}
