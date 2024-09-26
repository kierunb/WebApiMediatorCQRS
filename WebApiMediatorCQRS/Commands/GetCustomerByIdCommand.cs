using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApiMediatorCQRS.Database;

namespace WebApiMediatorCQRS.Commands;

public record GetCustomerByIdCommand(string Id) : IRequest<GetCustomerByIdCommandResponse>;

public class GetCustomerByIdCommandValidator : AbstractValidator<GetCustomerByIdCommand>
{
    public GetCustomerByIdCommandValidator()
    {
        RuleFor(x => x.Id).Length(5);
    }
}

public record GetCustomerByIdCommandResponse
{
    public string CustomerId { get; set; }

    public string CompanyName { get; set; }

    public string ContactName { get; set; }

    public string ContactTitle { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Region { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string Phone { get; set; }

    public string Fax { get; set; }
}

public class GetCustomerByIdCommandHandler(
    ILogger<GetCustomerByIdCommandHandler> logger,
    IMapper mapper,
    NorthwindContext northwindContext
) : IRequestHandler<GetCustomerByIdCommand, GetCustomerByIdCommandResponse?>
{
    public async Task<GetCustomerByIdCommandResponse?> Handle(
        GetCustomerByIdCommand request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation(
            "Handling {type}, Id: {id}",
            typeof(GetCustomerByIdCommand).Name,
            request.Id
        );

        return await northwindContext
            .Customers.ProjectTo<GetCustomerByIdCommandResponse>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.CustomerId == request.Id, cancellationToken);
    }
}
