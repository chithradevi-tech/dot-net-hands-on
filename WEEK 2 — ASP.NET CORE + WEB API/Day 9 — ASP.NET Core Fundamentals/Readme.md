# Day 9 — ASP.NET Core Fundamentals

Today we move from **C#/.NET console applications** into **web application development**.

The main goal of Day 9 is to understand **how an HTTP request enters an ASP.NET Core application, passes through middleware, reaches your application logic, and produces an HTTP response.**

---

# 1. What is ASP.NET Core?

**ASP.NET Core** is Microsoft's cross-platform framework for building web applications and web services using .NET and C#.

You can use it to build:

* REST APIs
* Web APIs
* Web applications
* Backend services
* Microservices
* Real-time applications
* Cloud applications

Example:

```text
Angular / React / Mobile App
          ↓
       HTTP Request
          ↓
    ASP.NET Core API
          ↓
       Business Logic
          ↓
       Database
          ↓
    ASP.NET Core API
          ↓
      HTTP Response
```

### Simple definition

> **ASP.NET Core is a cross-platform framework built on .NET for developing modern web applications and APIs.**

It runs on:

* Windows
* Linux
* macOS
* Cloud environments

---

# 2. .NET vs ASP.NET Core

This is an important interview question.

### .NET

.NET is the overall development platform.

It provides:

```text
.NET
├── C#
├── Runtime
├── Base Class Libraries
├── Console applications
├── Class libraries
├── ASP.NET Core
├── Worker Services
└── Other application models
```

### ASP.NET Core

ASP.NET Core is the **web development framework built on .NET**.

```text
.NET
   ↓
ASP.NET Core
   ↓
Web Applications / Web APIs
```

### Easy way to remember

**.NET = Platform**

**ASP.NET Core = Web framework on .NET**

---

# 3. Create an ASP.NET Core Web API

Since you are using **Visual Studio**, you can create it through Visual Studio.

## Visual Studio

Go to:

```text
File
 ↓
New
 ↓
Project
```

Select:

```text
ASP.NET Core Web API
```

Click:

```text
Next
```

Project name:

```text
Day09AspNetCoreFundamentals
```

Framework:

```text
.NET 8.0
```

Then create the project.

---

# 4. Create Using CLI

You can also create the same project using:

```bash
dotnet new webapi
```

For a specific project:

```bash
dotnet new webapi -n Day09AspNetCoreFundamentals
```

Then:

```bash
cd Day09AspNetCoreFundamentals
```

Run:

```bash
dotnet run
```

You will see something similar to:

```text
Now listening on: https://localhost:xxxx
Now listening on: http://localhost:xxxx
```

The port number can vary.

---

# 5. ASP.NET Core Web API Project Structure

A typical project looks like:

```text
Day09AspNetCoreFundamentals
│
├── Controllers
│
├── Properties
│   └── launchSettings.json
│
├── appsettings.json
│
├── appsettings.Development.json
│
├── Program.cs
│
└── Day09AspNetCoreFundamentals.csproj
```

Depending on the .NET template/version, additional files may also appear.

---

# 6. Controllers Folder

The `Controllers` folder normally contains API controllers.

For example:

```text
Controllers
└── WeatherForecastController.cs
```

A controller receives HTTP requests and returns HTTP responses.

Example:

```csharp
[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok("Products returned");
    }
}
```

We will study controllers deeply on a later day.

For Day 9, remember:

```text
HTTP Request
     ↓
Controller
     ↓
Action Method
     ↓
HTTP Response
```

---

# 7. Program.cs

This is one of the **most important files** in modern ASP.NET Core.

In .NET 6 and later, ASP.NET Core uses the **minimal hosting model** by default.

A typical `Program.cs` looks like:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
```

Let's understand every line.

---

# 8. `WebApplication.CreateBuilder`

```csharp
var builder = WebApplication.CreateBuilder(args);
```

This creates the application builder.

It prepares things such as:

* Configuration
* Logging
* Dependency Injection
* Hosting
* Environment
* Application services

Think:

```text
CreateBuilder()
      ↓
Prepare application
```

---

# 9. Service Registration

```csharp
builder.Services.AddControllers();
```

This registers MVC/Web API controller-related services with the **Dependency Injection container**.

Conceptually:

```text
builder.Services
       ↓
Service Collection
       ↓
Dependency Injection
```

Later you will register your own services:

```csharp
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
```

For now, remember:

> `builder.Services` is where application services are registered.

---

# 10. Build the Application

```csharp
var app = builder.Build();
```

This builds the configured web application.

Before this:

```text
builder
   ↓
