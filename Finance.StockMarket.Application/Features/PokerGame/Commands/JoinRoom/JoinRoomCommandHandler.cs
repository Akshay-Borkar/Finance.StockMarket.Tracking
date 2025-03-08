using Finance.StockMarket.Application.Contracts.PokerGame;
using Finance.StockMarket.Application.Features.PokerGame.Commands.CreateRoom;
using Finance.StockMarket.Domain.PokerGame;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.PokerGame.Commands.JoinRoom
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Room>
    {
        private readonly IGameRepository _gameRepository;
        public CreateRoomCommandHandler(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }
        public async Task<Room> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = new Room { RoomCode = Guid.NewGuid().ToString().Substring(0, 6) };
            await _gameRepository.AddRoom(room);
            return room;
        }
    }
}
