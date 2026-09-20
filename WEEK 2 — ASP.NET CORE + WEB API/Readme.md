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