# Day 30 — API Debugging, Troubleshooting & Final Project

Day 30 is the **final day of your .NET roadmap**. The purpose is to take everything you learned from Day 1–29 and understand how to **debug, troubleshoot, test, and complete a real ASP.NET Core API project**.

Your final flow should look like:

```text
Angular / Postman / Browser
          ↓
       HTTP Request
          ↓
     ASP.NET Core API
          ↓
       Middleware
          ↓
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

And when something goes wrong:

```text
Request
   ↓
Debug
   ↓
Find Error
   ↓
Identify Layer
   ↓
Fix
   ↓
Test Again
```

---

# 1. API Debugging — What Does Debugging Mean?

**Debugging** means finding and fixing the reason why your application is not behaving as expected.

For example:

```text
GET /api/employees/10
```

returns:

```text
500 Internal Server Error
```

You need to determine:

```text
Is routing wrong?
      ↓
Is controller wrong?
      ↓
Is service wrong?
      ↓
Is repository wrong?
      ↓
Is SQL wrong?
      ↓
Is database connection wrong?
```

Debugging helps you find the exact location.

---

# 2. Debugging Tools

You should know these five tools:

```text
Swagger
Postman
Browser DevTools
Visual Studio Debugger
Application Logs
```

Each has a different purpose.

---

# 3. Swagger

Swagger provides an interactive UI for testing your API.

For an ASP.NET Core API, you may have:

```text
https://localhost:xxxx/swagger
```

You can see:

```text
GET     /api/employees
GET     /api/employees/{id}
POST    /api/employees
PUT     /api/employees/{id}
DELETE  /api/employees/{id}
```

---

# 4. Swagger — GET Request

Suppose you have:

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetEmployee(int id)
{
    var employee = await _service.GetByIdAsync(id);

    if (employee == null)
        return NotFound();

    return Ok(employee);
}
```

Swagger lets you enter:

```text
id = 10
```

and execute:

```text
GET /api/employees/10
```

You can inspect:

```text
Request URL
Status Code
Response Headers
Response Body
```

---

# 5. Swagger — POST Request

Suppose your DTO is:

```csharp
public class EmployeeRequestDto
{
    public string Name { get; set; } = "";
    public string Department { get; set; } = "";
    public decimal Salary { get; set; }
}
```

Swagger can generate a request body:

```json
{
  "name": "John",
  "department": "IT",
  "salary": 50000
}
```

You can test the API without writing frontend code.

---

# 6. Why Swagger Is Useful

Swagger is excellent for:

* Quickly testing endpoints
* Understanding API contracts
* Testing request bodies
* Testing query parameters
* Testing route parameters
* Testing authorization
* Viewing response schemas
* Testing during development

Think:

```text
Swagger
   ↓
Quick API testing
```

---

# 7. Postman

Postman is another API testing tool.

Example:

```text
GET
https://localhost:7000/api/employees
```

You can configure:

```text
Method
URL
Headers
Authorization
Query Parameters
Body
```

---

# 8. Postman — GET

Example:

```text
GET https://localhost:7000/api/employees
```

Possible response:

```json
[
  {
    "employeeId": 1,
    "name": "John",
    "department": "IT"
  },
  {
    "employeeId": 2,
    "name": "Priya",
    "department": "HR"
  }
]
```

---

# 9. Postman — POST

Select:

```text
POST
```

URL:

```text
https://localhost:7000/api/employees
```

Body:

```text
Body
 ↓
raw
 ↓
JSON
```

Send:

```json
{
  "name": "Arun",
  "department": "Finance",
  "salary": 55000
}
```

Header:

```http
Content-Type: application/json
```

---

# 10. Postman — Authorization

If your API uses JWT:

```http
Authorization: Bearer eyJ...
```

In Postman:

```text
Authorization
     ↓
Bearer Token
     ↓
Paste Access Token
```

Then:

```text
POST /api/employees
```

will be authenticated.

---

# 11. Swagger vs Postman

### Swagger

Good for:

```text
API documentation
Quick testing
Developer understanding
```

### Postman

Good for:

```text
Advanced API testing
Collections
Environment variables
Authorization
Test scripts
Different request scenarios
```

You should know both.

---

# 12. Browser DevTools

Browser DevTools is especially useful when an Angular/frontend application calls your API.

Open:

```text
F12
```

Then:

```text
Network
```

You can see:

```text
Request URL
Request Method
Status Code
Request Headers
Request Payload
Response
Response Headers
Timing
```

---

# 13. Example — Frontend API Error

Suppose Angular calls:

```text
GET /api/employees
```

and receives:

```text
404 Not Found
```

