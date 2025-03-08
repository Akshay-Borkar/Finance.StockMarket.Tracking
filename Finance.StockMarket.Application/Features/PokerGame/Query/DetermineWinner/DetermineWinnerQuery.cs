using Finance.StockMarket.Domain.PokerGame;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.PokerGame.Query.DetermineWinner
{
    public record DetermineWinnerQuery(Room room) : IRequest<string>;
}
