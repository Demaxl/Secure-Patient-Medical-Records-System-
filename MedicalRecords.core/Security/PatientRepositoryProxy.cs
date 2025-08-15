using MedicalRecords.core.Data;
using MedicalRecords.core.Models;
using MedicalRecords.core.Services;

namespace MedicalRecords.core.Security;

/// <summary>
/// Protection Proxy enforcing RBAC and audit logging for patient operations.
/// Implements same interface as the real repository (IPatientRepository).
/// </summary>
public class PatientRepositoryProxy : IPatientRepository
{
    private readonly User _currentUser;
    private readonly IPatientRepository _real;
    private readonly AuditLogService _audit;

    /// <summary>
    /// Initializes a new instance of the PatientRepositoryProxy class.
    /// </summary>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="real">The real patient repository to delegate to.</param>
    /// <param name="audit">The audit service for logging operations.</param>
    public PatientRepositoryProxy(User currentUser, IPatientRepository real, AuditLogService audit)
    {
        _currentUser = currentUser;
        _real = real;
        _audit = audit;
    }

    /// <summary>
    /// Retrieves a patient by their unique identifier.
    /// </summary>
    /// <param name="id">The patient identifier to search for.</param>
    /// <returns>The patient if found; otherwise, null.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to read patient records.</exception>
    public Patient? GetPatientById(string id)
    {
        // Pattern matching to check if the user is a doctor, nurse, or admin
        var allowed = _currentUser.Role is Role.Doctor or Role.Nurse or Role.Admin;
        _audit.Log(_currentUser.Username, $"GetPatient({id})", allowed);
        if (!allowed) throw new UnauthorizedAccessException("Not allowed to read patient records.");

        // Delegate to the real repository
        return _real.GetPatientById(id);
    }

    /// <summary>
    /// Adds a new patient to the system.
    /// </summary>
    /// <param name="patient">The patient to add.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to add patients.</exception>
    public void AddPatient(Patient patient)
    {
        var allowed = _currentUser.Role is Role.Doctor or Role.Admin;
        _audit.Log(_currentUser.Username, $"AddPatient({patient.Id})", allowed);
        if (!allowed) throw new UnauthorizedAccessException("Not allowed to add patient.");
        _real.AddPatient(patient);
    }

    /// <summary>
    /// Updates an existing patient's information.
    /// </summary>
    /// <param name="patient">The patient with updated information.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to update patients.</exception>
    public void UpdatePatient(Patient patient)
    {
        var allowed = _currentUser.Role is Role.Doctor or Role.Admin;
        _audit.Log(_currentUser.Username, $"UpdatePatient({patient.Id})", allowed);
        if (!allowed) throw new UnauthorizedAccessException("Not allowed to update patient.");
        _real.UpdatePatient(patient);
    }

    /// <summary>
    /// Removes a patient from the system.
    /// </summary>
    /// <param name="id">The identifier of the patient to remove.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to delete patients.</exception>
    public void DeletePatient(string id)
    {
        var allowed = _currentUser.Role is Role.Admin;
        _audit.Log(_currentUser.Username, $"DeletePatient({id})", allowed);
        if (!allowed) throw new UnauthorizedAccessException("Only Admin can delete.");
        _real.DeletePatient(id);
    }

    /// <summary>
    /// Searches for patients by name filter.
    /// </summary>
    /// <param name="nameFilter">The name filter to search for.</param>
    /// <returns>A list of patients matching the filter.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to search patients.</exception>
    public List<Patient> SearchPatients(string nameFilter)
    {
        var allowed = _currentUser.Role is Role.Doctor or Role.Nurse or Role.Admin;
        _audit.Log(_currentUser.Username, $"SearchPatients('{nameFilter}')", allowed);
        if (!allowed) throw new UnauthorizedAccessException("Not allowed to search patients.");
        return _real.SearchPatients(nameFilter);
    }
}