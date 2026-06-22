using SseFileProcessingDemo.Progress;

namespace SseFileProcessingDemo.Processing;

public sealed class FileProcessingWorker(
    IFileProcessingQueue queue,
    IFileProcessingService processor,
    IProgressHub progressHub,
    ILogger<FileProcessingWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var job in queue.DequeueAllAsync(stoppingToken))
        {
            try
            {
                await processor.ProcessAsync(job, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                await progressHub.PublishAsync(job.JobId, ProgressUpdate.Cancelled(job.JobId), CancellationToken.None);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "File processing failed for job {JobId}", job.JobId);
                await progressHub.PublishAsync(job.JobId, ProgressUpdate.Failed(job.JobId, ex.Message), CancellationToken.None);
            }
        }
    }
}
