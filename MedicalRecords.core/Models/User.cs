namespace MedicalRecords.core.Models;

/// <summary>
/// Represents a user in the medical records system.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    
    /// <summary>
    /// Gets or sets the username for authentication.
    /// </summary>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the hashed password using bcrypt.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the role of the user (Admin, Doctor, or Nurse).
    /// Default is Nurse with the least access.
    /// </summary>
    public string Role { get; set; } = "Nurse";
} 