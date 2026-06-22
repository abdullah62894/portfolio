namespace LegacyApp.Repositories;

public sealed class CustomerRepository
{
    private readonly Dictionary<int, string> _customers = new()
    {
        [1001] = "Pending",
        [1002] = "Active"
    };

    public string? GetStatus(int customerId)
    {
        return _customers.TryGetValue(customerId, out var status) ? status : null;
    }

    public void SaveStatus(int customerId, string status)
    {
        _customers[customerId] = status;
    }
}
