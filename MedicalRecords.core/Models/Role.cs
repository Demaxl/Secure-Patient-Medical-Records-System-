namespace MedicalRecords.core.Models;

/// <summary>
/// Defines the available roles in the medical records system.
/// </summary>
public static class Role
{
    /// <summary>
    /// Administrator role with full access to all operations.
    /// </summary>
    public const string Admin = "Admin";
    
    /// <summary>
    /// Doctor role with access to patient and medical record operations.
    /// </summary>
    public const string Doctor = "Doctor";
    
    /// <summary>
    /// Nurse role with limited access to view patient information.
    /// </summary>
    public const string Nurse = "Nurse";

    /// <summary>
    /// Gets all available roles as an array.
    /// </summary>
    public static readonly string[] All = new[] { Admin, Doctor, Nurse };
} 