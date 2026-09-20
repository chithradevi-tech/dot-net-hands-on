# Day 14 — Middleware + Configuration + Logging

Day 14 is another **core ASP.NET Core topic**.

Today you will learn how a request travels through the application, how to handle exceptions globally, how to manage configuration safely, and how to add professional application logging.

The complete flow is:

```text
Client
   ↓
HTTP Request
   ↓
Middleware 1
   ↓
Middleware 2
   ↓
Middleware 3
   ↓
Controller
   ↓
Service
   ↓
Repository
   ↓
Response
   ↑
Middleware 3
   ↑
Middleware 2
   ↑
Middleware 1
   ↑
Client
```

---

# 1. What is Middleware?

Middleware is a component in the ASP.NET Core request pipeline.

It can:

* Inspect requests
* Modify requests
* Inspect responses
* Modify responses
* Perform authentication
* Handle exceptions
* Log requests
* Redirect HTTP → HTTPS
* Stop a request
* Pass the request to the next middleware

Basic idea:

```text
Request
   ↓
Middleware
   ↓
Next Middleware
   ↓
Controller
   ↓
Response
```

---

# 2. Middleware Pipeline

Suppose your application has:

```text
Middleware A
Middleware B
Middleware C
Controller
```

The request travels:

```text
Request
  ↓
A
  ↓
B
  ↓
C
  ↓
Controller
```

The response travels back:

```text
Controller
  ↓
C
  ↓
B
  ↓
A
  ↓
Response
```

This is why middleware ordering is important.

---

# 3. Simple Middleware Example

```csharp id="y8bq8a"
app.Use(async (context, next) =>
{
    Console.WriteLine("Before Controller");

    await next();

    Console.WriteLine("After Controller");
});
```

The important part is:

```csharp id="6kgqf3"
await next();
```

It means:

> Continue processing the request through the next middleware.

---

# 4. Before and After `next()`

Example:

```csharp id="2h6d3k"
app.Use(async (context, next) =>
{
    Console.WriteLine("Middleware - Before");

    await next();

    Console.WriteLine("Middleware - After");
});
```

Execution:

```text
Request
   ↓
Middleware - Before
   ↓
Next Middleware
   ↓
Controller
   ↓
Middleware - After
   ↓
Response
```

This is sometimes described as a **pipeline** or **onion-style execution**.

---

# 5. Built-in Middleware

ASP.NET Core provides many middleware components.

Common examples:

```csharp
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.UseExceptionHandler();
```

And:

```csharp
app.MapControllers();
```

maps controller endpoints.

---

# 6. `Use`

`Use` is used to add middleware that can perform work and optionally call the next middleware.

Example:

```csharp id="r0i5p6"
app.Use(async (context, next) =>
{
    Console.WriteLine("Before");

    await next();

    Console.WriteLine("After");
});
```

Think:

```text id="a1g4t7"
Use
 ↓
Do something
 ↓
Call next
 ↓
Continue
```

---

# 7. `Run`

`Run` creates terminal middleware.

Example:

```csharp id="tqj1nj"
app.Run(async context =>
{
    await context.Response.WriteAsync("Hello from middleware");
});
```

`Run` does not call another middleware.

So:

```text id="f9j5on"
Request
   ↓
Run
   ↓
Response
```

Anything registered after a terminal `Run` will not execute for that request.

---

# 8. `Use` vs `Run`

### `Use`

Can continue the pipeline:

```csharp id="i6p4p1"
app.Use(async (context, next) =>
{
    await next();
});
```

### `Run`

Terminates the pipeline:

```csharp id="n4f0qk"
app.Run(async context =>
{
    await context.Response.WriteAsync("Done");
});
```

Easy memory:

```text
Use → Continue
Run → Stop
```

---

# 9. `Map`

`Map` branches the middleware pipeline based on a request path.

Example:

```csharp id="n0k0nd"
app.Map("/admin", adminApp =>
{
    adminApp.Run(async context =>
    {
        await context.Response.WriteAsync(
            "Admin area");
    });
});
```