Open:

```text
F12
 ↓
Network
 ↓
employees request
```

Check:

```text
Request URL
```

Maybe frontend is calling:

```text
/api/employee
```

while backend route is:

```text
/api/employees
```

That's a routing mismatch.

---

# 14. Browser DevTools — CORS

Suppose the browser reports:

```text
Access to XMLHttpRequest has been blocked by CORS policy
```

The API may be working correctly, but the browser is blocking the cross-origin request.

Check:

```text
Frontend
   ↓
Different origin
   ↓
ASP.NET Core API
```

Then configure CORS appropriately in ASP.NET Core.

Example:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
```

Pipeline:

```csharp
app.UseCors("Frontend");
```

The exact origin must match your frontend.

---

# 15. Visual Studio Debugger

The Visual Studio debugger is one of the most important skills for a .NET developer.

You should know:

```text
Breakpoint
Step Over
Step Into
Step Out
Continue
Locals
Watch
Call Stack
Exception Details
```

---

# 16. Breakpoint

A breakpoint pauses execution at a specific line.

Example:

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetEmployee(int id)
{
    var employee = await _service.GetByIdAsync(id);

    return Ok(employee);
}
```

Click beside:

```csharp
var employee = await _service.GetByIdAsync(id);
```

A red dot appears.

Then call:

```text
GET /api/employees/10
```

Execution stops there.

---

# 17. Why Breakpoints?

You can inspect:

```text
id
employee
_service
request data
variables
```

Instead of guessing what your application is doing.

---

# 18. Step Over

Shortcut:

```text
F10
```

Step Over executes the current line and moves to the next line.

Example:

```csharp
var employee = await _service.GetByIdAsync(id);
return Ok(employee);
```

Press F10:

```text
GetByIdAsync()
      ↓
next line
```

Useful when you don't need to enter the method.

---

# 19. Step Into

Shortcut:

```text
F11
```

Step Into enters the method.

Example:

```csharp
var employee = await _service.GetByIdAsync(id);
```

Press F11.

You can enter:

```text
EmployeeService
      ↓
EmployeeRepository
```

This is very useful for understanding the full request flow.

---

# 20. Step Out

Shortcut:

```text
Shift + F11
```

Step Out exits the current method and returns to the caller.

Example:

```text
Controller
   ↓
Service
   ↓
Repository
```

If you're debugging inside Repository and want to return to Service:

```text
Shift + F11
```

---

# 21. Continue

Shortcut:

```text
F5
```

Continue execution until:

```text
Next breakpoint
```

or:

```text
Exception
```

or:

```text
Request finishes
```

---

# 22. Locals Window

The **Locals** window shows variables available in the current scope.

Example:

```csharp
public IActionResult GetEmployee(int id)
{
    var employee = _service.GetById(id);

    return Ok(employee);
}
```

You may see:

```text
id        10
employee  Employee
```

You can expand the object:

```text
employee
 ├── Id = 10
 ├── Name = "John"
 ├── Department = "IT"
 └── Salary = 50000
```

---

# 23. Watch Window

The Watch window lets you monitor specific expressions.

For example:

```text
id
employee.Name
employee.Salary
employee.Department
```

You can add:

```text
employee.Salary > 50000
```

and Visual Studio evaluates it while debugging.

---

# 24. Call Stack

The Call Stack shows how execution reached the current method.

For example:

```text
EmployeeRepository.GetByIdAsync()
        ↑
EmployeeService.GetByIdAsync()
        ↑
EmployeesController.GetById()
        ↑
ASP.NET Core Middleware
```

This is extremely useful when an exception occurs.

---

# 25. Call Stack Example

Suppose SQL throws an exception.

Call Stack may show:

```text
SqlCommand.ExecuteReaderAsync()
        ↓
EmployeeRepository.GetByIdAsync()
        ↓
EmployeeService.GetByIdAsync()
        ↓
EmployeesController.GetById()
        ↓
ASP.NET Core
```

You immediately know the request path.

---

# 26. Exception Details

Suppose:

```csharp
var result = employee.Name.ToUpper();
```

and `employee` is null.

You may get:

```text
NullReferenceException
```

Visual Studio shows:

```text
Exception type
Exception message
Stack trace
Source file
Line number
```

Don't just read:

```text
500 Internal Server Error
```

Find the actual exception.

---

# 27. Exception Debugging Process

Use:

```text
Exception
   ↓
Exception Type
   ↓
Message
   ↓
Stack Trace
   ↓
Source File
   ↓
Line Number
   ↓
Variable Values
   ↓
Root Cause
```

---

# 28. HTTP Status Codes

