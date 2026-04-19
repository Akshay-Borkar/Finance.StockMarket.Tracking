using Finance.StockMarket.Application.Features.Alerts;
using Finance.StockMarket.Application.Features.Alerts.CreateAlert;
using Finance.StockMarket.Application.Features.Alerts.DeleteAlert;
using Finance.StockMarket.Application.Features.Alerts.GetUserAlerts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Finance.StockMarket.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AlertsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AlertsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<AlertDTO>>> GetAlerts()
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();

        var alerts = await _mediator.Send(new GetUserAlertsQuery(userId));
        return Ok(alerts);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateAlert([FromBody] CreateAlertCommand command)
    {
        command.UserId = GetUserId();
        if (command.UserId == Guid.Empty) return Unauthorized();

        // Attach email from JWT claims
        command.UserEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
            ?? User.FindFirst("email")?.Value
            ?? string.Empty;

        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAlerts), new { }, new { id });
    }

    [HttpDelete("{alertId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAlert(Guid alertId)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();

        await _mediator.Send(new DeleteAlertCommand(alertId, userId));
        return NoContent();
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst("uid")?.Value;
        return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
    }
}
