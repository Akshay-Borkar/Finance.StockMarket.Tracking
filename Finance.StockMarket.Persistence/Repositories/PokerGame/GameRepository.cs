using Finance.StockMarket.Application.Contracts.PokerGame;
using Finance.StockMarket.Domain.DatabaseContext;
using Finance.StockMarket.Domain.PokerGame;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Persistence.Repositories.PokerGame
{
    public class GameRepository : IGameRepository
    {
        private readonly FinanceStockMarketDatabaseContext _context;
        public GameRepository(FinanceStockMarketDatabaseContext context)
        {
            _context = context;
        }

        public async Task AddRoom(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddPlayerToRoom(string roomCode, string username)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomCode == roomCode);
            if (room != null)
            {
                room.Players.Add(new Player { Username = username });
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> PlaceBet(int playerId, decimal amount)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player != null && player.WalletBalance >= amount)
            {
                player.WalletBalance -= amount;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        Task<List<Card>> IGameRepository.DealCards(string roomCode)
        {
            throw new NotImplementedException();
        }

        Task<string> IGameRepository.DetermineWinner(string roomCode)
        {
            throw new NotImplementedException();
        }

        public async Task AddGameAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
        }

        public Task<Game> GetGameAsync(Guid gameId)
        {
            var game = _context.Games.FirstOrDefault(g => g.Id == gameId);
            return Task.FromResult(game);
        }

        public Task<bool> UpdateGameAsync(Game game)
        {
            var existingGame = _context.Games.FirstOrDefault(g => g.Id == game.Id);
            if (existingGame != null)
            {
                _context.Entry(existingGame).CurrentValues.SetValues(game);
                _context.SaveChanges();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