Request:

```text
/admin
```

goes into that branch.

Conceptually:

```text id="n8x1aw"
                    Request
                       │
             ┌─────────┴─────────┐
             ↓                   ↓
          /admin              Other
             ↓                   ↓
       Admin pipeline       Main pipeline
```

---

# 10. `Use` vs `Run` vs `Map`

| Method | Purpose                                |
| ------ | -------------------------------------- |
| `Use`  | Add middleware and optionally continue |
| `Run`  | Add terminal middleware                |
| `Map`  | Branch pipeline based on path          |

Remember:

```text
Use → Continue
Run → Terminate
Map → Branch
```

---

# 11. Middleware Ordering

Middleware executes in the order it is registered.

Example:

```csharp id="l4ydxk"
app.Use(A);

app.Use(B);

app.Use(C);

app.MapControllers();
```

Request:

```text id="p3a5om"
Request
 ↓
A
 ↓
B
 ↓
C
 ↓
Controller
```

Response:

```text id="7t9q0h"
Controller
 ↓
C
 ↓
B
 ↓
A
 ↓
Response
```

---

# 12. Why Ordering Matters

Consider:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

Authentication should happen before authorization because authorization needs to know who the user is.

Typical order:

```csharp id="4m3i8g"
app.UseAuthentication();

app.UseAuthorization();
```

Similarly, exception handling should generally be placed early so it can catch exceptions from later middleware/endpoints.

---

# 13. Typical ASP.NET Core Pipeline

A common API application might look like:

```csharp id="7z3bpn"
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

The exact pipeline depends on your application and features.

---

# 14. Custom Middleware

You can create your own middleware.

For example:

```text
Request Logging Middleware
Authentication Middleware
Exception Middleware
Performance Middleware
```

Let's create a custom middleware.

---

# 15. Create `RequestLoggingMiddleware`

Create:

```text
Middleware
└── RequestLoggingMiddleware.cs
```

Code:

```csharp id="5sv0r6"
using System.Diagnostics;

namespace Day14MiddlewareConfigurationLogging.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        Console.WriteLine(
            $"Request: {context.Request.Method} {context.Request.Path}");

        await _next(context);

        stopwatch.Stop();

        Console.WriteLine(
            $"Response: {context.Response.StatusCode} " +
            $"Time: {stopwatch.ElapsedMilliseconds} ms");
    }
}
```

---

# 16. Register Custom Middleware

In `Program.cs`:

```csharp id="byf2ce"
app.UseMiddleware<RequestLoggingMiddleware>();
```

Example:

```csharp id="r8g1sm"
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
```

---

# 17. Middleware Execution

Request:

```text
GET /api/employees
```

Output:

```text
Request: GET /api/employees

Controller executes

Response: 200
Time: 15 ms
```

This is useful for understanding request processing.

---

# 18. Global Exception Middleware

This is one of today's most important practical exercises.

Instead of writing:

```csharp id="8vrx8k"
try
{
    // controller logic
}
catch (Exception ex)
{
    // error
}
```

inside every controller, we can handle unexpected exceptions centrally.

Architecture:

```text id="g7z1fp"
Client
  ↓
Global Exception Middleware
  ↓
Controller
  ↓
Service
  ↓
Repository
  ↓
Exception occurs
  ↓
Global Exception Middleware
  ↓
HTTP 500 Response
```

---

# 19. Why Global Exception Handling?

Without centralized handling:

```text
Controller 1 → try/catch
Controller 2 → try/catch
Controller 3 → try/catch
Service 1    → try/catch
Service 2    → try/catch
```

This becomes repetitive.

With global handling:

```text
                Global Exception Middleware
                         ↓
             Handles unexpected exceptions
                         ↓
        ┌────────────────┼────────────────┐
        ↓                ↓                ↓
   Controller 1     Controller 2     Controller 3