configuration
services
logging
middleware configuration
```

After:

```text
app
 ↓
ASP.NET Core application
```

---

# 11. Middleware

This is another **very important Day 9 concept**.

Example:

```csharp
app.UseHttpsRedirection();
```

Middleware is software that participates in processing HTTP requests and responses.

Think of middleware as a series of checkpoints:

```text
Client
  ↓
Middleware 1
  ↓
Middleware 2
  ↓
Middleware 3
  ↓
Controller
  ↓
Middleware 3
  ↓
Middleware 2
  ↓
Middleware 1
  ↓
Client
```

---

# 12. What is Middleware?

Middleware is a component in the ASP.NET Core request pipeline.

It can:

* Inspect requests
* Modify requests
* Inspect responses
* Modify responses
* Authenticate users
* Authorize users
* Handle exceptions
* Log requests
* Redirect HTTP → HTTPS
* Serve static files
* Route requests

Examples:

```csharp
app.UseHttpsRedirection();
```

Later:

```csharp
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
```

---

# 13. Middleware Pipeline

Imagine:

```text
Client
  |
  v
+-------------------+
| Exception Handler |
+-------------------+
          |
          v
+-------------------+
| HTTPS Redirect    |
+-------------------+
          |
          v
+-------------------+
| Authentication    |
+-------------------+
          |
          v
+-------------------+
| Authorization     |
+-------------------+
          |
          v
+-------------------+
| Routing/Endpoints |
+-------------------+
          |
          v
      Controller
```

The order of middleware can matter.

For example:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

Authentication should happen before authorization.

---

# 14. `app.Use` vs `app.Map`

You will see both.

### `Use`

Adds middleware to the request pipeline.

Example:

```csharp
app.UseHttpsRedirection();
```

### `Map`

Maps endpoints.

Example:

```csharp
app.MapControllers();
```

Simple memory:

```text
Use  → Middleware

Map  → Endpoint
```

---

# 15. `app.Run()`

```csharp
app.Run();
```

This starts the web application.

Think:

```text
Configure application
       ↓
Build application
       ↓
Run application
       ↓
Listen for HTTP requests
```

The application continues running and waits for requests.

---

# 16. Minimal Hosting Model

Before .NET 6, ASP.NET Core applications commonly had:

```text
Program.cs
Startup.cs
```

`Startup.cs` commonly contained:

```csharp
ConfigureServices()
Configure()
```

Modern ASP.NET Core uses the **minimal hosting model**.

Now much of that setup is in:

```text
Program.cs
```

Example:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

So:

```text
Older ASP.NET Core
Program.cs
     +
Startup.cs

Modern ASP.NET Core
       ↓
   Program.cs
```

---

# 17. Application Startup

When the application starts, roughly:

```text
Program.cs
   ↓
CreateBuilder
   ↓
Load configuration
   ↓
Register services
   ↓
Build application
   ↓
Configure middleware
   ↓
Map endpoints
   ↓
Run application
```

Example:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Service registration
builder.Services.AddControllers();

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();

app.UseAuthorization();

// Endpoints
app.MapControllers();

// Start
app.Run();
```

---

# 18. ASP.NET Core Architecture

A simplified architecture:

```text
                Client
                  |
                  | HTTP/HTTPS
                  ↓
          +----------------+
          | Web Server     |
          | Kestrel        |
          +----------------+
                  |
                  ↓
          ASP.NET Core Host
                  |
                  ↓
          Middleware Pipeline
                  |
        +---------+---------+
        |         |         |
        ↓         ↓         ↓
   Routing   Auth       Logging
        |
        ↓
   Controller
        |
        ↓
     Service
        |
        ↓
  Data Access
        |
        ↓
    Database
```

You will gradually build this architecture throughout your .NET roadmap.

---

# 19. What is Kestrel?

**Kestrel** is the cross-platform web server used by ASP.NET Core.

It listens for HTTP/HTTPS requests and passes them into your ASP.NET Core application.

Example:

```text
Browser
   |
   | HTTPS
   ↓
Kestrel
   |
   ↓
ASP.NET Core
   |
   ↓
Controller
```

When you run your API locally, you may see:

```text
Now listening on: https://localhost:7001
```

That means your application is listening for requests on that address/port.

---

# 20. Kestrel vs IIS

You may encounter both.

### Kestrel

ASP.NET Core's cross-platform web server.

```text
Application
    ↓
Kestrel
```

### IIS

Microsoft's Windows web server.

A common production arrangement is:

