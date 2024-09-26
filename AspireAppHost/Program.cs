var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.WebApiMediatorCQRS>("webapimediatorcqrs")
    .WithReplicas(2);

builder.Build().Run();