```

---

# 20. Create Exception Middleware

Create:

```text
Middleware
└── GlobalExceptionMiddleware.cs
```

Code:

```csharp id="6v0jz2"
using System.Net;
using System.Text.Json;

namespace Day14MiddlewareConfigurationLogging.Middleware;

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

            await HandleExceptionAsync(context);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context)
    {
        context.Response.StatusCode =
            (int)HttpStatusCode.InternalServerError;

        context.Response.ContentType =
            "application/json";

        var response = new
        {
            statusCode = 500,
            message = "An unexpected error occurred."
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}
```

---

# 21. Register Global Exception Middleware

In `Program.cs`:

```csharp id="j3nq3b"
app.UseMiddleware<GlobalExceptionMiddleware>();
```

Put it early in the pipeline:

```csharp id="6n3vkn"
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

Now exceptions occurring later in the pipeline can be caught by it.

---

# 22. Test Global Exception Middleware

Create a test endpoint:

```csharp id="bcb5ij"
[HttpGet("error")]
public IActionResult TestError()
{
    throw new Exception("Something went wrong.");
}
```

Request:

```text
GET /api/employees/error
```

The exception travels backward:

```text id="4kw2l3"
Controller
   ↓
Exception
   ↓
GlobalExceptionMiddleware
   ↓
Log exception
   ↓
Return 500
```

Response:

```json id="j0g5at"
{
  "statusCode": 500,
  "message": "An unexpected error occurred."
}
```

Notice that the internal exception details aren't exposed to the client.

---

# 23. Configuration

Now let's learn application configuration.

ASP.NET Core commonly uses:

```text
appsettings.json
appsettings.Development.json
appsettings.Production.json
```

---

# 24. `appsettings.json`

Example:

```json id="c4qz10"
{
  "AppSettings": {
    "ApplicationName": "Employee Management API",
    "Version": "1.0"
  }
}
```

This contains application configuration.

---

# 25. `appsettings.Development.json`

Example:

```json id="6q5c1e"
{
  "AppSettings": {
    "ApplicationName": "Employee Management API - Development",
    "Version": "1.0-DEV"
  }
}
```

When the application runs in Development environment, environment-specific configuration can override matching values from `appsettings.json`.

---

# 26. `appsettings.Production.json`

Example:

```json id="e9i3e0"
{
  "AppSettings": {
    "ApplicationName": "Employee Management API",
    "Version": "1.0"
  }
}
```

Production-specific settings can be placed here.

### Important

Don't put passwords, API keys, connection secrets, or other sensitive credentials into source-controlled `appsettings.json`.

For development, use **User Secrets**; in deployment, environment variables or your hosting platform's secret/configuration mechanism are common choices.

---

# 27. Configuration Hierarchy

Conceptually:

```text id="0r3m6v"
appsettings.json
       ↓
appsettings.Development.json
       ↓
Environment Variables
       ↓
Command Line
```

ASP.NET Core combines configuration from multiple providers.

The exact precedence depends on the configured providers, but with the default host configuration, later providers can override earlier values.

---

# 28. Configuration Sources

You need to know these:

```text
JSON
Environment Variables
Command Line
User Secrets
```

---

# 29. JSON Configuration

Example:

```json id="r36g9h"
{
  "AppSettings": {
    "ApplicationName": "Employee API",
    "MaxEmployees": 1000
  }
}
```

---

# 30. Environment Variables

You can configure values through environment variables.

For nested configuration:

```text
AppSettings__ApplicationName
```

The double underscore:

```text
__
```

represents a configuration hierarchy separator.

For example:

```text
AppSettings__ApplicationName=Employee API
```

maps conceptually to:

```json
{
  "AppSettings": {
    "ApplicationName": "Employee API"
  }
}
```

Environment variables are particularly useful in deployment environments.

---

# 31. Command Line Configuration

You can also provide configuration through command-line arguments.

For example, applications can receive configuration values through arguments such as:

```text
--AppSettings:ApplicationName="Employee API"
```

The exact command syntax depends on shell/platform and configuration provider parsing.

The important concept is:

```text
Command line
      ↓
Configuration
```

---

# 32. User Secrets

User Secrets are intended primarily for **development-time sensitive values**.

For example:

```text
Database connection string
API key
Development password
```

They keep secrets outside the project source files.

In Visual Studio:

```text
Right-click project
       ↓
Manage User Secrets
```

This creates a development-only secrets store associated with the project.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  }
}
```

Do not commit secret values into Git.

---

# 33. `IConfiguration`

ASP.NET Core provides:

```csharp id="i7aq2m"
IConfiguration
```

to read configuration values.

Example:

```csharp id="n6s0wy"
public class EmployeeService
{
    private readonly IConfiguration _configuration;

