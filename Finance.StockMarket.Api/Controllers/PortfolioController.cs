using Finance.StockMarket.Application.Features.Portfolio.Commands.AddInvestment;
using Finance.StockMarket.Application.Features.Portfolio.Commands.AddStock;
using Finance.StockMarket.Application.Features.Portfolio.Queries.GetPortfolioSummary;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finance.StockMarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PortfolioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PortfolioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<PortfolioSummaryDTO>> GetSummary()
        {
            var userId = GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var result = await _mediator.Send(new GetPortfolioSummaryQuery(userId));
            return Ok(result);
        }

        [HttpPost("stock")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddStock([FromBody] AddStockCommand command)
        {
            command.UserId = GetUserId();
            if (command.UserId == Guid.Empty)
                return Unauthorized();

            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetSummary), new { }, new { id });
        }

        [HttpPost("investment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddInvestment([FromBody] AddInvestmentCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetSummary), new { }, new { id });
        }

        private Guid GetUserId()
        {
            var claim = User.FindFirst("uid")?.Value;
            return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
        }
    }
}
