using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApiMediatorCQRS.Database;

namespace WebApiMediatorCQRS.Commands;

public record CreateOrderCommand : IRequest<CreateOrderCommandResponse>
{
    public string CustomerId { get; set; } = string.Empty;
    public int? EmployeeId { get; set; }
    public DateTime? RequiredDate { get; set; }
    public int? ShipVia { get; set; }
    public decimal? Freight { get; set; }
    public string? ShipName { get; set; }
    public string? ShipAddress { get; set; }
    public string? ShipCity { get; set; }
    public string? ShipRegion { get; set; }
    public string? ShipPostalCode { get; set; }
    public string? ShipCountry { get; set; }
}

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .Length(5)
            .WithMessage("CustomerId must be exactly 5 characters");
            
        RuleFor(x => x.RequiredDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .When(x => x.RequiredDate.HasValue)
            .WithMessage("Required date must be today or in the future");
            
        RuleFor(x => x.Freight)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Freight.HasValue)
            .WithMessage("Freight must be non-negative");
    }
}

public record CreateOrderCommandResponse(
    int OrderId,
    string CustomerId,
    DateTime OrderDate
);

public class CreateOrderCommandHandler(
    ILogger<CreateOrderCommandHandler> logger,
    NorthwindContext northwindContext
) : IRequestHandler<CreateOrderCommand, CreateOrderCommandResponse>
{
    public async Task<CreateOrderCommandResponse> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation(
            "Handling {type}, CustomerId: {customerId}",
            typeof(CreateOrderCommand).Name,
            request.CustomerId
        );

        // Verify customer exists
        var customerExists = await northwindContext.Customers
            .AnyAsync(c => c.CustomerId == request.CustomerId, cancellationToken);
            
        if (!customerExists)
        {
            throw new InvalidOperationException($"Customer with ID '{request.CustomerId}' does not exist");
        }

        // Verify employee exists if provided
        if (request.EmployeeId.HasValue)
        {
            var employeeExists = await northwindContext.Employees
                .AnyAsync(e => e.EmployeeId == request.EmployeeId.Value, cancellationToken);
                
            if (!employeeExists)
            {
                throw new InvalidOperationException($"Employee with ID '{request.EmployeeId}' does not exist");
            }
        }

        // Verify shipper exists if provided
        if (request.ShipVia.HasValue)
        {
            var shipperExists = await northwindContext.Shippers
                .AnyAsync(s => s.ShipperId == request.ShipVia.Value, cancellationToken);
                
            if (!shipperExists)
            {
                throw new InvalidOperationException($"Shipper with ID '{request.ShipVia}' does not exist");
            }
        }

        var order = new Orders
        {
            CustomerId = request.CustomerId,
            EmployeeId = request.EmployeeId,
            OrderDate = DateTime.UtcNow,
            RequiredDate = request.RequiredDate,
            ShipVia = request.ShipVia,
            Freight = request.Freight,
            ShipName = request.ShipName,
            ShipAddress = request.ShipAddress,
            ShipCity = request.ShipCity,
            ShipRegion = request.ShipRegion,
            ShipPostalCode = request.ShipPostalCode,
            ShipCountry = request.ShipCountry
        };

        northwindContext.Orders.Add(order);
        await northwindContext.SaveChangesAsync(cancellationToken);

        return new CreateOrderCommandResponse(
            order.OrderId,
            order.CustomerId,
            order.OrderDate.Value
        );
    }
}