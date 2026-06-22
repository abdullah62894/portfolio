namespace SseFileProcessingDemo.Application;

public interface IRequest<out TResponse>;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
