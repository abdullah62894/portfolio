using SseFileProcessingDemo.Processing;

namespace SseFileProcessingDemo.Application;

public sealed class SimpleMediator(IFileProcessingQueue queue) : IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return request switch
        {
            UploadCsvFileCommand command when typeof(TResponse) == typeof(UploadCsvFileResult) =>
                Cast<TResponse>(Handle(command, cancellationToken)),
            _ => throw new InvalidOperationException($"No handler registered for {request.GetType().Name}.")
        };
    }

    private async Task<UploadCsvFileResult> Handle(UploadCsvFileCommand command, CancellationToken cancellationToken)
    {
        var jobId = Guid.NewGuid();
        await queue.EnqueueAsync(new FileProcessingJob(jobId, command.FileName, command.Content), cancellationToken);
        return new UploadCsvFileResult(jobId, "Queued");
    }

    private static async Task<TResponse> Cast<TResponse>(Task<UploadCsvFileResult> task)
    {
        var result = await task;
        return (TResponse)(object)result;
    }
}
