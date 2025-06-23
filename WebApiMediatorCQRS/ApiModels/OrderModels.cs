namespace WebApiMediatorCQRS.ApiModels;

public record CreateOrderRequest(
    string CustomerId,
    int? EmployeeId = null,
    DateTime? RequiredDate = null,
    int? ShipVia = null,
    decimal? Freight = null,
    string? ShipName = null,
    string? ShipAddress = null,
    string? ShipCity = null,
    string? ShipRegion = null,
    string? ShipPostalCode = null,
    string? ShipCountry = null
);

public record CreateOrderResponse(
    int OrderId,
    string CustomerId,
    DateTime OrderDate
);