using EnterpriseAuditLogging.Models;
using EnterpriseAuditLogging.Repositories;

namespace EnterpriseAuditLogging.Services;

public sealed class RetentionPolicyService(IAuditEventStore store) : IRetentionPolicyService
{
    public async Task<RetentionResult> ApplyAsync(TimeSpan retentionWindow, CancellationToken cancellationToken = default)
    {
        var cutoffUtc = DateTimeOffset.UtcNow.Subtract(retentionWindow);
        var removed = await store.DeleteOlderThanAsync(cutoffUtc, cancellationToken);

        return new RetentionResult(removed, retentionWindow, DateTimeOffset.UtcNow);
    }
}
