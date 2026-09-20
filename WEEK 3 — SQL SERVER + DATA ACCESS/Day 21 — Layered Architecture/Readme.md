# Day 21 — Layered Architecture

Today we will combine everything learned so far into a **proper real-world ASP.NET Core Web API project**.

The main goal is to understand how a professional .NET application separates responsibilities:

```text
Client
   ↓
Controller
   ↓
Service / BAL
   ↓
Repository / DAL
   ↓
ADO.NET
   ↓
SQL Server
```

We will build an **Employee Management API** with:

* CRUD
* Search
* Filtering
* Pagination
* Sorting
* Validation
* Error handling
* Logging
* Dependency Injection
* ADO.NET
* SQL Server
* DTOs
* Middleware

---

# 1. What is Layered Architecture?

Layered architecture divides an application into separate layers.

Instead of putting everything inside the controller:

```text
Controller
 ├── SQL query
 ├── Database connection
 ├── Business logic
 ├── Validation
 ├── Logging
 └── Response
```

we separate responsibilities:

```text
Controller
   ↓
Service
   ↓
Repository
   ↓
Database
```

### Main benefit

Each layer has **one clear responsibility**.

For example:

```text
Controller  → HTTP/API responsibility
Service     → Business logic
Repository  → Database responsibility
Middleware  → Cross-cutting concerns
```

---

# 2. Final Project Architecture

Create the project like this:

```text
EmployeeManagement
│
├── Controllers
│   └── EmployeesController.cs
│
├── Services
│   ├── Interfaces
│   │   └── IEmployeeService.cs
│   │
│   └── Implementations
│       └── EmployeeService.cs
│
├── DAL
│   ├── Interfaces
│   │   └── IEmployeeRepository.cs
│   │
│   └── Implementations
│       └── EmployeeRepository.cs
│
├── Models
│   └── Employee.cs
│
├── DTOs
│   ├── EmployeeRequestDto.cs
│   ├── EmployeeResponseDto.cs
│   └── EmployeeQueryDto.cs
│
├── Middleware
│   └── GlobalExceptionMiddleware.cs
│
├── Helpers
│   └── PaginationHelper.cs
│
├── appsettings.json
├── Program.cs
└── EmployeeManagement.csproj
```

---

# 3. Create Project in Visual Studio

Since you are using **Visual Studio**:

### Step 1

Open Visual Studio.

### Step 2

Select:

```text
Create a new project
```

### Step 3

Select:

```text
ASP.NET Core Web API
```

### Step 4

Project name:

```text
EmployeeManagement
```

### Step 5

Select:

```text
.NET 8.0
```

### Step 6

Create the project.

---

# 4. Remove Unnecessary Default Files

Depending on the Visual Studio template, you may get:

```text
WeatherForecast.cs
WeatherForecastController.cs
```

You can delete them because we are creating our own Employee API.

---

# 5. Create Folders

In Solution Explorer:

```text
Right Click Project
→ Add
→ New Folder
```

Create:

```text
Controllers
Services
DAL
Models
DTOs
Middleware
Helpers
```

Inside `Services`:

```text
Interfaces
Implementations
```

Inside `DAL`:

```text
Interfaces
Implementations
```

Final structure:

```text
EmployeeManagement
│
├── Controllers
│
├── Services
│   ├── Interfaces
│   └── Implementations
│
├── DAL
│   ├── Interfaces
│   └── Implementations
│
├── Models
│
├── DTOs
│
├── Middleware
│
├── Helpers
│
├── Program.cs
└── appsettings.json
```

---

# 6. Database Design

We will use SQL Server.

Create database:

```sql
CREATE DATABASE EmployeeDB;
GO

USE EmployeeDB;
GO
```

Create table:

```sql
CREATE TABLE Employees
(
    EmployeeId INT IDENTITY(1,1)
        CONSTRAINT PK_Employees PRIMARY KEY,

    EmployeeName VARCHAR(100) NOT NULL,

    Department VARCHAR(100) NOT NULL,

    Salary DECIMAL(10,2) NOT NULL,

    Email VARCHAR(150) NULL
        CONSTRAINT UQ_Employees_Email UNIQUE,

    IsActive BIT NOT NULL
        CONSTRAINT DF_Employees_IsActive DEFAULT 1,

    CreatedDate DATETIME2 NOT NULL
        CONSTRAINT DF_Employees_CreatedDate DEFAULT SYSDATETIME()
);
```

