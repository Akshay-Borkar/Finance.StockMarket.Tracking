using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.StockSector.Queries.GetAllStockSectors
{
    //public class GetStockSectorQueries: IRequest<List<StockSectorDTO>>
    //{
    //}

    public record GetStockSectorQuery: IRequest<List<StockSectorDTO>>;
}
