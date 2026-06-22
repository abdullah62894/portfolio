namespace ModernApp.Domain;

public sealed class Customer
{
    public Customer(int id, string status)
    {
        Id = id;
        Status = status;
    }

    public int Id { get; }
    public string Status { get; private set; }

    public void Activate()
    {
        if (Status == "Suspended")
        {
            throw new InvalidOperationException("Suspended customers require manual review.");
        }

        Status = "Active";
    }
}
