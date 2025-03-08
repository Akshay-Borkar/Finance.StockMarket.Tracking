using Finance.StockMarket.Application.Contracts.PokerGame;
using Finance.StockMarket.Application.Games.Services;
using Finance.StockMarket.Domain.PokerGame;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.SignalRHub
{
    public class GameHub : Hub
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGameService _gameService;

        public GameHub(IGameRepository gameRepository, IGameService gameService)
        {
            _gameRepository = gameRepository;
            _gameService = gameService;
        }
        public async Task JoinRoom(string roomCode)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
        }

        public async Task LeaveRoom(string roomCode)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomCode);
        }

        public async Task BroadcastWinner(string roomCode, string winner)
        {
            await Clients.Group(roomCode).SendAsync("ReceiveWinner", winner);
        }

        public async Task JoinGame(Guid gameId, string player, Move move)
        {
            var game = await _gameRepository.GetGameAsync(gameId);
            if (game == null) return;

            game.Player2 = player;
            game.Player2Move = move;
            game.Result = _gameService.DetermineWinner(game.Player1Move, game.Player2Move.Value);

            await _gameRepository.UpdateGameAsync(game);
            await Clients.All.SendAsync("GameUpdated", game);
        }
    }
}
