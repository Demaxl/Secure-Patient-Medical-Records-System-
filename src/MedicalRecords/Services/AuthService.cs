using MedicalRecords.Data;
using MedicalRecords.Models;

namespace MedicalRecords.Services;

/// <summary>
/// Provides authentication and user management services.
/// </summary>
public class AuthService
{
    /// <summary>
    /// Gets or sets the currently authenticated user.
    /// </summary>
    public static User? CurrentUser { get; set; }

    private readonly IUserRepository _userRepo;
    private readonly IRoleRepository _roleRepo;
    private readonly AuditLogService _audit;
    private readonly PasswordHasher _hasher = new();

    /// <summary>
    /// Initializes a new instance of the AuthService class.
    /// </summary>
    /// <param name="userRepo">The user repository for data access.</param>
    /// <param name="roleRepo">The role repository for role validation.</param>
    /// <param name="audit">The audit service for logging authentication attempts.</param>
    public AuthService(IUserRepository userRepo, IRoleRepository roleRepo, AuditLogService audit)
    {
        _userRepo = userRepo; _roleRepo = roleRepo; _audit = audit;
    }

    /// <summary>
    /// Authenticates a user with the provided credentials.
    /// </summary>
    /// <param name="username">The username to authenticate.</param>
    /// <param name="password">The password to verify.</param>
    /// <returns>The authenticated user if successful; otherwise, null.</returns>
    public User? Authenticate(string username, string password)
    {
        var user = _userRepo.GetByUsername(username);
        var ok = user is not null && _hasher.Verify(password, user.PasswordHash);
        _audit.Log(username, "Authenticate", ok);
        return ok ? user : null;
    }

    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="username">The username for the new user.</param>
    /// <param name="password">The password for the new user.</param>
    /// <param name="role">The role for the new user.</param>
    /// <returns>The newly created user.</returns>
    /// <exception cref="ArgumentException">Thrown when role is invalid or credentials are empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when username already exists.</exception>
    public User Register(string username, string password, string role)
    {
        if (!_roleRepo.GetAllRoles().Contains(role)) throw new ArgumentException("Invalid role");
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Username and password required");

        if (_userRepo.GetByUsername(username) is not null) throw new InvalidOperationException("Username exists.");

        var user = new User { Username = username, Role = role, PasswordHash = _hasher.Hash(password) };
        _userRepo.Upsert(user);
        _audit.Log(username, "Register", true);
        return user;
    }
}