using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Application.Contracts.YahooFinance;
using Finance.StockMarket.Application.Exceptions;
using Finance.StockMarket.Domain;
using MediatR;

namespace Finance.StockMarket.Application.Features.Portfolio.Commands.AddStock
{
    public class AddStockCommandHandler : IRequestHandler<AddStockCommand, Guid>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockQuoteService _stockQuoteService;

        public AddStockCommandHandler(IStockRepository stockRepository, IStockQuoteService stockQuoteService)
        {
            _stockRepository = stockRepository;
            _stockQuoteService = stockQuoteService;
        }

        public async Task<Guid> Handle(AddStockCommand request, CancellationToken cancellationToken)
        {
            var validator = new AddStockCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.Errors.Count > 0)
                throw new BadRequestException("Invalid Stock data", validationResult);

            string currentPrice = "0";
            string marketCap = string.Empty;

            try
            {
                var quote = await _stockQuoteService.FetchStockQuoteAsync(request.Ticker);
                var meta = quote?.Chart?.Result?.FirstOrDefault()?.Meta;
                if (meta != null)
                {
                    currentPrice = meta.RegularMarketPrice.ToString("F2");
                }
            }
            catch { /* proceed with default if Yahoo Finance is unavailable */ }

            var stock = new Stock
            {
                Id = Guid.NewGuid(),
                Ticker = request.Ticker.ToUpperInvariant(),
                StockName = request.StockName,
                CurrentPrice = currentPrice,
                MarketCap = marketCap,
                StockPE = request.StockPE,
                UserId = request.UserId,
                StockSectorId = request.StockSectorId
            };

            await _stockRepository.CreateAsync(stock);
            return stock.Id;
        }
    }
}
