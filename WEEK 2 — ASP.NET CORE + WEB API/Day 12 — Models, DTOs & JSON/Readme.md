# Day 12 — Models, DTOs & JSON in ASP.NET Core

This is an important day because it connects your **Controller → Request → DTO → Model → Response → JSON** flow.

By the end of Day 12, you should understand:

```text
Client
   ↓
JSON Request
   ↓
Model Binding
   ↓
Request DTO
   ↓
Validation
   ↓
Service
   ↓
Entity / Domain Model
   ↓
Response DTO
   ↓
JSON Serialization
   ↓
Client
```

---

# 1. What is a Model?

A **model** is a C# class that represents data used by your application.

For example, an employee:

```csharp
public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }

    public string Email { get; set; } = "";
}
```

This class represents an employee.

---

# 2. Entity / Domain Model

An **Entity Model** generally represents data that belongs to your application's domain and often maps to a database table when using an ORM such as Entity Framework Core.

Example:

```csharp
public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }

    public string Email { get; set; } = "";

    public DateTime CreatedDate { get; set; }
}
```

You can think of it as:

```text
Employee Entity
       ↓
Employee database record
```

For example:

```text
Id       Name       Department    Salary
-----------------------------------------
1        Arun       IT            50000
2        Priya      HR            60000
```

---

# 3. What is a DTO?

DTO means:

> **Data Transfer Object**

A DTO is an object specifically designed to transfer data between different parts of an application.

For example:

```text
Client
   ↓
Controller
   ↓
DTO
   ↓
Service
   ↓
Entity
```

And the response:

```text
Entity
   ↓
Service
   ↓
DTO
   ↓
Controller
   ↓
Client
```

---

# 4. Why Do We Need DTOs?

You might ask:

> Why can't I directly use Employee everywhere?

You can in very small applications, but using DTOs is usually better for larger applications.

Suppose your entity contains:

```csharp
public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public decimal Salary { get; set; }

    public string Password { get; set; } = "";

    public DateTime CreatedDate { get; set; }
}
```

You don't want to return this entire entity to the client.

Especially:

```text
Password
CreatedDate
internal database fields
internal relationships
```

Instead, create a DTO.

```csharp
public class EmployeeResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public string Department { get; set; } = "";
}
```

Now the client receives only the required data.

---

# 5. Entity vs DTO

### Entity

Represents your application's/domain data.

```csharp
public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public decimal Salary { get; set; }

    public string Password { get; set; } = "";
}
```

### DTO

Represents data being transferred.

```csharp
public class EmployeeResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";
}
```

### Main difference

```text
Entity
   ↓
Application/database representation

DTO
   ↓
Data transfer representation
```

DTOs help with:

* Security
* API contract
* Validation
* Hiding internal fields
* Controlling request/response data
* Avoiding unnecessary data transfer
* Separating API contracts from persistence/domain models

---

# 6. Different DTOs

For an Employee API, we can create:

```text
EmployeeRequestDto
EmployeeResponseDto
EmployeeUpdateDto
```

Why three DTOs?

Because **creating, reading and updating** an employee can require different fields.

---

# 7. EmployeeRequestDto

Used when creating an employee.

```csharp
public class EmployeeRequestDto
{
    public string Name { get; set; } = "";

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }

    public string Email { get; set; } = "";
}
```

Request:

```http
POST /api/employees
```

JSON:

```json
{
  "name": "Arun",
  "department": "IT",
  "salary": 50000,
  "email": "arun@example.com"
}
```

---

# 8. EmployeeResponseDto

Used when returning employee information.

```csharp
public class EmployeeResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }

    public string Email { get; set; } = "";
}
```

Response:

```json
{
  "id": 1,
  "name": "Arun",
  "department": "IT",
  "salary": 50000,
  "email": "arun@example.com"
}
```

---

# 9. EmployeeUpdateDto

Update may require different rules.

```csharp
public class EmployeeUpdateDto
{
    public string Name { get; set; } = "";

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }

    public string Email { get; set; } = "";
}
```

For example:

