using MedicalRecords.core.CLI;
using MedicalRecords.core.Data;

namespace MedicalRecords.core;

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
        var db = new JsonDatabaseInitializer("data");
        db.EnsureCreated();

        // Start CLI
        var menu = new Menu(db.DataDir);
        menu.Start();
    }
}