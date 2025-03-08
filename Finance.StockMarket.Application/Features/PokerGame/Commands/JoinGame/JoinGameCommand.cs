using Finance.StockMarket.Application.Contracts.PokerGame;
using Finance.StockMarket.Application.Games.Services;
using Finance.StockMarket.Domain.PokerGame;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.PokerGame.Commands.JoinGame
{
    public class JoinGameCommand : IRequest<GameResult>
    {
        public Guid GameId { get; set; }
        public string Player2 { get; set; }
        public Move Player2Move { get; set; }
    }

    public class JoinGameHandler : IRequestHandler<JoinGameCommand, GameResult>
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGameService _gameService;

        public JoinGameHandler(IGameRepository gameRepository, IGameService gameService)
        {
            _gameRepository = gameRepository;
            _gameService = gameService;
        }

        public async Task<GameResult> Handle(JoinGameCommand request, CancellationToken cancellationToken)
        {
            var game = await _gameRepository.GetGameAsync(request.GameId);

            if (game == null) throw new Exception("Game not found");
            game.Player2 = request.Player2;
            game.Player2Move = request.Player2Move;
            game.Result = _gameService.DetermineWinner(game.Player1Move, game.Player2Move.Value);

            await _gameRepository.UpdateGameAsync(game);
            return game.Result.Value;
        }
    }
}
