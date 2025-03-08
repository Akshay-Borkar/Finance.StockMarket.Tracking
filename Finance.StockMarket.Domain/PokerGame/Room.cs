namespace Finance.StockMarket.Domain.PokerGame
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomCode { get; set; } = string.Empty;
        public List<Player> Players { get; set; } = new();
        public decimal Pot { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public List<Card> CommunityCards { get; set; } = new();
    }
}
