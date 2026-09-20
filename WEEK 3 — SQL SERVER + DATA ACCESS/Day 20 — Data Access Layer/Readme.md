# Day 20 — Data Access Layer (DAL)

Day 20 is where we bring together many topics you've already learned:

* ASP.NET Core
* Controllers
* REST APIs
* DTOs
* Dependency Injection
* ADO.NET
* SQL Server
* Stored Procedures
* Interfaces
* Repository Pattern
* Service/BAL

The final architecture will be:

```text
┌──────────────────────────┐
│       HTTP Client        │
│   Angular / Postman      │
└────────────┬─────────────┘
             ↓
┌──────────────────────────┐
│      Controller          │
│ EmployeesController      │
└────────────┬─────────────┘
             ↓
┌──────────────────────────┐
│     BAL / Service        │
│ IEmployeeService         │
│ EmployeeService          │
└────────────┬─────────────┘
             ↓
┌──────────────────────────┐
│          DAL             │
│ IEmployeeRepository      │
│ EmployeeRepository       │
└────────────┬─────────────┘
             ↓
┌──────────────────────────┐
│        ADO.NET           │
│ SqlConnection            │
│ SqlCommand               │
│ SqlParameter             │
│ SqlDataReader            │
└────────────┬─────────────┘
             ↓
┌──────────────────────────┐
│       SQL Server         │
└──────────────────────────┘
```

---

# 1. What is DAL?

**DAL = Data Access Layer**

The DAL is responsible for communicating with the database.

It should contain things such as:

```text
SQL queries
Stored procedure calls
SqlConnection
SqlCommand
SqlParameter
SqlDataReader
Database mapping
```

The controller should **not** directly contain SQL Server code.

### Bad architecture

```text
Controller
    ↓
SQL Server
```

For example, avoid putting this inside a controller:

```csharp
[HttpGet]
public IActionResult GetEmployees()
{
    using SqlConnection connection = ...;

    using SqlCommand command = ...;

    // SQL code...

    return Ok(...);
}
```

The controller becomes responsible for HTTP **and** database access.

---

# 2. Correct Architecture

Instead:

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
ADO.NET
    ↓
SQL Server
```

Each layer has a clear responsibility.

---

# 3. Responsibility of Each Layer

## Controller

Responsible for:

```text
HTTP request
Route
Model binding
DTO
HTTP response
Status codes
```

Example:

```text
GET /api/employees
```

---

## Service / BAL

BAL = **Business Access Layer**.

Responsible for:

```text
Business rules
Validation beyond basic request validation
Business calculations
Workflow/orchestration
Calling repositories
```

Example:

```text
Salary must be greater than 0
Employee cannot be deleted if business rule prevents it
```

---

## DAL / Repository

Responsible for:

```text
Database communication
SQL
Stored procedures
ADO.NET
Mapping database results
```

---

# 4. Separation of Concerns

This architecture follows **Separation of Concerns**.

Instead of one large class:

```text
EmployeeController
 ├── HTTP
 ├── Business logic
 ├── SQL
 ├── Database connection
 └── Mapping
```

we separate:

```text
EmployeeController
     ↓
HTTP responsibility

EmployeeService
     ↓
Business responsibility

EmployeeRepository
     ↓
Database responsibility
```

This makes the application easier to:

* maintain
* test
* debug
* modify
* extend

---

# 5. Repository Pattern

The **Repository Pattern** provides an abstraction over data access.

Instead of the service knowing:

```csharp
SqlConnection
SqlCommand
SqlDataReader
```

it can simply say:

```csharp
_employeeRepository.GetEmployees();
```

Architecture:

```text
Service
   ↓
IEmployeeRepository
   ↓
EmployeeRepository
   ↓
ADO.NET
   ↓
SQL Server
```

---

# 6. Why Interface?

We'll create:

```csharp
IEmployeeRepository
```

and:

```csharp
EmployeeRepository
```

The interface defines the contract.

Example:

```csharp
public interface IEmployeeRepository
{
    Task<List<Employee>> GetEmployeesAsync();
}
```

Implementation:

```csharp
public class EmployeeRepository : IEmployeeRepository
{
    public async Task<List<Employee>> GetEmployeesAsync()
    {
        // Database code
    }
}
```

The service depends on:

```csharp
IEmployeeRepository
```

rather than:

```csharp
EmployeeRepository
```

This gives us **loose coupling**.

---

# 7. Project Structure

Create a new ASP.NET Core Web API project in Visual Studio.

Project:

```text
Day20DataAccessLayer
```

Recommended structure:

```text
Day20DataAccessLayer
│
├── Controllers
│   └── EmployeesController.cs
│
├── Models
│   └── Employee.cs
│
├── DTOs
│   ├── EmployeeRequestDto.cs
│   └── EmployeeResponseDto.cs
│
├── Repositories
│   ├── IEmployeeRepository.cs
│   └── EmployeeRepository.cs
│
├── Services
│   ├── IEmployeeService.cs
│   └── EmployeeService.cs
│
├── Program.cs
├── appsettings.json
└── Day20DataAccessLayer.csproj
```

---

# 8. Install Microsoft.Data.SqlClient

In Visual Studio:

```text
Project
   ↓
