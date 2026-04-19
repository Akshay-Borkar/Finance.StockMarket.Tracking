using Finance.StockMarket.Application.Contracts.YahooFinance;
using Finance.StockMarket.Application.Features.Portfolio.Commands.AddInvestment;
using Finance.StockMarket.Application.Features.Portfolio.Commands.AddStock;
using Finance.StockMarket.Application.Features.Portfolio.Queries.GetInvestmentsByStockId;
using Finance.StockMarket.Application.Features.Portfolio.Queries.GetPortfolioSummary;
using Finance.StockMarket.Domain.Common;
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
        private readonly IStockQuoteService _stockQuoteService;

        public PortfolioController(IMediator mediator, IStockQuoteService stockQuoteService)
        {
            _mediator = mediator;
            _stockQuoteService = stockQuoteService;
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

        [HttpGet("chart/{ticker}")]
        public async Task<ActionResult<List<OhlcvBar>>> GetChart(
            string ticker,
            [FromQuery] string interval = "5m",
            [FromQuery] string range = "1d")
        {
            var bars = await _stockQuoteService.FetchOhlcvAsync(ticker, interval, range);
            return Ok(bars);
        }

        [HttpGet("investments/{stockId:guid}")]
        public async Task<ActionResult<List<InvestmentHistoryDTO>>> GetInvestments(Guid stockId)
        {
            var userId = GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var result = await _mediator.Send(new GetInvestmentsByStockIdQuery(stockId, userId));
            return Ok(result);
        }

        private Guid GetUserId()
        {
            var claim = User.FindFirst("uid")?.Value;
            return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
        }
    }
}
