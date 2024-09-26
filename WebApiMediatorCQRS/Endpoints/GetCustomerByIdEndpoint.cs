using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Reprise;
using WebApiMediatorCQRS.Commands;

namespace WebApiMediatorCQRS.Endpoints;

[Endpoint]
public class GetCustomerByIdEndpoint
{
    [Get("/customers/{id}")]
    [Produces(StatusCodes.Status200OK)]
    [Produces(StatusCodes.Status404NotFound)]
    [Produces(StatusCodes.Status400BadRequest)]
    [OutputCache]
    public static async Task<IResult> Handle(
        string id,
        IMediator mediator,
        IValidator<GetCustomerByIdCommand> validator
    )
    {
        // manual validation or enable ValidationBehavior
        var validationResult = await validator.ValidateAsync(new GetCustomerByIdCommand(id));
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var response = await mediator.Send(new GetCustomerByIdCommand(id));
        if (response == null)
            return Results.NotFound();

        return TypedResults.Ok(response);
    }
}
