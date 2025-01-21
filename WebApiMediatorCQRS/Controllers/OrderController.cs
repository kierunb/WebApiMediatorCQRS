using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApiMediatorCQRS.Commands;
using WebApiMediatorCQRS.Database;

namespace WebApiMediatorCQRS.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OrderController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Orders order)
    {
        var command = _mapper.Map<AddOrderCommand>(order);
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
