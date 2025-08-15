using BCryptNet = BCrypt.Net.BCrypt;

namespace MedicalRecords.core.Services;

/// <summary>
/// Wraps bcrypt for secure password hashing and verification.
/// </summary>
public class PasswordHasher
{
    /// <summary>
    /// Hashes a password using bcrypt.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>The hashed password.</returns>
    public string Hash(string password) => BCryptNet.HashPassword(password);

    /// <summary>
    /// Verifies a password against a hash.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <param name="hash">The hash to verify against.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    public bool Verify(string password, string hash) => BCryptNet.Verify(password, hash);
}