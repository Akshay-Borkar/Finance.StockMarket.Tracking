using AutoMapper;
using Finance.StockMarket.Application.Contracts.Logging;
using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.StockSector.Commands.CreateStockSector
{
    public class CreateStockSectorCommandHandler : IRequestHandler<CreateStockSectorCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IStockSectorRepository _stockSectorRepository;
        private readonly IAppLogger<CreateStockSectorCommandHandler> _logger;

        public CreateStockSectorCommandHandler(IMapper mapper, 
                                                IStockSectorRepository stockSectorRepository,
                                                IAppLogger<CreateStockSectorCommandHandler> logger)
        {
            _mapper = mapper;
            _stockSectorRepository = stockSectorRepository;
            _logger = logger;
        }
        public async Task<Guid> Handle(CreateStockSectorCommand request, CancellationToken cancellationToken)
        {
            // Validate incoming data
            var validator = new CreateStockSectorCommandValidator(_stockSectorRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any()) {
                _logger.LogWarning("Validation error in CreateStockSectorCommandHandler {0} - {1}", nameof(CreateStockSectorCommandHandler), request.StockSectorName);
                throw new BadRequestException("Invalid Stock Sector", validationResult);
            }
                
            // Convert to domain entity
            var stockSector = _mapper.Map<Domain.StockSector>(request);
            // Add to database
            await _stockSectorRepository.CreateAsync(stockSector);
            // Return id
            return stockSector.Id;
        }
    }
}
