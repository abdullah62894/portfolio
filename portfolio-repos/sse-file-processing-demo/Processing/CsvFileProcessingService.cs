using SseFileProcessingDemo.Progress;

namespace SseFileProcessingDemo.Processing;

public sealed class CsvFileProcessingService(IProgressHub progressHub) : IFileProcessingService
{
    public async Task ProcessAsync(FileProcessingJob job, CancellationToken cancellationToken)
    {
        using var linkedCancellation = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            progressHub.GetCancellationToken(job.JobId));
        var jobCancellation = linkedCancellation.Token;

        await progressHub.PublishAsync(job.JobId, ProgressUpdate.Started(job.JobId, job.FileName), jobCancellation);

        using var reader = new StreamReader(job.Content);
        var processedRows = 0;

        while (!reader.EndOfStream)
        {
            jobCancellation.ThrowIfCancellationRequested();

            var line = await reader.ReadLineAsync(jobCancellation);
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            processedRows++;

            if (processedRows % 100 == 0)
            {
                await progressHub.PublishAsync(
                    job.JobId,
                    ProgressUpdate.Running(job.JobId, processedRows, $"Processed {processedRows:N0} rows"),
                    jobCancellation);
            }
        }

        await progressHub.PublishAsync(
            job.JobId,
            ProgressUpdate.Completed(job.JobId, processedRows),
            jobCancellation);
    }
}
