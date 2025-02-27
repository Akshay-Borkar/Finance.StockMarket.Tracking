﻿using Finance.StockMarket.Application.Features.StockSector.Commands.CreateStockSector;
using Finance.StockMarket.Application.Features.StockSector.Commands.DeletStockSector;
using Finance.StockMarket.Application.Features.StockSector.Commands.UpdateStockSector;
using Finance.StockMarket.Application.Features.StockSector.Queries.GetAllStockSectors;
using Finance.StockMarket.Application.Features.StockSector.Queries.GetStockSectorDetails;
using Finance.StockMarket.Application.SignalRHub;
using Finance.StockMarket.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Finance.StockMarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockSectorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<StockPriceHub> _hubContext;

        public StockSectorController(IMediator mediator, IHubContext<StockPriceHub> hubContext)
        {
            this._mediator = mediator;
            this._hubContext = hubContext;
        }
        // GET: api/<StockSectorController>
        [HttpGet]
        public async Task<List<StockSectorDTO>> Get()
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var stockSectors = await _mediator.Send(new GetStockSectorQuery());
            return stockSectors;
        }

        // GET api/<StockSectorController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StockSectorDetailDTO>> Get(Guid id)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var stockSector = await _mediator.Send(new GetStockSectorDetailQuery(id));
            return Ok(stockSector);
        }

        // POST api/<StockSectorController>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Post(CreateStockSectorCommand createStockSectorCommand)
        {
            var res = await _mediator.Send(createStockSectorCommand);
            return CreatedAtAction(nameof(Get), new { id = res });
        }

        // PUT api/<StockSectorController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Put(UpdateStockSectorCommand updateStockSectorCommand)
        {
            await _mediator.Send(updateStockSectorCommand);
            return NoContent();
        }

        // DELETE api/<StockSectorController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteStockSectorCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("update-price")]
        public async Task<IActionResult> UpdateStockPrice([FromBody] StockPriceUpdateModel model)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveStockPrice", model.Symbol, model.Price);
            return Ok("Stock price updated!");
        }
    }

    public class StockPriceUpdateModel
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
    }
}
