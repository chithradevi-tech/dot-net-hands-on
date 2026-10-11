# 🗓️ WEEK 2 — ASP.NET CORE + WEB API

# Day 8 — Exception Handling + Async Programming

## Exception Handling

Learn:

```csharp
try
catch
finally
throw
```

Also:

* Custom exceptions
* Exception hierarchy
* Multiple catch
* Exception filters
* Global exception handling

Understand:

```text
Exception
SystemException
ApplicationException
```

## Async Programming

Learn:

* Synchronous vs asynchronous
* `Task`
* `Task<T>`
* `async`
* `await`
* `Task.WhenAll`
* `Task.WhenAny`
* CancellationToken
* Parallel vs async

Understand:

```text
Thread
Task
ThreadPool
async/await
```

---

# Day 9 — ASP.NET Core Fundamentals

Learn:

* What is ASP.NET Core?
* ASP.NET Core architecture
* Request/Response lifecycle
* Hosting
* Kestrel
* Web server
* HTTP
* HTTPS

Create:

```bash
dotnet new webapi
```

Understand project structure:

```text
Controllers
Program.cs
appsettings.json
appsettings.Development.json
Properties
.csproj
```

Learn:

* `Program.cs`
* Minimal hosting model
* Application startup
* Service registration
* Middleware pipeline

---

# Day 10 — Controllers & Routing

Learn:

## Controllers

```csharp
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
```

## HTTP methods

* GET
* POST
* PUT
* PATCH
* DELETE

## Routing

Learn:

* Conventional routing
* Attribute routing
* Route parameters
* Query parameters
* Route constraints

Example:

```text
GET /api/employees
GET /api/employees/10
POST /api/employees
PUT /api/employees/10
DELETE /api/employees/10
```

---

# Day 11 — REST APIs

Learn REST principles:

* Resource-based URLs
* Statelessness
* HTTP methods
* HTTP status codes
* Request headers
* Response headers
* Content negotiation

Status codes:

```text
200 OK
201 Created
204 No Content
400 Bad Request
401 Unauthorized
403 Forbidden
404 Not Found
409 Conflict
500 Internal Server Error
```

Learn:

* Request body
* Query string
* Route parameter
* Headers
* API response format

---

# Day 12 — Models, DTOs & JSON

Learn:

## Models

Entity/domain models.

## DTOs

```text
EmployeeRequestDto
EmployeeResponseDto
EmployeeUpdateDto
```

Understand:

**Entity vs DTO**

Learn:

* Model binding
* Validation
* Data annotations
* Required
* StringLength
* Range
* EmailAddress

## JSON

Learn:

* Serialization
* Deserialization
* `System.Text.Json`
* JSON options
* Property naming
* Null handling
* Nested objects
* Lists
* DateTime handling

---

# Day 13 — Dependency Injection

This is a core ASP.NET Core concept.

Learn:

### DI concepts

* Dependency
* Dependency Injection
* IoC
* Service container

### Lifetimes

```text
Transient
Scoped
Singleton
```

Understand when each should be used.

Example architecture:

```text
Controller
     ↓
Service
     ↓
Repository
     ↓
Database
```

Learn:

```csharp
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
```

Also understand:

* Constructor injection
* Service registration
* Interface-based design

---

# Day 14 — Middleware + Configuration + Logging

## Middleware

Understand:

```text
Request
 ↓
Middleware
 ↓
Middleware
 ↓
Controller
 ↓
Response
```

Learn:

* Built-in middleware
* Custom middleware
* `Use`
* `Run`
* `Map`
* Middleware ordering

Create:

**Global Exception Middleware**

---

## Configuration

Learn:

```text
appsettings.json
appsettings.Development.json
appsettings.Production.json
```

Configuration sources:

* JSON
* Environment variables
* Command line
* User secrets

Learn:

* `IConfiguration`
* Options pattern
* `IOptions<T>`

---

## Logging

Learn:

* `ILogger`
* Log levels
* Information
* Warning
* Error
* Critical
* Structured logging
* Logging exceptions

---

# Phase 3: .NET and ASP.NET Core Basics — Complete Class Notes

Level: Beginner to Intermediate · IDE: Visual Studio · Target framework: .NET 8 · Goal: Build a strong foundation for ASP.NET Core Web API development

These notes are organized for classroom teaching. Each topic includes definitions, practical examples, explanations and demonstrations you can perform in Visual Studio.

