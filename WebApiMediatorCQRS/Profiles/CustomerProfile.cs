using AutoMapper;
using WebApiMediatorCQRS.Commands;
using WebApiMediatorCQRS.Database;

namespace WebApiMediatorCQRS.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customers, GetCustomerByIdCommandResponse>();
    }
}
