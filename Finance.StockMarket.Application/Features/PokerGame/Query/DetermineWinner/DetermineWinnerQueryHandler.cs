using Finance.StockMarket.Application.Common;
using Finance.StockMarket.Application.Contracts.PokerGame;
using Finance.StockMarket.Application.SignalRHub;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.PokerGame.Query.DetermineWinner
{
    public class DetermineWinnerQueryHandler : IRequestHandler<DetermineWinnerQuery, string>
    {
        private readonly IGameRepository _gameRepository;
        private readonly IHubContext<GameHub> _hubContext;

        public DetermineWinnerQueryHandler(IGameRepository gameRepository, IHubContext<GameHub> hubContext)
        {
            _gameRepository = gameRepository;
            _hubContext = hubContext;
        }
        public async Task<string> Handle(DetermineWinnerQuery request, CancellationToken cancellationToken)
        {
            //var room = await _gameRepository.GetRoomByCode(request.RoomCode);
            string winner = PokerLogic.CalculateWinner(request.room);

            // Broadcast the winner to all players in the room
            await _hubContext.Clients.Group(request.room.RoomCode).SendAsync("ReceiveWinner", winner);

            return winner;
        }
    }
}
