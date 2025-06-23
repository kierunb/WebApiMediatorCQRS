using AutoMapper;
using FluentValidation;
using MediatR;
using Reprise;
using WebApiMediatorCQRS.ApiModels;
using WebApiMediatorCQRS.Commands;

namespace WebApiMediatorCQRS.Endpoints;

[Endpoint]
public class CreateOrderEndpoint
{
    [Post("/orders")]
    [Produces(StatusCodes.Status201Created)]
    [Produces(StatusCodes.Status400BadRequest)]
    [Produces(StatusCodes.Status500InternalServerError)]
    public static async Task<IResult> Handle(
        CreateOrderRequest request,
        IMediator mediator,
        IMapper mapper,
        IValidator<CreateOrderCommand> validator
    )
    {
        try
        {
            var command = mapper.Map<CreateOrderCommand>(request);
            
            // Validate the command
            var validationResult = await validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            var response = await mediator.Send(command);
            var apiResponse = mapper.Map<CreateOrderResponse>(response);
            
            return Results.Created($"/orders/{apiResponse.OrderId}", apiResponse);
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An error occurred while creating the order"
            );
        }
    }
}