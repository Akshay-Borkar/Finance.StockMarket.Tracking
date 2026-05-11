using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Domain;
using MediatR;

namespace Finance.StockMarket.Application.Features.Portfolio.Commands.AddInvestment
{
    public class AddInvestmentCommandHandler : IRequestHandler<AddInvestmentCommand, Guid>
    {
        private readonly IInvestmentRepository _investmentRepository;
        private readonly IStockRepository _stockRepository;

        public AddInvestmentCommandHandler(IInvestmentRepository investmentRepository, IStockRepository stockRepository)
        {
            _investmentRepository = investmentRepository;
            _stockRepository = stockRepository;
        }

        public async Task<Guid> Handle(AddInvestmentCommand request, CancellationToken cancellationToken)
        {
            var stock = await _stockRepository.GetByIdAsync(request.StockId);
            if (stock == null || stock.UserId != request.UserId)
                throw new UnauthorizedAccessException("Stock not found or does not belong to the current user.");

            var investment = new Investment
            {
                Id = Guid.NewGuid(),
                InvestedAmount = request.InvestedAmount,
                BuyingPrice = request.BuyingPrice,
                InvestmentDate = request.InvestmentDate,
                StockDetailsId = request.StockId
            };

            await _investmentRepository.CreateAsync(investment);
            return investment.Id;
        }
    }
}
