namespace MedicalRecords.core.Models;

/// <summary>
/// Represents a medical record for a patient.
/// </summary>
public class MedicalRecord
{
    /// <summary>
    /// Gets or sets the unique identifier for the medical record.
    /// </summary>
    public string RecordId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the identifier of the patient this record belongs to.
    /// </summary>
    public string PatientId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the date when the medical record was created in ISO format (YYYY-MM-DD).
    /// </summary>
    public string RecordDate { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description containing diagnosis and treatment information.
    /// </summary>
    public string Description { get; set; } = string.Empty;
} 