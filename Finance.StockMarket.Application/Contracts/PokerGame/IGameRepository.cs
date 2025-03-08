using Finance.StockMarket.Domain.PokerGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Contracts.PokerGame
{
    public interface IGameRepository
    {
        Task AddRoom(Room room);
        Task<bool> AddPlayerToRoom(string roomCode, string username);
        Task<bool> PlaceBet(int playerId, decimal amount);
        Task<List<Card>> DealCards(string roomCode);
        Task<string> DetermineWinner(string roomCode);

        Task AddGameAsync(Game game);
        Task<Game> GetGameAsync(Guid gameId);
        Task<bool> UpdateGameAsync(Game game);
    }
}
