using MedicalRecords.core.Data;
using MedicalRecords.core.Models;
using MedicalRecords.core.Security;
using MedicalRecords.core.Services;

namespace MedicalRecords.core.CLI;

/// <summary>
/// Provides the command-line interface for the Medical Records System.
/// </summary>
public class Menu
{
    private readonly string _dataDir;
    private readonly IUserRepository _userRepo;
    private readonly IRoleRepository _roleRepo;
    private readonly IPatientRepository _patientRepo;
    private readonly IMedicalRecordRepository _recordRepo;
    private readonly IAuditLogRepository _auditRepo;

    /// <summary>
    /// Initializes a new instance of the Menu class.
    /// </summary>
    /// <param name="dataDir">The directory containing the data files.</param>
    public Menu(string dataDir)
    {
        _dataDir = dataDir;
        _userRepo = new JsonUserRepository(Path.Combine(_dataDir, "users.json"));
        _roleRepo = new JsonRoleRepository(Path.Combine(_dataDir, "roles.json"));
        _patientRepo = new JsonPatientRepository(Path.Combine(_dataDir, "patients.json"));
        _recordRepo = new JsonMedicalRecordRepository(Path.Combine(_dataDir, "medicalrecords.json"));
        _auditRepo = new JsonAuditLogRepository(Path.Combine(_dataDir, "auditlogs.json"));
    }

    /// <summary>
    /// Starts the main application loop with authentication.
    /// </summary>
    public void Start()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Secure Patient Records System ===");
            Console.Write("Username: ");
            var username = Console.ReadLine()?.Trim() ?? string.Empty;
            Console.Write("Password: ");
            var password = ReadHidden();

            var auditSvc = new AuditLogService(_auditRepo);
            var authSvc = new AuthService(_userRepo, _roleRepo, auditSvc);

            var user = authSvc.Authenticate(username, password);
            if (user is null)
            {
                Console.WriteLine("Invalid credentials. Press any key to retry...");
                Console.ReadKey();
                continue;
            }

            AuthService.CurrentUser = user;
            Console.WriteLine($"Login successful. Welcome, {user.Username} ({user.Role}).");
            Thread.Sleep(800);

            // Wrap repositories with proxies for RBAC + audit
            var patientProxy = new PatientRepositoryProxy(AuthService.CurrentUser, _patientRepo, new AuditLogService(_auditRepo));
            var recordProxy  = new MedicalRecordRepositoryProxy(AuthService.CurrentUser, _recordRepo, new AuditLogService(_auditRepo));