You must know these very well:

```text
400 → Bad Request
401 → Unauthorized
403 → Forbidden
404 → Not Found
409 → Conflict
500 → Internal Server Error
502 → Bad Gateway
503 → Service Unavailable
```

---

# 29. 400 — Bad Request

Meaning:

> The request is invalid.

Common causes:

```text
Invalid JSON
Invalid model
Validation failure
Invalid parameter
Malformed request
```

Example:

```json
{
  "name": "",
  "salary": -5000
}
```

If validation says:

```csharp
[Required]
public string Name { get; set; }

[Range(0, 10000000)]
public decimal Salary { get; set; }
```

the API may return:

```text
400 Bad Request
```

---

# 30. Debugging 400

Check:

```text
Request Body
       ↓
JSON format
       ↓
Property names
       ↓
Data types
       ↓
Validation attributes
       ↓
Model binding
```

---

# 31. 401 — Unauthorized

Meaning:

> The request has not successfully authenticated.

Typical causes:

```text
No access token
Invalid access token
Expired token
Wrong issuer
Wrong audience
Authentication middleware missing
```

Request:

```http
GET /api/employees
```

without:

```http
Authorization: Bearer <token>
```

may result in:

```text
401 Unauthorized
```

Remember:

```text
401 → Who are you?
```

---

# 32. 403 — Forbidden

Meaning:

> The caller is authenticated, but doesn't have permission to access the resource.

Example:

```csharp
[Authorize(Roles = "Employee.Admin")]
[HttpDelete("{id}")]
public IActionResult Delete(int id)
{
    ...
}
```

A logged-in user without the required role can receive:

```text
403 Forbidden
```

Remember:

```text
401 → Authentication problem
403 → Authorization problem
```

---

# 33. 404 — Not Found

Meaning:

> The requested route/resource could not be found.

Possible causes:

### Wrong URL

Backend:

```text
/api/employees
```

Frontend:

```text
/api/employee
```

### Wrong ID

```text
GET /api/employees/99999
```

when employee 99999 doesn't exist.

### Wrong route attribute

```csharp
[HttpGet("details/{id}")]
```

but you call:

```text
/api/employees/10
```

---

# 34. Debugging 404

Check:

```text
HTTP Method
      ↓
URL
      ↓
Controller Route
      ↓
Action Route
      ↓
Route Parameters
      ↓
Database Resource
```

---

# 35. 409 — Conflict

Meaning:

> The request conflicts with the current state of the resource.

Common examples:

```text
Duplicate employee email
Duplicate username
Duplicate record
Concurrency conflict
Business rule conflict
```

Example:

```text
POST /api/employees
```

with:

```json
{
  "email": "john@example.com"
}
```

when that email already exists.

The API may return:

```text
409 Conflict
```

---

# 36. 500 — Internal Server Error

Meaning:

> Something unexpected failed on the server.

Examples:

```text
Unhandled exception
Database failure
NullReferenceException
SQL error
Configuration problem
Unexpected application error
```

Example:

```csharp
var employee = await repository.GetByIdAsync(id);

Console.WriteLine(employee.Name);
```

If employee is null:

```text
NullReferenceException
```

could result in:

```text
500
```

---

# 37. Debugging 500

Don't immediately change the controller.

Follow:

```text
500
 ↓
Check logs
 ↓
Check exception
 ↓
Check stack trace
 ↓
Set breakpoint
 ↓
Inspect variables
 ↓
Check Service
 ↓
Check Repository
 ↓
Check SQL
```

---

# 38. 502 — Bad Gateway

Usually means:

> A gateway/proxy received an invalid response from an upstream service.

Architecture:

```text
Client
  ↓
Gateway / Reverse Proxy
  ↓
API
```

If the gateway cannot get a valid response from the backend:

```text
502 Bad Gateway
```

Potential causes include:

```text
Backend unavailable
Incorrect upstream configuration
Proxy communication problem
Invalid upstream response
```

---

# 39. 503 — Service Unavailable

Meaning:

> The service is currently unavailable.

Possible causes:

```text
Application is down
Server unavailable
Service overloaded
Deployment/restart
Dependency unavailable
Health check failure
```

Example:

```text
Client
  ↓
Azure App Service
  ↓
Application unavailable
```

may result in:

```text
503 Service Unavailable
```

---

# 40. Status Code Quick Memory

```text
400 → My request is wrong
401 → I am not authenticated
403 → I am authenticated but not allowed
404 → Resource/route not found
409 → Request conflicts with current state
500 → Server/application error
502 → Gateway/upstream problem
503 → Service unavailable
```

---

