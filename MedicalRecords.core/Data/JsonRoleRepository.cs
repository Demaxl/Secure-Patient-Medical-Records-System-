using System.Text.Json;
using MedicalRecords.core.Models;

namespace MedicalRecords.core.Data;

/// <summary>
/// JSON-based implementation of the role repository.
/// Provides access to system roles.
/// </summary>
public class JsonRoleRepository : IRoleRepository
{
    private readonly string _file;
    
    /// <summary>
    /// Initializes a new instance of the JsonRoleRepository class.
    /// </summary>
    /// <param name="file">The path to the JSON file for storing role data.</param>
    public JsonRoleRepository(string file) { _file = file; }

    /// <summary>
    /// Retrieves all available roles from the JSON file.
    /// </summary>
    /// <returns>A list of all role names.</returns>
    public List<string> GetAllRoles()
    {
        if (!File.Exists(_file)) return Role.All.ToList();
        var json = File.ReadAllText(_file);
        var roles = JsonSerializer.Deserialize<List<string>>(json) ?? Role.All.ToList();
        return roles;
    }
} 