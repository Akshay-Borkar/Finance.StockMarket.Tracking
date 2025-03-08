using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Domain.PokerGame
{
    public enum Move { Rock, Paper, Scissors }
    public enum GameResult { Win, Lose, Draw }

    public class Game
    {
        public Guid Id { get; set; }
        public string Player1 { get; set; }
        public Move Player1Move { get; set; }
        public string? Player2 { get; set; }
        public Move? Player2Move { get; set; }
        public GameResult? Result { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
