using Finance.StockMarket.Application.Contracts.Persistence;
using Finance.StockMarket.Application.Exceptions;
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
            var validator = new AddInvestmentCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.Errors.Count > 0)
                throw new BadRequestException("Invalid Investment data", validationResult);

            var investment = new Investment
            {
                Id = Guid.NewGuid(),
                InvestedAmount = request.InvestedAmount.ToString("F2"),
                BuyingPrice = request.BuyingPrice,
                InvestmentDate = request.InvestmentDate,
                StockDetailsId = request.StockId
            };

            await _investmentRepository.CreateAsync(investment);
            return investment.Id;
        }
    }
}
