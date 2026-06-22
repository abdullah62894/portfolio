using EnterpriseAuditLogging.Models;

namespace EnterpriseAuditLogging.Repositories;

public sealed class InMemoryAuditEventStore : IAuditEventStore
{
    private readonly List<AuditEvent> _events = [];
    private readonly object _gate = new();

    public Task SaveAsync(AuditEvent auditEvent, CancellationToken cancellationToken = default)
    {
        lock (_gate)
        {
            _events.Add(auditEvent);
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<AuditEvent>> SearchAsync(string? category, string? actor, CancellationToken cancellationToken = default)
    {
        lock (_gate)
        {
            var results = _events
                .Where(auditEvent => string.IsNullOrWhiteSpace(category) || auditEvent.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .Where(auditEvent => string.IsNullOrWhiteSpace(actor) || auditEvent.Actor.Equals(actor, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(auditEvent => auditEvent.TimestampUtc)
                .ToArray();

            return Task.FromResult<IReadOnlyCollection<AuditEvent>>(results);
        }
    }

    public Task<int> DeleteOlderThanAsync(DateTimeOffset cutoffUtc, CancellationToken cancellationToken = default)
    {
        lock (_gate)
        {
            var removed = _events.RemoveAll(auditEvent => auditEvent.TimestampUtc < cutoffUtc);
            return Task.FromResult(removed);
        }
    }
}
