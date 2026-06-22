namespace EnterpriseAuditLogging.Models;

public sealed record RetentionResult(int RemovedCount, TimeSpan RetentionWindow, DateTimeOffset CompletedAtUtc);