    public EmployeeService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}
```

Read:

```csharp id="3pmbn7"
string? appName =
    _configuration["AppSettings:ApplicationName"];
```

---

# 34. Configuration Key Syntax

Given:

```json id="f2o3z1"
{
  "AppSettings": {
    "ApplicationName": "Employee API",
    "Version": "1.0"
  }
}
```

Read:

```csharp id="fpxqna"
_configuration["AppSettings:ApplicationName"]
```

and:

```csharp id="z7i0a9"
_configuration["AppSettings:Version"]
```

The colon:

```text
:
```

represents the hierarchy.

---

# 35. Strongly Typed Configuration

Instead of doing this everywhere:

```csharp
_configuration["AppSettings:ApplicationName"]
```

we can create a configuration class.

This is called the:

> **Options Pattern**

---

# 36. Options Class

Create:

```text
Configuration
└── AppSettings.cs
```

```csharp id="4n8sqp"
namespace Day14MiddlewareConfigurationLogging.Configuration;

public class AppSettings
{
    public string ApplicationName { get; set; } = "";

    public string Version { get; set; } = "";
}
```

---

# 37. Register Options

In `Program.cs`:

```csharp id="g1h0mg"
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));
```

This connects:

```text
appsettings.json
       ↓
AppSettings
```

---

# 38. `IOptions<T>`

Now a service can receive:

```csharp id="v3y0w5"
using Microsoft.Extensions.Options;

public class EmployeeService
{
    private readonly AppSettings _settings;

    public EmployeeService(
        IOptions<AppSettings> options)
    {
        _settings = options.Value;
    }

    public void PrintSettings()
    {
        Console.WriteLine(
            _settings.ApplicationName);

        Console.WriteLine(
            _settings.Version);
    }
}
```

This is much cleaner than repeatedly accessing string keys.

---

# 39. Options Pattern Flow

```text id="bqg7e4"
appsettings.json
       ↓
"AppSettings"
       ↓
Configure<AppSettings>()
       ↓
IOptions<AppSettings>
       ↓
EmployeeService
```

---

# 40. `IOptions<T>` vs `IConfiguration`

### IConfiguration

Good for directly reading individual configuration values:

```csharp id="2a8i9u"
_configuration["AppSettings:Version"]
```

### IOptions<T>

Good for strongly typed related settings:

```csharp id="6h4p4b"
IOptions<AppSettings>
```

Example:

```text id="d3w9g6"
AppSettings
├── ApplicationName
├── Version
└── MaxEmployees
```

---

# 41. `IOptionsSnapshot<T>` and `IOptionsMonitor<T>`

You should also know these names for interviews.

### `IOptions<T>`

Provides configured options and is commonly used when settings don't need to be refreshed dynamically.

### `IOptionsSnapshot<T>`

Useful for scoped scenarios and can provide updated configuration values for a new scope/request when the underlying configuration source supports reload.

### `IOptionsMonitor<T>`

Supports monitoring options changes and is useful when you need current values and change notifications.

Memory:

```text id="8xv8v5"
IOptions
    ↓
Basic options

IOptionsSnapshot
    ↓
Scoped/request-oriented options

IOptionsMonitor
    ↓