Important for your class: .NET 8 is an LTS release, but its support ends in November 2026. Since your broader learning goal is .NET 10, you can teach the fundamentals using .NET 8 as requested, then demonstrate the same concepts in .NET 10. For a new production project today, prefer a supported .NET version. [Official .NET support policy](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core).

# 1. What are .NET, .NET Framework and ASP.NET Core?

Students often confuse these three terms. Explain their differences first.

## 1.1 What is .NET?

.NET is a development platform used to build and run applications.

It provides:

- A runtime for executing managed code.
- Base class libraries for strings, collections, files and networking.
- Tools for compiling, testing and publishing applications.
- Frameworks for web development, desktop development, cloud applications and more.

Example C# program:

```
Console.WriteLine("Welcome to .NET");
```

C# is the programming language, and .NET provides the runtime and libraries that support the application.

## 1.2 What is .NET Framework?

.NET Framework is Microsoft's original Windows-focused implementation of .NET.

It is commonly found in legacy enterprise applications, older ASP.NET applications and Windows desktop software.

## 1.3 What is ASP.NET Core?

ASP.NET Core is a modern, cross-platform framework for building web applications and HTTP APIs.

It supports:

- RESTful Web APIs
- MVC web applications
- Razor Pages
- Minimal APIs
- Authentication and authorization
- Middleware and dependency injection

### Comparison

| Feature             | .NET                       | .NET Framework                                       | ASP.NET Core                    |
| ------------------- | -------------------------- | ---------------------------------------------------- | ------------------------------- |
| What is it?         | Development platform       | Original Windows-focused .NET implementation         | Web framework                   |
| Primary purpose     | Build and run applications | Maintain and develop compatible Windows applications | Build web applications and APIs |
| Cross-platform      | Yes, for modern .NET       | Primarily Windows                                    | Yes                             |
| Web API development | Platform foundation        | Supports legacy ASP.NET Web API                      | Modern recommended approach     |

Classroom example: If you build an employee REST API in C#, you can use ASP.NET Core for HTTP endpoints, .NET libraries for application functionality, and the .NET runtime to execute the application.

