using MediatR;

namespace Finance.StockMarket.Application.Features.Portfolio.Commands.AddInvestment
{
    public class AddInvestmentCommand : IRequest<Guid>
    {
        public Guid StockId { get; set; }
        public double InvestedAmount { get; set; }
        public double BuyingPrice { get; set; }
        public DateTime InvestmentDate { get; set; }
    }
}
