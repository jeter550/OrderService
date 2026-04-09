using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands;
using OrderService.Application.DTOs;
using OrderService.Application.Queries;
namespace OrderService.API.Controllers;

[ApiController]
[Authorize]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderCommand cmd)
    {
        var id = await _mediator.Send(cmd);
        return Ok(id);
    }

    [HttpPost("{id}/confirm")]
    public async Task<IActionResult> Confirm(Guid id)
    {
        await _mediator.Send(new ConfirmOrderCommand(id));
        return Ok();
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _mediator.Send(new CancelOrderCommand(id));
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id));
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ListOrdersRequestDto request)
    {
        var result = await _mediator.Send(new ListOrdersQuery(
            request.CustomerId,
            request.Status,
            request.From,
            request.To,
            request.Page,
            request.PageSize));

        return Ok(result);
    }
}
