namespace ModernApp.Application.Customers;

public sealed record ActivateCustomerCommand(int CustomerId) : IRequest<ActivateCustomerResult>;

public sealed record ActivateCustomerResult(int CustomerId, bool Found, string Status);
