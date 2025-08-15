using System.Text.Json;
using MedicalRecords.Models;

namespace MedicalRecords.Data;

/// <summary>
/// JSON-based implementation of the medical record repository.
/// Provides thread-safe operations for medical record data persistence.
/// </summary>
public class JsonMedicalRecordRepository : IMedicalRecordRepository
{
    private readonly string _file;
    private readonly object _lock = new();

    /// <summary>
    /// Initializes a new instance of the JsonMedicalRecordRepository class.
    /// </summary>
    /// <param name="file">The path to the JSON file for storing medical record data.</param>
    public JsonMedicalRecordRepository(string file) { _file = file; }

    /// <summary>
    /// Loads all medical records from the JSON file.
    /// </summary>
    /// <returns>A list of all medical records.</returns>
    private List<MedicalRecord> Load()
    {
        if (!File.Exists(_file)) return new();
        var json = File.ReadAllText(_file);
        return JsonSerializer.Deserialize<List<MedicalRecord>>(json) ?? new();
    }

    /// <summary>
    /// Saves the medical record list to the JSON file.
    /// </summary>
    /// <param name="recs">The list of medical records to save.</param>
    private void Save(List<MedicalRecord> recs) =>
        File.WriteAllText(_file, JsonSerializer.Serialize(recs, new JsonSerializerOptions { WriteIndented = true }));

    /// <summary>
    /// Adds a new medical record to the system.
    /// </summary>
    /// <param name="record">The medical record to add.</param>
    /// <exception cref="ArgumentException">Thrown when RecordId or PatientId is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when RecordId already exists.</exception>
    public void AddRecord(MedicalRecord record)
    {
        if (string.IsNullOrWhiteSpace(record.RecordId)) throw new ArgumentException("RecordId required");
        if (string.IsNullOrWhiteSpace(record.PatientId)) throw new ArgumentException("PatientId required");

        lock (_lock)
        {
            var recs = Load();
            if (recs.Any(r => r.RecordId.Equals(record.RecordId, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("RecordId already exists.");
            recs.Add(record);
            Save(recs);
        }
    }

    /// <summary>
    /// Retrieves all medical records for a specific patient.
    /// </summary>
    /// <param name="patientId">The patient identifier to search for.</param>
    /// <returns>A list of medical records for the specified patient.</returns>
    public List<MedicalRecord> GetRecordsByPatientId(string patientId)
    {
        lock (_lock) { return Load().Where(r => r.PatientId.Equals(patientId, StringComparison.OrdinalIgnoreCase)).ToList(); }
    }

    /// <summary>
    /// Removes a medical record from the system.
    /// </summary>
    /// <param name="recordId">The identifier of the medical record to remove.</param>
    public void DeleteRecord(string recordId)
    {
        lock (_lock)
        {
            var recs = Load();
            var removed = recs.RemoveAll(r => r.RecordId.Equals(recordId, StringComparison.OrdinalIgnoreCase));
            if (removed > 0) Save(recs);
        }
    }
} 