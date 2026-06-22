using LegacyApp.Repositories;

namespace LegacyApp.Services;

public sealed class CustomerService(CustomerRepository repository)
{
    public bool ActivateCustomer(int customerId)
    {
        var currentStatus = repository.GetStatus(customerId);

        if (currentStatus is null)
        {
            return false;
        }

        if (currentStatus == "Suspended")
        {
            throw new InvalidOperationException("Suspended customers require manual review.");
        }

        repository.SaveStatus(customerId, "Active");
        return true;
    }
}