---

# 7. Insert Sample Data

```sql
INSERT INTO Employees
(EmployeeName, Department, Salary, Email)
VALUES
('Arun', 'IT', 50000, 'arun@gmail.com'),
('Priya', 'HR', 60000, 'priya@gmail.com'),
('Kumar', 'IT', 55000, 'kumar@gmail.com'),
('Divya', 'Finance', 65000, 'divya@gmail.com'),
('Rahul', 'IT', 70000, 'rahul@gmail.com'),
('Meena', 'HR', 58000, 'meena@gmail.com'),
('Suresh', 'Finance', 72000, 'suresh@gmail.com');
```

---

# 8. Install ADO.NET Package

For modern .NET applications, use:

```text
Microsoft.Data.SqlClient
```

In Visual Studio:

```text
Project
→ Manage NuGet Packages
→ Browse
→ Microsoft.Data.SqlClient
→ Install
```

---

# 9. Connection String

Open:

```text
appsettings.json
```

Add:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EmployeeDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

If your SQL Server is SQL Express:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=EmployeeDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

---

# 10. Model

Create:

```text
Models
└── Employee.cs
```

```csharp
namespace EmployeeManagement.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = "";

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }
}
```

The **Model** represents our domain/data object.

---

# 11. Request DTO

Create:

```text
DTOs
└── EmployeeRequestDto.cs
```

```csharp
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.DTOs;

public class EmployeeRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string EmployeeName { get; set; } = "";

    [Required]
    [StringLength(100)]
    public string Department { get; set; } = "";

    [Range(1, 10000000)]
    public decimal Salary { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
}
```

This DTO is used for:

```text
POST
PUT
```

---

# 12. Response DTO

Create:

```text
DTOs
└── EmployeeResponseDto.cs
```

```csharp
namespace EmployeeManagement.DTOs;

public class EmployeeResponseDto
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = "";

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }
}
```

We don't expose the entity directly from the API.

Instead:

```text
Entity
 ↓
Service
 ↓
Response DTO
 ↓
Client
```

---

# 13. Query DTO

For search, filtering, sorting and pagination, create:

```text
DTOs
└── EmployeeQueryDto.cs
```

```csharp
namespace EmployeeManagement.DTOs;

public class EmployeeQueryDto
{
    public string? Search { get; set; }

    public string? Department { get; set; }

    public decimal? MinSalary { get; set; }

    public decimal? MaxSalary { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string SortBy { get; set; } = "EmployeeName";

    public string SortOrder { get; set; } = "asc";
}
```

This allows URLs such as:

```text
GET /api/employees?search=arun
```

or:

```text
GET /api/employees?department=IT
```

or:

```text
GET /api/employees?pageNumber=2&pageSize=5
```

or:

```text
GET /api/employees?sortBy=Salary&sortOrder=desc
```

---

# 14. Repository Interface

Create:

```text
DAL
└── Interfaces
    └── IEmployeeRepository.cs
```

```csharp
using EmployeeManagement.DTOs;
using EmployeeManagement.Models;

namespace EmployeeManagement.DAL.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetEmployeesAsync(
        EmployeeQueryDto query);

    Task<Employee?> GetEmployeeByIdAsync(int id);

    Task<int> AddEmployeeAsync(Employee employee);

    Task<bool> UpdateEmployeeAsync(Employee employee);

    Task<bool> DeleteEmployeeAsync(int id);
}
```

This interface defines what the DAL can do.

The service does not need to know how SQL works.

---

# 15. Repository Implementation

Create:

```text
DAL
└── Implementations
    └── EmployeeRepository.cs
```