```text
Internet
   ↓
IIS / Reverse Proxy
   ↓
Kestrel
   ↓
ASP.NET Core
```

But ASP.NET Core applications can also run directly behind Kestrel depending on the deployment architecture.

---

# 21. What is a Web Server?

A web server receives HTTP requests and sends HTTP responses.

Example:

```text
Browser
   |
   | GET /api/products
   ↓
Web Server
   |
   ↓
Application
   |
   ↓
HTTP Response
   |
   ↓
Browser
```

Examples of web servers:

* Kestrel
* IIS
* Nginx
* Apache

For ASP.NET Core:

```text
Kestrel
    ↓
ASP.NET Core application
```

---

# 22. What is HTTP?

**HTTP = HyperText Transfer Protocol**

It is a protocol used for communication between clients and servers.

Example:

```text
Client
   |
   | HTTP Request
   ↓
Server
   |
   | HTTP Response
   ↓
Client
```

---

# 23. HTTP Request

An HTTP request contains information such as:

* HTTP method
* URL
* Headers
* Body

Example:

```http
GET /api/products HTTP/1.1
Host: localhost:7001
Accept: application/json
```

---

# 24. HTTP Methods

The most important methods:

### GET

Retrieve data.

```http
GET /api/products
```

### POST

Create data.

```http
POST /api/products
```

### PUT

Update/replacement of a resource.

```http
PUT /api/products/10
```

### PATCH

Partial update.

```http
PATCH /api/products/10
```

### DELETE

Delete data.

```http
DELETE /api/products/10
```

You will study REST APIs in detail later.

---

# 25. HTTP Response

A server sends a response.

Example:

```http
HTTP/1.1 200 OK
Content-Type: application/json
```

Body:

```json
{
  "id": 1,
  "name": "Laptop"
}
```

A response usually contains:

```text
Status Code
Headers
Body
```

---

# 26. HTTP Status Codes

Important codes:

### 2xx — Success

```text
200 OK
201 Created
204 No Content
```

### 3xx — Redirection

```text
301
302
304
```

### 4xx — Client error

```text
400 Bad Request
401 Unauthorized
403 Forbidden
404 Not Found
```

### 5xx — Server error

```text
500 Internal Server Error
502 Bad Gateway
503 Service Unavailable
```

You will use these extensively when building APIs.

---

# 27. HTTPS

**HTTPS = HTTP Secure**

HTTPS encrypts HTTP communication using TLS.

Simple difference:

```text
HTTP
Client ─────────────── Server
       unencrypted

HTTPS
Client ═══════════════ Server
       encrypted
```

HTTPS protects data while it travels between client and server.

Examples of sensitive information:

```text
Username
Password
Token
Payment information
Personal information
```

---

# 28. HTTP vs HTTPS

```text
HTTP
↓
Not encrypted at the transport layer

HTTPS
↓
HTTP + TLS encryption
```

Default ports commonly associated with:

```text
HTTP  → 80
HTTPS → 443
```

For local ASP.NET Core development, the actual port is usually assigned by the project configuration and may look like:

```text
https://localhost:7001
```

---

# 29. Request/Response Lifecycle

This is one of the most important concepts of Day 9.

Suppose Angular sends:

```http
GET /api/employees
```

The flow is approximately:

```text
Angular
   |
   | HTTP GET
   ↓
Kestrel
   |
   ↓
ASP.NET Core
   |
   ↓
Middleware Pipeline
   |
   ↓
Routing
   |
   ↓
EmployeeController
   |
   ↓
EmployeeService
   |
   ↓
Database
   |
   ↓
EmployeeService
   |
   ↓
EmployeeController
   |
   ↓
HTTP Response
   |
   ↓
Kestrel
   |
   ↓
Angular
```

---

# 30. Complete Request Lifecycle

Let's make it more detailed.

```text
1. Client sends request
        ↓
2. Kestrel receives request
        ↓
3. ASP.NET Core host processes it
        ↓
4. Middleware pipeline starts
        ↓
5. Request logging / exception handling
        ↓
6. HTTPS handling
        ↓
7. Authentication
        ↓
8. Authorization
        ↓
9. Routing
        ↓
10. Controller selected
        ↓
11. Controller action executes
        ↓
12. Service/business logic
        ↓
13. Data access
        ↓
14. Result returned
        ↓
15. Response travels back
        ↓
16. Client receives response
```

---

# 31. Example API Request

Suppose we have:

```csharp
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployees()
    {
        return Ok(new[]
        {
            new { Id = 1, Name = "Arun" },
            new { Id = 2, Name = "Priya" }
        });
    }
}
```

