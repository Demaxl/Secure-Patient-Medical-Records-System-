using System.Text.Json;
using MedicalRecords.core.Models;

namespace MedicalRecords.core.Data;

/// <summary>
/// JSON-based implementation of the user repository.
/// Provides thread-safe operations for user data persistence.
/// </summary>
public class JsonUserRepository : IUserRepository
{
    private readonly string _file;
    private readonly object _lock = new();

    /// <summary>
    /// Initializes a new instance of the JsonUserRepository class.
    /// </summary>
    /// <param name="file">The path to the JSON file for storing user data.</param>
    public JsonUserRepository(string file) { _file = file; }

    /// <summary>
    /// Retrieves all users from the JSON file.
    /// </summary>
    /// <returns>A list of all users.</returns>
    public List<User> GetAll()
    {
        lock (_lock)
        {
            if (!File.Exists(_file)) return new();
            var json = File.ReadAllText(_file);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new();
        }
    }

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    public User? GetByUsername(string username) =>
        GetAll().FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Inserts a new user or updates an existing one.
    /// </summary>
    /// <param name="user">The user to insert or update.</param>
    public void Upsert(User user)
    {
        lock (_lock)
        {
            var users = GetAll();
            var idx = users.FindIndex(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase));
            if (idx >= 0) users[idx] = user; else users.Add(user);
            File.WriteAllText(_file, JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
} 