```csharp
using System.Data;
using EmployeeManagement.DAL.Interfaces;
using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeManagement.DAL.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly string _connectionString;

    public EmployeeRepository(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Database connection string is missing.");
    }

    public async Task<List<Employee>> GetEmployeesAsync(
        EmployeeQueryDto query)
    {
        var employees = new List<Employee>();

        await using SqlConnection connection =
            new(_connectionString);

        await connection.OpenAsync();

        string sql = """
            SELECT
                EmployeeId,
                EmployeeName,
                Department,
                Salary,
                Email,
                IsActive,
                CreatedDate
            FROM Employees
            WHERE IsActive = 1
              AND (@Search IS NULL
                   OR EmployeeName LIKE '%' + @Search + '%'
                   OR Email LIKE '%' + @Search + '%')
              AND (@Department IS NULL
                   OR Department = @Department)
              AND (@MinSalary IS NULL
                   OR Salary >= @MinSalary)
              AND (@MaxSalary IS NULL
                   OR Salary <= @MaxSalary)
            ORDER BY EmployeeName
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY;
            """;

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add("@Search", SqlDbType.VarChar, 100)
            .Value = (object?)query.Search ?? DBNull.Value;

        command.Parameters.Add("@Department", SqlDbType.VarChar, 100)
            .Value = (object?)query.Department ?? DBNull.Value;

        var minSalary = command.Parameters.Add(
            "@MinSalary",
            SqlDbType.Decimal);

        minSalary.Precision = 10;
        minSalary.Scale = 2;
        minSalary.Value =
            (object?)query.MinSalary ?? DBNull.Value;

        var maxSalary = command.Parameters.Add(
            "@MaxSalary",
            SqlDbType.Decimal);

        maxSalary.Precision = 10;
        maxSalary.Scale = 2;
        maxSalary.Value =
            (object?)query.MaxSalary ?? DBNull.Value;

        command.Parameters.Add(
            "@Offset",
            SqlDbType.Int).Value =
            (query.PageNumber - 1) * query.PageSize;

        command.Parameters.Add(
            "@PageSize",
            SqlDbType.Int).Value =
            query.PageSize;

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            employees.Add(new Employee
            {
                EmployeeId = reader.GetInt32(
                    reader.GetOrdinal("EmployeeId")),

                EmployeeName = reader.GetString(
                    reader.GetOrdinal("EmployeeName")),

                Department = reader.GetString(
                    reader.GetOrdinal("Department")),

                Salary = reader.GetDecimal(
                    reader.GetOrdinal("Salary")),

                Email = reader.IsDBNull(
                    reader.GetOrdinal("Email"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Email")),

                IsActive = reader.GetBoolean(
                    reader.GetOrdinal("IsActive")),

                CreatedDate = reader.GetDateTime(
                    reader.GetOrdinal("CreatedDate"))
            });
        }

        return employees;
    }
```

Continue the repository with CRUD methods:

