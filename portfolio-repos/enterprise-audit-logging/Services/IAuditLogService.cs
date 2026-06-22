using EnterpriseAuditLogging.Models;

namespace EnterpriseAuditLogging.Services;

public interface IAuditLogService
{
    Task<AuditEvent> WriteAsync(AuditEvent auditEvent, CancellationToken cancellationToken = default);
    Task<AuditEvent> AuthenticationSucceededAsync(string username, string sourceIp, CancellationToken cancellationToken = default);
    Task<AuditEvent> AuthenticationFailedAsync(string username, string sourceIp, string reason, CancellationToken cancellationToken = default);
}
