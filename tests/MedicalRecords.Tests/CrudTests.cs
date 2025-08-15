using MedicalRecords.Data;
using MedicalRecords.Models;
using MedicalRecords.Security;
using MedicalRecords.Services;
using Xunit;
using System.IO;
using System;

namespace MedicalRecords.Tests;

/// <summary>
/// Unit tests for CRUD operations.
/// </summary>
public class CrudTests
{
    private readonly string _dataDir;

    /// <summary>
    /// Initializes a new instance of the CrudTests class.
    /// </summary>
    public CrudTests()
    {
        _dataDir = Path.Combine(Path.GetTempPath(), "MedicalRecordsTests", Guid.NewGuid().ToString());
        new JsonDatabaseInitializer(_dataDir).EnsureCreated();
    }

    /// <summary>
    /// Tests that admin users can successfully delete patients.
    /// </summary>
    [Fact]
    public void Admin_DeletePatient_Succeeds()
    {
        var admin = new User { Username = "admin", Role = Role.Admin, PasswordHash = "x" };
        var audit = new AuditLogService(new JsonAuditLogRepository(Path.Combine(_dataDir, "auditlogs.json")));
        var real = new JsonPatientRepository(Path.Combine(_dataDir, "patients.json"));
        var proxy = new PatientRepositoryProxy(admin, real, audit);

        proxy.AddPatient(new Patient { Id = "PX-DEL", Name = "Temp", DateOfBirth = "1999-09-09" });
        Assert.NotNull(proxy.GetPatientById("PX-DEL"));

        proxy.DeletePatient("PX-DEL");
        Assert.Null(proxy.GetPatientById("PX-DEL"));
    }
} 