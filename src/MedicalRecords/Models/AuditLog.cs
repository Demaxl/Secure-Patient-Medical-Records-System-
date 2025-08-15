namespace MedicalRecords.Models;

/// <summary>
/// Represents an audit log entry for tracking system activities.
/// </summary>
public class AuditLog
{
    /// <summary>
    /// Gets or sets the unique identifier for the audit log entry.
    /// </summary>
    public string LogId { get; set; } = Guid.NewGuid().ToString("N");
    
    /// <summary>
    /// Gets or sets the UTC timestamp when the action occurred.
    /// </summary>
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets the username of the user who performed the action.
    /// </summary>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the action performed.
    /// </summary>
    public string Action { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets a value indicating whether the action was successful.
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Gets or sets additional details about the action (optional).
    /// </summary>
    public string? Details { get; set; }
} 