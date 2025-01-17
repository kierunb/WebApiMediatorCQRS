using FluentValidation;
using MediatR;
using WebApiMediatorCQRS.Database;

namespace WebApiMediatorCQRS.Commands;

public record CreateOrderCommand : IRequest<CreateOrderCommandResponse>
{
    public string CustomerId { get; set; }
    public int? EmployeeId { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public int? ShipVia { get; set; }
    public decimal? Freight { get; set; }
    public string ShipName { get; set; }
    public string ShipAddress { get; set; }
    public string ShipCity { get; set; }
    public string ShipRegion { get; set; }
    public string ShipPostalCode { get; set; }
    public string ShipCountry { get; set; }
}

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.OrderDate).NotEmpty();
    }
}

public record CreateOrderCommandResponse(int OrderId);

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreateOrderCommandResponse>
{
    private readonly NorthwindContext _context;

    public CreateOrderCommandHandler(NorthwindContext context)
    {
        _context = context;
    }

    public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Orders
        {
            CustomerId = request.CustomerId,
            EmployeeId = request.EmployeeId,
            OrderDate = request.OrderDate,
            RequiredDate = request.RequiredDate,
            ShippedDate = request.ShippedDate,
            ShipVia = request.ShipVia,
            Freight = request.Freight,
            ShipName = request.ShipName,
            ShipAddress = request.ShipAddress,
            ShipCity = request.ShipCity,
            ShipRegion = request.ShipRegion,
            ShipPostalCode = request.ShipPostalCode,
            ShipCountry = request.ShipCountry
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateOrderCommandResponse(order.OrderId);
    }
}
