namespace Finance.StockMarket.Application.Features.Portfolio.Queries.GetPortfolioSummary
{
    public class PortfolioHoldingDTO
    {
        public Guid StockId { get; set; }
        public string Ticker { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
        public string SectorName { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public double AvgBuyingPrice { get; set; }
        public double CurrentPrice { get; set; }
        public double InvestedAmount { get; set; }
        public double CurrentValue { get; set; }
        public double PnL { get; set; }
        public double PnLPercent { get; set; }
    }
}