```csharp
    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        await using SqlConnection connection =
            new(_connectionString);

        await connection.OpenAsync();

        const string sql = """
            SELECT
                EmployeeId,
                EmployeeName,
                Department,
                Salary,
                Email,
                IsActive,
                CreatedDate
            FROM Employees
            WHERE EmployeeId = @EmployeeId
              AND IsActive = 1;
            """;

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add(
            "@EmployeeId",
            SqlDbType.Int).Value = id;

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new Employee
        {
            EmployeeId = reader.GetInt32(
                reader.GetOrdinal("EmployeeId")),

            EmployeeName = reader.GetString(
                reader.GetOrdinal("EmployeeName")),

            Department = reader.GetString(
                reader.GetOrdinal("Department")),

            Salary = reader.GetDecimal(
                reader.GetOrdinal("Salary")),

            Email = reader.IsDBNull(
                reader.GetOrdinal("Email"))
                ? null
                : reader.GetString(
                    reader.GetOrdinal("Email")),

            IsActive = reader.GetBoolean(
                reader.GetOrdinal("IsActive")),

            CreatedDate = reader.GetDateTime(
                reader.GetOrdinal("CreatedDate"))
        };
    }

    public async Task<int> AddEmployeeAsync(Employee employee)
    {
        await using SqlConnection connection =
            new(_connectionString);

        await connection.OpenAsync();

        const string sql = """
            INSERT INTO Employees
            (
                EmployeeName,
                Department,
                Salary,
                Email
            )
            OUTPUT INSERTED.EmployeeId
            VALUES
            (
                @EmployeeName,
                @Department,
                @Salary,
                @Email
            );
            """;

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add(
            "@EmployeeName",
            SqlDbType.VarChar, 100).Value =
            employee.EmployeeName;

        command.Parameters.Add(
            "@Department",
            SqlDbType.VarChar, 100).Value =
            employee.Department;

        var salaryParameter = command.Parameters.Add(
            "@Salary",
            SqlDbType.Decimal);

        salaryParameter.Precision = 10;
        salaryParameter.Scale = 2;
        salaryParameter.Value = employee.Salary;

        command.Parameters.Add(
            "@Email",
            SqlDbType.VarChar, 150).Value =
            (object?)employee.Email ?? DBNull.Value;

        object? result =
            await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    public async Task<bool> UpdateEmployeeAsync(
        Employee employee)
    {
        await using SqlConnection connection =
            new(_connectionString);

        await connection.OpenAsync();

        const string sql = """
            UPDATE Employees
            SET
                EmployeeName = @EmployeeName,
                Department = @Department,
                Salary = @Salary,
                Email = @Email
            WHERE EmployeeId = @EmployeeId
              AND IsActive = 1;
            """;

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add(
            "@EmployeeId",
            SqlDbType.Int).Value =
            employee.EmployeeId;

        command.Parameters.Add(
            "@EmployeeName",
            SqlDbType.VarChar, 100).Value =
            employee.EmployeeName;

        command.Parameters.Add(
            "@Department",
            SqlDbType.VarChar, 100).Value =
            employee.Department;

        var salaryParameter = command.Parameters.Add(
            "@Salary",
            SqlDbType.Decimal);

        salaryParameter.Precision = 10;
        salaryParameter.Scale = 2;
        salaryParameter.Value = employee.Salary;

        command.Parameters.Add(
            "@Email",
            SqlDbType.VarChar, 150).Value =
            (object?)employee.Email ?? DBNull.Value;

        int rowsAffected =
            await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        await using SqlConnection connection =
            new(_connectionString);

        await connection.OpenAsync();

        const string sql = """
            UPDATE Employees
            SET IsActive = 0
            WHERE EmployeeId = @EmployeeId
              AND IsActive = 1;
            """;

        await using SqlCommand command =
            new(sql, connection);

        command.Parameters.Add(
            "@EmployeeId",
            SqlDbType.Int).Value = id;

        int rowsAffected =
            await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }
}
```

Notice something important:

```text
Repository
   ↓
SqlConnection
SqlCommand
SqlParameter
SqlDataReader
```

Only the DAL knows these details.

---

# 16. Why Search/Filtering Should Be in SQL

We could retrieve every employee:

```text
Database
   ↓
100,000 records
   ↓
.NET
   ↓
filter
```

That is inefficient.

Instead:

```text
Client
 ↓
search/filter
 ↓
SQL Server
 ↓
only required records
 ↓
.NET
```

This becomes especially important when working with large databases.

---

# 17. Sorting — Important Security Point

Never directly concatenate user input into SQL:

```csharp
string sql =
    $"SELECT * FROM Employees ORDER BY {query.SortBy}";
```

This is dangerous because SQL identifiers cannot be parameterized in the same way as values.

Use a **whitelist**.

For example:

```csharp
string sortColumn = query.SortBy.ToLower() switch
{
    "name" => "EmployeeName",
    "department" => "Department",
    "salary" => "Salary",
    "createddate" => "CreatedDate",
    _ => "EmployeeName"
};

string sortDirection =
    query.SortOrder.Equals(
        "desc",
        StringComparison.OrdinalIgnoreCase)
        ? "DESC"
        : "ASC";
```

Then construct SQL only from these controlled values.

For a production implementation, the repository query would use:

```sql
ORDER BY EmployeeName ASC
```

or another whitelisted column/direction.

---

# 18. Pagination

Pagination means returning data in smaller pages.

Suppose we have:

```text
1000 employees
```

Instead of:

```text
GET /api/employees
```