# 41. Real Troubleshooting Scenario

Suppose:

```text
POST /api/employees
```

returns:

```text
500
```

Don't randomly modify code.

Follow this process.

### Step 1 — Postman

Check:

```text
Request
Response
Status
Response body
```

### Step 2 — Visual Studio

Set breakpoint:

```csharp
[HttpPost]
public async Task<IActionResult> Create(EmployeeRequestDto dto)
{
    var employee = await _service.CreateAsync(dto);

    return Ok(employee);
}
```

### Step 3 — Check DTO

```text
dto.Name
dto.Department
dto.Salary
dto.Email
```

### Step 4 — Step Into Service

```text
F11
```

### Step 5 — Step Into Repository

```text
F11
```

### Step 6 — Check SQL

Maybe:

```text
SQL constraint violation
```

### Step 7 — Fix

Correct the actual root cause.

---

# 42. Common API Problems

## Problem 1 — 404

```text
Wrong route
```

Check:

```csharp
[Route("api/[controller]")]
[HttpGet("{id}")]
```

---

## Problem 2 — 400

```text
Invalid request
```

Check:

```text
JSON
DTO
Validation
Model binding
```

---

## Problem 3 — 401

```text
Authentication
```

Check:

```text
Token
Issuer
Audience
Authentication middleware
```

---

## Problem 4 — 403

```text
Authorization
```

Check:

```text
Role
Scope
Policy
Claims
```

---

## Problem 5 — 500

```text
Server exception
```

Check:

```text
Logs
Exception
Stack trace
Debugger
Database
```

---

## Problem 6 — Slow API

Check:

```text
Controller
 ↓
Service
 ↓
Repository
 ↓
SQL
```

Possible causes:

```text
Slow SQL query
Missing index
Too much data
N+1 calls
External API latency
Unnecessary serialization
```

---

# 43. Logging

Your debugging should not depend only on breakpoints.

Use:

```csharp
private readonly ILogger<EmployeesController> _logger;
```

Constructor:

```csharp
public EmployeesController(
    IEmployeeService service,
    ILogger<EmployeesController> logger)
{
    _service = service;
    _logger = logger;
}
```

Log:

```csharp
_logger.LogInformation(
    "Getting employee with ID {EmployeeId}",
    id);
```

If an exception occurs:

```csharp
catch (Exception ex)
{
    _logger.LogError(
        ex,
        "Error while retrieving employee {EmployeeId}",
        id);

    throw;
}
```

Structured logging is better than building strings manually.

---

# 44. Debugging Architecture

For your final project:

```text
Client
  ↓
Swagger / Postman / Angular
  ↓
HTTP
  ↓
Middleware
  ↓
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

At every layer you can debug:

```text
Controller
    ↓
Request values

Service
    ↓
Business rules

Repository
    ↓
SQL parameters

ADO.NET
    ↓
Database connection

SQL Server
    ↓
Query execution
```

---

# 45. Final Project — Employee Management API

Now bring your entire roadmap together.

## Project

```text
EmployeeManagementAPI
```

---

# 46. Final Project Architecture

```text
EmployeeManagementAPI
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
│   ├── EmployeeUpdateDto.cs
│   └── EmployeeResponseDto.cs
│
├── Middleware
│   └── GlobalExceptionMiddleware.cs
│
├── Helpers
│
├── Program.cs
├── appsettings.json
└── EmployeeManagementAPI.csproj
```

---

# 47. Database

Create:

```text
EmployeeDB
```

Table:

```sql
CREATE TABLE Employees
(
    EmployeeId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Department NVARCHAR(100) NOT NULL,
    Salary DECIMAL(18,2) NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE()
);
```

---

# 48. Final API Endpoints

Your project should support:

```text
GET     /api/employees
GET     /api/employees/{id}
POST    /api/employees
PUT     /api/employees/{id}
DELETE  /api/employees/{id}
```

And preferably:

```text
GET /api/employees/search
```

with:

```text
search
department
pageNumber
pageSize
sortBy
sortOrder
```

---

# 49. Final Request Flow

### GET

```text
GET /api/employees/10
        ↓
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
SqlConnection
        ↓
SqlCommand
        ↓
SQL Server
        ↓
Employee
        ↓
Service
        ↓
EmployeeResponseDto
        ↓
Controller
        ↓
