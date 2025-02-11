using Finance.StockMarket.Domain.Common;

namespace Finance.StockMarket.Domain;

public class Investment: BaseEntity
{
    public string InvestedAmount { get; set; } = string.Empty;

    public double BuyingPrice { get; set; }

    public DateTime InvestmentDate { get; set; }

    public Stock StockDetails { get; set; }
}
