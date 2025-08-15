using MedicalRecords.core.Data;
using MedicalRecords.core.Models;
using MedicalRecords.core.Security;
using MedicalRecords.core.Services;
using Xunit;
using System.IO;
using System;
using System.Linq;

namespace MedicalRecords.Tests;

/// <summary>
/// Unit tests for the proxy pattern implementation.
/// </summary>
public class ProxyTests
{
    private readonly string _dataDir;

    /// <summary>
    /// Initializes a new instance of the ProxyTests class.
    /// </summary>
    public ProxyTests()
    {
        _dataDir = Path.Combine(Path.GetTempPath(), "MedicalRecordsTests", Guid.NewGuid().ToString());
        new JsonDatabaseInitializer(_dataDir).EnsureCreated();
    }

    /// <summary>
    /// Tests that nurses cannot add patients (role-based access control).
    /// </summary>
    [Fact]
    public void Nurse_Cannot_AddPatient()
    {
        var nurse = new User { Username = "nurseX", Role = Role.Nurse, PasswordHash = "x" };
        var audit = new AuditLogService(new JsonAuditLogRepository(Path.Combine(_dataDir, "auditlogs.json")));
        var real = new JsonPatientRepository(Path.Combine(_dataDir, "patients.json"));
        var proxy = new PatientRepositoryProxy(nurse, real, audit);

        Assert.Throws<UnauthorizedAccessException>(() =>
            proxy.AddPatient(new Patient { Id = "PX-1", Name = "Test", DateOfBirth = "2000-01-01" }));
    }

    /// <summary>
    /// Tests that doctors can add records and admins can delete records.
    /// </summary>
    [Fact]
    public void Doctor_Can_AddRecord_Admin_Can_DeleteRecord()
    {
        var doc = new User { Username = "doc", Role = Role.Doctor, PasswordHash = "x" };
        var admin = new User { Username = "admin", Role = Role.Admin, PasswordHash = "x" };
        var audit = new AuditLogService(new JsonAuditLogRepository(Path.Combine(_dataDir, "auditlogs.json")));
        var real = new JsonMedicalRecordRepository(Path.Combine(_dataDir, "medicalrecords.json"));
        var docProxy = new MedicalRecordRepositoryProxy(doc, real, audit);
        var adminProxy = new MedicalRecordRepositoryProxy(admin, real, audit);

        var rec = new MedicalRecord { RecordId = "T-REC-1", PatientId = "P-1001", RecordDate = "2025-01-01", Description = "Flu | Rest" };
        docProxy.AddRecord(rec);

        var list = docProxy.GetRecordsByPatientId("P-1001");
        Assert.Contains(list, r => r.RecordId == "T-REC-1");

        adminProxy.DeleteRecord("T-REC-1");
        var list2 = adminProxy.GetRecordsByPatientId("P-1001");
        Assert.DoesNotContain(list2, r => r.RecordId == "T-REC-1");
    }
} 