Client sends:

```http
GET /api/employees
```

Routing identifies:

```text
EmployeesController
       ↓
GetEmployees()
```

The method returns:

```csharp
Ok(...)
```

ASP.NET Core converts the result to an HTTP response.

Response:

```http
200 OK
Content-Type: application/json
```

Body:

```json
[
  {
    "id": 1,
    "name": "Arun"
  },
  {
    "id": 2,
    "name": "Priya"
  }
]
```

---

# 32. appsettings.json

`appsettings.json` contains application configuration.

Example:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Later you might have:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=..."
  }
}
```

Or:

```json
{
  "ApplicationSettings": {
    "ApplicationName": "Employee API",
    "Version": "1.0"
  }
}
```

---

# 33. appsettings.Development.json

This file contains configuration specific to the **Development environment**.

```text
appsettings.json
        +
appsettings.Development.json
```

Development-specific values can override general configuration values.

For example:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  }
}
```

Later you may have:

```text
appsettings.json
appsettings.Development.json
appsettings.Production.json
```

Important:

> Do not store real production passwords, secrets, or connection credentials directly in source-controlled configuration files.

Later you will learn:

* User Secrets
* Environment variables
* Azure configuration
* Managed Identity
* Key Vault

---

# 34. Properties Folder

You may see:

```text
Properties
└── launchSettings.json
```

`launchSettings.json` is mainly used for local development and launch profiles.

It can define:

* Application URLs
* HTTP/HTTPS profiles
* Environment variables
* Launch configuration

Example:

```json
{
  "profiles": {
    "https": {
      "commandName": "Project",
      "launchBrowser": true,
      "applicationUrl": "https://localhost:7001;http://localhost:5001"
    }
  }
}
```

The actual ports in your project may be different.

---

# 35. `.csproj`

The project file contains project configuration.

Example:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

</Project>
```

Notice:

```xml
Microsoft.NET.Sdk.Web
```

This indicates the project uses the web SDK.

---

# 36. Program.cs — Full Basic Example

For your Day 9 project, understand this code very clearly:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();

// Authorization middleware
app.UseAuthorization();

// Map controller endpoints
app.MapControllers();

// Start application
app.Run();
```

Mental model:

```text
builder
  ↓
Register Services
  ↓
Build
  ↓
Configure Middleware
  ↓
Map Endpoints
  ↓
Run
```

---

# 37. Service Registration vs Middleware

This distinction is very important.

### Service Registration

```csharp
builder.Services.AddControllers();
```

This tells the DI container:

> "These services are available to the application."

### Middleware

```csharp
app.UseHttpsRedirection();
```

This tells the application:

> "Add this processing step to the HTTP request pipeline."

So:

```text
builder.Services
       ↓
Dependency Injection

app.Use...
       ↓
Middleware Pipeline
```

---

# 38. DI Connection to Day 5

Remember Day 5?

We learned:

```csharp
interface IOrderRepository
{
    void Save();
}
```

and:

```csharp
class OrderService
{
    private readonly IOrderRepository repository;

    public OrderService(IOrderRepository repository)
    {
        this.repository = repository;
    }
}
```

ASP.NET Core provides Dependency Injection built into the framework.

Later you can register:

```csharp
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
```

Then ASP.NET Core can create the dependency automatically.

This is one of the major reasons Day 5's **DIP/SOLID** concepts are important.

---

# 39. Hosting

Hosting means providing the environment in which your ASP.NET Core application runs.

The host manages things such as:

* Application lifetime
* Configuration
* Logging
* Dependency Injection
* Web server
* Environment

Simplified:

```text
Host
├── Configuration
├── Logging
├── Dependency Injection
├── Environment
└── Web Server
```

---

# 40. ASP.NET Core Hosting Flow

```text
Program.cs
    ↓
WebApplication.CreateBuilder()
    ↓
Host configuration
    ↓
Service registration
    ↓
Build
    ↓
WebApplication
    ↓
Kestrel
    ↓
HTTP requests
```

---

# 41. Development vs Production

ASP.NET Core applications have environments.

Common environments:

```text
Development
Staging
Production
```

Development:

```text
Local machine
Debugging
Developer settings
Detailed diagnostics
```

Production:

```text
Real users
Real traffic
Production database
Security-sensitive configuration
```

You should not expose detailed exception information to production users.

---

# 42. Your First Day 9 API

Create:

```text
Day09AspNetCoreFundamentals
```

Then create:

```text
Controllers
└── HelloController.cs
```

