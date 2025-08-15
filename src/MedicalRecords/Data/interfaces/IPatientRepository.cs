using MedicalRecords.Models;

namespace MedicalRecords.Data;

/// <summary>
/// Defines the contract for patient data access operations.
/// </summary>
public interface IPatientRepository
{
    /// <summary>
    /// Retrieves a patient by their unique identifier.
    /// </summary>
    /// <param name="id">The patient identifier to search for.</param>
    /// <returns>The patient if found; otherwise, null.</returns>
    Patient? GetPatientById(string id);
    
    /// <summary>
    /// Adds a new patient to the system.
    /// </summary>
    /// <param name="patient">The patient to add.</param>
    void AddPatient(Patient patient);
    
    /// <summary>
    /// Updates an existing patient's information.
    /// </summary>
    /// <param name="patient">The patient with updated information.</param>
    void UpdatePatient(Patient patient);
    
    /// <summary>
    /// Removes a patient from the system.
    /// </summary>
    /// <param name="id">The identifier of the patient to remove.</param>
    void DeletePatient(string id);
    
    /// <summary>
    /// Searches for patients by name filter.
    /// </summary>
    /// <param name="nameFilter">The name filter to search for.</param>
    /// <returns>A list of patients matching the filter.</returns>
    List<Patient> SearchPatients(string nameFilter);
} 