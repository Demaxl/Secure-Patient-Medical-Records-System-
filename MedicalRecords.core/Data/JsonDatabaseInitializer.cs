using System.Text.Json;
using MedicalRecords.core.Models;
using MedicalRecords.core.Services;

namespace MedicalRecords.core.Data;

/// <summary>
/// Initializes the JSON database with default data and ensures all required files exist.
/// </summary>
public class JsonDatabaseInitializer
{
    /// <summary>
    /// Gets the directory where data files are stored.
    /// </summary>
    public string DataDir { get; }

    /// <summary>
    /// Initializes a new instance of the JsonDatabaseInitializer class.
    /// </summary>
    /// <param name="dataDir">The directory where data files should be stored.</param>
    public JsonDatabaseInitializer(string dataDir) { DataDir = dataDir; }

    /// <summary>
    /// Ensures all required JSON files exist with default data.
    /// </summary>
    public void EnsureCreated()
    {
        Directory.CreateDirectory(DataDir);
        EnsureFile("roles.json", JsonSerializer.Serialize(Role.All.ToList(), new JsonSerializerOptions { WriteIndented = true }));
        EnsureUsers();
        EnsureFile("patients.json", JsonSerializer.Serialize(new List<Patient>
        {
            new() { Id = "P-1001", Name = "Alice Smith", DateOfBirth = "1985-07-12" },
            new() { Id = "P-1002", Name = "Bayo Ade",   DateOfBirth = "1990-02-03" }
        }, new JsonSerializerOptions { WriteIndented = true }));

        EnsureFile("medicalrecords.json", JsonSerializer.Serialize(new List<MedicalRecord>
        {
            new() { RecordId = "R-2001", PatientId = "P-1001", RecordDate = "2025-08-01", Description = "Hypertension | Lisinopril 10mg" }
        }, new JsonSerializerOptions { WriteIndented = true }));

        EnsureFile("auditlogs.json", JsonSerializer.Serialize(new List<AuditLog>(), new JsonSerializerOptions { WriteIndented = true }));
    }

    /// <summary>
    /// Ensures the users.json file exists with default users.
    /// </summary>
    private void EnsureUsers()
    {
        var file = Path.Combine(DataDir, "users.json");
        if (File.Exists(file)) return;

        var hasher = new PasswordHasher();
        var users = new List<User>
        {
            new() { Username = "admin1",  Role = Role.Admin,  PasswordHash = hasher.Hash("Admin@123") },
            new() { Username = "doctor1", Role = Role.Doctor, PasswordHash = hasher.Hash("Doctor@123") },
            new() { Username = "nurse1",  Role = Role.Nurse,  PasswordHash = hasher.Hash("Nurse@123") }
        };
        File.WriteAllText(file, JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));
    }

    /// <summary>
    /// Ensures a specific file exists with the given content.
    /// </summary>
    /// <param name="name">The name of the file to create.</param>
    /// <param name="content">The content to write to the file.</param>
    private void EnsureFile(string name, string content)
    {
        var path = Path.Combine(DataDir, name);
        if (!File.Exists(path)) File.WriteAllText(path, content);
    }
}