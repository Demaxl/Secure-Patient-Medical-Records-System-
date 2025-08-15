namespace MedicalRecordsSystem.Models;

public class User
{
    public string Username { get; set; }
    public string HashedPassword { get; set; }
    public string Role { get; set; }
}
