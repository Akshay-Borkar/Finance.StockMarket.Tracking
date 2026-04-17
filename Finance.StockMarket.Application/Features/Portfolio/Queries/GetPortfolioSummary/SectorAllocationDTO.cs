namespace Finance.StockMarket.Application.Features.Portfolio.Queries.GetPortfolioSummary
{
    public class SectorAllocationDTO
    {
        public string SectorName { get; set; } = string.Empty;
        public double InvestedAmount { get; set; }
        public double AllocationPercent { get; set; }
    }
}