Manage NuGet Packages
   ↓
Browse
   ↓
Microsoft.Data.SqlClient
   ↓
Install
```

Then:

```csharp
using Microsoft.Data.SqlClient;
```

---

# 9. SQL Server Database

We'll use:

```text
EmployeeDB
```

Create:

```sql
CREATE DATABASE EmployeeDB;
GO

USE EmployeeDB;
GO
```

Create Employees:

```sql
CREATE TABLE Employees
(
    EmployeeId INT IDENTITY(1,1)
        CONSTRAINT PK_Employees PRIMARY KEY,

    EmployeeName VARCHAR(100) NOT NULL,

    DepartmentId INT NOT NULL,

    Salary DECIMAL(10,2) NOT NULL,

    Email VARCHAR(150) NULL
        CONSTRAINT UQ_Employees_Email UNIQUE
);
```

Insert sample data:

```sql
INSERT INTO Employees
(
    EmployeeName,
    DepartmentId,
    Salary,
    Email
)
VALUES
('Arun', 1, 50000, 'arun@gmail.com'),
('Priya', 2, 60000, 'priya@gmail.com'),
('Kumar', 1, 55000, 'kumar@gmail.com');
```

---

# 10. Model

Create:

```text
Models/Employee.cs
```

```csharp
namespace Day20DataAccessLayer.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = "";

    public int DepartmentId { get; set; }

    public decimal Salary { get; set; }

    public string? Email { get; set; }
}
```

This represents the database/domain data.

---

# 11. Request DTO

Create:

```text
DTOs/EmployeeRequestDto.cs
```

```csharp
using System.ComponentModel.DataAnnotations;

namespace Day20DataAccessLayer.DTOs;

public class EmployeeRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string EmployeeName { get; set; } = "";

    [Range(1, int.MaxValue)]
    public int DepartmentId { get; set; }

    [Range(1, 10000000)]
    public decimal Salary { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
}
```

---

# 12. Response DTO

Create:

```text
DTOs/EmployeeResponseDto.cs
```

```csharp
namespace Day20DataAccessLayer.DTOs;

public class EmployeeResponseDto
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = "";

    public int DepartmentId { get; set; }

    public decimal Salary { get; set; }

    public string? Email { get; set; }
}
```

Now the API doesn't need to expose the database model directly.

---

# 13. IEmployeeRepository

Create:

```text
Repositories/IEmployeeRepository.cs
```

```csharp
using Day20DataAccessLayer.Models;

namespace Day20DataAccessLayer.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetEmployeesAsync();

    Task<Employee?> GetEmployeeByIdAsync(int id);

    Task<int> AddEmployeeAsync(Employee employee);

    Task<bool> UpdateEmployeeAsync(Employee employee);

    Task<bool> DeleteEmployeeAsync(int id);
}
```

This is the repository contract.

---

# 14. EmployeeRepository

Create:

```text
Repositories/EmployeeRepository.cs
```

```csharp
using Day20DataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace Day20DataAccessLayer.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly string _connectionString;

    public EmployeeRepository(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString(
                "DefaultConnection")
            ?? throw new InvalidOperationException(
                "DefaultConnection is not configured.");
    }

    // Methods...
}
```

Notice something important:

```text
EmployeeRepository
        ↓
IConfiguration
        ↓
appsettings.json
        ↓
