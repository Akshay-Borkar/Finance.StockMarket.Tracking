using Finance.StockMarket.Application.Contracts.PokerGame;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.PokerGame.Commands.PlaceBet
{
    public class PlaceBetCommandHandler : IRequestHandler<PlaceBetCommand, bool>
    {
        private readonly IGameRepository _gameRepository;
        public PlaceBetCommandHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }
        public async Task<bool> Handle(PlaceBetCommand request, CancellationToken cancellationToken)
        {
            return await _gameRepository.PlaceBet(request.PlayerId, request.Amount);
        }
    }
}
