# Medical Records Proxy System

A secure, role-based medical records management system implemented in C# using the Proxy design pattern for access control and audit logging.

## Features

-   **Role-Based Access Control (RBAC)**: Three user roles (Admin, Doctor, Nurse) with different permissions
-   **Proxy Pattern**: Protection proxies enforce access control and audit logging
-   **Secure Authentication**: BCrypt password hashing
-   **Audit Logging**: All operations are logged for compliance and security
-   **JSON Data Storage**: Simple, portable data storage using JSON files
-   **Console Interface**: Easy-to-use command-line interface

## Architecture

The system uses the Proxy design pattern to implement:

-   **Access Control**: Proxies check user permissions before allowing operations
-   **Audit Logging**: All operations are automatically logged
-   **Separation of Concerns**: Business logic is separated from security enforcement

### Key Components

-   **Models**: User, Patient, MedicalRecord, AuditLog, Role
-   **Data Layer**: JSON-based repositories with thread-safe operations
-   **Security**: Proxy classes that enforce RBAC and audit logging
-   **Services**: Authentication, audit logging, and password hashing
-   **CLI**: Console-based user interface

## User Roles and Permissions

### Admin

-   Full access to all operations
-   Can delete patients and medical records
-   Can perform all CRUD operations

### Doctor

-   Can view, add, and update patients
-   Can add and view medical records
-   Cannot delete patients or medical records

### Nurse

-   Can view patients and search for them
-   Can view medical records
-   Cannot modify any data

## Default Login Credentials

The system comes with three pre-configured users:

| Username | Password   | Role   |
| -------- | ---------- | ------ |
| admin1   | Admin@123  | Admin  |
| doctor1  | Doctor@123 | Doctor |
| nurse1   | Nurse@123  | Nurse  |

## Building and Running

### Prerequisites

-   .NET 6.0 SDK or later
-   Windows, macOS, or Linux

### Build the Solution

```bash
dotnet build
```

### Run Tests

```bash
dotnet test
```

### Run the Application

```bash
dotnet run --project src/MedicalRecords
```

## Project Structure

```
MedicalRecordsSystem/
├── MedicalRecordsSystem.sln
├── src/
│   └── MedicalRecords/
│       ├── MedicalRecords.csproj
│       ├── CLI/
│       │   ├── Program.cs
│       │   └── Menu.cs
│       ├── Models/
│       │   ├── User.cs
│       │   ├── Role.cs
│       │   ├── Patient.cs
│       │   ├── MedicalRecord.cs
│       │   └── AuditLog.cs
│       ├── Data/
│       │   ├── IUserRepository.cs
│       │   ├── IRoleRepository.cs
│       │   ├── IPatientRepository.cs
│       │   ├── IMedicalRecordRepository.cs
│       │   ├── IAuditLogRepository.cs
│       │   ├── JsonUserRepository.cs
│       │   ├── JsonRoleRepository.cs
│       │   ├── JsonPatientRepository.cs
│       │   ├── JsonMedicalRecordRepository.cs
│       │   ├── JsonAuditLogRepository.cs
│       │   └── JsonDatabaseInitializer.cs
│       ├── Security/
│       │   ├── PatientRepositoryProxy.cs
│       │   └── MedicalRecordRepositoryProxy.cs
│       └── Services/
│           ├── AuthService.cs
│           ├── AuditLogService.cs
│           └── PasswordHasher.cs
├── tests/
│   └── MedicalRecords.Tests/
│       ├── MedicalRecords.Tests.csproj
│       ├── AuthTests.cs
│       ├── ProxyTests.cs
│       ├── CrudTests.cs
│       └── SearchTests.cs
└── DataFiles/
    ├── users.json
    ├── roles.json
    ├── patients.json
    ├── medicalrecords.json
    └── auditlogs.json
```

## Design Patterns Used

### Proxy Pattern

-   **PatientRepositoryProxy**: Controls access to patient data based on user role
-   **MedicalRecordRepositoryProxy**: Controls access to medical records based on user role

### Repository Pattern

-   Abstract interfaces for data access
-   JSON-based implementations
-   Thread-safe operations with file locking

### Service Layer Pattern

-   Authentication service
-   Audit logging service
-   Password hashing service

## Security Features

-   **Password Hashing**: BCrypt for secure password storage
-   **Role-Based Access Control**: Different permissions for different user types
-   **Audit Logging**: Complete audit trail of all operations
-   **Input Validation**: Validation of user inputs and data integrity
-   **Thread Safety**: File-based locking for concurrent access

## Testing

The system includes comprehensive unit tests covering:

-   Authentication functionality
-   Proxy pattern behavior
-   CRUD operations
-   Search functionality
-   Role-based access control

Run tests with:

```bash
dotnet test
```

## Data Storage

All data is stored in JSON files in the `DataFiles/` directory:

-   `users.json`: User accounts and credentials
-   `roles.json`: Available system roles
-   `patients.json`: Patient information
-   `medicalrecords.json`: Medical record data
-   `auditlogs.json`: System audit trail

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request

## License

This project is for educational purposes and demonstrates the implementation of the Proxy design pattern in a medical records system.

## Disclaimer

This is a demonstration system and should not be used in production environments without proper security review and compliance verification.
