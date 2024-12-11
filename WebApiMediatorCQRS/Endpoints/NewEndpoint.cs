using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reprise;
using WebApiMediatorCQRS.ApiModels;
using WebApiMediatorCQRS.Commands;

namespace WebApiMediatorCQRS.Endpoints;

[Endpoint]
public class NewEndpoint
{
    [Post("repr/new")]
    public static async Task<NewCommandResponse> Handle(
        NewRequest request,
        IMediator mediator,
        IMapper mapper
    ) => await mediator.Send(mapper.Map<NewCommand>(request));
}