returning all 1000:

```text
Page 1 → 10 employees
Page 2 → 10 employees
Page 3 → 10 employees
```

SQL Server uses:

```sql
OFFSET @Offset ROWS
FETCH NEXT @PageSize ROWS ONLY
```

Formula:

```text
Offset = (PageNumber - 1) × PageSize
```

Example:

```text
PageNumber = 3
PageSize   = 10

Offset = (3 - 1) × 10
       = 20
```

Therefore:

```text
skip first 20
take next 10
```

---

# 19. Service Interface

Create:

```text
Services
└── Interfaces
    └── IEmployeeService.cs
```

```csharp
using EmployeeManagement.DTOs;

namespace EmployeeManagement.Services.Interfaces;

public interface IEmployeeService
{
    Task<List<EmployeeResponseDto>> GetEmployeesAsync(
        EmployeeQueryDto query);

    Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id);

    Task<EmployeeResponseDto> CreateEmployeeAsync(
        EmployeeRequestDto request);

    Task<bool> UpdateEmployeeAsync(
        int id,
        EmployeeRequestDto request);

    Task<bool> DeleteEmployeeAsync(int id);
}
```

---

# 20. Service Implementation

Create:

```text
Services
└── Implementations
    └── EmployeeService.cs
```

```csharp
using EmployeeManagement.DAL.Interfaces;
using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
using EmployeeManagement.Services.Interfaces;

namespace EmployeeManagement.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IEmployeeRepository repository,
        ILogger<EmployeeService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<EmployeeResponseDto>>
        GetEmployeesAsync(EmployeeQueryDto query)
    {
        _logger.LogInformation(
            "Getting employees. Page: {Page}, Size: {Size}",
            query.PageNumber,
            query.PageSize);

        var employees =
            await _repository.GetEmployeesAsync(query);

        return employees
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<EmployeeResponseDto?>
        GetEmployeeByIdAsync(int id)
    {
        _logger.LogInformation(
            "Getting employee {EmployeeId}",
            id);

        var employee =
            await _repository.GetEmployeeByIdAsync(id);

        return employee == null
            ? null
            : MapToResponse(employee);
    }

    public async Task<EmployeeResponseDto>
        CreateEmployeeAsync(EmployeeRequestDto request)
    {
        if (request.Salary <= 0)
        {
            throw new ArgumentException(
                "Salary must be greater than zero.");
        }

        var employee = new Employee
        {
            EmployeeName = request.EmployeeName,
            Department = request.Department,
            Salary = request.Salary,
            Email = request.Email,
            IsActive = true
        };

        int employeeId =
            await _repository.AddEmployeeAsync(employee);

        employee.EmployeeId = employeeId;

        _logger.LogInformation(
            "Employee {EmployeeId} created",
            employeeId);

        return MapToResponse(employee);
    }

    public async Task<bool> UpdateEmployeeAsync(
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
            Department = request.Department,
            Salary = request.Salary,
            Email = request.Email,
            IsActive = true
        };

        bool result =
            await _repository.UpdateEmployeeAsync(employee);

        if (result)
        {
            _logger.LogInformation(
                "Employee {EmployeeId} updated",
                id);
        }

        return result;
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        bool result =
            await _repository.DeleteEmployeeAsync(id);

        if (result)
        {
            _logger.LogInformation(
                "Employee {EmployeeId} deleted",
                id);
        }

        return result;
    }

    private static EmployeeResponseDto MapToResponse(
        Employee employee)
    {
        return new EmployeeResponseDto
        {
            EmployeeId = employee.EmployeeId,
            EmployeeName = employee.EmployeeName,
            Department = employee.Department,
            Salary = employee.Salary,
            Email = employee.Email,
            IsActive = employee.IsActive,
            CreatedDate = employee.CreatedDate
        };
    }
}
```

---

# 21. Controller

Create:

```text
Controllers
└── EmployeesController.cs
```