Official resource: [Introduction to .NET](https://learn.microsoft.com/en-us/dotnet/core/introduction).

# 2. .NET SDK and Runtime

## 2.1 What is the .NET SDK?

SDK stands for Software Development Kit.

The SDK contains the tools required to create, build, test and publish .NET applications.

It includes the `dotnet` CLI, compiler tooling and build infrastructure.

## 2.2 What is the .NET Runtime?

The runtime provides the environment required to execute a compiled .NET application.

For example, a deployed framework-dependent ASP.NET Core application needs a compatible runtime on the target machine.

### Difference

| SDK                                    | Runtime                                        |
| -------------------------------------- | ---------------------------------------------- |
| Used to develop and build applications | Used to execute applications                   |
| Includes development and build tools   | Includes execution components                  |
| Needed on a developer machine          | Needed to run framework-dependent applications |

## 2.3 Check the installed versions

Open the Visual Studio terminal, Command Prompt or PowerShell.

```
dotnet --info
```

List installed SDKs:

```
dotnet --list-sdks
```

List installed runtimes:

```
dotnet --list-runtimes
```

Create a .NET 8 Web API project:

```
dotnet new webapi -n EmployeeApi --framework net8.0
```

Move into the project:

```
cd EmployeeApi
```

Build the project:

```
dotnet build
```

Run the project:

```
dotnet run
```

Teaching point: A target framework such as `net8.0` identifies the framework the project targets. The installed SDK and available runtime determine whether you can build and run it on your machine.

Official resource: [Install .NET](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

# 3. CLR, Managed Code, Assemblies and NuGet

## 3.1 CLR — Common Language Runtime

The CLR is the execution environment used by the .NET Framework. In modern .NET, the corresponding runtime implementation is CoreCLR.

Its responsibilities include:

- Executing managed code.
- Garbage collection.
- Exception handling.
- Type safety and runtime services.
- Supporting debugging and diagnostics.

### How a C# program executes

C# source code

Program.cs

C# compiler

Compiles the source into intermediate language and metadata

Assembly

DLL or EXE containing IL and metadata

.NET runtime and JIT

IL is compiled to machine code as needed for execution

Explain that JIT means Just-In-Time compilation. The runtime also provides services such as garbage collection and exception handling.

## 3.2 Managed code

Managed code is code executed under the control of a .NET runtime.

Example:

```
string name = "Arun";
Console.WriteLine(name);
```

The runtime manages memory for the string object, and garbage collection can reclaim memory for objects that are no longer reachable.

Important: Garbage collection manages memory, but it does not automatically dispose every external resource, such as file handles or database connections.

## 3.3 Assemblies

An assembly is a compiled deployment and versioning unit in .NET.

Common assembly file extensions include:

- `.dll`
- `.exe`

A Web API project typically produces an assembly such as `EmployeeApi.dll`.

Assemblies contain compiled code and metadata, and can include resources.

## 3.4 NuGet

NuGet is the package management system for .NET.

It allows developers to install and manage reusable libraries.

Examples of package categories include:

- Database access libraries
- Logging integrations
- Authentication libraries
- Testing frameworks

Install a package using the CLI:

```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

The exact package version should be compatible with your target framework and the Entity Framework Core version you choose.

Visual Studio also provides Manage NuGet Packages through the project context menu.

Official resources:

- [CLR and managed execution](https://learn.microsoft.com/en-us/dotnet/standard/clr)
- [Assemblies in .NET](https://learn.microsoft.com/en-us/dotnet/standard/assembly/)
- [NuGet documentation](https://learn.microsoft.com/en-us/nuget/)

# 4. Create a Web API Project Using Visual Studio

This is a practical demonstration you can perform during class.

## Step 1: Create the project

[How to Build Your First REST API with ASP.NET Core 8 | by Valivati Navya | Medium](https://images.openai.com/static-rsc-4/bb7Dq2ejIqawW8pkusAugcodQ-lW_qiNZwfclkphBN9Bjug0dqaBi3FhWgiaYuK0Y1NQCfPg9tKrWeNIqMBchxAwZztlGX0n8MZQSks9SX5qr-J9kyN9ia0zxhPU5UPldDgTlGWIxIa9ExvazunIWR-zVOvDKIS4ZTlAMgc2_Ow?purpose=inline)

Open Visual Studio

Select Create a new project.

[How Fast is ASP.NET Core Web API? A Performance Analysis](https://images.openai.com/static-rsc-4/wk2PnR2qqOGcUAmCyUuVUJnT6ibR0TBKC2_t3tqKV0RA0ns030f7wsCD7BQPfC8W1FC97VhxlRU0RyVDIhU3qOfJ9IaEcb_SsDP_8OMKl8i9-bMsY_L9CBtdDo4v30g6zZuqxxZFE7jBFrpkdlRqa8LaqK21suqRl8bJClrdCxw?purpose=inline)

Choose the template

Search for `ASP.NET Core Web API`, select C#, and click Next.

[Building an ASP.NET Core Backend (Linux and Windows) for the Document Editor and Viewer](https://images.openai.com/static-rsc-4/KGr73BY79gnrXK_R3N8QxOXfGF2EYQ4nakRXhxuJcdc4TONLfwiKnHvOYhe0pNNFhA6lHXbhKVFqewzeF5Od9iymaixYev7tsBrE1B4qESKFDNfkKg_VN9qywJqjvdgPywOonsi1KeXg-ZUy0x3IFInuqha36K8aSct11oyaSPs?purpose=inline)

Configure the project

Project name: `EmployeeApi`

Framework: `.NET 8.0`, if available and installed.

Select the appropriate authentication setting for the exercise. For a beginner demo, choose no authentication, and keep HTTPS enabled.

The template may offer options such as controllers, OpenAPI support and container support. For this phase, a controller-based Web API is a useful choice.

Click Create.

## Step 2: Run the application

Press `F5` to run with debugging, or `Ctrl + F5` to run without debugging.

Visual Studio starts the application and may open a browser or Swagger UI, depending on the template and project configuration.

## Step 3: Test an endpoint

If the generated template includes the weather forecast endpoint, use the URL shown by Visual Studio or the OpenAPI interface to send a GET request.

Do not assume a fixed port: the development port is usually configured in the project settings.

Official guide: [Create a web API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api).

# 5. Understand the Web API Project Structure

A typical controller-based project may look like this:

EmployeeApi/

Program.cs

Configures services and the HTTP request pipeline.

appsettings.json

Stores general configuration such as logging settings.

appsettings.Development.json

Optional configuration overrides for Development.

Controllers/

Contains controller classes and API endpoints when using controllers.

EmployeeApi.csproj

Defines target framework, dependencies and build properties.

launchSettings.json

Defines local development launch profiles.

obj/ and bin/

Generated build and intermediate files; normally not edited by hand.

Depending on the template, you may also see `Properties/launchSettings.json`, `WeatherForecast.cs`, `appsettings.Production.json`, or an OpenAPI configuration file.

Explain that a Minimal API project may not contain a `Controllers` folder. The project structure depends on the template and architecture.

# 6. Program.cs and Application Startup

In modern ASP.NET Core, `Program.cs` is the entry point used to configure and start the application.

Example:

```
var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();

var app = builder.Build();

// Configure middleware
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
```

## Explain each line

| Code                                 | Purpose                                                                               |
| ------------------------------------ | ------------------------------------------------------------------------------------- |
| `WebApplication.CreateBuilder(args)` | Creates the application builder and loads standard configuration and hosting defaults |
| `builder.Services`                   | Registers services in the dependency injection container                              |
| `builder.Build()`                    | Builds the configured web application                                                 |
| `app.UseHttpsRedirection()`          | Redirects eligible HTTP requests to HTTPS                                             |
| `app.MapControllers()`               | Maps controller routes to endpoints                                                   |
| `app.Run()`                          | Starts the web application and blocks until shutdown                                  |

Teaching point: Register services before building the application. Configure the request pipeline and endpoints before calling `Run()`.

## Controller example

Create a file named `Controllers/EmployeesController.cs`.

```
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployees()
    {
        var employees = new[]
        {
            new { Id = 1, Name = "Arun" },
            new { Id = 2, Name = "Priya" }
        };

        return Ok(employees);
    }
}
```

Send a GET request to:

```
/api/employees
```

Expected HTTP status: `200 OK`.

The response contains two employee objects in JSON format.

# 7. appsettings.json and Environment Settings

`appsettings.json` stores application configuration in JSON format.

Example:

```
{
  "AppSettings": {
    "ApplicationName": "Employee API",
    "MaxEmployees": 100
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Read a configuration value:

```
string? appName =
    builder.Configuration["AppSettings:ApplicationName"];

Console.WriteLine(appName);
```

The colon separates nested configuration keys.

For example:

```
AppSettings:ApplicationName
```

refers to `ApplicationName` inside `AppSettings`.

## Environment-specific files

ASP.NET Core supports environment-specific configuration files, such as:

- `appsettings.Development.json`
- `appsettings.Staging.json`
- `appsettings.Production.json`

Example `appsettings.Development.json`:

```
{
  "AppSettings": {
    "ApplicationName": "Employee API - Development"
  }
}
```

Environment-specific configuration normally overrides matching settings from `appsettings.json`.

Security warning: Do not commit production passwords, API keys or database credentials into source-controlled configuration files. Use suitable secret stores or environment-based configuration.

Official resource: [Configuration in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/).

# 8. Dependency Injection (DI)

Dependency Injection is a technique for supplying an object with the dependencies it needs rather than having it construct them internally.

ASP.NET Core includes a built-in dependency injection container.

## 8.1 Without DI

```
public class EmployeeController
{
    private readonly EmployeeService _service =
        new EmployeeService();
}
```

The controller directly creates its dependency, making replacement and isolated testing more difficult.

## 8.2 With DI

Create an interface:

```
public interface IEmployeeService
{
    string GetEmployeeName();
}
```

Implement it:

```
public class EmployeeService : IEmployeeService
{
    public string GetEmployeeName()
    {
        return "Arun";
    }
}
```

Register the service in `Program.cs`:

```
builder.Services.AddScoped<
    IEmployeeService, EmployeeService>();
```

Inject it into a controller:

```
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeesController(IEmployeeService service)
    {
        _service = service;
    }

    [HttpGet("name")]
    public IActionResult GetName()
    {
        return Ok(_service.GetEmployeeName());
    }
}
```

When a request invokes the controller, ASP.NET Core supplies the registered service through its constructor.

## 8.3 DI lifetimes

| Lifetime  | Behaviour                                                    | Common example               |
| --------- | ------------------------------------------------------------ | ---------------------------- |
| Transient | A new instance is created each time the service is requested | Lightweight stateless helper |
| Scoped    | One instance per request scope in a typical Web API          | EF Core `DbContext`          |
| Singleton | One instance shared by the application container             | Thread-safe shared service   |

A singleton should not directly capture a scoped service because their lifetimes are incompatible.

Classroom exercise: Register `IEmployeeService` with DI, inject it into a controller and return the service result from a GET endpoint.

Official resource: [Dependency injection in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection).

# 9. Built-in Logging

Logging records useful information about what an application is doing.

ASP.NET Core provides the `ILogger<T>` abstraction.

## Example

```
public class EmployeeService
{
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        ILogger<EmployeeService> logger)
    {
        _logger = logger;
    }

    public string GetEmployeeName()
    {
        _logger.LogInformation(
            "Fetching employee name");

        return "Arun";
    }
}
```

## Common logging levels

| Level         | Purpose                                          |
| ------------- | ------------------------------------------------ |
| `Trace`       | Very detailed diagnostic information             |
| `Debug`       | Diagnostic information useful during development |
| `Information` | Normal application events                        |
| `Warning`     | Unexpected situations that may need attention    |
| `Error`       | Failures affecting an operation                  |
| `Critical`    | Severe failures                                  |

Example:

```
_logger.LogInformation("Employee retrieved");
_logger.LogWarning("Employee was not found");
_logger.LogError("An error occurred while saving employee");
```

Use structured logging:

```
_logger.LogInformation(
    "Employee {EmployeeId} retrieved",
    employeeId);
```

Avoid logging passwords, access tokens or sensitive personal information.

Official resource: [Logging in .NET and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/).

# 10. Development, Staging and Production Environments

Environments allow applications to use different settings and behaviours during development, testing and deployment.

## Development

Used by developers to build and debug the application.

Typical characteristics:

- Detailed developer exception information.
- Local configuration.
- Development-only tools.

## Staging

Used to validate a release in an environment resembling production.

Typical characteristics:

- Pre-release verification.
- Integration and acceptance testing.
- Production-like infrastructure where possible.

## Production

The live environment used by actual users.

Typical characteristics:

- Appropriate security controls.
- Restricted diagnostic information.
- Production configuration and monitoring.

ASP.NET Core recognizes environment names such as `Development`, `Staging` and `Production`.

Check the current environment:

```
if (app.Environment.IsDevelopment())
{
    app.Logger.LogInformation(
        "Running in Development");
}
```

Use environment-specific middleware or configuration where needed. Never expose detailed exception pages to public production users.

Official resource: [Use multiple environments in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/environments).

# 11. Middleware and the HTTP Request Pipeline

Middleware components process incoming HTTP requests and outgoing HTTP responses.

A request may pass through several middleware components before reaching the selected endpoint.

HTTP Request

Exception handling

HTTPS redirection / routing

Authentication / authorization

Controller or endpoint

HTTP Response

This diagram illustrates a common flow, not a mandatory sequence for every application. Middleware ordering depends on the features being used.

## Example of custom middleware

Add this to `Program.cs` before endpoint mapping:

```
app.Use(async (context, next) =>
{
    app.Logger.LogInformation(
        "Incoming request: {Path}",
        context.Request.Path);

    await next();

    app.Logger.LogInformation(
        "Response status: {StatusCode}",
        context.Response.StatusCode);
});
```

`await next()` invokes the next middleware component. Code after it runs when control returns from the downstream pipeline.

If middleware short-circuits the request and does not call `next()`, later middleware and endpoints may not execute.

### Common built-in middleware

- `UseExceptionHandler`
- `UseHttpsRedirection`
- `UseRouting`
- `UseAuthentication`
- `UseAuthorization`

Teaching point: Middleware order matters. For example, authentication must be configured before authorization so that authorization can evaluate the authenticated user.

Official resource: [ASP.NET Core middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/).

# 12. launchSettings.json

`launchSettings.json` configures how a project starts during local development.

It is usually located at:

```
Properties/launchSettings.json
```

Example:

```
{
  "profiles": {
    "EmployeeApi": {
      "commandName": "Project",
      "launchBrowser": true,
      "applicationUrl": "https://localhost:7001;http://localhost:5001",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

The port numbers are illustrative; use the actual values generated by Visual Studio for your project.

Explain:

- `profiles` — defines launch profiles.
- `commandName` — specifies how the application is launched.
- `launchBrowser` — controls whether the browser opens automatically.
- `applicationUrl` — configures the local listening URLs for the profile.
- `environmentVariables` — supplies environment variables during launch.

Important distinction:

- `launchSettings.json` is primarily for local development.
- It is not normally published as production application configuration.
- Production hosting settings should be configured using the appropriate deployment and hosting mechanisms.

# 13. Configuration Providers and the Options Pattern

This topic builds on `appsettings.json` and dependency injection.

## 13.1 Configuration providers

A configuration provider supplies key-value settings to the application.

Common providers include:

- JSON files
- Environment variables
- Command-line arguments
- User Secrets during development
- Custom or external configuration providers

ASP.NET Core combines configuration providers. If the same key appears in multiple providers, the later provider generally takes precedence.

For the default modern web application builder, environment variables and command-line arguments can override settings from JSON files.

Example:

```
$env:AppSettings__ApplicationName = "Employee API - Local"
dotnet run
```

The double underscore `__` represents a colon in hierarchical configuration keys. In this example, it corresponds to:

```
AppSettings:ApplicationName
```

## 13.2 The Options Pattern

The Options Pattern maps related configuration values into a strongly typed C# class.

Create `AppSettings.cs`:

```
public class AppSettings
{
    public string ApplicationName { get; set; } = "";

    public int MaxEmployees { get; set; }
}
```

Add the settings to `appsettings.json`:

```
{
  "AppSettings": {
    "ApplicationName": "Employee API",
    "MaxEmployees": 100
  }
}
```

Register the options in `Program.cs`:

```
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));
```

Inject `IOptions<AppSettings>` into a service:

```
using Microsoft.Extensions.Options;

public class ApplicationInfoService
{
    private readonly AppSettings _settings;

    public ApplicationInfoService(
        IOptions<AppSettings> options)
    {
        _settings = options.Value;
    }

    public string GetApplicationName()
    {
        return _settings.ApplicationName;
    }
}
```

Register the service:

```
builder.Services.AddScoped<ApplicationInfoService>();
```

This approach avoids scattering configuration key strings throughout your application.

### Options interfaces

| Interface             | Behaviour                                                                                  |
| --------------------- | ------------------------------------------------------------------------------------------ |
| `IOptions<T>`         | Provides configured options; generally suitable for stable settings                        |
| `IOptionsSnapshot<T>` | Recomputes options per scope; commonly used for scoped web requests                        |
| `IOptionsMonitor<T>`  | Supports monitoring current values and change notifications when supported by the provider |

For configuration that requires validation, ASP.NET Core also supports options validation during application startup.

Official resource: [Options pattern in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/options).

# Final classroom practical: Employee Web API foundation

Ask students to build a small API that combines the topics from Phase 3.

## Requirements

- Create a controller-based ASP.NET Core Web API project.
- Create an `EmployeesController`.
- Create an `IEmployeeService` interface and `EmployeeService` implementation.
- Register the service using dependency injection.
- Return sample employee data through a GET endpoint.
- Add logging to the service.
- Store application settings in `appsettings.json`.
- Read the settings through the Options Pattern.
- Add request logging middleware.
- Configure and test a Development launch profile.
- Build and run the application in Visual Studio.
- Test the endpoint using Swagger UI, if available, or another HTTP client.

## Suggested teaching schedule

| Session | Topics                                                         |
| ------- | -------------------------------------------------------------- |
| 1       | .NET, .NET Framework, ASP.NET Core, SDK and runtime            |
| 2       | CLR, managed code, assemblies, NuGet                           |
| 3       | Visual Studio project creation and project structure           |
| 4       | `Program.cs`, configuration and launch settings                |
| 5       | Dependency injection and service lifetimes                     |
| 6       | Logging and application environments                           |
| 7       | Middleware and the HTTP request pipeline                       |
| 8       | Configuration providers, Options Pattern and practical project |

## Questions to ask students

1. What is the difference between .NET and ASP.NET Core?
2. What is the difference between the SDK and runtime?
3. What does the CLR do?
4. What is an assembly?
5. What is dependency injection, and why do we use it?
6. What are transient, scoped and singleton lifetimes?
7. What is middleware?
8. Why does middleware order matter?
9. What is the purpose of `appsettings.json`?
10. How does the Options Pattern simplify configuration?
11. What is the difference between `launchSettings.json` and production configuration?
12. Why should detailed exception information be restricted in production?

