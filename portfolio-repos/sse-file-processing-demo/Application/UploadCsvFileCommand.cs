namespace SseFileProcessingDemo.Application;

public sealed record UploadCsvFileCommand(string FileName, Stream Content) : IRequest<UploadCsvFileResult>;

public sealed record UploadCsvFileResult(Guid JobId, string Status);