```csharp
using EmployeeManagement.DTOs;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers;

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
    public async Task<ActionResult<List<EmployeeResponseDto>>>
        GetEmployees([FromQuery] EmployeeQueryDto query)
    {
        var employees =
            await _service.GetEmployeesAsync(query);

        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeResponseDto>>
        GetEmployee(int id)
    {
        var employee =
            await _service.GetEmployeeByIdAsync(id);

        if (employee == null)
            return NotFound(
                new { message = "Employee not found." });

        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeResponseDto>>
        CreateEmployee(EmployeeRequestDto request)
    {
        var employee =
            await _service.CreateEmployeeAsync(request);

        return CreatedAtAction(
            nameof(GetEmployee),
            new { id = employee.EmployeeId },
            employee);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEmployee(
        int id,
        EmployeeRequestDto request)
    {
        bool updated =
            await _service.UpdateEmployeeAsync(
                id,
                request);

        if (!updated)
            return NotFound(
                new { message = "Employee not found." });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEmployee(
        int id)
    {
        bool deleted =
            await _service.DeleteEmployeeAsync(id);

        if (!deleted)
            return NotFound(
                new { message = "Employee not found." });

        return NoContent();
    }
}
```

---

# 22. CRUD Endpoints

Now our API provides:

### Get all

```http
GET /api/employees
```

### Get one

```http
GET /api/employees/5
```

### Create

```http
POST /api/employees
```

Body:

```json
{
  "employeeName": "Anitha",
  "department": "IT",
  "salary": 65000,
  "email": "anitha@gmail.com"
}
```

### Update

```http
PUT /api/employees/5
```

### Delete

```http
DELETE /api/employees/5
```

---

# 23. Search

Example:

```http
GET /api/employees?search=Arun
```

SQL logic:

```sql
EmployeeName LIKE '%Arun%'
OR Email LIKE '%Arun%'
```

---

# 24. Filtering

Department:

```http
GET /api/employees?department=IT
```

Salary:

```http
GET /api/employees?minSalary=50000
```

Salary range:

```http
GET /api/employees?minSalary=50000&maxSalary=70000
```

Combined:

```http
GET /api/employees?department=IT&minSalary=50000
```

---

# 25. Pagination

```http
GET /api/employees?pageNumber=1&pageSize=5
```

Second page:

```http
GET /api/employees?pageNumber=2&pageSize=5
```

Third page:

```http
GET /api/employees?pageNumber=3&pageSize=5
```

---

# 26. Sorting

Examples:

```http
GET /api/employees?sortBy=salary&sortOrder=asc
```

```http
GET /api/employees?sortBy=salary&sortOrder=desc
```

```http
GET /api/employees?sortBy=name&sortOrder=asc
```

```http
GET /api/employees?sortBy=department&sortOrder=desc
```

The repository should whitelist allowed sort columns.

---

# 27. Validation

Our DTO already contains:

```csharp
[Required]
[StringLength(100, MinimumLength = 2)]
[Range(1, 10000000)]
[EmailAddress]
```

For example:

```json
{
  "employeeName": "",
  "department": "IT",
  "salary": -100,
  "email": "wrong-email"
}
```

With:

```csharp
[ApiController]
```

ASP.NET Core automatically performs model validation and can return:

```text
400 Bad Request
```

---

# 28. Global Exception Handling

Instead of writing:

```csharp
try
{
    ...
}
catch
{
    ...
}
```

inside every controller, create centralized middleware.

Create:

```text
Middleware
└── GlobalExceptionMiddleware.cs
```

```csharp
using System.Net;
using System.Text.Json;

namespace EmployeeManagement.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception occurred.");

            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            context.Response.ContentType =
                "application/json";

            var response = new
            {
                message = "An unexpected error occurred."
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
```

---

# 29. Why Global Exception Middleware?

Without centralized handling:

```text
Controller 1 → try/catch
Controller 2 → try/catch
Controller 3 → try/catch
Controller 4 → try/catch
```

Lots of repeated code.

With middleware:

```text
                    ┌───────────────────┐
Request ───────────→│ Exception         │
                    │ Middleware        │
                    └─────────┬─────────┘
                              ↓
                         Controller
```

One place handles unexpected exceptions.

---

# 30. Program.cs

Now connect everything using Dependency Injection.