            MainMenuLoop(patientProxy, recordProxy, authSvc);
            AuthService.CurrentUser = null;
        }
    }

    /// <summary>
    /// Runs the main menu loop after successful authentication.
    /// </summary>
    /// <param name="patientRepo">The patient repository proxy.</param>
    /// <param name="recordRepo">The medical record repository proxy.</param>
    /// <param name="auth">The authentication service.</param>
    private void MainMenuLoop(IPatientRepository patientRepo, IMedicalRecordRepository recordRepo, AuthService auth)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Main Menu:");
            Console.WriteLine("1. Search patients");
            Console.WriteLine("2. View patient by ID");
            Console.WriteLine("3. Add patient (Doctor/Admin)");
            Console.WriteLine("4. Update patient (Doctor/Admin)");
            Console.WriteLine("5. Delete patient (Admin)");
            Console.WriteLine("6. Add medical record to patient (Doctor)");
            Console.WriteLine("7. View medical records for patient");
            Console.WriteLine("8. Logout");
            Console.Write("Select option: ");
            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter name filter: ");
                        var filter = Console.ReadLine() ?? "";
                        var results = patientRepo.SearchPatients(filter);
                        Console.WriteLine($"Found {results.Count} patient(s).");
                        foreach (var p in results) Console.WriteLine($"{p.Id} - {p.Name} (DOB: {p.DateOfBirth})");
                        Pause();
                        break;

                    case "2":
                        Console.Write("Enter patient ID: ");
                        var id = Console.ReadLine() ?? "";
                        var patient = patientRepo.GetPatientById(id);
                        if (patient is null) Console.WriteLine("Patient not found.");
                        else Console.WriteLine($"{patient.Id} - {patient.Name} (DOB: {patient.DateOfBirth})");
                        Pause();
                        break;

                    case "3":
                        Console.Write("New patient ID: ");
                        var newId = Console.ReadLine() ?? "";
                        Console.Write("Name: ");
                        var name = Console.ReadLine() ?? "";
                        Console.Write("DOB (YYYY-MM-DD): ");
                        var dob = Console.ReadLine() ?? "";
                        patientRepo.AddPatient(new Patient { Id = newId, Name = name, DateOfBirth = dob });
                        Console.WriteLine("Patient added.");
                        Pause();
                        break;

                    case "4":
                        Console.Write("Patient ID to update: ");
                        var upId = Console.ReadLine() ?? "";
                        var existing = patientRepo.GetPatientById(upId);
                        if (existing is null) { Console.WriteLine("Not found."); Pause(); break; }
                        Console.Write($"New name (blank to keep '{existing.Name}'): ");
                        var nn = Console.ReadLine();
                        Console.Write($"New DOB (blank to keep '{existing.DateOfBirth}'): ");
                        var nd = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(nn)) existing.Name = nn!;
                        if (!string.IsNullOrWhiteSpace(nd)) existing.DateOfBirth = nd!;
                        patientRepo.UpdatePatient(existing);
                        Console.WriteLine("Patient updated.");
                        Pause();
                        break;

                    case "5":
                        Console.Write("Patient ID to delete: ");
                        var delId = Console.ReadLine() ?? "";
                        patientRepo.DeletePatient(delId);
                        Console.WriteLine("Patient deleted (if existed).");
                        Pause();
                        break;

                    case "6":
                        Console.Write("Patient ID: ");
                        var pid = Console.ReadLine() ?? "";
                        Console.Write("Record ID: ");
                        var rid = Console.ReadLine() ?? "";
                        Console.Write("Record Date (YYYY-MM-DD): ");
                        var rdate = Console.ReadLine() ?? "";
                        Console.Write("Diagnosis: ");
                        var diag = Console.ReadLine() ?? "";
                        Console.Write("Treatment: ");
                        var treat = Console.ReadLine() ?? "";
                        recordRepo.AddRecord(new MedicalRecord { RecordId = rid, PatientId = pid, RecordDate = rdate, Description = $"{diag} | {treat}" });
                        Console.WriteLine("Medical record added.");
                        Pause();
                        break;

                    case "7":
                        Console.Write("Patient ID: ");
                        var pid2 = Console.ReadLine() ?? "";
                        var recs = recordRepo.GetRecordsByPatientId(pid2);
                        Console.WriteLine($"Found {recs.Count} record(s).");
                        foreach (var r in recs) Console.WriteLine($"{r.RecordId} - {r.RecordDate} - {r.Description}");
                        Pause();
                        break;

                    case "8":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        Pause();
                        break;
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"ACCESS DENIED: {ex.Message}");
                Pause();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Pause();
            }
        }
    }

    /// <summary>
    /// Pauses execution and waits for user input.
    /// </summary>
    private static void Pause()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    /// <summary>
    /// Reads a password from the console without displaying the characters.
    /// </summary>
    /// <returns>The entered password.</returns>
    private static string ReadHidden()
    {
        var pwd = string.Empty;
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter) break;
            if (key.Key == ConsoleKey.Backspace)
            {
                if (pwd.Length > 0) { pwd = pwd[..^1]; Console.Write("\b \b"); }
            }
            else { pwd += key.KeyChar; Console.Write("*"); }
        } while (true);
        Console.WriteLine();
        return pwd;
    }
} 