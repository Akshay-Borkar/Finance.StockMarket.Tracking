using Finance.StockMarket.Application.Features.PokerGame.Commands.CreateRoom;
using Finance.StockMarket.Application.Features.PokerGame.Commands.JoinRoom;
using Finance.StockMarket.Application.Features.PokerGame.Commands.PlaceBet;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Finance.StockMarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;
        public GameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-room")]
        public async Task<IActionResult> CreateRoom([FromBody] string username)
        {
            var room = await _mediator.Send(new CreateRoomCommand(username));
            return Ok(room);
        }

        [HttpPost("join-room")]
        public async Task<IActionResult> JoinRoom([FromBody] JoinRoomCommand request)
        {
            var result = await _mediator.Send(request);
            return result ? Ok("Joined successfully") : BadRequest("Failed to join");
        }

        [HttpPost("place-bet")]
        public async Task<IActionResult> PlaceBet([FromBody] PlaceBetCommand request)
        {
            var result = await _mediator.Send(request);
            return result ? Ok("Bet placed successfully") : BadRequest("Insufficient balance");
        }
    }
}