```csharp
using EmployeeManagement.DAL.Implementations;
using EmployeeManagement.DAL.Interfaces;
using EmployeeManagement.Middleware;
using EmployeeManagement.Services.Implementations;
using EmployeeManagement.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<
    IEmployeeRepository,
    EmployeeRepository>();

builder.Services.AddScoped<
    IEmployeeService,
    EmployeeService>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

---

# 31. Final Dependency Injection Flow

When a request comes in:

```text
GET /api/employees
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
SQL Server
```

ASP.NET Core DI creates these objects for us.

---

# 32. Complete Request Flow

This is the most important diagram for Day 21:

```text
                    CLIENT
                      │
                      ▼
               HTTP REQUEST
                      │
                      ▼
             ASP.NET CORE / KESTREL
                      │
                      ▼
              MIDDLEWARE
                      │
                      ▼
                CONTROLLER
                      │
                      ▼
              SERVICE / BAL
                      │
                      ▼
             REPOSITORY / DAL
                      │
                      ▼
                 ADO.NET
                      │
              ┌───────┴────────┐
              │                │
       SqlConnection       SqlCommand
              │                │
              └───────┬────────┘
                      ▼
                 SQL SERVER
                      │
                      ▼
                   TABLE
                      │
                      ▼
                 DATA READER
                      │
                      ▼
                 REPOSITORY
                      │
                      ▼
                  SERVICE
                      │
                 Entity → DTO
                      │
                      ▼
                 CONTROLLER
                      │
                      ▼
                  JSON
                      │
                      ▼
               HTTP RESPONSE
                      │
                      ▼
                   CLIENT
```

---

# 33. What Each Layer Does

### Controller

```text
HTTP responsibility
```

Handles:

* Routing
* HTTP methods
* Request
* Response
* Status codes
* DTO binding

Should **not** contain SQL.

---

### Service / BAL

```text
Business responsibility
```

Handles:

* Business rules
* Validation beyond basic model validation
* Workflow
* Mapping
* Calling repositories

Should **not** directly use `SqlConnection`.

---

### Repository / DAL

```text
Database responsibility
```

Handles:

* SQL
* Stored procedures
* ADO.NET
* Connections
* Parameters
* DataReader
* Mapping database records

---

### DTO

```text
API data contract
```

Controls what enters and leaves the API.

---

### Middleware

```text
Cross-cutting responsibility
```

Examples:

* Exception handling
* Request logging
* Authentication
* Authorization
* CORS

---

# 34. Separation of Concerns

This is the main concept of Day 21.

Bad:

```text
Controller
 ├── SQL
 ├── Database connection
 ├── Business logic
 ├── Validation
 ├── Logging
 └── HTTP response
```

Good:

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
Database
```

Each layer has a specific job.

---

# 35. Why Interfaces?

We use:

```csharp
IEmployeeService
IEmployeeRepository
```

instead of directly depending on:

```csharp
EmployeeService
EmployeeRepository
```

This gives loose coupling.

For example:

```csharp
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeesController(
        IEmployeeService service)
    {
        _service = service;
    }
}
```

The controller doesn't care whether the implementation uses:

```text
SQL Server
```

or:

```text
another database
```

or:

```text
mock repository for testing
```

---

# 36. Logging

We already inject:

```csharp
ILogger<EmployeeService>
```

Example:

```csharp
_logger.LogInformation(
    "Employee {EmployeeId} created",
    employeeId);
```

Error:

```csharp
_logger.LogError(
    ex,
    "Error while creating employee");
```

Warning:

```csharp
_logger.LogWarning(
    "Employee {EmployeeId} was not found",
    id);
```

Logging should use structured placeholders rather than manually building strings.

---

# 37. API Testing

You can test the API using:

```text
Swagger
```

or:

```text
Postman
```

### GET

```http
GET /api/employees
```

### Search

```http
GET /api/employees?search=Priya
```

### Filter

```http
GET /api/employees?department=IT
```

### Pagination

```http
GET /api/employees?pageNumber=1&pageSize=5
```

### Sort

```http
GET /api/employees?sortBy=salary&sortOrder=desc
```

### Create

```http
POST /api/employees
```

Body:

