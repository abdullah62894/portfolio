using EnterpriseAuditLogging.Models;

namespace EnterpriseAuditLogging.Repositories;

public interface IAuditEventStore
{
    Task SaveAsync(AuditEvent auditEvent, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AuditEvent>> SearchAsync(string? category, string? actor, CancellationToken cancellationToken = default);
    Task<int> DeleteOlderThanAsync(DateTimeOffset cutoffUtc, CancellationToken cancellationToken = default);
}