Connection String
```

The connection string is not hard-coded inside the repository.

---

# 15. Get All Employees

Add:

```csharp
public async Task<List<Employee>> GetEmployeesAsync()
{
    var employees = new List<Employee>();

    const string sql = """
        SELECT
            EmployeeId,
            EmployeeName,
            DepartmentId,
            Salary,
            Email
        FROM Employees
        ORDER BY EmployeeId;
        """;

    await using SqlConnection connection =
        new SqlConnection(_connectionString);

    await using SqlCommand command =
        new SqlCommand(sql, connection);

    await connection.OpenAsync();

    await using SqlDataReader reader =
        await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        employees.Add(new Employee
        {
            EmployeeId =
                reader.GetInt32(
                    reader.GetOrdinal("EmployeeId")),

            EmployeeName =
                reader.GetString(
                    reader.GetOrdinal("EmployeeName")),

            DepartmentId =
                reader.GetInt32(
                    reader.GetOrdinal("DepartmentId")),

            Salary =
                reader.GetDecimal(
                    reader.GetOrdinal("Salary")),

            Email =
                reader.IsDBNull(
                    reader.GetOrdinal("Email"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Email"))
        });
    }

    return employees;
}
```

Notice that we're now using the **async ADO.NET APIs**:

```text
OpenAsync()
ExecuteReaderAsync()
ReadAsync()
```

This connects directly with your Day 8 `async/await` topic.

---

# 16. Get Employee By ID

```csharp
public async Task<Employee?> GetEmployeeByIdAsync(int id)
{
    const string sql = """
        SELECT
            EmployeeId,
            EmployeeName,
            DepartmentId,
            Salary,
            Email
        FROM Employees
        WHERE EmployeeId = @EmployeeId;
        """;

    await using SqlConnection connection =
        new SqlConnection(_connectionString);

    await using SqlCommand command =
        new SqlCommand(sql, connection);

    command.Parameters.Add(
        "@EmployeeId",
        System.Data.SqlDbType.Int).Value = id;

    await connection.OpenAsync();

    await using SqlDataReader reader =
        await command.ExecuteReaderAsync();

    if (!await reader.ReadAsync())
    {
        return null;
    }

    int emailOrdinal =
        reader.GetOrdinal("Email");

    return new Employee
    {
        EmployeeId =
            reader.GetInt32(
                reader.GetOrdinal("EmployeeId")),

        EmployeeName =
            reader.GetString(
                reader.GetOrdinal("EmployeeName")),

        DepartmentId =
            reader.GetInt32(
                reader.GetOrdinal("DepartmentId")),

        Salary =
            reader.GetDecimal(
                reader.GetOrdinal("Salary")),

        Email =
            reader.IsDBNull(emailOrdinal)
                ? null
                : reader.GetString(emailOrdinal)
    };
}
```

Important:

```csharp
WHERE EmployeeId = @EmployeeId
```

not:

```csharp
WHERE EmployeeId = " + id
```

Always use parameters.

---

# 17. Add Employee

```csharp
public async Task<int> AddEmployeeAsync(
    Employee employee)
{
    const string sql = """
        INSERT INTO Employees
        (
            EmployeeName,
            DepartmentId,
            Salary,
            Email
        )
        OUTPUT INSERTED.EmployeeId
        VALUES
        (
            @EmployeeName,
            @DepartmentId,
            @Salary,
            @Email
        );
        """;

    await using SqlConnection connection =
        new SqlConnection(_connectionString);

    await using SqlCommand command =
        new SqlCommand(sql, connection);

    command.Parameters.Add(
        "@EmployeeName",
        System.Data.SqlDbType.VarChar,
        100).Value =
        employee.EmployeeName;

    command.Parameters.Add(
        "@DepartmentId",
        System.Data.SqlDbType.Int).Value =
        employee.DepartmentId;

    var salaryParameter =
        command.Parameters.Add(
            "@Salary",
            System.Data.SqlDbType.Decimal);

    salaryParameter.Precision = 10;
    salaryParameter.Scale = 2;
    salaryParameter.Value = employee.Salary;

    command.Parameters.Add(
        "@Email",
        System.Data.SqlDbType.VarChar,
        150).Value =
        (object?)employee.Email ?? DBNull.Value;

    await connection.OpenAsync();

    object? result =
        await command.ExecuteScalarAsync();

    return Convert.ToInt32(result);
}
```

Here:

```text
INSERT
 ↓
OUTPUT INSERTED.EmployeeId
 ↓
ExecuteScalarAsync()
 ↓
New Employee ID
```

---

# 18. Update Employee

```csharp
public async Task<bool> UpdateEmployeeAsync(
    Employee employee)
{
    const string sql = """
        UPDATE Employees
        SET
            EmployeeName = @EmployeeName,
            DepartmentId = @DepartmentId,
            Salary = @Salary,
            Email = @Email
        WHERE EmployeeId = @EmployeeId;
        """;

    await using SqlConnection connection =
        new SqlConnection(_connectionString);

    await using SqlCommand command =
        new SqlCommand(sql, connection);

    command.Parameters.Add(
        "@EmployeeId",
        System.Data.SqlDbType.Int).Value =
        employee.EmployeeId;

    command.Parameters.Add(
        "@EmployeeName",
        System.Data.SqlDbType.VarChar,
        100).Value =
        employee.EmployeeName;

    command.Parameters.Add(
        "@DepartmentId",
        System.Data.SqlDbType.Int).Value =
        employee.DepartmentId;

    var salaryParameter =
        command.Parameters.Add(
            "@Salary",
            System.Data.SqlDbType.Decimal);

    salaryParameter.Precision = 10;
    salaryParameter.Scale = 2;
    salaryParameter.Value = employee.Salary;

    command.Parameters.Add(
        "@Email",
        System.Data.SqlDbType.VarChar,
        150).Value =
        (object?)employee.Email ?? DBNull.Value;

    await connection.OpenAsync();

    int rowsAffected =
        await command.ExecuteNonQueryAsync();

    return rowsAffected > 0;
}
```

---

# 19. Delete Employee

```csharp
public async Task<bool> DeleteEmployeeAsync(int id)
{
    const string sql = """
        DELETE FROM Employees
        WHERE EmployeeId = @EmployeeId;
        """;

    await using SqlConnection connection =
        new SqlConnection(_connectionString);

    await using SqlCommand command =
        new SqlCommand(sql, connection);

    command.Parameters.Add(
        "@EmployeeId",
        System.Data.SqlDbType.Int).Value = id;

    await connection.OpenAsync();

    int rowsAffected =
        await command.ExecuteNonQueryAsync();

    return rowsAffected > 0;
}
```

---

# 20. Complete Repository

Your repository now looks like:

```csharp
using System.Data;
using Day20DataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace Day20DataAccessLayer.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly string _connectionString;

    public EmployeeRepository(
        IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString(
                "DefaultConnection")
            ?? throw new InvalidOperationException(
                "DefaultConnection is not configured.");
    }

    public async Task<List<Employee>> GetEmployeesAsync()
    {
        var employees = new List<Employee>();

        const string sql = """
            SELECT
                EmployeeId,
                EmployeeName,
                DepartmentId,
                Salary,
                Email
            FROM Employees
            ORDER BY EmployeeId;
            """;

        await using SqlConnection connection =
            new SqlConnection(_connectionString);

        await using SqlCommand command =
            new SqlCommand(sql, connection);

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            int emailOrdinal =
                reader.GetOrdinal("Email");

            employees.Add(new Employee
            {
                EmployeeId =
                    reader.GetInt32(
                        reader.GetOrdinal("EmployeeId")),

                EmployeeName =
                    reader.GetString(
                        reader.GetOrdinal("EmployeeName")),

                DepartmentId =
                    reader.GetInt32(
                        reader.GetOrdinal("DepartmentId")),

                Salary =
                    reader.GetDecimal(
                        reader.GetOrdinal("Salary")),

                Email =
                    reader.IsDBNull(emailOrdinal)
                        ? null
                        : reader.GetString(emailOrdinal)
            });
        }

        return employees;
    }

    public async Task<Employee?> GetEmployeeByIdAsync(
        int id)
    {
        const string sql = """
            SELECT
                EmployeeId,
                EmployeeName,
                DepartmentId,
                Salary,
                Email
            FROM Employees
            WHERE EmployeeId = @EmployeeId;
            """;

        await using SqlConnection connection =
            new SqlConnection(_connectionString);

        await using SqlCommand command =
            new SqlCommand(sql, connection);

        command.Parameters.Add(
            "@EmployeeId",
            SqlDbType.Int).Value = id;

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        int emailOrdinal =
            reader.GetOrdinal("Email");

        return new Employee
        {
            EmployeeId =
                reader.GetInt32(
                    reader.GetOrdinal("EmployeeId")),

            EmployeeName =
                reader.GetString(
                    reader.GetOrdinal("EmployeeName")),

            DepartmentId =
                reader.GetInt32(
                    reader.GetOrdinal("DepartmentId")),

            Salary =
                reader.GetDecimal(
                    reader.GetOrdinal("Salary")),

            Email =
                reader.IsDBNull(emailOrdinal)
                    ? null
                    : reader.GetString(emailOrdinal)
        };
    }

    public async Task<int> AddEmployeeAsync(
        Employee employee)
    {
        const string sql = """
            INSERT INTO Employees
            (
                EmployeeName,
                DepartmentId,
                Salary,
                Email
            )
            OUTPUT INSERTED.EmployeeId
            VALUES
            (
                @EmployeeName,
                @DepartmentId,
                @Salary,
                @Email
            );
            """;

        await using SqlConnection connection =
            new SqlConnection(_connectionString);

        await using SqlCommand command =
            new SqlCommand(sql, connection);

        command.Parameters.Add(
            "@EmployeeName",
            SqlDbType.VarChar,
            100).Value =
            employee.EmployeeName;

        command.Parameters.Add(
            "@DepartmentId",
            SqlDbType.Int).Value =
            employee.DepartmentId;

        var salaryParameter =
            command.Parameters.Add(
                "@Salary",
                SqlDbType.Decimal);

        salaryParameter.Precision = 10;
        salaryParameter.Scale = 2;
        salaryParameter.Value = employee.Salary;

        command.Parameters.Add(
            "@Email",
            SqlDbType.VarChar,
            150).Value =
            (object?)employee.Email ?? DBNull.Value;

        await connection.OpenAsync();

        object? result =
            await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    public async Task<bool> UpdateEmployeeAsync(
        Employee employee)
    {
        const string sql = """
            UPDATE Employees
            SET
                EmployeeName = @EmployeeName,
                DepartmentId = @DepartmentId,
                Salary = @Salary,
                Email = @Email
            WHERE EmployeeId = @EmployeeId;
            """;

        await using SqlConnection connection =
            new SqlConnection(_connectionString);

        await using SqlCommand command =
            new SqlCommand(sql, connection);

        command.Parameters.Add(
            "@EmployeeId",
            SqlDbType.Int).Value =
            employee.EmployeeId;

        command.Parameters.Add(
            "@EmployeeName",
            SqlDbType.VarChar,
            100).Value =
            employee.EmployeeName;

        command.Parameters.Add(
            "@DepartmentId",
            SqlDbType.Int).Value =
            employee.DepartmentId;

        var salaryParameter =
            command.Parameters.Add(
                "@Salary",
                SqlDbType.Decimal);

        salaryParameter.Precision = 10;
        salaryParameter.Scale = 2;
        salaryParameter.Value = employee.Salary;

        command.Parameters.Add(
            "@Email",
            SqlDbType.VarChar,
            150).Value =
            (object?)employee.Email ?? DBNull.Value;

        await connection.OpenAsync();

        int rowsAffected =
            await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteEmployeeAsync(
        int id)
    {
        const string sql = """
            DELETE FROM Employees
            WHERE EmployeeId = @EmployeeId;
            """;

        await using SqlConnection connection =
            new SqlConnection(_connectionString);

        await using SqlCommand command =
            new SqlCommand(sql, connection);

        command.Parameters.Add(
            "@EmployeeId",
            SqlDbType.Int).Value = id;

        await connection.OpenAsync();

        int rowsAffected =
            await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }
}
```

---

# 21. IEmployeeService

Now create:

```text
Services/IEmployeeService.cs
```

```csharp
using Day20DataAccessLayer.DTOs;

namespace Day20DataAccessLayer.Services;

public interface IEmployeeService
{
    Task<List<EmployeeResponseDto>>
        GetEmployeesAsync();

    Task<EmployeeResponseDto?>
        GetEmployeeByIdAsync(int id);

    Task<EmployeeResponseDto>
        AddEmployeeAsync(EmployeeRequestDto request);

    Task<bool>
        UpdateEmployeeAsync(
            int id,
            EmployeeRequestDto request);

    Task<bool>
        DeleteEmployeeAsync(int id);
}
```

Notice:

```text
Controller
   ↓
IEmployeeService
```

The controller does not know about ADO.NET.

---

# 22. EmployeeService

Create:

```text
Services/EmployeeService.cs
```

```csharp
using Day20DataAccessLayer.DTOs;
using Day20DataAccessLayer.Models;
using Day20DataAccessLayer.Repositories;

namespace Day20DataAccessLayer.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(
        IEmployeeRepository repository)
    {
        _repository = repository;
    }

    // Methods...
}
```

This is **constructor injection**.

---

# 23. Get All Employees in Service

```csharp
public async Task<List<EmployeeResponseDto>>
    GetEmployeesAsync()
{
    var employees =
        await _repository.GetEmployeesAsync();

    return employees
        .Select(MapToResponseDto)
        .ToList();
}
```

Mapping method:

```csharp
private static EmployeeResponseDto
    MapToResponseDto(Employee employee)
{
    return new EmployeeResponseDto
    {
        EmployeeId = employee.EmployeeId,
        EmployeeName = employee.EmployeeName,
        DepartmentId = employee.DepartmentId,
        Salary = employee.Salary,
        Email = employee.Email
    };
}
```

---

# 24. Get Employee By ID

```csharp
public async Task<EmployeeResponseDto?>
    GetEmployeeByIdAsync(int id)
{
    var employee =
        await _repository.GetEmployeeByIdAsync(id);

    if (employee is null)
    {
        return null;
    }

    return MapToResponseDto(employee);
}
```

---

# 25. Add Employee

```csharp
public async Task<EmployeeResponseDto>
    AddEmployeeAsync(EmployeeRequestDto request)
{
    if (request.Salary <= 0)
    {
        throw new ArgumentException(
            "Salary must be greater than zero.");
    }

    var employee = new Employee
    {
        EmployeeName = request.EmployeeName,
        DepartmentId = request.DepartmentId,
        Salary = request.Salary,
        Email = request.Email
    };

    int id =
        await _repository.AddEmployeeAsync(employee);

    employee.EmployeeId = id;

    return MapToResponseDto(employee);
}
```

Notice the separation:

```text
DTO
 ↓
