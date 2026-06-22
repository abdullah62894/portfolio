namespace EnterpriseAuditLogging.Models;

public sealed record AuditEvent(
    Guid Id,
    DateTimeOffset TimestampUtc,
    string Actor,
    string EventName,
    string Category,
    string Resource,
    string Outcome,
    string SourceIp,
    IReadOnlyDictionary<string, string> Metadata);