```http
PUT /api/employees/1
```

Body:

```json
{
  "name": "Arun Kumar",
  "department": "Development",
  "salary": 65000,
  "email": "arun@example.com"
}
```

---

# 10. Recommended Project Structure

For your Day 12 project:

```text
Day12ModelsDtosJson
│
├── Controllers
│   └── EmployeesController.cs
│
├── Models
│   └── Employee.cs
│
├── DTOs
│   ├── EmployeeRequestDto.cs
│   ├── EmployeeResponseDto.cs
│   └── EmployeeUpdateDto.cs
│
├── Program.cs
├── appsettings.json
└── Day12ModelsDtosJson.csproj
```

This is a good foundation for the layered architecture you'll learn later.

---

# 11. Model Binding

Model binding is one of the most important ASP.NET Core concepts.

Suppose the client sends:

```json
{
  "name": "Arun",
  "department": "IT",
  "salary": 50000,
  "email": "arun@example.com"
}
```

Controller:

```csharp
[HttpPost]
public IActionResult CreateEmployee(EmployeeRequestDto request)
{
    return Ok(request);
}
```

ASP.NET Core automatically converts the JSON request body into:

```csharp
EmployeeRequestDto
```

This process is called:

> **Model Binding**

Conceptually:

```text
JSON
 ↓
ASP.NET Core
 ↓
Model Binding
 ↓
EmployeeRequestDto object
```

---

# 12. `[FromBody]`

You can explicitly tell ASP.NET Core to get data from the request body.

```csharp
[HttpPost]
public IActionResult CreateEmployee(
    [FromBody] EmployeeRequestDto request)
{
    return Ok(request);
}
```

For API controllers, body binding is commonly inferred for complex types, but explicitly using `[FromBody]` is useful when teaching or when you want to make the source clear.

---

# 13. `[FromRoute]`

Suppose URL:

```text
GET /api/employees/10
```

Controller:

```csharp
[HttpGet("{id:int}")]
public IActionResult GetEmployee(
    [FromRoute] int id)
{
    return Ok(id);
}
```

Here:

```text
10
↓
Route
↓
id
```

---

# 14. `[FromQuery]`

URL:

```text
GET /api/employees?department=IT
```

Controller:

```csharp
[HttpGet]
public IActionResult GetEmployees(
    [FromQuery] string? department)
{
    return Ok(department);
}
```

Result:

```text
department = IT
```

---

# 15. `[FromHeader]`

You can also read a value from a request header.

```csharp
[HttpGet]
public IActionResult GetData(
    [FromHeader(Name = "X-Client-Id")] string clientId)
{
    return Ok(clientId);
}
```

Request:

```text
X-Client-Id: ABC123
```

---

# 16. Model Binding Summary

```text
Route
/api/employees/10
       ↓
[FromRoute]

Query
/api/employees?department=IT
       ↓
[FromQuery]

Body
{
   "name": "Arun"
}
       ↓
[FromBody]

Header
X-Client-Id: ABC123
       ↓
[FromHeader]
```

---

# 17. Validation

Validation checks whether incoming data is valid.

For example:

```text
Name → required
Email → valid email
Salary → between 10000 and 500000
Name → maximum 100 characters
```

ASP.NET Core supports validation using **Data Annotations**.

---

# 18. Data Annotations

Namespace:

```csharp
using System.ComponentModel.DataAnnotations;
```

Example:

```csharp
public class EmployeeRequestDto
{
    [Required]
    public string Name { get; set; } = "";

    [Required]
    public string Department { get; set; } = "";

    [Range(10000, 500000)]
    public decimal Salary { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
}
```

---

# 19. `[Required]`

Used when a value must be provided.

```csharp
[Required]
public string Name { get; set; } = "";
```

Request:

```json
{
  "department": "IT",
  "salary": 50000,
  "email": "arun@example.com"
}
```

`Name` is missing.

Validation fails.

---

# 20. `[StringLength]`

Controls string length.

```csharp
[StringLength(100)]
public string Name { get; set; } = "";
```

