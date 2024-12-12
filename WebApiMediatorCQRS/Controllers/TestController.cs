using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApiMediatorCQRS.Commands;

namespace WebApiMediatorCQRS.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly IMediator _mediator;

    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Post(TestCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
