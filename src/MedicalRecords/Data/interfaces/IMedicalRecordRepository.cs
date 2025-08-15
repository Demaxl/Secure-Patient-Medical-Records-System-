using MedicalRecords.Models;

namespace MedicalRecords.Data;

/// <summary>
/// Defines the contract for medical record data access operations.
/// </summary>
public interface IMedicalRecordRepository
{
    /// <summary>
    /// Adds a new medical record to the system.
    /// </summary>
    /// <param name="record">The medical record to add.</param>
    void AddRecord(MedicalRecord record);
    
    /// <summary>
    /// Retrieves all medical records for a specific patient.
    /// </summary>
    /// <param name="patientId">The patient identifier to search for.</param>
    /// <returns>A list of medical records for the specified patient.</returns>
    List<MedicalRecord> GetRecordsByPatientId(string patientId);
    
    /// <summary>
    /// Removes a medical record from the system.
    /// </summary>
    /// <param name="recordId">The identifier of the medical record to remove.</param>
    void DeleteRecord(string recordId);
} 