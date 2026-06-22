using LegacyApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LegacyApp.Controllers;

[ApiController]
[Route("legacy/customers")]
public sealed class CustomersController(CustomerService service) : ControllerBase
{
    [HttpPost("{id:int}/activate")]
    public IActionResult Activate(int id)
    {
        var result = service.ActivateCustomer(id);
        return result ? Ok(new { customerId = id, status = "Activated" }) : NotFound();
    }
}
