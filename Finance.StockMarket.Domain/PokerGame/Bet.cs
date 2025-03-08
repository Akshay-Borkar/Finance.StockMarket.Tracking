namespace Finance.StockMarket.Domain.PokerGame
{
    public class Bet
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public decimal Amount { get; set; }
        public string BetType { get; set; } = string.Empty;
    }
}
