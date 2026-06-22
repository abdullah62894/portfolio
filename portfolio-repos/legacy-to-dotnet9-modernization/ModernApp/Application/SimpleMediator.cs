using ModernApp.Application.Customers;
using ModernApp.Infrastructure;

namespace ModernApp.Application;

public sealed class SimpleMediator(ICustomerRepository repository) : IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return request switch
        {
            ActivateCustomerCommand command when typeof(TResponse) == typeof(ActivateCustomerResult) =>
                Cast<TResponse>(Handle(command, cancellationToken)),
            _ => throw new InvalidOperationException($"No handler registered for {request.GetType().Name}.")
        };
    }

    private async Task<ActivateCustomerResult> Handle(ActivateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = await repository.GetAsync(command.CustomerId, cancellationToken);

        if (customer is null)
        {
            return new ActivateCustomerResult(command.CustomerId, false, "NotFound");
        }

        customer.Activate();
        await repository.SaveAsync(customer, cancellationToken);

        return new ActivateCustomerResult(command.CustomerId, true, customer.Status);
    }

    private static async Task<TResponse> Cast<TResponse>(Task<ActivateCustomerResult> task)
    {
        var result = await task;
        return (TResponse)(object)result;
    }
}