Code:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace Day09AspNetCoreFundamentals.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello from ASP.NET Core!");
    }
}
```

Run the application.

Then open:

```text
https://localhost:<your-port>/api/hello
```

You should receive:

```text
Hello from ASP.NET Core!
```

---

# 43. Understand What Just Happened

When you open:

```text
GET /api/hello
```

The flow is:

```text
Browser
   ↓
HTTPS
   ↓
Kestrel
   ↓
ASP.NET Core
   ↓
Middleware
   ↓
Routing
   ↓
HelloController
   ↓
Get()
   ↓
Ok(...)
   ↓
HTTP Response
   ↓
Browser
```

This is the foundation for everything you will learn from Day 10 onward.

---

# 44. Important Day 9 Mental Model

Memorize this:

```text
                 ASP.NET CORE

Client
  ↓
HTTP / HTTPS
  ↓
Kestrel
  ↓
ASP.NET Core Host
  ↓
Middleware Pipeline
  ↓
Routing
  ↓
Controller
  ↓
Service
  ↓
DAL
  ↓
Database
```

Response:

```text
Database
   ↓
DAL
   ↓
Service
   ↓
Controller
   ↓
Middleware
   ↓
Kestrel
   ↓
Client
```

---

# 45. Day 9 Interview Questions

### 1. What is ASP.NET Core?

A cross-platform web framework built on .NET for developing web applications, Web APIs, and backend services.

### 2. What is Kestrel?

Kestrel is the cross-platform web server used by ASP.NET Core.

### 3. What is middleware?

Middleware is a component in the HTTP request pipeline that can process requests and responses.

### 4. What is `Program.cs`?

It is the primary application startup/configuration file in the modern ASP.NET Core minimal hosting model.

### 5. What is minimal hosting?

A simplified hosting model introduced with .NET 6 that combines much of the previous `Program.cs` and `Startup.cs` setup into `Program.cs`.

### 6. What does `builder.Services` represent?

The service collection used to register dependencies for Dependency Injection.

### 7. What does `builder.Build()` do?

It builds the configured web application/host.

### 8. What does `app.Use...` generally do?

It adds middleware to the HTTP request pipeline.

### 9. What does `app.MapControllers()` do?

It maps controller actions as endpoints so incoming requests can be routed to them.

### 10. What does `app.Run()` do?

It starts the application and begins listening for requests.

### 11. HTTP vs HTTPS?

HTTP transfers data without transport encryption; HTTPS uses TLS to protect HTTP communication.

### 12. What is the default HTTP port?

Commonly:

```text
80
```

### 13. What is the default HTTPS port?

Commonly:

```text
443
```

Local ASP.NET Core projects can use different ports.

### 14. What is `appsettings.json`?

A configuration file used to store application settings.

### 15. What is `appsettings.Development.json`?

Environment-specific configuration for the Development environment.

### 16. What is `launchSettings.json`?

A local development launch/profile configuration file.

### 17. What is a web server?

Software that receives HTTP requests and sends HTTP responses.

### 18. What is the ASP.NET Core request lifecycle?

A request is received by the web server, processed through middleware/routing, handled by an endpoint such as a controller, and converted into an HTTP response.

---

# 46. Day 9 — Final Revision

Remember these **10 points**:

```text
1. ASP.NET Core
   → Web framework on .NET

2. Program.cs
   → Application startup/configuration

3. WebApplication.CreateBuilder()
   → Creates application builder

4. builder.Services
   → Dependency Injection registration

5. builder.Build()
   → Builds application

6. app.Use(...)
   → Middleware

7. app.Map(...)
   → Endpoint mapping

8. app.Run()
   → Starts application

9. Kestrel
   → Web server

10. HTTP/HTTPS
    → Client-server communication
```

And the most important flow:

```text
Client
  ↓
HTTP/HTTPS
  ↓
Kestrel
  ↓
ASP.NET Core
  ↓
Middleware
  ↓
Routing
  ↓
Controller
  ↓
Service
  ↓
Database
  ↓
Response
```

### Day 9 → Day 10 connection

You now understand **how an ASP.NET Core application starts and how an HTTP request moves through it**.

Next, the natural step is:

```text
Day 9
ASP.NET Core Fundamentals
        ↓
Day 10
ASP.NET Core Web API
        ↓
Controllers
        ↓
Routing
        ↓
HTTP Methods
        ↓
REST API
        ↓
Models / DTOs
        ↓
JSON
        ↓
CRUD API
```

That is where your .NET knowledge starts turning into a **real backend API project**.
