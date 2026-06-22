using System.Collections.Concurrent;
using System.Threading.Channels;

namespace SseFileProcessingDemo.Progress;

public sealed class InMemoryProgressHub : IProgressHub
{
    private readonly ConcurrentDictionary<Guid, Channel<ProgressUpdate>> _channels = new();
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _cancellations = new();

    public async Task PublishAsync(Guid jobId, ProgressUpdate update, CancellationToken cancellationToken = default)
    {
        var channel = _channels.GetOrAdd(jobId, _ => Channel.CreateUnbounded<ProgressUpdate>());
        await channel.Writer.WriteAsync(update, cancellationToken);

        if (update.Status is "Completed" or "Failed" or "Cancelled")
        {
            channel.Writer.TryComplete();
            _cancellations.TryRemove(jobId, out _);
        }
    }

    public async IAsyncEnumerable<ProgressUpdate> WatchAsync(Guid jobId, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var channel = _channels.GetOrAdd(jobId, _ => Channel.CreateUnbounded<ProgressUpdate>());

        await foreach (var update in channel.Reader.ReadAllAsync(cancellationToken))
        {
            yield return update;
        }
    }

    public CancellationToken GetCancellationToken(Guid jobId)
    {
        return _cancellations.GetOrAdd(jobId, _ => new CancellationTokenSource()).Token;
    }

    public Task CancelAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        var source = _cancellations.GetOrAdd(jobId, _ => new CancellationTokenSource());
        source.Cancel();
        return PublishAsync(jobId, ProgressUpdate.Cancelled(jobId), cancellationToken);
    }
}