```json
{
  "employeeName": "Vijay",
  "department": "IT",
  "salary": 75000,
  "email": "vijay@gmail.com"
}
```

---

# 38. Important HTTP Status Codes

Our API can return:

```text
200 OK
```

Successful GET.

```text
201 Created
```

Successful POST.

```text
204 No Content
```

Successful update/delete without response body.

```text
400 Bad Request
```

Invalid request.

```text
404 Not Found
```

Employee doesn't exist.

```text
500 Internal Server Error
```

Unexpected server-side error.

---

# 39. Day 21 Architecture Cheat Sheet

Remember this:

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

### Controller

```text
HTTP
```

### Service

```text
Business Logic
```

### Repository

```text
Database
```

### DTO

```text
API Contract
```

### Model

```text
Domain/Data Object
```

### Middleware

```text
Cross-Cutting Concerns
```

### DI

```text
Connects all layers
```

---

# 40. Day 21 Interview Questions

### 1. What is layered architecture?

An architecture that separates an application into layers based on responsibilities, such as Controller, Service, and Data Access layers.

### 2. What is the responsibility of a Controller?

Handling HTTP requests, routing, model binding and HTTP responses.

### 3. What is the responsibility of a Service?

Implementing business/application logic and coordinating operations between controllers and repositories.

### 4. What is the responsibility of a Repository?

Handling data-access operations such as SQL queries, stored procedures and ADO.NET.

### 5. Why should controllers not contain SQL?

To maintain separation of concerns, testability and maintainability.

### 6. Why use interfaces?

To reduce coupling and allow implementations to be replaced or mocked.

### 7. What is BAL?

Business Access Layer. It generally represents the service/business logic layer.

### 8. What is DAL?

Data Access Layer. It handles communication with the database.

### 9. What is DTO?

Data Transfer Object. It defines the data exchanged between the API and clients.

### 10. Why use DTO instead of exposing Entity directly?

DTOs allow you to control the API contract and avoid unnecessarily exposing internal/domain/database structure.

### 11. What is pagination?

Dividing a large result set into smaller pages.

```text
Page 1 → records 1–10
Page 2 → records 11–20
```

### 12. What is filtering?

Restricting records based on criteria.

```text
Department = IT
```

### 13. What is searching?

Finding records based on text or other search criteria.

```text
EmployeeName contains "Arun"
```

### 14. What is repository pattern?

A pattern that abstracts data-access operations behind an interface.

### 15. What is dependency injection?

Providing a class with the dependencies it needs instead of having the class create those dependencies itself.

### 16. Why use middleware for global exception handling?

It centralizes unexpected exception handling instead of duplicating `try/catch` logic across controllers.

---

# 41. Day 21 — Final Architecture to Remember

```text
                 ┌───────────────┐
                 │    CLIENT     │
                 │ Angular/Postman│
                 └───────┬───────┘
                         │
                         ▼
                 ┌───────────────┐
                 │  Middleware   │
                 │ Error/Logging │
                 └───────┬───────┘
                         │
                         ▼
                 ┌───────────────┐
                 │  Controller   │
                 │ HTTP / DTO    │
                 └───────┬───────┘
                         │
                         ▼
                 ┌───────────────┐
                 │    Service    │
                 │ Business Logic│
                 └───────┬───────┘
                         │
                         ▼
                 ┌───────────────┐
                 │  Repository   │
                 │     DAL       │
                 └───────┬───────┘
                         │
                         ▼
                 ┌───────────────┐
                 │    ADO.NET    │
                 │ SqlConnection │
                 │ SqlCommand    │
                 │ SqlDataReader │
                 └───────┬───────┘
                         │
                         ▼
                 ┌───────────────┐
                 │  SQL SERVER   │
                 │  Employees    │
                 └───────────────┘
```


The most important thing is not just writing CRUD code. You should be able to explain **why each piece exists**:

```text
Controller  → Handles HTTP
Service     → Handles business logic
Repository  → Handles database
DTO         → Controls API data
Model       → Represents domain/data
Middleware  → Handles cross-cutting concerns
DI          → Connects dependencies
ADO.NET     → Communicates with SQL Server
```


