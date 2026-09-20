# Day 13 — Dependency Injection (DI) in ASP.NET Core

Dependency Injection is one of the **most important concepts in ASP.NET Core**. You will use it almost everywhere in real-world .NET applications.

The main architecture you should understand today is:

```text
HTTP Request
     ↓
Controller
     ↓
Service / BAL
     ↓
Repository / DAL
     ↓
Database
```

Instead of a class creating its own dependencies, ASP.NET Core's **DI container creates and provides them**.

---

# 1. What is a Dependency?

A dependency is an object/class that another class needs to perform its work.

Example:

```csharp
public class EmployeeService
{
    private readonly EmployeeRepository _repository;

    public EmployeeService()
    {
        _repository = new EmployeeRepository();
    }
}
```

Here:

```text
EmployeeService
       ↓
needs
       ↓
EmployeeRepository
```

So `EmployeeRepository` is a **dependency** of `EmployeeService`.

---

# 2. The Problem Without Dependency Injection

Suppose we have:

```csharp
public class EmployeeService
{
    private readonly EmployeeRepository _repository;

    public EmployeeService()
    {
        _repository = new EmployeeRepository();
    }

    public void GetEmployees()
    {
        _repository.GetEmployees();
    }
}
```

It works.

But there is a problem.

`EmployeeService` is tightly coupled to:

```csharp
EmployeeRepository
```

If you later want:

```text
SqlEmployeeRepository
MockEmployeeRepository
RedisEmployeeRepository
ApiEmployeeRepository
```

you have to modify `EmployeeService`.

---

# 3. Better Design — Interface

Create an interface:

```csharp
public interface IEmployeeRepository
{
    void GetEmployees();
}
```

Implementation:

```csharp
public class EmployeeRepository : IEmployeeRepository
{
    public void GetEmployees()
    {
        Console.WriteLine("Getting employees from database");
    }
}
```

Now the service depends on the interface:

```csharp
public class EmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public void GetEmployees()
    {
        _repository.GetEmployees();
    }
}
```

Notice the important difference.

Instead of:

```csharp
new EmployeeRepository()
```

we have:

```csharp
IEmployeeRepository repository
```

The dependency is supplied from outside.

That is **Dependency Injection**.

---

# 4. What is Dependency Injection?

Dependency Injection means:

> A class receives the objects it depends on from an external source instead of creating those objects itself.

Without DI:

```text
EmployeeService
      ↓
new EmployeeRepository()
```

With DI:

```text
DI Container
      ↓
EmployeeRepository
      ↓
EmployeeService
```

---

# 5. Simple Real-World Example

Think about a laptop.

A laptop depends on:

```text
Battery
Keyboard
Screen
Processor
```

The laptop doesn't need to create every component itself.

Similarly:

```text
Controller
   ↓ depends on
EmployeeService

EmployeeService
   ↓ depends on
EmployeeRepository
```

ASP.NET Core can create these dependencies and provide them automatically.

---

# 6. What is IoC?

IoC means:

> **Inversion of Control**

Normally, a class controls the creation of its dependencies.

```csharp
public class EmployeeService
{
    private EmployeeRepository _repository =
        new EmployeeRepository();
}
```

The class controls object creation.

With IoC:

```text
Application
    ↓
DI Container
    ↓
creates dependency
    ↓
provides dependency
    ↓
EmployeeService
```

Control over object creation is moved outside the class.

Therefore:

> **Dependency Injection is one way of implementing Inversion of Control.**

---

# 7. IoC vs DI

This is a common interview question.

### IoC

A broader principle:

> Control of object creation/dependency management is transferred from the class to another mechanism.

### DI

A technique for implementing IoC:

> Dependencies are supplied to a class from outside.

Simple memory trick:

```text
IoC = Principle
DI  = Technique
```

---

# 8. What is a Service Container?

The **DI service container** is responsible for:

* Registering services
* Creating objects
* Resolving dependencies
* Managing object lifetimes
* Providing dependencies to constructors

In ASP.NET Core:

```csharp
builder.Services
```

is used to register services.

Example:

```csharp
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
```

Conceptually:

