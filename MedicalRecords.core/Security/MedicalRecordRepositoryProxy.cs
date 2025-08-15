using MedicalRecords.core.Data;
using MedicalRecords.core.Models;
using MedicalRecords.core.Services;

namespace MedicalRecords.core.Security;

/// <summary>
/// Protection Proxy for medical records repository.
/// Doctors can add; all clinical roles can view; Admin view allowed.
/// </summary>
public class MedicalRecordRepositoryProxy : IMedicalRecordRepository
{
    private readonly User _currentUser;
    private readonly IMedicalRecordRepository _real;
    private readonly AuditLogService _audit;

    /// <summary>
    /// Initializes a new instance of the MedicalRecordRepositoryProxy class.
    /// </summary>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="real">The real medical record repository to delegate to.</param>
    /// <param name="audit">The audit service for logging operations.</param>
    public MedicalRecordRepositoryProxy(User currentUser, IMedicalRecordRepository real, AuditLogService audit)
    {
        _currentUser = currentUser;
        _real = real;
        _audit = audit;
    }

    /// <summary>
    /// Adds a new medical record to the system.
    /// </summary>
    /// <param name="record">The medical record to add.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to add medical records.</exception>
    public void AddRecord(MedicalRecord record)
    {
        var allowed = _currentUser.Role is Role.Doctor;
        _audit.Log(_currentUser.Username, $"AddRecord({record.RecordId} -> {record.PatientId})", allowed);
        if (!allowed) throw new UnauthorizedAccessException("Only Doctors can add medical records.");
        _real.AddRecord(record);
    }

    /// <summary>
    /// Retrieves all medical records for a specific patient.
    /// </summary>
    /// <param name="patientId">The patient identifier to search for.</param>
    /// <returns>A list of medical records for the specified patient.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to view medical records.</exception>
    public List<MedicalRecord> GetRecordsByPatientId(string patientId)
    {
        var allowed = _currentUser.Role is Role.Doctor or Role.Nurse or Role.Admin;
        _audit.Log(_currentUser.Username, $"GetRecordsByPatient({patientId})", allowed);
        if (!allowed) throw new UnauthorizedAccessException("Not allowed to view medical records.");
        return _real.GetRecordsByPatientId(patientId);
    }

    /// <summary>
    /// Removes a medical record from the system.
    /// </summary>
    /// <param name="recordId">The identifier of the medical record to remove.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to delete medical records.</exception>
    public void DeleteRecord(string recordId)
    {
        var allowed = _currentUser.Role is Role.Admin;
        _audit.Log(_currentUser.Username, $"DeleteRecord({recordId})", allowed);
        if (!allowed) throw new UnauthorizedAccessException("Only Admin can delete records.");
        _real.DeleteRecord(recordId);
    }
} 