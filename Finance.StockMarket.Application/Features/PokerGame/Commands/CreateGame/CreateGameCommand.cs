using Finance.StockMarket.Application.Contracts.PokerGame;
using Finance.StockMarket.Domain.PokerGame;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.PokerGame.Commands.CreateGame
{
    public class CreateGameCommand : IRequest<Guid>
    {
        public string Player1 { get; set; }
        public Move Player1Move { get; set; }
    }

    public class CreateGameHandler : IRequestHandler<CreateGameCommand, Guid>
    {
        private readonly IGameRepository _gameRepository;

        public CreateGameHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<Guid> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var game = new Game
            {
                Id = Guid.NewGuid(),
                Player1 = request.Player1,
                Player1Move = request.Player1Move
            };

            await _gameRepository.AddGameAsync(game);
            return game.Id;
        }
    }
}