```text
Service Container
      │
      ├── IEmployeeService → EmployeeService
      ├── IEmployeeRepository → EmployeeRepository
      └── Other services
```

---

# 9. Basic DI Registration

Suppose:

```csharp
public interface IEmployeeService
{
    string GetEmployee();
}
```

Implementation:

```csharp
public class EmployeeService : IEmployeeService
{
    public string GetEmployee()
    {
        return "Arun";
    }
}
```

Register:

```csharp
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
```

Now ASP.NET Core knows:

```text
IEmployeeService
       ↓
EmployeeService
```

---

# 10. Constructor Injection

Constructor injection is the most common form of DI in ASP.NET Core.

```csharp
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(
        IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
}
```

ASP.NET Core sees:

```text
EmployeesController
       ↓
needs IEmployeeService
       ↓
DI Container
       ↓
EmployeeService
       ↓
inject into constructor
```

---

# 11. Complete Example

Let's build:

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
Database
```

For now, we won't connect a real database.

We'll use an in-memory list.

---

# 12. Project Structure

Create:

```text
Day13DependencyInjection
│
├── Controllers
│   └── EmployeesController.cs
│
├── Services
│   ├── IEmployeeService.cs
│   └── EmployeeService.cs
│
├── Repositories
│   ├── IEmployeeRepository.cs
│   └── EmployeeRepository.cs
│
├── Models
│   └── Employee.cs
│
├── Program.cs
├── appsettings.json
└── Day13DependencyInjection.csproj
```

This structure will prepare you for the layered architecture you listed in your roadmap.

---

# 13. Employee Model

### Models/Employee.cs

```csharp
namespace Day13DependencyInjection.Models;

public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Department { get; set; } = "";
}
```

---

# 14. Repository Interface

### Repositories/IEmployeeRepository.cs

```csharp
using Day13DependencyInjection.Models;

namespace Day13DependencyInjection.Repositories;

public interface IEmployeeRepository
{
    List<Employee> GetEmployees();

    Employee? GetEmployeeById(int id);

    void AddEmployee(Employee employee);
}
```

The interface defines the repository contract.

---

# 15. Repository Implementation

### Repositories/EmployeeRepository.cs

```csharp
using Day13DependencyInjection.Models;

namespace Day13DependencyInjection.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly List<Employee> _employees = new()
    {
        new Employee
        {
            Id = 1,
            Name = "Arun",
            Department = "IT"
        },
        new Employee
        {
            Id = 2,
            Name = "Priya",
            Department = "HR"
        }
    };

    public List<Employee> GetEmployees()
    {
        return _employees;
    }

    public Employee? GetEmployeeById(int id)
    {
        return _employees.FirstOrDefault(e => e.Id == id);
    }

    public void AddEmployee(Employee employee)
    {
        _employees.Add(employee);
    }
}
```

---

# 16. Service Interface

### Services/IEmployeeService.cs

```csharp
using Day13DependencyInjection.Models;

namespace Day13DependencyInjection.Services;

public interface IEmployeeService
{
    List<Employee> GetEmployees();

    Employee? GetEmployeeById(int id);

    void AddEmployee(Employee employee);
}
```

---

# 17. Service Implementation

### Services/EmployeeService.cs

```csharp
using Day13DependencyInjection.Models;
using Day13DependencyInjection.Repositories;

namespace Day13DependencyInjection.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public List<Employee> GetEmployees()
    {
        return _repository.GetEmployees();
    }

    public Employee? GetEmployeeById(int id)
    {
        return _repository.GetEmployeeById(id);
    }

    public void AddEmployee(Employee employee)
    {
        _repository.AddEmployee(employee);
    }
}
```

Notice:

```csharp
public EmployeeService(IEmployeeRepository repository)
```

The service doesn't create:

```csharp
new EmployeeRepository()
```

Instead, it asks for:

```csharp
IEmployeeRepository
```

The DI container supplies the implementation.

---

# 18. Register Services

Now go to:

### Program.cs

```csharp
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

app.MapControllers();

