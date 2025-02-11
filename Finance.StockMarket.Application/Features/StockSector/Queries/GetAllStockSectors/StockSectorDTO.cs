namespace Finance.StockMarket.Application.Features.StockSector.Queries.GetAllStockSectors
{
    public class StockSectorDTO
    {
        public int Id { get; set; }
        public string StockSectorName { get; set; } = string.Empty;

        public double? SectorPE { get; set; }
    }
}
