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

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Orders order)
    {
        var command = new AddOrderCommand
        {
            CustomerId = order.CustomerId,
            EmployeeId = order.EmployeeId,
            OrderDate = order.OrderDate,
            RequiredDate = order.RequiredDate,
            ShippedDate = order.ShippedDate,
            ShipVia = order.ShipVia,
            Freight = order.Freight,
            ShipName = order.ShipName,
            ShipAddress = order.ShipAddress,
            ShipCity = order.ShipCity,
            ShipRegion = order.ShipRegion,
            ShipPostalCode = order.ShipPostalCode,
            ShipCountry = order.ShipCountry
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }
}
