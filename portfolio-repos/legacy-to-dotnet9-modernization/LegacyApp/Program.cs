using LegacyApp.Repositories;
using LegacyApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<CustomerRepository>();
builder.Services.AddSingleton<CustomerService>();

var app = builder.Build();

app.MapControllers();
app.MapPost("/legacy/customers/{id:int}/activate", (int id, CustomerService service) =>
{
    var result = service.ActivateCustomer(id);
    return result ? Results.Ok(new { customerId = id, status = "Activated" }) : Results.NotFound();
});

app.Run();
