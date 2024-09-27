using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApiMediatorCQRS.Database;

namespace WebApiMediatorCQRS.Queries;

public record GetCustomerByIdQuery(string Id) : IRequest<GetCustomerByIdQueryResponse>;

public class GetCustomerByIdQueryValidator : AbstractValidator<GetCustomerByIdQuery>
{
    public GetCustomerByIdQueryValidator()
    {
        RuleFor(x => x.Id).Length(5);
    }
}

public record GetCustomerByIdQueryResponse
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
) : IRequestHandler<GetCustomerByIdQuery, GetCustomerByIdQueryResponse?>
{
    public async Task<GetCustomerByIdQueryResponse?> Handle(
        GetCustomerByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation(
            "Handling {type}, Id: {id}",
            typeof(GetCustomerByIdQuery).Name,
            request.Id
        );

        return await northwindContext
            .Customers.ProjectTo<GetCustomerByIdQueryResponse>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.CustomerId == request.Id, cancellationToken);
    }
}
