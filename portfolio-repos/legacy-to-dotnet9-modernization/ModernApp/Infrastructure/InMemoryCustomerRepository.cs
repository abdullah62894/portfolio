using ModernApp.Domain;

namespace ModernApp.Infrastructure;

public sealed class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly Dictionary<int, Customer> _customers = new()
    {
        [1001] = new Customer(1001, "Pending"),
        [1002] = new Customer(1002, "Active")
    };

    public Task<Customer?> GetAsync(int customerId, CancellationToken cancellationToken = default)
    {
        _customers.TryGetValue(customerId, out var customer);
        return Task.FromResult(customer);
    }

    public Task SaveAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _customers[customer.Id] = customer;
        return Task.CompletedTask;
    }
}