You can specify minimum and maximum:

```csharp
[StringLength(100, MinimumLength = 3)]
public string Name { get; set; } = "";
```

Meaning:

```text
Minimum = 3
Maximum = 100
```

---

# 21. `[Range]`

Used to restrict numeric values.

```csharp
[Range(10000, 500000)]
public decimal Salary { get; set; }
```

Valid:

```text
50000
```

Invalid:

```text
5000
```

Invalid:

```text
600000
```

---

# 22. `[EmailAddress]`

Used for email validation.

```csharp
[EmailAddress]
public string Email { get; set; } = "";
```

Example:

```text
arun@example.com
```

Valid format.

```text
arun
```

Invalid format.

Usually combine it with:

```csharp
[Required]
[EmailAddress]
public string Email { get; set; } = "";
```

---

# 23. Complete Request DTO

Now let's combine everything.

```csharp
using System.ComponentModel.DataAnnotations;

public class EmployeeRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = "";

    [Required]
    [StringLength(50)]
    public string Department { get; set; } = "";

    [Range(10000, 500000)]
    public decimal Salary { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
}
```

---

# 24. `[ApiController]` and Validation

This is very important.

When you use:

```csharp
[ApiController]
```

ASP.NET Core automatically performs model validation and can return a **400 Bad Request** response when the model state is invalid.

Example:

```csharp
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateEmployee(
        EmployeeRequestDto request)
    {
        return Ok(request);
    }
}
```

You normally don't need:

```csharp
if (!ModelState.IsValid)
{
    return BadRequest(ModelState);
}
```

for the normal `[ApiController]` automatic-validation path.

---

# 25. JSON

JSON means:

> **JavaScript Object Notation**

It is a lightweight data format commonly used for APIs.

Example:

```json
{
  "id": 1,
  "name": "Arun",
  "department": "IT",
  "salary": 50000
}
```

ASP.NET Core APIs commonly exchange JSON.

---

# 26. Serialization

Serialization means:

> C# object → JSON

Example C# object:

```csharp
Employee employee = new Employee
{
    Id = 1,
    Name = "Arun",
    Department = "IT",
    Salary = 50000
};
```

Convert to JSON:

```text
C# Object
   ↓
Serialization
   ↓
JSON
```

---

# 27. Deserialization

Deserialization means:

> JSON → C# object

Example JSON:

```json
{
  "id": 1,
  "name": "Arun",
  "department": "IT",
  "salary": 50000
}
```

Convert into:

```csharp
Employee
```

Flow:

```text
JSON
 ↓
Deserialization
 ↓
C# Object
```

---

# 28. `System.Text.Json`

Modern .NET includes:

```csharp
System.Text.Json
```

Namespace:

```csharp
using System.Text.Json;
```

---

# 29. Serialization Example

```csharp
using System.Text.Json;

Employee employee = new Employee
{
    Id = 1,
    Name = "Arun",
    Department = "IT",
    Salary = 50000
};

string json = JsonSerializer.Serialize(employee);

Console.WriteLine(json);
```

Output:

```json
{"Id":1,"Name":"Arun","Department":"IT","Salary":50000}
```

---

# 30. Deserialization Example

```csharp
string json = """
{
    "Id": 1,
    "Name": "Arun",
    "Department": "IT",
    "Salary": 50000
}
""";

Employee? employee =
    JsonSerializer.Deserialize<Employee>(json);

Console.WriteLine(employee?.Name);
```

Output:

```text
Arun
```

---

# 31. JSON Options

You can control JSON behavior using:

```csharp
JsonSerializerOptions
```

Example:

```csharp
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};
```

Then:

```csharp
Employee? employee =
    JsonSerializer.Deserialize<Employee>(json, options);
```

---

# 32. Property Naming

C# normally uses:

```csharp
EmployeeName
```

API JSON commonly uses:

```json
{
  "employeeName": "Arun"
}
```

ASP.NET Core's default JSON configuration uses a web-friendly camelCase naming policy.

You can explicitly configure it.

