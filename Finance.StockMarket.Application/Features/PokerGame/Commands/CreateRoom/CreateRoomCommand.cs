using Finance.StockMarket.Domain.PokerGame;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Features.PokerGame.Commands.CreateRoom
{
    public record CreateRoomCommand(string Username) : IRequest<Room>;
}
