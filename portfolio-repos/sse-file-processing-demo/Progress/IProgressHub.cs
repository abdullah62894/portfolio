namespace SseFileProcessingDemo.Progress;

public interface IProgressHub
{
    Task PublishAsync(Guid jobId, ProgressUpdate update, CancellationToken cancellationToken = default);
    IAsyncEnumerable<ProgressUpdate> WatchAsync(Guid jobId, CancellationToken cancellationToken = default);
    CancellationToken GetCancellationToken(Guid jobId);
    Task CancelAsync(Guid jobId, CancellationToken cancellationToken = default);
}