Monitor changes
```

For your current fundamentals, focus first on:

```csharp
IOptions<AppSettings>
```

---

# 42. Logging

Now the third major topic:

> **Logging**

Logging helps you understand what your application is doing.

Examples:

```text
Application started
Request received
Employee created
Database operation failed
Unexpected exception
```

---

# 43. `ILogger`

ASP.NET Core provides:

```csharp id="7s8h2a"
ILogger<T>
```

Example:

```csharp id="7xw7gk"
public class EmployeeService
{
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        ILogger<EmployeeService> logger)
    {
        _logger = logger;
    }
}
```

The logger itself is supplied through DI.

---

# 44. Log Levels

Important log levels:

```text
Trace
Debug
Information
Warning
Error
Critical
```

You specifically need to remember:

```text
Information
Warning
Error
Critical
```

---

# 45. Information

Use for normal application events.

```csharp id="r5s4pz"
_logger.LogInformation(
    "Employee {EmployeeId} was created.",
    employee.Id);
```

Example:

```text
Employee 10 was created.
```

---

# 46. Warning

Use when something unusual happened but the application can continue.

```csharp id="z5cb1q"
_logger.LogWarning(
    "Employee {EmployeeId} was not found.",
    id);
```

---

# 47. Error

Use when an operation failed.

```csharp id="j38c2m"
_logger.LogError(
    "Failed to create employee {EmployeeId}.",
    employee.Id);
```

When an exception object is available, pass it separately:

```csharp id="7g3l8v"
_logger.LogError(
    ex,
    "Failed to process employee {EmployeeId}.",
    employee.Id);
```

This preserves exception information for the logging provider.

---

# 48. Critical

Critical indicates a serious failure that may require immediate attention.

```csharp id="4d5jcz"
_logger.LogCritical(
    "Application startup configuration is invalid.");
```

Use it for genuinely severe failures, not ordinary errors.

---

# 49. Logging Example in Service

```csharp id="a0u0d4"
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

    public List<Employee> GetEmployees()
    {
        _logger.LogInformation(
            "Fetching all employees.");

        var employees = _repository.GetEmployees();

        _logger.LogInformation(
            "Fetched {Count} employees.",
            employees.Count);

        return employees;
    }
}
```

---

# 50. Structured Logging

This is very important.

Avoid:

```csharp id="0l6fpy"
_logger.LogInformation(
    "Employee " + employee.Id + " created");
```

Prefer:

```csharp id="k4g5z9"
_logger.LogInformation(
    "Employee {EmployeeId} created.",
    employee.Id);
```

Here:

```text
{EmployeeId}
```

is a structured property.

The logging system can retain the value as structured data rather than treating the whole message as one preformatted string.

---

# 51. Another Structured Logging Example

```csharp id="qf4i3n"
_logger.LogInformation(
    "Employee {EmployeeId} from {Department} created with salary {Salary}.",
    employee.Id,
    employee.Department,
    employee.Salary);
```

Properties:

```text
EmployeeId
Department
Salary
```

This is better for searching and analyzing logs.

---

# 52. Logging Exceptions

Don't do only:

```csharp id="nq3l5c"
_logger.LogError(ex.Message);
```

Prefer:

```csharp id="x0v3lm"
_logger.LogError(
    ex,
    "Error while retrieving employee {EmployeeId}.",
    id);
```

Passing `ex` allows the logging system to capture exception details such as the exception type and stack trace.

---

# 53. Global Exception Middleware + Logging

This is where today's topics come together.

```csharp id="f7h8ax"
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

        await HandleExceptionAsync(context);
    }
}
```

Flow:

```text id="q8x9bw"
Request
   ↓
Middleware
   ↓
Controller
   ↓
Service
   ↓
Exception
   ↓
Global Exception Middleware
   ↓
ILogger.LogError()
   ↓
