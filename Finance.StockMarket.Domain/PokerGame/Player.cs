using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Domain.PokerGame
{
    public class Player
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public decimal WalletBalance { get; set; }
        public List<Card> Hand { get; set; } = new();
    }
}
