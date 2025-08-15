using System.Text.Json;
using MedicalRecords.core.Models;

namespace MedicalRecords.core.Data;

/// <summary>
/// JSON-based implementation of the patient repository.
/// Provides thread-safe operations for patient data persistence.
/// </summary>
public class JsonPatientRepository : IPatientRepository
{
    private readonly string _file;
    private readonly object _lock = new();

    /// <summary>
    /// Initializes a new instance of the JsonPatientRepository class.
    /// </summary>
    /// <param name="file">The path to the JSON file for storing patient data.</param>
    public JsonPatientRepository(string file) { _file = file; }

    /// <summary>
    /// Loads all patients from the JSON file.
    /// </summary>
    /// <returns>A list of all patients.</returns>
    private List<Patient> Load()
    {
        if (!File.Exists(_file)) return new();
        var json = File.ReadAllText(_file);
        return JsonSerializer.Deserialize<List<Patient>>(json) ?? new();
    }

    /// <summary>
    /// Saves the patient list to the JSON file.
    /// </summary>
    /// <param name="pts">The list of patients to save.</param>
    private void Save(List<Patient> pts) =>
        File.WriteAllText(_file, JsonSerializer.Serialize(pts, new JsonSerializerOptions { WriteIndented = true }));

    /// <summary>
    /// Retrieves a patient by their unique identifier.
    /// </summary>
    /// <param name="id">The patient identifier to search for.</param>
    /// <returns>The patient if found; otherwise, null.</returns>
    public Patient? GetPatientById(string id)
    {
        lock (_lock) { return Load().FirstOrDefault(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase)); }
    }

    /// <summary>
    /// Adds a new patient to the system.
    /// </summary>
    /// <param name="patient">The patient to add.</param>
    /// <exception cref="ArgumentException">Thrown when patient ID is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when patient ID already exists.</exception>
    public void AddPatient(Patient patient)
    {
        if (string.IsNullOrWhiteSpace(patient.Id)) throw new ArgumentException("Patient.Id required");
        lock (_lock)
        {
            var pts = Load();
            if (pts.Any(p => p.Id.Equals(patient.Id, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Patient ID already exists.");
            pts.Add(patient);
            Save(pts);
        }
    }

    /// <summary>
    /// Updates an existing patient's information.
    /// </summary>
    /// <param name="patient">The patient with updated information.</param>
    /// <exception cref="ArgumentException">Thrown when patient ID is null or empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when patient is not found.</exception>
    public void UpdatePatient(Patient patient)
    {
        if (string.IsNullOrWhiteSpace(patient.Id)) throw new ArgumentException("Patient.Id required");
        lock (_lock)
        {
            var pts = Load();
            var idx = pts.FindIndex(p => p.Id.Equals(patient.Id, StringComparison.OrdinalIgnoreCase));
            if (idx < 0) throw new KeyNotFoundException("Patient not found.");
            pts[idx] = patient;
            Save(pts);
        }
    }

    /// <summary>
    /// Removes a patient from the system.
    /// </summary>
    /// <param name="id">The identifier of the patient to remove.</param>
    public void DeletePatient(string id)
    {
        lock (_lock)
        {
            var pts = Load();
            var removed = pts.RemoveAll(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (removed > 0) Save(pts);
        }
    }

    /// <summary>
    /// Searches for patients by name filter.
    /// </summary>
    /// <param name="nameFilter">The name filter to search for.</param>
    /// <returns>A list of patients matching the filter.</returns>
    public List<Patient> SearchPatients(string nameFilter)
    {
        lock (_lock)
        {
            var pts = Load();
            if (string.IsNullOrWhiteSpace(nameFilter)) return pts;
            return pts.Where(p => p.Name.Contains(nameFilter, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
} 