using Finance.StockMarket.Domain.PokerGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Games.Services
{
    public interface IGameService
    {
        GameResult DetermineWinner(Move player1Move, Move player2Move);
    }
}
