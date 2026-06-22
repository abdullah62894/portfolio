using System.Text.Json;

namespace SseFileProcessingDemo.Progress;

public sealed record ProgressUpdate(Guid JobId, string Status, int ProcessedRows, string Message, DateTimeOffset TimestampUtc)
{
    public static ProgressUpdate Started(Guid jobId, string fileName)
        => new(jobId, "Started", 0, $"Started processing {fileName}", DateTimeOffset.UtcNow);

    public static ProgressUpdate Running(Guid jobId, int processedRows, string message)
        => new(jobId, "Running", processedRows, message, DateTimeOffset.UtcNow);

    public static ProgressUpdate Completed(Guid jobId, int processedRows)
        => new(jobId, "Completed", processedRows, "File processing completed", DateTimeOffset.UtcNow);

    public static ProgressUpdate Failed(Guid jobId, string reason)
        => new(jobId, "Failed", 0, reason, DateTimeOffset.UtcNow);

    public static ProgressUpdate Cancelled(Guid jobId)
        => new(jobId, "Cancelled", 0, "File processing cancelled", DateTimeOffset.UtcNow);

    public string ToJson() => JsonSerializer.Serialize(this);
}
