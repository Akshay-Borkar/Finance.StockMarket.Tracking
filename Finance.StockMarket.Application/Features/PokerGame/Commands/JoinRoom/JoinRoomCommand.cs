using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.PokerGame.Commands.JoinRoom
{
    public record JoinRoomCommand(string RoomCode, string Username) : IRequest<bool>;
}