HTTP 500 Response
```

---

# 54. Complete Day 14 Project Structure

I recommend creating:

```text id="i2k0j7"
Day14MiddlewareConfigurationLogging
│
├── Controllers
│   └── EmployeesController.cs
│
├── Middleware
│   ├── RequestLoggingMiddleware.cs
│   └── GlobalExceptionMiddleware.cs
│
├── Configuration
│   └── AppSettings.cs
│
├── Models
│   └── Employee.cs
│
├── Services
│   ├── IEmployeeService.cs
│   └── EmployeeService.cs
│
├── Repositories
│   ├── IEmployeeRepository.cs
│   └── EmployeeRepository.cs
│
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── Day14MiddlewareConfigurationLogging.csproj
```

---

# 55. Complete `appsettings.json`

```json id="9s7jka"
{
  "AppSettings": {
    "ApplicationName": "Employee Management API",
    "Version": "1.0",
    "MaxEmployees": 1000
  },

  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

---

# 56. Configuration Class

```csharp id="5t4m1e"
namespace Day14MiddlewareConfigurationLogging.Configuration;

public class AppSettings
{
    public string ApplicationName { get; set; } = "";

    public string Version { get; set; } = "";

    public int MaxEmployees { get; set; }
}
```

---

# 57. Program.cs

```csharp id="8sj5ip"
using Day14MiddlewareConfigurationLogging.Configuration;
using Day14MiddlewareConfigurationLogging.Middleware;
using Day14MiddlewareConfigurationLogging.Repositories;
using Day14MiddlewareConfigurationLogging.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<
    IEmployeeRepository,
    EmployeeRepository>();

builder.Services.AddScoped<
    IEmployeeService,
    EmployeeService>();

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

---

# 58. Middleware Ordering in This Project

Our pipeline is:

```text id="2gkwzq"
Request
   ↓
GlobalExceptionMiddleware
   ↓
RequestLoggingMiddleware
   ↓
HTTPS Redirection
   ↓
Authorization
   ↓
Controller
   ↓
Response
   ↑
RequestLoggingMiddleware
   ↑
GlobalExceptionMiddleware
   ↑
Response
```

Putting exception middleware early allows it to catch exceptions from downstream components.

---

# 59. Important Note About `UseHttpsRedirection`

If HTTPS redirection happens after custom logging middleware:

```text
Request
 ↓
RequestLoggingMiddleware
 ↓
UseHttpsRedirection
```

the logging middleware can observe the request before the redirect.

Middleware ordering determines what each component can observe and handle.

---

# 60. Logging Configuration

In:

```text
appsettings.json
```

we have:

```json id="g5p2g5"
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  }
}
```

This means:

```text
Application logs
→ Information and above

Microsoft.AspNetCore logs
→ Warning and above
```

---

# 61. Why Configure Different Log Levels?

Framework components can generate many logs.

For example:

```text
Your Application
→ Information

Microsoft.AspNetCore
→ Warning
```

This can reduce unnecessary framework log noise while keeping your application logs useful.

---

# 62. `ILogger<T>` and DI

Notice that logging also uses dependency injection.

```csharp id="n4v7uc"
public EmployeeService(
    IEmployeeRepository repository,
    ILogger<EmployeeService> logger)
{
    _repository = repository;
    _logger = logger;
}
```

ASP.NET Core automatically provides:

```text
ILogger<EmployeeService>
```

You don't do:

```csharp id="z9nq6k"
new Logger()
```

---

# 63. Day 14 Architecture

You now have:

```text id="qj3i2g"
                    CLIENT
                       ↓
                 HTTP Request
                       ↓
        ┌──────────────────────────┐
        │ Global Exception         │
        │ Middleware               │
        └──────────────────────────┘
                       ↓
        ┌──────────────────────────┐
        │ Request Logging          │
        │ Middleware               │
        └──────────────────────────┘
                       ↓
                  Controller
                       ↓
                    Service
                       ↓
                  Repository
                       ↓
                   Database
                       ↓
                   Response
                       ↓
                 Middleware
                       ↓
                    CLIENT
