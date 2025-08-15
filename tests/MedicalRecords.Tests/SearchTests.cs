using MedicalRecords.Data;
using MedicalRecords.Models;
using MedicalRecords.Security;
using MedicalRecords.Services;
using Xunit;
using System.IO;
using System;
using System.Linq;

namespace MedicalRecords.Tests;

/// <summary>
/// Unit tests for search functionality.
/// </summary>
public class SearchTests
{
    private readonly string _dataDir;

    /// <summary>
    /// Initializes a new instance of the SearchTests class.
    /// </summary>
    public SearchTests()
    {
        _dataDir = Path.Combine(Path.GetTempPath(), "MedicalRecordsTests", Guid.NewGuid().ToString());
        new JsonDatabaseInitializer(_dataDir).EnsureCreated();
    }

    /// <summary>
    /// Tests that patient search by name returns matching results.
    /// </summary>
    [Fact]
    public void Search_ByName_ReturnsMatches()
    {
        var doctor = new User { Username = "doctor", Role = Role.Doctor, PasswordHash = "x" };
        var audit = new AuditLogService(new JsonAuditLogRepository(Path.Combine(_dataDir, "auditlogs.json")));
        var real = new JsonPatientRepository(Path.Combine(_dataDir, "patients.json"));
        var proxy = new PatientRepositoryProxy(doctor, real, audit);

        var res = proxy.SearchPatients("Alice");
        Assert.True(res.Any());
        Assert.Contains(res, p => p.Name.Contains("Alice", StringComparison.OrdinalIgnoreCase));
    }
} 