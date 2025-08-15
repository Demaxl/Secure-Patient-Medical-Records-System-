using MedicalRecords.CLI;
using MedicalRecords.Data;

namespace MedicalRecords;

/// <summary>
/// Main entry point for the Medical Records System console application.
/// </summary>
public class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public static void Main()
    {
        // Ensure JSON files exist / seed default content
        var db = new JsonDatabaseInitializer(Path.Combine(AppContext.BaseDirectory, "DataFiles"));
        db.EnsureCreated();

        // Start CLI
        var menu = new Menu(db.DataDir);
        menu.Start();
    }
} 