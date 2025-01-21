using FluentValidation;
using WebApiMediatorCQRS.Commands;

namespace WebApiMediatorCQRS.Validators;

public class OrderValidator : AbstractValidator<AddOrderCommand>
{
    public OrderValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
        RuleFor(x => x.OrderDate).NotEmpty().WithMessage("OrderDate is required.");
        RuleFor(x => x.ShipName).NotEmpty().WithMessage("ShipName is required.");
        RuleFor(x => x.ShipAddress).NotEmpty().WithMessage("ShipAddress is required.");
        RuleFor(x => x.ShipCity).NotEmpty().WithMessage("ShipCity is required.");
        RuleFor(x => x.ShipCountry).NotEmpty().WithMessage("ShipCountry is required.");
    }
}
