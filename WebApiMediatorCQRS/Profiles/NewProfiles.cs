using AutoMapper;
using WebApiMediatorCQRS.ApiModels;
using WebApiMediatorCQRS.Commands;

namespace WebApiMediatorCQRS.Profiles;

public class NewProfiles : Profile
{
    public NewProfiles()
    {
        CreateMap<NewRequest, NewCommand>();
        CreateMap<NewCommandResponse, NewResponse>();
    }
}