```csharp
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy =
            JsonNamingPolicy.CamelCase;
    });

var app = builder.Build();

app.MapControllers();

app.Run();
```

---

# 33. `JsonPropertyName`

Suppose your C# property is:

```csharp
public string EmployeeName { get; set; } = "";
```

But you want JSON:

```json
{
  "employee_name": "Arun"
}
```

Use:

```csharp
using System.Text.Json.Serialization;

public class EmployeeDto
{
    [JsonPropertyName("employee_name")]
    public string EmployeeName { get; set; } = "";
}
```

Now:

```text
C#
EmployeeName

JSON
employee_name
```

---

# 34. Null Handling

Suppose:

```csharp
public class EmployeeResponseDto
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Department { get; set; }
}
```

Possible object:

```csharp
var employee = new EmployeeResponseDto
{
    Id = 1,
    Name = "Arun",
    Department = null
};
```

Depending on your configured JSON options, null properties can be included or ignored.

For example:

```csharp
using System.Text.Json.Serialization;

var options = new JsonSerializerOptions
{
    DefaultIgnoreCondition =
        JsonIgnoreCondition.WhenWritingNull
};
```

Then a null property won't be written to JSON.

---

# 35. Nested Objects

JSON can contain objects inside objects.

Example:

```json
{
  "id": 1,
  "name": "Arun",
  "department": {
    "id": 10,
    "name": "IT"
  }
}
```

C#:

```csharp
public class DepartmentDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
}
```

Employee:

```csharp
public class EmployeeResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public DepartmentDto? Department { get; set; }
}
```

This represents:

```text
Employee
   │
   └── Department
          ├── Id
          └── Name
```

---

# 36. JSON Lists

JSON array:

```json
{
  "id": 1,
  "name": "Arun",
  "skills": [
    "C#",
    "ASP.NET Core",
    "SQL"
  ]
}
```

C#:

```csharp
public class EmployeeResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public List<string> Skills { get; set; } = new();
}
```

---

# 37. List of Objects

JSON:

```json
{
  "id": 1,
  "name": "Arun",
  "skills": [
    {
      "id": 1,
      "name": "C#"
    },
    {
      "id": 2,
      "name": "SQL"
    }
  ]
}
```

C#:

```csharp
public class SkillDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
}
```

```csharp
public class EmployeeResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public List<SkillDto> Skills { get; set; } = new();
}
```

---

# 38. DateTime Handling

C#:

```csharp
public DateTime CreatedDate { get; set; }
```

JSON commonly represents it using an ISO 8601 format:

```json
{
  "createdDate": "2026-09-20T10:30:00Z"
}
```

ASP.NET Core / `System.Text.Json` supports standard `DateTime` and `DateTimeOffset` serialization/deserialization.

For APIs, `DateTimeOffset` can be useful when the offset/time-zone context matters:

```csharp
public DateTimeOffset CreatedDate { get; set; }
```

Example:

```json
{
  "createdDate": "2026-09-20T10:30:00+05:30"
}
```

### Important

Don't casually treat a `DateTime` as local/UTC without deciding what your application means by that value.

For distributed applications, consistently storing/transmitting UTC or using `DateTimeOffset` where offset information matters helps avoid time-zone problems.

---

# 39. Complete Employee Model

Let's create a proper example.

### Models/Employee.cs

```csharp
namespace Day12ModelsDtosJson.Models;

public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }

    public string Email { get; set; } = "";

    public DateTimeOffset CreatedDate { get; set; }
}
```

---

# 40. EmployeeRequestDto

### DTOs/EmployeeRequestDto.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace Day12ModelsDtosJson.DTOs;

public class EmployeeRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = "";

    [Required]
    [StringLength(50)]
    public string Department { get; set; } = "";

    [Range(10000, 500000)]
    public decimal Salary { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
}
```

---

# 41. EmployeeUpdateDto

### DTOs/EmployeeUpdateDto.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace Day12ModelsDtosJson.DTOs;

public class EmployeeUpdateDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = "";

    [Required]
    [StringLength(50)]
    public string Department { get; set; } = "";

    [Range(10000, 500000)]
    public decimal Salary { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
}
```