```

Configuration:

```text id="q5g0t2"
appsettings.json
       ↓
IConfiguration
       ↓
Options Pattern
       ↓
IOptions<AppSettings>
       ↓
Service
```

Logging:

```text id="3l9h9b"
Controller / Service / Middleware
             ↓
          ILogger<T>
             ↓
       Logging Provider
             ↓
           Output
```

---

# 64. Very Important Interview Questions

### 1. What is middleware?

A component in the ASP.NET Core request pipeline that can process HTTP requests and responses and optionally pass control to the next component.

### 2. What does `next()` do?

It invokes the next middleware in the pipeline.

### 3. Difference between `Use` and `Run`?

```text
Use → can continue pipeline
Run → terminal middleware
```

### 4. What does `Map` do?

It branches the middleware pipeline based on a path or other mapping condition.

### 5. Why does middleware order matter?

Because middleware executes in registration order for requests and in reverse order as control returns for responses.

### 6. Where should global exception handling middleware generally be placed?

Early in the pipeline so it can observe and handle exceptions from downstream middleware/endpoints.

### 7. What is `IConfiguration`?

An abstraction for accessing configuration values from the application's configuration system.

### 8. What is the Options Pattern?

A strongly typed approach for binding a configuration section to a C# options class.

### 9. What is `IOptions<T>`?

A DI-provided abstraction for accessing configured options of type `T`.

### 10. What is `IOptionsSnapshot<T>`?

A scoped options abstraction useful when configuration values may need to be refreshed for new scopes/requests.

### 11. What is `IOptionsMonitor<T>`?

An abstraction that supports observing configuration changes and accessing current option values.

### 12. What is `ILogger<T>`?

A typed logging abstraction used to write application logs.

### 13. What are common log levels?

```text
Trace
Debug
Information
Warning
Error
Critical
```

### 14. What is structured logging?

Logging named values as structured properties rather than only building a formatted string.

Example:

```csharp
_logger.LogInformation(
    "Employee {EmployeeId} created.",
    employee.Id);
```

### 15. How should exceptions be logged?

Pass the exception object to the logger:

```csharp
_logger.LogError(
    ex,
    "Error while processing employee {EmployeeId}.",
    id);
```

---

# 65. Day 14 Cheat Sheet

```text id="2j8m3q"
MIDDLEWARE
│
├── Use
│   └── Continue pipeline
│
├── Run
│   └── Terminal
│
├── Map
│   └── Branch pipeline
│
├── Ordering
│   └── Very important
│
└── Custom Middleware
    └── RequestDelegate


CONFIGURATION
│
├── appsettings.json
├── appsettings.Development.json
├── appsettings.Production.json
│
├── JSON
├── Environment Variables
├── Command Line
└── User Secrets
│
├── IConfiguration
└── Options Pattern
    └── IOptions<T>


LOGGING
│
├── ILogger<T>
│
├── Trace
├── Debug
├── Information
├── Warning
├── Error
└── Critical
│
└── Structured Logging
```

## Final mental model

```text
                    ASP.NET CORE
                         │
        ┌────────────────┼────────────────┐
        ↓                ↓                ↓
   Middleware       Configuration       Logging
        │                │                │
   Request/Response  IConfiguration    ILogger<T>
        │                │                │
   Use / Run / Map   IOptions<T>       Log levels
        │                │                │
   Exception          appsettings       Structured
   Handling           Environment       Logging
                      Variables
```

### The three things to remember from Day 14

```text
1. Middleware
   → Controls the HTTP request/response pipeline.

2. Configuration
   → Keeps application settings outside application code.

3. Logging
   → Records what the application is doing and helps diagnose failures.
```

And your growing ASP.NET Core architecture is now:

```text
Client
  ↓
Middleware
  ↓
Controller
  ↓
Service
  ↓
Repository
  ↓
Database

Cross-cutting concerns:
├── Exception Handling → Middleware
├── Configuration      → IConfiguration / Options
└── Logging            → ILogger<T>
```


