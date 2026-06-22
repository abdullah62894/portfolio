# SSE File Processing Demo

Large-file processing platform using ASP.NET Core, a MediatR-style command pipeline, background processing, and Server-Sent Events.

## What it demonstrates

- CSV upload endpoint
- Background processing worker
- Real-time progress updates over `text/event-stream`
- Cancellation endpoint
- Command/handler boundary compatible with MediatR
- Validation before a job enters the processing queue

## Run

```powershell
dotnet run
```

Upload a CSV file:

```powershell
curl -F "file=@sample.csv" https://localhost:5001/files
```

Watch progress:

```powershell
curl https://localhost:5001/files/{jobId}/progress
```

## Production next steps

- Swap `SimpleMediator` for MediatR.
- Store processing jobs in SQL Server or a queue.
- Add row-level validation reports.
- Add integration tests around SSE event ordering and cancellation.
