using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.StockSector.Commands.DeletStockSector
{
    public class DeleteStockSectorCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
