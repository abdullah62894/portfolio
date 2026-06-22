namespace SseFileProcessingDemo.Processing;

public interface IFileProcessingService
{
    Task ProcessAsync(FileProcessingJob job, CancellationToken cancellationToken);
}