JSON
```

---

# 50. Final Project Technologies

Your final project should demonstrate:

```text
C#
.NET 8
ASP.NET Core Web API
REST API
Controllers
Routing
DTOs
Validation
Dependency Injection
Middleware
Exception Handling
Logging
SQL Server
Stored Procedures
ADO.NET
Repository Pattern
Service/BAL
LINQ
Async/Await
JWT/Entra ID
Redis
Swagger
Postman
Git
Azure DevOps
YAML CI/CD
```

You don't necessarily need to force every technology into one small project. Some topics such as Redis, Entra ID, and Azure deployment can be demonstrated as separate integration exercises around the core API.

---

# 51. Final Project — CI/CD

Your final pipeline:

```text
Developer
    ↓
Visual Studio
    ↓
Git
    ↓
Azure Repos
    ↓
Azure Pipeline
    ↓
Restore
    ↓
Build
    ↓
Test
    ↓
Publish
    ↓
Artifact
    ↓
Development
    ↓
Staging
    ↓
Production
```

---

# 52. Final Project Debugging Flow

When API fails:

```text
API Error
   ↓
Check Status Code
   ↓
Check Swagger/Postman
   ↓
Check Browser Network
   ↓
Check Logs
   ↓
Set Breakpoint
   ↓
Check Locals
   ↓
Check Watch
   ↓
Check Call Stack
   ↓
Step Into
   ↓
Find Root Cause
   ↓
Fix
   ↓
Run Tests
   ↓
Push
   ↓
CI/CD Pipeline
```

---

# 53. Day 30 Final Checklist

You should be comfortable with:

### API Testing

* Swagger
* Postman
* Browser DevTools
* Request headers
* Request body
* Query parameters
* Route parameters
* Authorization headers

### Visual Studio Debugging

* Breakpoints
* F5
* F10
* F11
* Shift + F11
* Continue
* Locals
* Watch
* Call Stack
* Exception details

### HTTP Troubleshooting

```text
400 → Bad Request
401 → Authentication
403 → Authorization
404 → Not Found
409 → Conflict
500 → Server Error
502 → Bad Gateway
503 → Service Unavailable
```

### Application troubleshooting

* Routing problems
* Model binding problems
* Validation problems
* Authentication problems
* Authorization problems
* Database problems
* SQL problems
* Configuration problems
* Dependency Injection problems
* CORS problems
* External service problems

---

# 🎯 Your Complete 30-Day .NET Journey

You have now covered the roadmap from **C# fundamentals to CI/CD and production-style API troubleshooting**:

```text
DAY 01 → .NET Fundamentals
DAY 02 → C# Fundamentals
DAY 03 → Methods, Strings & Arrays
DAY 04 → OOP Fundamentals
DAY 05 → Advanced OOP + SOLID
DAY 06 → Collections & Generics
DAY 07 → LINQ + Lambda
DAY 08 → Exceptions + Async/Await
DAY 09 → ASP.NET Core Fundamentals
DAY 10 → Controllers + Routing
DAY 11 → REST APIs
DAY 12 → Models + DTOs + JSON
DAY 13 → Dependency Injection
DAY 14 → Middleware + Configuration + Logging
DAY 15 → SQL Server Fundamentals
DAY 16 → SQL Joins
DAY 17 → Stored Procedures + CTE + Transactions
DAY 18 → SQL Performance + Indexes + Transactions
DAY 19 → ADO.NET
DAY 20 → DAL / Repository
DAY 21 → Layered Architecture
DAY 22 → Project Integration / Practice
DAY 23 → MDX + ADOMD.NET
DAY 24 → Microsoft Entra ID
DAY 25 → MSAL + JWT Authentication
DAY 26 → Managed Identity + Azure Security
DAY 27 → Redis
DAY 28 → Azure DevOps + Git
DAY 29 → CI/CD
DAY 30 → Debugging + Troubleshooting + Final Project
```

## Final architecture to remember

```text
                         CLIENT
                           │
                  Angular / Postman
                           │
                           ▼
                    ASP.NET CORE API
                           │
                     Middleware
                           │
                           ▼
                      Controller
                           │
                           ▼
                        Service
                           │
                           ▼
                      Repository
                           │
                           ▼
                        ADO.NET
                           │
                           ▼
                       SQL SERVER
```

Supporting infrastructure:

```text
              ┌──────────────────────┐
              │ Microsoft Entra ID    │
              │ Authentication       │
              └──────────┬───────────┘
                         │
                         ▼
Client ───────────────► API
                         │
              ┌──────────┴───────────┐
              │                      │
           Redis                 SQL Server
           Cache                 Database
```

And delivery:

```text
Developer
   ↓
Git
   ↓
Azure DevOps
   ↓
CI
   ├── Restore
   ├── Build
   ├── Test
   └── Publish
        ↓
     Artifact
        ↓
       CD
        ↓
 Development
        ↓
   Staging
        ↓
 Production
```


