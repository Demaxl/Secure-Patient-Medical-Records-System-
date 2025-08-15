using System.Text.Json;
using MedicalRecords.core.Models;

namespace MedicalRecords.core.Data;

/// <summary>
/// JSON-based implementation of the audit log repository.
/// Provides thread-safe operations for audit log data persistence.
/// </summary>
public class JsonAuditLogRepository : IAuditLogRepository
{
    private readonly string _file;
    private readonly object _lock = new();

    /// <summary>
    /// Initializes a new instance of the JsonAuditLogRepository class.
    /// </summary>
    /// <param name="file">The path to the JSON file for storing audit log data.</param>
    public JsonAuditLogRepository(string file) { _file = file; }

    /// <summary>
    /// Appends a new audit log entry to the system.
    /// </summary>
    /// <param name="entry">The audit log entry to append.</param>
    public void Append(AuditLog entry)
    {
        lock (_lock)
        {
            var logs = GetAll();
            logs.Add(entry);
            File.WriteAllText(_file, JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true }));
        }
    }

    /// <summary>
    /// Retrieves all audit log entries.
    /// </summary>
    /// <returns>A list of all audit log entries.</returns>
    public List<AuditLog> GetAll()
    {
        lock (_lock)
        {
            if (!File.Exists(_file)) return new();
            var json = File.ReadAllText(_file);
            return JsonSerializer.Deserialize<List<AuditLog>>(json) ?? new();
        }
    }
} 