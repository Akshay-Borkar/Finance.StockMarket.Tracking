using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.StockSector.Commands.UpdateStockSector
{
    public class UpdateStockSectorCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string StockSectorName { get; set; } = string.Empty;
        public double? SectorPE { get; set; }
    }
}
