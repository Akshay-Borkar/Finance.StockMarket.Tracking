using AutoMapper;
using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Application.Exceptions;
using MediatR;

namespace Finance.StockMarket.Application.Features.StockSector.Queries.GetStockSectorDetails
{
    public class GetStockSectorDetailQueryHandler: IRequestHandler<GetStockSectorDetailQuery, StockSectorDetailDTO>
    {
        private readonly IMapper _mapper;
        private readonly IStockSectorRepository _stockSectorRepository;

        public GetStockSectorDetailQueryHandler(IMapper mapper, IStockSectorRepository stockSectorRepository) {
            this._mapper = mapper;
            this._stockSectorRepository = stockSectorRepository;
        }

        public async Task<StockSectorDetailDTO> Handle(GetStockSectorDetailQuery request, CancellationToken cancellationToken)
        {
            var stockSector = await _stockSectorRepository.GetByIdAsync(request.Id);

            if (stockSector == null)
            {
                throw new NotFoundException(nameof(StockSector), request.Id);
            }

            return _mapper.Map<StockSectorDetailDTO>(stockSector);
        }
    }
}
