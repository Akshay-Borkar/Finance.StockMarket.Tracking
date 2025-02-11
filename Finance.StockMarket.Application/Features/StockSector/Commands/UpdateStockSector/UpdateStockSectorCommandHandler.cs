using AutoMapper;
using Finance.StockMarket.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.StockSector.Commands.UpdateStockSector
{
    public class UpdateStockSectorCommandHandler : IRequestHandler<UpdateStockSectorCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IStockSectorRepository _stockSectorRepository;

        public UpdateStockSectorCommandHandler(IMapper mapper, IStockSectorRepository stockSectorRepository)
        {
            this._mapper = mapper;
            this._stockSectorRepository = stockSectorRepository;
        }

        public async Task<Unit> Handle(UpdateStockSectorCommand request, CancellationToken cancellationToken)
        {
            var stockSector = await _stockSectorRepository.GetByIdAsync(request.Id);
            
            await _stockSectorRepository.UpdateAsync(_mapper.Map<Domain.StockSector>(stockSector));
            return Unit.Value;
        }
    }
}