---

# 42. EmployeeResponseDto

### DTOs/EmployeeResponseDto.cs

```csharp
namespace Day12ModelsDtosJson.DTOs;

public class EmployeeResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }

    public string Email { get; set; } = "";

    public DateTimeOffset CreatedDate { get; set; }
}
```

---

# 43. Controller Using DTOs

Now let's connect everything.

```csharp
using Day12ModelsDtosJson.DTOs;
using Day12ModelsDtosJson.Models;
using Microsoft.AspNetCore.Mvc;

namespace Day12ModelsDtosJson.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateEmployee(EmployeeRequestDto request)
    {
        var employee = new Employee
        {
            Id = 1,
            Name = request.Name,
            Department = request.Department,
            Salary = request.Salary,
            Email = request.Email,
            CreatedDate = DateTimeOffset.UtcNow
        };

        var response = new EmployeeResponseDto
        {
            Id = employee.Id,
            Name = employee.Name,
            Department = employee.Department,
            Salary = employee.Salary,
            Email = employee.Email,
            CreatedDate = employee.CreatedDate
        };

        return CreatedAtAction(
            nameof(GetEmployee),
            new { id = response.Id },
            response);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetEmployee(int id)
    {
        var employee = new Employee
        {
            Id = id,
            Name = "Arun",
            Department = "IT",
            Salary = 50000,
            Email = "arun@example.com",
            CreatedDate = DateTimeOffset.UtcNow
        };

        var response = new EmployeeResponseDto
        {
            Id = employee.Id,
            Name = employee.Name,
            Department = employee.Department,
            Salary = employee.Salary,
            Email = employee.Email,
            CreatedDate = employee.CreatedDate
        };

        return Ok(response);
    }
}
```

---

# 44. What Happens During POST?

Client sends:

```http
POST /api/employees
Content-Type: application/json
```

Body:

```json
{
  "name": "Arun",
  "department": "IT",
  "salary": 50000,
  "email": "arun@example.com"
}
```

ASP.NET Core processes it:

```text
JSON Request
     ↓
Model Binding
     ↓
EmployeeRequestDto
     ↓
Validation
     ↓
Controller
     ↓
Employee Entity
     ↓
EmployeeResponseDto
     ↓
JSON Serialization
     ↓
HTTP Response
```

---

# 45. Example Response

The API can return:

```json
{
  "id": 1,
  "name": "Arun",
  "department": "IT",
  "salary": 50000,
  "email": "arun@example.com",
  "createdDate": "2026-09-20T10:30:00+00:00"
}
```

Notice:

```text
Entity
↓
Response DTO
↓
JSON
```

The entity itself doesn't have to be exposed directly.

---

# 46. Entity → DTO Mapping

In the previous example, we manually mapped:

```csharp
var response = new EmployeeResponseDto
{
    Id = employee.Id,
    Name = employee.Name,
    Department = employee.Department,
    Salary = employee.Salary,
    Email = employee.Email,
    CreatedDate = employee.CreatedDate
};
```

This is called:

> **Mapping**

You are converting:

```text
Employee
   ↓
EmployeeResponseDto
```

Later, in real projects, you may use mapping libraries such as AutoMapper or Mapster, but first understand manual mapping clearly.

---

# 47. Request DTO → Entity

Similarly:

```csharp
var employee = new Employee
{
    Name = request.Name,
    Department = request.Department,
    Salary = request.Salary,
    Email = request.Email,
    CreatedDate = DateTimeOffset.UtcNow
};
```

Flow:

```text
EmployeeRequestDto
        ↓
      Entity
```

---

# 48. Why Not Return the Entity Directly?

Technically you can:

```csharp
return Ok(employee);
```

But in a properly designed API, returning DTOs gives you better control over your API contract.

For example, your database entity may later become:

```csharp
public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public decimal Salary { get; set; }

    public string PasswordHash { get; set; } = "";

    public DateTimeOffset CreatedDate { get; set; }

    public int InternalStatus { get; set; }
}
```

