using FluentValidation;
using MediatR;
using WebApiMediatorCQRS.Database;

namespace WebApiMediatorCQRS.Commands;

public record AddOrderCommand : IRequest<AddOrderCommandResponse>
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

public record AddOrderCommandResponse(int OrderId);

public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, AddOrderCommandResponse>
{
    private readonly NorthwindContext _context;

    public AddOrderCommandHandler(NorthwindContext context)
    {
        _context = context;
    }

    public async Task<AddOrderCommandResponse> Handle(AddOrderCommand request, CancellationToken cancellationToken)
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

        return new AddOrderCommandResponse(order.OrderId);
    }
}
