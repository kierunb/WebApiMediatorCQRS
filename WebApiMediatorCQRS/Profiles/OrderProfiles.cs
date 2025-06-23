using AutoMapper;
using WebApiMediatorCQRS.ApiModels;
using WebApiMediatorCQRS.Commands;

namespace WebApiMediatorCQRS.Profiles;

public class OrderProfiles : Profile
{
    public OrderProfiles()
    {
        CreateMap<CreateOrderRequest, CreateOrderCommand>();
        CreateMap<CreateOrderCommandResponse, CreateOrderResponse>();
    }
}