Your API response can remain:

```csharp
public class EmployeeResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";
}
```

Therefore:

```text
Database/domain changes
          ↓
     Entity changes
          ↓
API contract can remain controlled
          ↓
       DTO
```

---

# 49. Complete Day 12 Architecture

You should now understand this architecture:

```text
                 CLIENT
                   │
                   │ JSON
                   ↓
        ┌─────────────────────┐
        │     CONTROLLER      │
        └─────────────────────┘
                   │
                   ↓
          EmployeeRequestDto
                   │
                   ↓
              Validation
                   │
                   ↓
               Service
                   │
                   ↓
              Employee
               Entity
                   │
                   ↓
              Database
                   │
                   ↓
              Employee
               Entity
                   │
                   ↓
          EmployeeResponseDto
                   │
                   ↓
            JSON Serialization
                   │
                   ↓
                CLIENT
```

This will become:

```text
Controller
    ↓
Service / BAL
    ↓
Repository / DAL
    ↓
Database
```

in your upcoming layered architecture topics.

---

# 50. `System.Text.Json` Important Classes

Remember these:

```csharp
JsonSerializer
```

Main methods:

```csharp
JsonSerializer.Serialize()
```

and

```csharp
JsonSerializer.Deserialize<T>()
```

Options:

```csharp
JsonSerializerOptions
```

Attributes:

```csharp
[JsonPropertyName]
```

```csharp
[JsonIgnore]
```

Example:

```csharp
using System.Text.Json.Serialization;

public class EmployeeDto
{
    public int Id { get; set; }

    [JsonPropertyName("employee_name")]
    public string Name { get; set; } = "";

    [JsonIgnore]
    public string InternalCode { get; set; } = "";
}
```

---

# 51. Important JSON Options

A few options you'll commonly encounter:

```csharp
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
```

For null handling:

```csharp
DefaultIgnoreCondition =
    JsonIgnoreCondition.WhenWritingNull
```

For readable JSON during manual serialization:

```csharp
WriteIndented = true
```

Example:

```csharp
var options = new JsonSerializerOptions
{
    WriteIndented = true
};

string json = JsonSerializer.Serialize(employee, options);
```

Output becomes easier to read:

```json
{
  "id": 1,
  "name": "Arun",
  "department": "IT",
  "salary": 50000
}
```

---

# 52. Common Validation Attributes

You should remember these:

```text
[Required]
[StringLength]
[Range]
[EmailAddress]
```

Some other useful ones:

```text
[MinLength]
[MaxLength]
[RegularExpression]
[Compare]
[Phone]
[Url]
```

Example:

```csharp
[Required]
[StringLength(100, MinimumLength = 3)]
public string Name { get; set; } = "";

[Range(18, 60)]
public int Age { get; set; }

[EmailAddress]
public string Email { get; set; } = "";
```

---

# 53. Model Binding vs JSON Deserialization

These two concepts are related but not exactly the same.

### JSON deserialization

```text
JSON
 ↓
C# object
```

Performed by the JSON serializer.

### Model binding

ASP.NET Core takes incoming HTTP request data from sources such as:

```text
Route
Query string
Headers
Body
Form
```

and supplies values to action parameters/models.

For a JSON body:

```text
HTTP Request
     ↓
Model Binding
     ↓
JSON input formatter
     ↓
JSON Deserialization
     ↓
DTO
```

This distinction is useful in interviews.

---

# 54. Complete Day 12 Practice

Create this project in Visual Studio:

```text
DotNetHandsOn
│
├── Day09AspNetCoreFundamentals
├── Day10ControllersRouting
├── Day11RestApi
└── Day12ModelsDtosJson
```

Inside Day 12:

```text
Day12ModelsDtosJson
│
├── Controllers
│   └── EmployeesController.cs
│
├── Models
│   └── Employee.cs
│
├── DTOs
│   ├── EmployeeRequestDto.cs
│   ├── EmployeeResponseDto.cs
│   └── EmployeeUpdateDto.cs
│
├── Program.cs
├── appsettings.json
└── Day12ModelsDtosJson.csproj
```

