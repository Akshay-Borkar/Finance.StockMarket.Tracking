using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Domain;
using MediatR;

namespace Finance.StockMarket.Application.Features.Portfolio.Commands.AddInvestment
{
    public class AddInvestmentCommandHandler : IRequestHandler<AddInvestmentCommand, Guid>
    {
        private readonly IInvestmentRepository _investmentRepository;

        public AddInvestmentCommandHandler(IInvestmentRepository investmentRepository)
        {
            _investmentRepository = investmentRepository;
        }

        public async Task<Guid> Handle(AddInvestmentCommand request, CancellationToken cancellationToken)
        {
            var investment = new Investment
            {
                Id = Guid.NewGuid(),
                InvestedAmount = (decimal)request.InvestedAmount,
                BuyingPrice = request.BuyingPrice,
                InvestmentDate = request.InvestmentDate,
                StockDetailsId = request.StockId
            };

            await _investmentRepository.CreateAsync(investment);
            return investment.Id;
        }
    }
}
