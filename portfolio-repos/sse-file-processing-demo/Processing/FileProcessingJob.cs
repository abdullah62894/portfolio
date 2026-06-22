namespace SseFileProcessingDemo.Processing;

public sealed record FileProcessingJob(Guid JobId, string FileName, Stream Content);
