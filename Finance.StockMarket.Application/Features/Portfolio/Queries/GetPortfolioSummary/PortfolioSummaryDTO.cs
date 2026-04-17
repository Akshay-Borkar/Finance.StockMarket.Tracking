namespace Finance.StockMarket.Application.Features.Portfolio.Queries.GetPortfolioSummary
{
    public class PortfolioSummaryDTO
    {
        public double TotalInvested { get; set; }
        public double CurrentValue { get; set; }
        public double TotalPnL { get; set; }
        public double TotalPnLPercent { get; set; }
        public List<PortfolioHoldingDTO> Holdings { get; set; } = [];
        public List<SectorAllocationDTO> SectorAllocations { get; set; } = [];
    }
}
