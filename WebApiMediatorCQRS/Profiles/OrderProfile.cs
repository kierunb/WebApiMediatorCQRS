using AutoMapper;
using WebApiMediatorCQRS.ApiModels;
using WebApiMediatorCQRS.Commands;
using WebApiMediatorCQRS.Database;

namespace WebApiMediatorCQRS.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderRequest, CreateOrderCommand>();
        CreateMap<CreateOrderCommand, Orders>();
    }
}
