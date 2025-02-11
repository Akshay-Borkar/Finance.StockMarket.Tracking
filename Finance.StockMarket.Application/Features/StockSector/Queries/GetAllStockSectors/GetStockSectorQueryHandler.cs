using AutoMapper;
using Finance.StockMarket.Application.Contracts.Logging;
using Finance.StockMarket.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.StockSector.Queries.GetAllStockSectors
{
    public class GetStockSectorQueryHandler : IRequestHandler<GetStockSectorQuery, List<StockSectorDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IStockSectorRepository _stockSectorRepository;
        private readonly IAppLogger<GetStockSectorQueryHandler> _logger;

        public GetStockSectorQueryHandler(IMapper mapper, IStockSectorRepository stockSectorRepository, IAppLogger<GetStockSectorQueryHandler> logger)
        {
            _mapper = mapper;
            _stockSectorRepository = stockSectorRepository;
            _logger = logger;
        }
        public async Task<List<StockSectorDTO>> Handle(GetStockSectorQuery request, CancellationToken cancellationToken)
        {
            // Query the database
            var stockSectors = await _stockSectorRepository.GetAsync();

            // Map to DTO
            var data = _mapper.Map<List<StockSectorDTO>>(stockSectors);
            _logger.LogInformation("Stock Sector List Fetched");
            // return the list of DTO
            return data;
        }
    }
}
