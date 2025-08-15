using MedicalRecords.Data;
using MedicalRecords.Models;

namespace MedicalRecords.Services;

/// <summary>
/// Provides audit logging services for tracking system activities.
/// </summary>
public class AuditLogService
{
    private readonly IAuditLogRepository _repo;

    /// <summary>
    /// Initializes a new instance of the AuditLogService class.
    /// </summary>
    /// <param name="repo">The audit log repository for data persistence.</param>
    public AuditLogService(IAuditLogRepository repo) { _repo = repo; }

    /// <summary>
    /// Logs an action performed by a user.
    /// </summary>
    /// <param name="username">The username of the user who performed the action.</param>
    /// <param name="action">The description of the action performed.</param>
    /// <param name="success">Indicates whether the action was successful.</param>
    /// <param name="details">Additional details about the action (optional).</param>
    public void Log(string username, string action, bool success, string? details = null)
    {
        _repo.Append(new AuditLog
        {
            TimestampUtc = DateTime.UtcNow,
            Username = username,
            Action = action,
            Success = success,
            Details = details
        });
    }
} 