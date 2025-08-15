using MedicalRecords.Data;
using MedicalRecords.Models;
using MedicalRecords.Services;
using Xunit;
using System.IO;
using System;

namespace MedicalRecords.Tests;

/// <summary>
/// Unit tests for the authentication service.
/// </summary>
public class AuthTests
{
    private readonly string _dataDir;

    /// <summary>
    /// Initializes a new instance of the AuthTests class.
    /// </summary>
    public AuthTests()
    {
        _dataDir = Path.Combine(Path.GetTempPath(), "MedicalRecordsTests", Guid.NewGuid().ToString());
        new JsonDatabaseInitializer(_dataDir).EnsureCreated();
    }

    /// <summary>
    /// Tests that valid credentials result in successful authentication.
    /// </summary>
    [Fact]
    public void Authenticate_ValidCredentials_Succeeds()
    {
        // Create a test user with a known password hash
        var hasher = new PasswordHasher();
        var testUser = new User { Username = "testdoctor", Role = Role.Doctor, PasswordHash = hasher.Hash("TestPass123") };
        
        var users = new JsonUserRepository(Path.Combine(_dataDir, "users.json"));
        users.Upsert(testUser);
        
        var roles = new JsonRoleRepository(Path.Combine(_dataDir, "roles.json"));
        var audit = new AuditLogService(new JsonAuditLogRepository(Path.Combine(_dataDir, "auditlogs.json")));

        var auth = new AuthService(users, roles, audit);

        var user = auth.Authenticate("testdoctor", "TestPass123");
        Assert.NotNull(user);
        Assert.Equal(Role.Doctor, user!.Role);
    }

    /// <summary>
    /// Tests that invalid credentials result in failed authentication.
    /// </summary>
    [Fact]
    public void Authenticate_InvalidCredentials_Fails()
    {
        var users = new JsonUserRepository(Path.Combine(_dataDir, "users.json"));
        var roles = new JsonRoleRepository(Path.Combine(_dataDir, "roles.json"));
        var audit = new AuditLogService(new JsonAuditLogRepository(Path.Combine(_dataDir, "auditlogs.json")));
        var auth = new AuthService(users, roles, audit);

        var user = auth.Authenticate("doctor1", "WrongPass");
        Assert.Null(user);
    }
} 