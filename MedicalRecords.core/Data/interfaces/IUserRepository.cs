using MedicalRecords.core.Models;

namespace MedicalRecords.core.Data;

/// <summary>
/// Defines the contract for user data access operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    User? GetByUsername(string username);
    
    /// <summary>
    /// Retrieves all users in the system.
    /// </summary>
    /// <returns>A list of all users.</returns>
    List<User> GetAll();
    
    /// <summary>
    /// Inserts a new user or updates an existing one.
    /// </summary>
    /// <param name="user">The user to insert or update.</param>
    void Upsert(User user);
} 