using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApiMediatorCQRS.ApiModels;
using WebApiMediatorCQRS.Commands;

namespace WebApiMediatorCQRS.Controllers;

[ApiController]
[Route("mvc")]
public class NewController(IMediator mediator) : ControllerBase
{
    [HttpPost("new")]
    public async Task<IActionResult> New(NewCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("new-mapper")]
    public async Task<IActionResult> NewMapper(NewRequest request, [FromServices] IMapper mapper)
    {
        var response = await mediator.Send(mapper.Map<NewCommand>(request));
        return Ok(mapper.Map<NewResponse>(response));
    }

    [HttpPost("new-validation")]
    public async Task<IActionResult> NewValidation(
        NewCommand command,
        [FromServices] IValidator<NewCommand> validator
    )
    {
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToDictionary());

        var response = await mediator.Send(command);
        return Ok(response);
    }
}
