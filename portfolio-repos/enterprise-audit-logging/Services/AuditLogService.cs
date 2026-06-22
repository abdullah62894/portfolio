using EnterpriseAuditLogging.Models;
using EnterpriseAuditLogging.Repositories;

namespace EnterpriseAuditLogging.Services;

public sealed class AuditLogService(IAuditEventStore store, ILogger<AuditLogService> logger) : IAuditLogService
{
    public async Task<AuditEvent> WriteAsync(AuditEvent auditEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Audit event {EventName} for {Actor} on {Resource} completed with {Outcome}",
            auditEvent.EventName,
            auditEvent.Actor,
            auditEvent.Resource,
            auditEvent.Outcome);

        await store.SaveAsync(auditEvent, cancellationToken);
        return auditEvent;
    }

    public Task<AuditEvent> AuthenticationSucceededAsync(string username, string sourceIp, CancellationToken cancellationToken = default)
    {
        return WriteAsync(new AuditEvent(
            Id: Guid.NewGuid(),
            TimestampUtc: DateTimeOffset.UtcNow,
            Actor: username,
            EventName: "LoginAttempt",
            Category: "Authentication",
            Resource: "Login",
            Outcome: "Success",
            SourceIp: sourceIp,
            Metadata: new Dictionary<string, string> { ["Policy"] = "JWT" }),
            cancellationToken);
    }

    public Task<AuditEvent> AuthenticationFailedAsync(string username, string sourceIp, string reason, CancellationToken cancellationToken = default)
    {
        return WriteAsync(new AuditEvent(
            Id: Guid.NewGuid(),
            TimestampUtc: DateTimeOffset.UtcNow,
            Actor: username,
            EventName: "FailedLoginAttempt",
            Category: "Authentication",
            Resource: "Login",
            Outcome: "Failure",
            SourceIp: sourceIp,
            Metadata: new Dictionary<string, string> { ["Reason"] = reason }),
            cancellationToken);
    }
}
