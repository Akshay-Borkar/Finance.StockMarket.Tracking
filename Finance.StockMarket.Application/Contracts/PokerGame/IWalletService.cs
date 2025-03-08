namespace Finance.StockMarket.Application.Contracts.PokerGame
{
    public interface IWalletService
    {
        Task<decimal> GetBalance(int playerId);
        Task<bool> UpdateBalance(int playerId, decimal amount);
    }
}
