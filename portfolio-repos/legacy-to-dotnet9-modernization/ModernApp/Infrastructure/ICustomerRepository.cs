using ModernApp.Domain;

namespace ModernApp.Infrastructure;

public interface ICustomerRepository
{
    Task<Customer?> GetAsync(int customerId, CancellationToken cancellationToken = default);
    Task SaveAsync(Customer customer, CancellationToken cancellationToken = default);
}
