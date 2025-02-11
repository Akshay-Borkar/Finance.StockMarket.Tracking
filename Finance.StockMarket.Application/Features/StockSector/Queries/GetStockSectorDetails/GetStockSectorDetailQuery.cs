using MediatR;

namespace Finance.StockMarket.Application.Features.StockSector.Queries.GetStockSectorDetails
{
    public record GetStockSectorDetailQuery(int Id): IRequest<StockSectorDetailDTO>;
}