---

# 55. Practice APIs

Implement these:

### Create

```http
POST /api/employees
```

Body:

```json
{
  "name": "Arun",
  "department": "IT",
  "salary": 50000,
  "email": "arun@example.com"
}
```

### Get

```http
GET /api/employees/1
```

### Update

```http
PUT /api/employees/1
```

Body:

```json
{
  "name": "Arun Kumar",
  "department": "Development",
  "salary": 65000,
  "email": "arun.kumar@example.com"
}
```

### Invalid request

Try:

```json
{
  "name": "A",
  "department": "",
  "salary": 5000,
  "email": "wrong-email"
}
```

You should see validation errors.

---

# 56. Day 12 Interview Questions

### 1. What is a model?

A C# class representing application/domain data.

### 2. What is DTO?

DTO means Data Transfer Object. It is used to transfer controlled data between application boundaries.

### 3. Entity vs DTO?

```text
Entity → domain/persistence representation

DTO → data transfer/API representation
```

### 4. Why use DTOs?

To control API contracts, hide internal data, apply request-specific validation, and avoid coupling API contracts directly to persistence models.

### 5. What is model binding?

ASP.NET Core's process of obtaining request data and binding it to action parameters/models.

### 6. What is `[FromBody]`?

Specifies that a parameter should be bound from the HTTP request body.

### 7. What is `[FromRoute]`?

Binds a parameter from route data.

### 8. What is `[FromQuery]`?

Binds a parameter from the query string.

### 9. What is `[Required]`?

Specifies that a value is required for validation.

### 10. What is `[StringLength]`?

Restricts a string's length.

### 11. What is `[Range]`?

Restricts a numeric value to a specified range.

### 12. What is `[EmailAddress]`?

Validates that a string has an email-like format.

### 13. What is serialization?

```text
C# Object → JSON
```

### 14. What is deserialization?

```text
JSON → C# Object
```

### 15. What is `System.Text.Json`?

The built-in JSON serialization/deserialization library provided by modern .NET.

### 16. What is `JsonSerializer.Serialize()`?

Converts an object to JSON.

### 17. What is `JsonSerializer.Deserialize<T>()`?

Converts JSON into an object of type `T`.

### 18. What is `JsonPropertyName`?

It allows you to specify the JSON property name independently of the C# property name.

### 19. What is `JsonSerializerOptions`?

A configuration object used to control JSON serialization/deserialization behavior.

### 20. Why use `DateTimeOffset`?

It represents a date/time together with an offset, which can be useful when time-zone/offset context matters.

---

# Day 12 — Final Revision

```text
DAY 12
│
├── MODELS
│   ├── Entity
│   └── Domain Model
│
├── DTOs
│   ├── Request DTO
│   ├── Response DTO
│   └── Update DTO
│
├── MODEL BINDING
│   ├── FromBody
│   ├── FromRoute
│   ├── FromQuery
│   └── FromHeader
│
├── VALIDATION
│   ├── Required
│   ├── StringLength
│   ├── Range
│   └── EmailAddress
│
└── JSON
    ├── Serialization
    ├── Deserialization
    ├── System.Text.Json
    ├── JsonSerializerOptions
    ├── Property Naming
    ├── Null Handling
    ├── Nested Objects
    ├── Lists
    └── DateTime / DateTimeOffset
```

## Most important mental model

```text
                HTTP REQUEST
                     │
                     ↓
                  JSON
                     │
                     ↓
              MODEL BINDING
                     │
                     ↓
            EmployeeRequestDto
                     │
                     ↓
                VALIDATION
                     │
                     ↓
                 SERVICE
                     │
                     ↓
             Employee Entity
                     │
                     ↓
                DATABASE
                     │
                     ↓
             Employee Entity
                     │
                     ↓
           EmployeeResponseDto
                     │
                     ↓
              SERIALIZATION
                     │
                     ↓
                  JSON
                     │
                     ↓
                RESPONSE
```


