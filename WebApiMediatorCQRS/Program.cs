using FluentValidation;
using Reprise;
using WebApiMediatorCQRS.Behaviors;
using WebApiMediatorCQRS.Database;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults(); // Aspire configuration

var domainAssembly = typeof(Program).Assembly;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// OutputCache
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromSeconds(10)));
    options.AddPolicy("Expire20", builder => builder.Expire(TimeSpan.FromSeconds(20)));
    options.AddPolicy("Expire30", builder => builder.Expire(TimeSpan.FromSeconds(30)));
});

// Entity Framework Core
builder.AddSqlServerDbContext<NorthwindContext>(
    "NorthwindDB",
    static settings => settings.CommandTimeout = 15
);

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(domainAssembly);
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    //cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// FluentValidation
builder.Services.AddValidatorsFromAssembly(domainAssembly);

// AutoMapper
builder.Services.AddAutoMapper(domainAssembly);

// Reprise
builder.ConfigureServices();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
    exceptionHandlerApp.Run(async context => await Results.Problem().ExecuteAsync(context))
);

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseOutputCache();
app.UseAuthorization();
app.MapEndpoints();
app.MapControllers();

app.Run();
