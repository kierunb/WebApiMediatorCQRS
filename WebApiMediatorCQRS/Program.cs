using FluentValidation;
using Reprise;
using WebApiMediatorCQRS.Behaviors;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var domainAssembly = typeof(Program).Assembly;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(domainAssembly);
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// FluentValidation
builder.Services.AddValidatorsFromAssembly(domainAssembly);

// AutoMapper
builder.Services.AddAutoMapper(domainAssembly);

// Reprise
builder.ConfigureServices();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapEndpoints();
app.MapControllers();

app.Run();
