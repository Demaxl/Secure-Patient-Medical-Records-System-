namespace MedicalRecords.core.Models;

/// <summary>
/// Represents a patient in the medical records system.
/// </summary>
public class Patient
{
    /// <summary>
    /// Gets or sets the unique identifier for the patient.
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the full name of the patient.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the date of birth in ISO format (YYYY-MM-DD).
    /// </summary>
    public string DateOfBirth { get; set; } = string.Empty;
} 