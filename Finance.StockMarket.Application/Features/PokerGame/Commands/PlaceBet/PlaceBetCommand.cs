using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.PokerGame.Commands.PlaceBet
{
    public record PlaceBetCommand(int PlayerId, decimal Amount) : IRequest<bool>;
}
