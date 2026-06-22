using ModernApp.Application;
using ModernApp.Application.Customers;
using ModernApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();
builder.Services.AddSingleton<IMediator, SimpleMediator>();

var app = builder.Build();

app.MapPost("/customers/{id:int}/activate", async (int id, IMediator mediator, CancellationToken cancellationToken) =>
{
    var result = await mediator.Send(new ActivateCustomerCommand(id), cancellationToken);
    return result.Found ? Results.Ok(result) : Results.NotFound(result);
});

app.Run();
