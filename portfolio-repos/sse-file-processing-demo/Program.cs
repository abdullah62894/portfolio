using SseFileProcessingDemo.Application;
using SseFileProcessingDemo.Processing;
using SseFileProcessingDemo.Progress;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IProgressHub, InMemoryProgressHub>();
builder.Services.AddSingleton<IFileProcessingQueue, InMemoryFileProcessingQueue>();
builder.Services.AddSingleton<IFileProcessingService, CsvFileProcessingService>();
builder.Services.AddHostedService<FileProcessingWorker>();
builder.Services.AddSingleton<IMediator, SimpleMediator>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    service = "SSE File Processing Demo",
    endpoints = new[] { "POST /files", "GET /files/{jobId}/progress", "DELETE /files/{jobId}" }
}));

app.MapPost("/files", async (IFormFile file, IMediator mediator, CancellationToken cancellationToken) =>
{
    if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
    {
        return Results.BadRequest(new { error = "Only CSV files are supported." });
    }

    var command = new UploadCsvFileCommand(file.FileName, file.OpenReadStream());
    var result = await mediator.Send(command, cancellationToken);
    return Results.Accepted($"/files/{result.JobId}/progress", result);
}).DisableAntiforgery();

app.MapGet("/files/{jobId:guid}/progress", async (Guid jobId, IProgressHub progressHub, HttpContext context) =>
{
    context.Response.Headers.ContentType = "text/event-stream";

    await foreach (var update in progressHub.WatchAsync(jobId, context.RequestAborted))
    {
        await context.Response.WriteAsync("event: progress\n", context.RequestAborted);
        await context.Response.WriteAsync($"data: {update.ToJson()}\n\n", context.RequestAborted);
        await context.Response.Body.FlushAsync(context.RequestAborted);
    }
});

app.MapDelete("/files/{jobId:guid}", async (Guid jobId, IProgressHub progressHub) =>
{
    await progressHub.CancelAsync(jobId);
    return Results.Accepted($"/files/{jobId}/progress");
});

app.Run();
