using EnterpriseAuditLogging.Models;

namespace EnterpriseAuditLogging.Services;

public interface IRetentionPolicyService
{
    Task<RetentionResult> ApplyAsync(TimeSpan retentionWindow, CancellationToken cancellationToken = default);
}
