namespace Finance.StockMarket.Application.Features.StockSector.Queries.GetStockSectorDetails
{
    public class StockSectorDetailDTO
    {
        public int Id { get; set; }
        public string StockSectorName { get; set; } = string.Empty;
        public double? SectorPE { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
