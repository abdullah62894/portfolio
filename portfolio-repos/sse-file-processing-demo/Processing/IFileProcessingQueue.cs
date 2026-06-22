using System.Threading.Channels;

namespace SseFileProcessingDemo.Processing;

public interface IFileProcessingQueue
{
    ValueTask EnqueueAsync(FileProcessingJob job, CancellationToken cancellationToken);
    IAsyncEnumerable<FileProcessingJob> DequeueAllAsync(CancellationToken cancellationToken);
}

public sealed class InMemoryFileProcessingQueue : IFileProcessingQueue
{
    private readonly Channel<FileProcessingJob> _channel = Channel.CreateUnbounded<FileProcessingJob>();

    public ValueTask EnqueueAsync(FileProcessingJob job, CancellationToken cancellationToken)
    {
        return _channel.Writer.WriteAsync(job, cancellationToken);
    }

    public IAsyncEnumerable<FileProcessingJob> DequeueAllAsync(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}
