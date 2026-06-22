using System.Security.Claims;
using System.Text;
using EnterpriseAuditLogging.Models;
using EnterpriseAuditLogging.Repositories;
using EnterpriseAuditLogging.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/operations-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddSingleton<IAuditEventStore, InMemoryAuditEventStore>();
builder.Services.AddSingleton<IAuditLogService, AuditLogService>();
builder.Services.AddSingleton<IRetentionPolicyService, RetentionPolicyService>();

var signingKey = builder.Configuration["Jwt:SigningKey"]
    ?? "portfolio-demo-key-change-for-production-32-chars";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => Results.Ok(new
{
    service = "Enterprise Audit Logging",
    endpoints = new[] { "POST /audit/events", "GET /audit/events", "POST /security/login-success", "POST /security/login-failure" }
}));

app.MapPost("/audit/events", async (
    AuditEventRequest request,
    ClaimsPrincipal user,
    IAuditLogService auditLogService,
    HttpContext httpContext) =>
{
    var actor = user.Identity?.Name ?? request.Actor;
    var sourceIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? request.SourceIp;

    var auditEvent = await auditLogService.WriteAsync(new AuditEvent(
        Id: Guid.NewGuid(),
        TimestampUtc: DateTimeOffset.UtcNow,
        Actor: actor,
        EventName: request.EventName,
        Category: request.Category,
        Resource: request.Resource,
        Outcome: request.Outcome,
        SourceIp: sourceIp,
        Metadata: request.Metadata));

    return Results.Created($"/audit/events/{auditEvent.Id}", auditEvent);
}).RequireAuthorization();

app.MapGet("/audit/events", async (
    string? category,
    string? actor,
    IAuditEventStore store) =>
{
    var events = await store.SearchAsync(category, actor);
    return Results.Ok(events);
}).RequireAuthorization();

app.MapPost("/security/login-success", async (
    LoginAuditRequest request,
    IAuditLogService auditLogService) =>
{
    var auditEvent = await auditLogService.AuthenticationSucceededAsync(request.Username, request.SourceIp);
    return Results.Created($"/audit/events/{auditEvent.Id}", auditEvent);
});

app.MapPost("/security/login-failure", async (
    LoginAuditRequest request,
    IAuditLogService auditLogService) =>
{
    var auditEvent = await auditLogService.AuthenticationFailedAsync(request.Username, request.SourceIp, request.Reason ?? "Invalid credentials");
    return Results.Created($"/audit/events/{auditEvent.Id}", auditEvent);
});

app.MapPost("/admin/retention/run", async (IRetentionPolicyService retentionPolicyService) =>
{
    var result = await retentionPolicyService.ApplyAsync(TimeSpan.FromDays(90));
    return Results.Ok(result);
}).RequireAuthorization();

app.Run();

public sealed record AuditEventRequest(
    string Actor,
    string EventName,
    string Category,
    string Resource,
    string Outcome,
    string SourceIp,
    Dictionary<string, string> Metadata);

public sealed record LoginAuditRequest(string Username, string SourceIp, string? Reason);