app.Run();
```

You can also write them on one line:

```csharp
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
```

---

# 19. What Does `AddScoped` Mean?

This is one of the three important service lifetimes:

```text
Transient
Scoped
Singleton
```

We'll study them in detail below.

For now:

```csharp
AddScoped<IEmployeeService, EmployeeService>();
```

means ASP.NET Core creates one `EmployeeService` instance for a given **DI scope**.

For an HTTP application, a scope is commonly associated with one request.

---

# 20. Controller

### Controllers/EmployeesController.cs

```csharp
using Day13DependencyInjection.Models;
using Day13DependencyInjection.Services;
using Microsoft.AspNetCore.Mvc;

namespace Day13DependencyInjection.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(
        IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public IActionResult GetEmployees()
    {
        var employees = _employeeService.GetEmployees();

        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetEmployee(int id)
    {
        var employee = _employeeService.GetEmployeeById(id);

        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpPost]
    public IActionResult AddEmployee(Employee employee)
    {
        _employeeService.AddEmployee(employee);

        return Ok(employee);
    }
}
```

---

# 21. What Happens When API Is Called?

Suppose:

```http
GET /api/employees
```

ASP.NET Core needs:

```text
EmployeesController
```

But its constructor requires:

```text
IEmployeeService
```

DI container checks:

```text
IEmployeeService
        ↓
EmployeeService
```

Then `EmployeeService` requires:

```text
IEmployeeRepository
```

DI container checks:

```text
IEmployeeRepository
        ↓
EmployeeRepository
```

So the container creates the dependency graph:

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
```

Then:

```text
Controller
    ↓
Service
    ↓
Repository
```

---

# 22. Dependency Graph

This is very important.

```text
                DI Container
                     │
          ┌──────────┴──────────┐
          ↓                     ↓
   IEmployeeService     IEmployeeRepository
          ↓                     ↓
   EmployeeService      EmployeeRepository
          │
          └─────────┐
                    ↓
              Controller
```

Actually, during controller activation, the dependency resolution happens roughly as:

```text
EmployeesController
       │
       ↓
IEmployeeService
       │
       ↓
EmployeeService
       │
       ↓
IEmployeeRepository
       │
       ↓
EmployeeRepository
```

---

# 23. The Three Service Lifetimes

ASP.NET Core provides three common lifetimes:

```text
AddTransient
AddScoped
AddSingleton
```

---

# 24. Transient

Registration:

```csharp
builder.Services.AddTransient<
    IEmployeeService,
    EmployeeService>();
```

Meaning:

> A new instance is created each time the service is requested from the container.

Conceptually:

```text
Request
 ├── Resolve Service → Instance A
 ├── Resolve Service → Instance B
 └── Resolve Service → Instance C
```

Every resolution can produce a different instance.

### Use Transient when

The service is:

* Lightweight
* Stateless
* Doesn't need to be shared
* Cheap to create

Example:

```csharp
builder.Services.AddTransient<IEmailFormatter, EmailFormatter>();
```

---

# 25. Scoped

Registration:

```csharp
builder.Services.AddScoped<
    IEmployeeService,
    EmployeeService>();
```

Meaning:

> One instance is created per DI scope.

In a typical ASP.NET Core web request:

```text
HTTP Request 1
    ↓
EmployeeService Instance A

HTTP Request 2
    ↓
EmployeeService Instance B
```

So:

```text
Request 1 → A
Request 1 → A

Request 2 → B
Request 2 → B
```

Within the same scope, repeated requests for that service resolve to the same instance.

### Common use

Scoped is commonly used for:

* Application services
* Business logic services
* Repository services
* Entity Framework Core `DbContext`

Example:

```csharp
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
```

---

# 26. Singleton

Registration:

```csharp
builder.Services.AddSingleton<
    IEmployeeService,
    EmployeeService>();
```

Meaning:

> One instance is created and reused for the lifetime of the application/service provider.

Conceptually:

```text
Application
     │
     ↓
EmployeeService Instance A
     │
     ├── Request 1
     ├── Request 2
     ├── Request 3
     └── Request 4
```

### Use Singleton when

The service is suitable for application-wide sharing and is designed to be thread-safe.

Examples can include:

* Stateless reusable components
* Application-wide caches
* Configuration-related services
* Carefully designed expensive-to-create services

Be careful with mutable state.

---

# 27. Transient vs Scoped vs Singleton

Remember:

```text
Transient
→ New instance per resolution

Scoped
→ One instance per scope
→ Commonly one HTTP request

Singleton
→ One instance for application lifetime
```

A simple memory diagram:

```text
TRANSIENT

Request
 ├── A
 ├── B
 └── C


SCOPED

Request 1 → A
Request 1 → A

Request 2 → B
Request 2 → B


SINGLETON

Request 1 → A
Request 2 → A
Request 3 → A
```

---

# 28. Which Lifetime Should You Choose?

There isn't one lifetime that is universally correct.

A practical starting point:

```text
Business/Application Service
        ↓
      Scoped

Repository
        ↓
      Scoped

EF Core DbContext
        ↓
      Scoped

Lightweight stateless helper
        ↓
     Transient

Application-wide shared component
        ↓
     Singleton
```

But always consider the dependency's state, thread-safety, and lifetime relationships.

---

# 29. Important Rule — Lifetime Compatibility

Be careful when a longer-lived service depends on a shorter-lived service.

For example:

```text
Singleton
   ↓
Scoped
```

This is problematic because the singleton could effectively hold onto a request-scoped dependency beyond its intended lifetime.

A classic example:

```csharp
builder.Services.AddSingleton<MyService>();
builder.Services.AddScoped<MyDbContext>();
```

If `MyService` directly depends on `MyDbContext`, that is an invalid lifetime relationship in normal DI usage.

ASP.NET Core can detect many such problems during service validation.

### Easy rule

```text
Singleton
   ↓
Don't directly depend on Scoped
```

This is especially important with:

```text
Singleton → DbContext
```

because `DbContext` is normally scoped.

---

# 30. Why Use Interfaces?

Instead of:

```csharp
public class EmployeeService
{
    private readonly EmployeeRepository _repository;

    public EmployeeService(EmployeeRepository repository)
    {
        _repository = repository;
    }
}
```

prefer:

```csharp
public class EmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }
}
```

This creates loose coupling.

---

# 31. Interface-Based Design

Think:

```text
Service
   ↓
IEmployeeRepository
```

rather than:

```text
Service
   ↓
EmployeeRepository
```

The service knows **what it needs**, not necessarily **how it is implemented**.

---

# 32. Multiple Implementations

Suppose:

```csharp
public interface IEmployeeRepository
{
    List<Employee> GetEmployees();
}
```

Implementation 1:

```csharp
public class SqlEmployeeRepository : IEmployeeRepository
{
    public List<Employee> GetEmployees()
    {
        return new List<Employee>();
    }
}
```

Implementation 2:

```csharp
public class InMemoryEmployeeRepository : IEmployeeRepository
{
    public List<Employee> GetEmployees()
    {
        return new List<Employee>
        {
            new Employee
            {
                Id = 1,
                Name = "Arun"
            }
        };
    }
}
```

The service still depends on:

```csharp
IEmployeeRepository
```

It doesn't need to know which implementation is being used.

---

# 33. DI and SOLID

This connects directly to what you learned on Day 5.

DI strongly supports:

### Dependency Inversion Principle

> High-level modules should not depend directly on low-level concrete implementations. Both should depend on abstractions.

Example:

```text
Bad:

EmployeeService
      ↓
EmployeeRepository


Better:

EmployeeService
      ↓
IEmployeeRepository
      ↑
EmployeeRepository
```

---

# 34. Constructor Injection vs Creating Dependencies

### Without DI

```csharp
public class EmployeeService
{
    private readonly EmployeeRepository _repository;

    public EmployeeService()
    {
        _repository = new EmployeeRepository();
    }
}
```

Problems:

```text
Tight coupling
Harder testing
Harder replacement
Class controls dependency creation
```

### With DI

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

Benefits:

```text
Loose coupling
Easy testing
Easy replacement
Clear dependencies
Better maintainability
```

---

# 35. Unit Testing Becomes Easier

Suppose:

```csharp
public class EmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }
}
```

For testing, you can provide a fake/mock implementation:

```csharp
public class FakeEmployeeRepository : IEmployeeRepository
{
    public List<Employee> GetEmployees()
    {
        return new List<Employee>
        {
            new Employee
            {
                Id = 1,
                Name = "Test Employee"
            }
        };
    }

    public Employee? GetEmployeeById(int id)
    {
        return GetEmployees()
            .FirstOrDefault(e => e.Id == id);
    }

    public void AddEmployee(Employee employee)
    {
    }
}
```

Then:

```csharp
var repository = new FakeEmployeeRepository();

var service = new EmployeeService(repository);
```

No real database is required.

This is one of the major benefits of DI.

---

# 36. Service Registration Methods

You will commonly see:

```csharp
builder.Services.AddTransient<IService, Service>();
builder.Services.AddScoped<IService, Service>();
builder.Services.AddSingleton<IService, Service>();
```

You may also register a concrete class:

```csharp
builder.Services.AddScoped<EmployeeService>();
```

Then:

```csharp
public EmployeesController(EmployeeService service)
{
    ...
}
```

But interface-based registration is commonly preferred when you want abstraction and replaceability.

---

# 37. Registering Multiple Services

Real applications might have:

```csharp
builder.Services.AddScoped<
    IEmployeeService,
    EmployeeService>();

builder.Services.AddScoped<
    IEmployeeRepository,
    EmployeeRepository>();

builder.Services.AddScoped<
    IDepartmentService,
    DepartmentService>();

builder.Services.AddScoped<
    IDepartmentRepository,
    DepartmentRepository>();
```

This creates a dependency graph.

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
```

---

# 38. Real-World Architecture

This is the architecture you should remember for your .NET roadmap:

```text
                   CLIENT
                     ↓
                  HTTP
                     ↓
              CONTROLLER
                     ↓
              SERVICE / BAL
                     ↓
                REPOSITORY
                     ↓
                  DAL
                     ↓
                DATABASE
```

Dependency direction:

```text
Controller
    ↓
IEmployeeService
    ↓
IEmployeeRepository
    ↓
Database
```

DI creates the implementations required by this graph.

---

# 39. Program.cs — Final Version

For today's practice:

```csharp
using Day13DependencyInjection.Repositories;
using Day13DependencyInjection.Services;

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

app.MapControllers();

app.Run();
```

---

# 40. What Happens Internally?

When this executes:

```csharp
builder.Services.AddScoped<
    IEmployeeService,
    EmployeeService>();
```

you're effectively telling the DI system:

```text
Whenever somebody asks for:

IEmployeeService

create/provide:

EmployeeService
```

And:

```csharp
builder.Services.AddScoped<
    IEmployeeRepository,
    EmployeeRepository>();
```

means:

```text
Whenever somebody asks for:

IEmployeeRepository

create/provide:

EmployeeRepository
```

---

# 41. Complete Dependency Chain

When ASP.NET Core needs:

```csharp
EmployeesController
```

it sees:

```csharp
public EmployeesController(
    IEmployeeService employeeService)
```

DI resolves:

```text
IEmployeeService
      ↓
EmployeeService
```

Then it sees:

```csharp
public EmployeeService(
    IEmployeeRepository repository)
```

DI resolves:

```text
IEmployeeRepository
      ↓
EmployeeRepository
```

Final object graph:

```text
EmployeesController
       │
       ↓
EmployeeService
       │
       ↓
EmployeeRepository
```

This entire process is handled by the DI container.

---

# 42. Constructor Injection Is Preferred

There are different ways people discuss dependency injection, but in ASP.NET Core, **constructor injection is the normal/default approach**.

Example:

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

Advantages:

* Dependencies are explicit
* Required dependencies are available when the object is created
* Easy to test
* Encourages clean design
* Avoids hidden dependencies

---

# 43. Don't Do This

Avoid service locator style code inside your business classes:

```csharp
var service =
    serviceProvider.GetService<IEmployeeService>();
```

when ordinary constructor injection can be used.

Prefer:

```csharp
public EmployeeController(
    IEmployeeService employeeService)
{
    _employeeService = employeeService;
}
```

The dependency becomes clear from the constructor.

---

# 44. DI vs Factory

Don't confuse DI with a factory.

### Factory

A factory is responsible for deciding/creating which object to construct.

```text
Factory
   ↓
creates object
```

### DI

The DI container resolves registered dependencies and supplies them.

```text
DI Container
     ↓
resolves dependency
     ↓
injects object
```

They can be used together in larger systems.

---

# 45. Common Mistakes

### Mistake 1 — Forgetting registration

You create:

```csharp
public interface IEmployeeService
{
}
```

and:

```csharp
public class EmployeeService : IEmployeeService
{
}
```

But forget:

```csharp
builder.Services.AddScoped<
    IEmployeeService,
    EmployeeService>();
```

The application may fail when trying to resolve the controller dependency.

---

### Mistake 2 — Depending on concrete implementations everywhere

Avoid:

```csharp
private readonly EmployeeRepository _repository;
```

when an abstraction is appropriate.

Prefer:

```csharp
private readonly IEmployeeRepository _repository;
```

---

### Mistake 3 — Wrong lifetime

For example, unnecessarily making a stateful/request-specific service singleton can create concurrency and lifetime problems.

---

### Mistake 4 — Singleton depending on scoped service

Avoid:

```text
Singleton
   ↓
Scoped
```

such as a singleton directly holding an EF Core `DbContext`.

---

# 46. Interview Questions

### What is Dependency Injection?

A technique where dependencies are supplied to a class externally instead of the class creating them itself.

### What is IoC?

Inversion of Control is the broader principle of transferring control of object creation/dependency management away from the dependent class.

### Is DI the same as IoC?

No.

```text
IoC = principle
DI  = technique/pattern used to implement IoC
```

### What is a service container?

A component that registers, creates, resolves and manages dependencies according to their configured lifetimes.

### What is constructor injection?

Dependencies are supplied through a class constructor.

### What is `AddTransient`?

Creates a new service instance each time the service is requested/resolved.

### What is `AddScoped`?

Creates one instance per DI scope; in typical ASP.NET Core request processing, this generally means one instance per HTTP request.

### What is `AddSingleton`?

Uses one instance for the lifetime of the service provider/application.

### Which lifetime is commonly used for EF Core DbContext?

Scoped.

### Why use interfaces?

To reduce coupling, improve replaceability and make testing easier.

### What is loose coupling?

A class depends on abstractions rather than concrete implementation details.

### What is tight coupling?

A class directly depends on a specific concrete implementation.

---

# 47. Very Important Comparison

```text
                    TRANSIENT       SCOPED          SINGLETON
                    ---------       ------          ---------
Instance            New per         One per         One per
                    resolution      scope           application

HTTP Request        Multiple        Usually one     Same instance
                    possible        instance        across requests

Lifetime            Short           Request scope   Application

Typical use         Lightweight     Services/       Shared,
                    stateless       repositories    thread-safe
                                    / DbContext     components
```

Remember:

```text
Transient → Every resolution
Scoped    → Every scope
Singleton → Application/service-provider lifetime
```

---

# 48. Day 13 Final Mental Model

```text
                    DI CONTAINER
                         │
          ┌──────────────┴──────────────┐
          ↓                             ↓
IEmployeeService              IEmployeeRepository
          ↓                             ↓
EmployeeService               EmployeeRepository
          │
          ↓
EmployeesController
```

Request flow:

```text
Client
  ↓
HTTP Request
  ↓
Controller
  ↓
Service
  ↓
Repository
  ↓
Database
  ↓
Repository
  ↓
Service
  ↓
Controller
  ↓
HTTP Response
```

And the dependencies are supplied by:

```text
ASP.NET Core DI Container
```

---

# Day 13 — What You Must Remember

```text
DI
│
├── Dependency
│   └── Object another class needs
│
├── Dependency Injection
│   └── Supply dependency from outside
│
├── IoC
│   └── Broader principle
│
├── Service Container
│   └── Creates/resolves/manages dependencies
│
├── Constructor Injection
│   └── Most common ASP.NET Core approach
│
├── Interfaces
│   └── Loose coupling
│
└── Lifetimes
    ├── Transient
    ├── Scoped
    └── Singleton
```

### The one line you should remember

```csharp
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
```

means:

> **"When something asks for `IEmployeeService`, ASP.NET Core DI should provide an `EmployeeService` according to the Scoped lifetime."**

And your core application architecture is:

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
Database
```


