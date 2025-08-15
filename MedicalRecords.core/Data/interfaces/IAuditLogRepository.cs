using MedicalRecords.core.Models;

namespace MedicalRecords.core.Data;

/// <summary>
/// Defines the contract for audit log data access operations.
/// </summary>
public interface IAuditLogRepository
{
    /// <summary>
    /// Appends a new audit log entry to the system.
    /// </summary>
    /// <param name="entry">The audit log entry to append.</param>
    void Append(AuditLog entry);
    
    /// <summary>
    /// Retrieves all audit log entries.
    /// </summary>
    /// <returns>A list of all audit log entries.</returns>
    List<AuditLog> GetAll();
} 