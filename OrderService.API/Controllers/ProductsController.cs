using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands;

namespace OrderService.API.Controllers;

[ApiController]
[Authorize]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand cmd)
    {
        var id = await _mediator.Send(cmd);
        return Ok(id);
    }
}
