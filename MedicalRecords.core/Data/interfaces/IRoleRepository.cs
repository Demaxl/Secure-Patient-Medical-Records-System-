namespace MedicalRecords.core.Data;

/// <summary>
/// Defines the contract for role data access operations.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Retrieves all available roles in the system.
    /// </summary>
    /// <returns>A list of all role names.</returns>
    List<string> GetAllRoles();
} 