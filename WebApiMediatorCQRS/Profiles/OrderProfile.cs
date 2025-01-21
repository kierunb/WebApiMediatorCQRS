using AutoMapper;
using WebApiMediatorCQRS.Commands;
using WebApiMediatorCQRS.Database;

namespace WebApiMediatorCQRS.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<AddOrderCommand, Orders>();
    }
}
