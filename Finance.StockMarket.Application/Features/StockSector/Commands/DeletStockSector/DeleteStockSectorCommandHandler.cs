using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.StockSector.Commands.DeletStockSector
{
    internal class DeleteStockSectorCommandHandler: IRequestHandler<DeleteStockSectorCommand, Unit>
    {
        private readonly IStockSectorRepository _stockSectorRepository;

        public DeleteStockSectorCommandHandler(IStockSectorRepository stockSectorRepository)
        {
            this._stockSectorRepository = stockSectorRepository;
        }

        public async Task<Unit> Handle(DeleteStockSectorCommand request, CancellationToken cancellationToken)
        {
            var stockSectorToDelete = await _stockSectorRepository.GetByIdAsync(request.Id);
            if (stockSectorToDelete == null)
            {
                throw new NotFoundException(nameof(StockSector), request.Id);
            }

            await _stockSectorRepository.DeleteAsync(stockSectorToDelete);

            return Unit.Value;
        }
    }
}
