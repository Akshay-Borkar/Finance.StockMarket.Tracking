using Finance.StockMarket.Domain.PokerGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Games.Services
{
    public class GameService: IGameService
    {
        public GameResult DetermineWinner(Move player1Move, Move player2Move)
        {
            if (player1Move == player2Move) return GameResult.Draw;
            return (player1Move, player2Move) switch
            {
                (Move.Rock, Move.Scissors) => GameResult.Win,
                (Move.Paper, Move.Rock) => GameResult.Win,
                (Move.Scissors, Move.Paper) => GameResult.Win,
                _ => GameResult.Lose
            };
        }
    }
}