Service
 ↓
Entity
 ↓
Repository
```

---

# 26. Update Employee

```csharp
public async Task<bool>
    UpdateEmployeeAsync(
        int id,
        EmployeeRequestDto request)
{
    if (request.Salary <= 0)
    {
        throw new ArgumentException(
            "Salary must be greater than zero.");
    }

    var employee = new Employee
    {
        EmployeeId = id,
        EmployeeName = request.EmployeeName,
        DepartmentId = request.DepartmentId,
        Salary = request.Salary,
        Email = request.Email
    };

    return await _repository
        .UpdateEmployeeAsync(employee);
}
```

---

# 27. Delete Employee

```csharp
public async Task<bool>
    DeleteEmployeeAsync(int id)
{
    return await _repository
        .DeleteEmployeeAsync(id);
}
```

---

# 28. Complete EmployeeService

```csharp
using Day20DataAccessLayer.DTOs;
using Day20DataAccessLayer.Models;
using Day20DataAccessLayer.Repositories;

namespace Day20DataAccessLayer.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(
        IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<EmployeeResponseDto>>
        GetEmployeesAsync()
    {
        var employees =
            await _repository.GetEmployeesAsync();

        return employees
            .Select(MapToResponseDto)
            .ToList();
    }

    public async Task<EmployeeResponseDto?>
        GetEmployeeByIdAsync(int id)
    {
        var employee =
            await _repository.GetEmployeeByIdAsync(id);

        return employee is null
            ? null
            : MapToResponseDto(employee);
    }

    public async Task<EmployeeResponseDto>
        AddEmployeeAsync(
            EmployeeRequestDto request)
    {
        if (request.Salary <= 0)
        {
            throw new ArgumentException(
                "Salary must be greater than zero.");
        }

        var employee = new Employee
        {
            EmployeeName = request.EmployeeName,
            DepartmentId = request.DepartmentId,
            Salary = request.Salary,
            Email = request.Email
        };

        int id =
            await _repository.AddEmployeeAsync(employee);

        employee.EmployeeId = id;

        return MapToResponseDto(employee);
    }

    public async Task<bool>
        UpdateEmployeeAsync(
            int id,
            EmployeeRequestDto request)
    {
        if (request.Salary <= 0)
        {
            throw new ArgumentException(
                "Salary must be greater than zero.");
        }

        var employee = new Employee
        {
            EmployeeId = id,
            EmployeeName = request.EmployeeName,
            DepartmentId = request.DepartmentId,
            Salary = request.Salary,
            Email = request.Email
        };

        return await _repository
            .UpdateEmployeeAsync(employee);
    }

    public async Task<bool>
        DeleteEmployeeAsync(int id)
    {
        return await _repository
            .DeleteEmployeeAsync(id);
    }

    private static EmployeeResponseDto
        MapToResponseDto(Employee employee)
    {
        return new EmployeeResponseDto
        {
            EmployeeId = employee.EmployeeId,
            EmployeeName = employee.EmployeeName,
            DepartmentId = employee.DepartmentId,
            Salary = employee.Salary,
            Email = employee.Email
        };
    }
}
```

---

# 29. EmployeesController

Create:

```text
Controllers/EmployeesController.cs
```

```csharp
using Day20DataAccessLayer.DTOs;
using Day20DataAccessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Day20DataAccessLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeesController(
        IEmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<
        List<EmployeeResponseDto>>> GetEmployees()
    {
        var employees =
            await _service.GetEmployeesAsync();

        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeResponseDto>>
        GetEmployee(int id)
    {
        var employee =
            await _service.GetEmployeeByIdAsync(id);

        if (employee is null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeResponseDto>>
        CreateEmployee(
            EmployeeRequestDto request)
    {
        var employee =
            await _service.AddEmployeeAsync(request);

        return CreatedAtAction(
            nameof(GetEmployee),
            new { id = employee.EmployeeId },
            employee);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult>
        UpdateEmployee(
            int id,
            EmployeeRequestDto request)
    {
        bool updated =
            await _service.UpdateEmployeeAsync(
                id,
                request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult>
        DeleteEmployee(int id)
    {
        bool deleted =
            await _service.DeleteEmployeeAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
```

Now the controller is very clean.

---

# 30. Program.cs — Dependency Injection

Now register the interfaces.

```csharp
using Day20DataAccessLayer.Repositories;
using Day20DataAccessLayer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<
    IEmployeeRepository,
    EmployeeRepository>();

builder.Services.AddScoped<
    IEmployeeService,
    EmployeeService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

---

# 31. appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EmployeeDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

For SQL Server Express, your server might instead be:

```text
localhost\SQLEXPRESS
```

depending on your installation.

---

# 32. How Dependency Injection Works

When ASP.NET Core sees:

```csharp
public EmployeesController(
    IEmployeeService service)
```

it asks the DI container:

```text
IEmployeeService?
      ↓
EmployeeService
```

Then `EmployeeService` requires:

```csharp
IEmployeeRepository
```

DI resolves:

```text
IEmployeeRepository
      ↓
EmployeeRepository
```

And:

```text
EmployeeRepository
      ↓
IConfiguration
      ↓
appsettings.json
```

So the complete resolution is:

```text
EmployeesController
        ↓
IEmployeeService
        ↓
EmployeeService
        ↓
IEmployeeRepository
        ↓
EmployeeRepository
        ↓
IConfiguration
        ↓
Connection String
```

---

# 33. Complete Request Flow

Suppose Angular/Postman sends:

```http
GET /api/employees/1
```

The flow is:

```text
HTTP Request
     ↓
Kestrel
     ↓
Routing
     ↓
EmployeesController
     ↓
IEmployeeService
     ↓
EmployeeService
     ↓
IEmployeeRepository
     ↓
EmployeeRepository
     ↓
SqlConnection
     ↓
SqlCommand
     ↓
SqlParameter
     ↓
SQL Server
     ↓
SqlDataReader
     ↓
Employee
     ↓
EmployeeService
     ↓
EmployeeResponseDto
     ↓
Controller
     ↓
JSON
     ↓
HTTP Response
```

---

# 34. Why Not Controller → Repository Directly?

You might ask:

```text
Controller
    ↓
Repository
```

Why add Service?

For a very small application, direct access can be technically possible.

But your target architecture is:

```text
Controller
    ↓
Service
    ↓
Repository
```

because the Service layer gives a place for business logic.

Example:

```text
Controller
    ↓
"Create employee"
    ↓
Service
    ├── Validate salary
    ├── Check department rules
    ├── Apply business rules
    └── Call repository
              ↓
          Database
```

The controller shouldn't become the business-logic container.

---

# 35. Separation of Concerns Example

### Controller

```csharp
return Ok(employees);
```

Responsible for HTTP.

### Service

```csharp
if (request.Salary <= 0)
{
    throw new ArgumentException(...);
}
```

Responsible for business rules.

### Repository

```csharp
await command.ExecuteReaderAsync();
```

Responsible for database access.

This is separation of concerns.

---

# 36. Repository Interface = Abstraction

The service sees:

```csharp
IEmployeeRepository
```

It doesn't care whether the implementation uses:

```text
ADO.NET
EF Core
Dapper
Mock Repository
Web API
```

For example:

```csharp
public class EmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(
        IEmployeeRepository repository)
    {
        _repository = repository;
    }
}
```

The service depends on the abstraction.

---

# 37. Testing Advantage

Suppose you want to test:

```text
EmployeeService
```

You don't necessarily want every unit test to hit SQL Server.

You can create:

```csharp
public class FakeEmployeeRepository
    : IEmployeeRepository
{
    // Fake implementation
}
```

Then:

```text
EmployeeService
      ↓
FakeEmployeeRepository
```

instead of:

```text
EmployeeService
      ↓
SQL Server
```

This makes unit testing easier.

---

# 38. Repository Pattern — Simple Definition

### Interview answer

> The Repository Pattern abstracts data-access logic behind an interface, allowing the business/service layer to work with data without depending directly on database-specific implementation details.

---

# 39. Service Layer — Simple Definition

> The Service Layer contains application/business logic and coordinates operations between controllers and repositories.

---

# 40. Separation of Concerns — Simple Definition

> Separation of Concerns means dividing an application into components where each component has a clear responsibility.

In our application:

```text
Controller
→ HTTP

Service
→ Business Logic

Repository
→ Database

SQL Server
→ Data
```

---

# 41. Interface — Simple Definition

> An interface defines a contract that an implementing class must fulfill.

Example:

```csharp
public interface IEmployeeRepository
{
    Task<List<Employee>> GetEmployeesAsync();
}
```

Implementation:

```csharp
public class EmployeeRepository
    : IEmployeeRepository
{
    public async Task<List<Employee>>
        GetEmployeesAsync()
    {
        // implementation
    }
}
```

---

# 42. Dependency Injection — Simple Definition

> Dependency Injection is a technique where a class receives the objects it depends on from an external dependency injection container rather than creating those objects itself.

Instead of:

```csharp
_repository =
    new EmployeeRepository();
```

we use:

```csharp
public EmployeeService(
    IEmployeeRepository repository)
{
    _repository = repository;
}
```

---

# 43. Why `AddScoped`?

For this Web API:

```csharp
builder.Services.AddScoped<
    IEmployeeRepository,
    EmployeeRepository>();

builder.Services.AddScoped<
    IEmployeeService,
    EmployeeService>();
```

`Scoped` creates one instance per DI scope, which for normal ASP.NET Core HTTP request processing is typically one instance per request.

This is a common lifetime for request-oriented application services and repositories.

---

# 44. Full Architecture

This is the architecture you should remember for interviews and real projects:

```text
                         CLIENT
                           │
                           ↓
                    HTTP REQUEST
                           │
                           ↓
                  ┌─────────────────┐
                  │   CONTROLLER    │
                  │                 │
                  │ HTTP / Routing  │
                  │ DTO / Response  │
                  └────────┬────────┘
                           │
                           ↓
                  ┌─────────────────┐
                  │     SERVICE     │
                  │      / BAL      │
                  │                 │
                  │ Business Logic  │
                  └────────┬────────┘
                           │
                           ↓
                  ┌─────────────────┐
                  │   REPOSITORY    │
                  │      / DAL      │
                  │                 │
                  │ Data Access     │
                  └────────┬────────┘
                           │
                           ↓
                  ┌─────────────────┐
                  │    ADO.NET      │
                  │                 │
                  │ SqlConnection   │
                  │ SqlCommand      │
                  │ SqlParameter    │
                  │ SqlDataReader   │
                  └────────┬────────┘
                           │
                           ↓
                  ┌─────────────────┐
                  │   SQL SERVER    │
                  │                 │
                  │ Tables          │
                  │ SPs             │
                  │ Views           │
                  │ Functions       │
                  └─────────────────┘
```

---

# 45. Request/Response Example

### Request

```http
POST /api/employees
Content-Type: application/json
```

```json
{
  "employeeName": "Arun",
  "departmentId": 1,
  "salary": 55000,
  "email": "arun@gmail.com"
}
```

### Flow

```text
JSON
 ↓
EmployeeRequestDto
 ↓
EmployeeService
 ↓
Employee
 ↓
EmployeeRepository
 ↓
ADO.NET
 ↓
SQL Server
 ↓
New EmployeeId
 ↓
EmployeeResponseDto
 ↓
JSON
```

### Response

```json
{
  "employeeId": 4,
  "employeeName": "Arun",
  "departmentId": 1,
  "salary": 55000,
  "email": "arun@gmail.com"
}
```

---

# 46. Important Design Rule

Keep this separation:

```text
❌ Controller
   ↓
   SQL

❌ Controller
   ↓
   SqlConnection
```

Instead:

```text
✅ Controller
      ↓
   Service
      ↓
  Repository
      ↓
    ADO.NET
      ↓
 SQL Server
```

---

# 47. Day 20 Interview Questions

### 1. What is DAL?

DAL stands for Data Access Layer. It contains database-access logic.

### 2. What is Repository Pattern?

It abstracts database/data-access operations behind a repository interface.

### 3. Why use `IEmployeeRepository`?

It provides an abstraction and reduces coupling between the service and the concrete repository implementation.

### 4. Why should controllers not contain SQL?

Because controllers should focus on HTTP concerns. Database access belongs in the data-access layer.

### 5. What is BAL?

BAL means Business Access Layer. It contains business/application logic.

### 6. Why use a Service Layer?

To keep business logic separate from HTTP and database-access code.

### 7. Why use interfaces?

Interfaces provide contracts and abstractions, supporting loose coupling and easier testing/replacement.

### 8. What is constructor injection?

A dependency is supplied through the class constructor.

```csharp
public EmployeeService(
    IEmployeeRepository repository)
{
    _repository = repository;
}
```

### 9. What does `AddScoped` mean?

One service instance is created per DI scope; in normal Web API request processing, this commonly corresponds to one instance per HTTP request.

### 10. Where does ADO.NET belong?

Typically in the DAL/Repository layer.

### 11. Why use DTOs?

To control the API contract and avoid exposing internal/domain/database models directly.

### 12. What is separation of concerns?

Separating different responsibilities into appropriate components.

---

# 48. Day 20 Practice Tasks

Don't just read this day. Build it.

### Task 1

Create:

```text
Day20DataAccessLayer
```

### Task 2

Create:

```text
Models/Employee.cs
```

### Task 3

Create:

```text
DTOs/EmployeeRequestDto.cs
DTOs/EmployeeResponseDto.cs
```

### Task 4

Create:

```text
Repositories/IEmployeeRepository.cs
Repositories/EmployeeRepository.cs
```

Implement:

```text
GetEmployeesAsync()
GetEmployeeByIdAsync()
AddEmployeeAsync()
UpdateEmployeeAsync()
DeleteEmployeeAsync()
```

### Task 5

Create:

```text
Services/IEmployeeService.cs
Services/EmployeeService.cs
```

### Task 6

Create:

```text
Controllers/EmployeesController.cs
```

### Task 7

Configure:

```text
appsettings.json
```

with SQL Server connection string.

### Task 8

Configure DI:

```csharp
builder.Services.AddScoped<
    IEmployeeRepository,
    EmployeeRepository>();

builder.Services.AddScoped<
    IEmployeeService,
    EmployeeService>();
```

### Task 9

Test:

```text
GET     /api/employees
GET     /api/employees/1
POST    /api/employees
PUT     /api/employees/1
DELETE  /api/employees/1
```

---

# 49. Day 20 Final Cheat Sheet

```text
DAL
→ Database access

Repository
→ Abstracts data access

Service / BAL
→ Business logic

Controller
→ HTTP/API

Interface
→ Contract

DI
→ Supplies dependencies

ADO.NET
→ Database communication
```

### Final architecture

```text
Controller
     ↓
IEmployeeService
     ↓
EmployeeService
     ↓
IEmployeeRepository
     ↓
EmployeeRepository
     ↓
ADO.NET
     ↓
SQL Server
```

### Responsibility map

```text
┌────────────────────┬─────────────────────────────┐
│ Layer              │ Responsibility              │
├────────────────────┼─────────────────────────────┤
│ Controller         │ HTTP / API                  │
│ Service / BAL      │ Business logic              │
│ Repository / DAL   │ Database access             │
│ ADO.NET            │ SQL Server communication    │
│ SQL Server         │ Data storage/query          │
└────────────────────┴─────────────────────────────┘
```

### Most important concept

```text
Controller should NOT know how SQL works.

Service should NOT know how SqlConnection works.

Repository should handle database access.

Interfaces should separate the layers.

Dependency Injection connects the layers.
```

So your Day 20 architecture is:

```text
                    ┌───────────────┐
                    │  Controller   │
                    └───────┬───────┘
                            ↓
                    ┌───────────────┐
                    │    Service    │
                    │     / BAL     │
                    └───────┬───────┘
                            ↓
                    ┌───────────────┐
                    │   Repository  │
                    │     / DAL     │
                    └───────┬───────┘
                            ↓
                    ┌───────────────┐
                    │    ADO.NET    │
                    └───────┬───────┘
                            ↓
                    ┌───────────────┐
                    │  SQL Server   │
                    └───────────────┘
```


