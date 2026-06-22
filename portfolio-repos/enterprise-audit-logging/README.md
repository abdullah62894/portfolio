# Enterprise Audit Logging

Enterprise-grade audit and operational logging demo built with ASP.NET Core.

## What it demonstrates

- Structured operational logging with Serilog
- FedRAMP-style audit event shape: actor, event, resource, outcome, source IP, metadata
- Authentication success and failure events
- Authorization-protected audit search endpoint
- Retention policy service
- File logging plus replaceable database logging boundary

## Portfolio-safe architecture

This is a clean-room demo inspired by real enterprise logging requirements. It does not include employer code, proprietary database schemas, or business-specific namespaces.

## Run

```powershell
dotnet restore
dotnet run
```

## Production next steps

- Replace `InMemoryAuditEventStore` with SQL Server or PostgreSQL.
- Add immutable append-only storage for compliance events.
- Add OpenTelemetry correlation IDs.
- Add integration tests for retention and search policies.
