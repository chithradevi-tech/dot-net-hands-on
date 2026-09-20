# Day 10 — Controllers & Routing

Today we move one step deeper into ASP.NET Core Web API.

In **Day 9**, you learned:

```text
Client
   ↓
HTTP/HTTPS
   ↓
Kestrel
   ↓
Middleware
   ↓
ASP.NET Core
```

Today we learn how ASP.NET Core decides:

> **Which C# method should handle a particular HTTP request?**

That is mainly the job of **Controllers + Routing**.

---

# 1. What is a Controller?

A **Controller** is a C# class that handles HTTP requests and produces HTTP responses.

For example:

```text
GET /api/employees
        ↓
EmployeesController
        ↓
GetEmployees()
```

A controller usually contains **action methods**.

Example:

```csharp
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployees()
    {
        return Ok("Employees returned");
    }
}
```

Here:

```text
EmployeesController
       ↓
GetEmployees()
       ↓
HTTP Response
```

---

# 2. Why `ControllerBase`?

Your controller normally inherits from:

```csharp
ControllerBase
```

Example:

```csharp
public class EmployeesController : ControllerBase
{
}
```

`ControllerBase` provides functionality useful for Web APIs.

For example:

```csharp
return Ok();
return BadRequest();
return NotFound();
return Created();
return Unauthorized();
return Forbid();
```

It also provides access to things such as:

```csharp
Request
Response
ModelState
User
```

---

# 3. `Controller` vs `ControllerBase`

You may see both.

### `ControllerBase`

Generally used for Web APIs:

```csharp
public class EmployeesController : ControllerBase
{
}
```

### `Controller`

Used when you need MVC view functionality:

```csharp
public class HomeController : Controller
{
}
```

Simple rule:

```text
Web API
   ↓
ControllerBase

MVC Views
   ↓
Controller
```

For the Web API projects in your roadmap, you will mostly use:

```csharp
ControllerBase
```

---

# 4. `[ApiController]`

Example:

```csharp
[ApiController]
public class EmployeesController : ControllerBase
{
}
```

`[ApiController]` enables API-specific behavior.

Important features include:

* Automatic model validation behavior
* Better parameter binding
* API-focused conventions
* Automatic HTTP 400 responses for invalid model state in common scenarios

For example, later when you have:

```csharp
public class Employee
{
    [Required]
    public string Name { get; set; } = "";
}
```

ASP.NET Core can automatically detect invalid request data when `[ApiController]` is used.

So generally, for Web API controllers:

```csharp
[ApiController]
```

is recommended.

---

# 5. `[Route("api/[controller]")]`

This defines the route template for the controller.

```csharp
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
}
```

The special token:

```text
[controller]
```

is replaced by the controller name without the `Controller` suffix.

Therefore:

```text
EmployeesController
        ↓
employees
```

So:

```csharp
[Route("api/[controller]")]
```

becomes:

```text
/api/employees
```

Therefore:

```text
GET /api/employees
```

can reach the `EmployeesController`.

---

# 6. Complete Basic Controller

Let's create:

```text
Controllers
└── EmployeesController.cs
```

Code:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace Day10ControllersRouting.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployees()
    {
        return Ok("All employees");
    }
}
```

Run the application.

Request:

```text
GET /api/employees
```

Response:

```text
200 OK
```

Body:

```text
All employees
```

---

# 7. What is an Action Method?

A public method inside a controller that handles an HTTP request is commonly called an **action method**.

Example:

```csharp
[HttpGet]
public IActionResult GetEmployees()
{
    return Ok("Employees");
}
```

Here:

```text
[HttpGet]
      ↓
HTTP method mapping

GetEmployees()
      ↓
Action method
```

---

# 8. HTTP Methods

The main HTTP methods you need to know:

```text
GET
POST
PUT
PATCH
DELETE
```

They represent different kinds of operations.

A common API mapping is:

```text
GET
 ↓
Read

POST
 ↓
Create

PUT
 ↓
Replace/update

PATCH
 ↓
Partial update

DELETE
 ↓
Delete
```

---

# 9. GET

GET is generally used to retrieve data.

Example:

```http
GET /api/employees
```

Controller:

```csharp
[HttpGet]
public IActionResult GetEmployees()
{
    return Ok("All employees");
}
```

Another example:

```http
GET /api/employees/10
```

Controller:

```csharp
[HttpGet("{id}")]
public IActionResult GetEmployee(int id)
{
    return Ok($"Employee ID: {id}");
}
```

---

# 10. POST

POST is commonly used to create a new resource.

Request:

```http
POST /api/employees
```

Example:

```csharp
[HttpPost]
public IActionResult CreateEmployee()
{
    return Ok("Employee created");
}
```

Later, you will receive an object from the request body:

```csharp
[HttpPost]
public IActionResult CreateEmployee(Employee employee)
{
    return Ok(employee);
}
```

---

# 11. PUT

PUT is generally used to replace/update a resource.

Example:

```http
PUT /api/employees/10
```

Controller:

```csharp
[HttpPut("{id}")]
public IActionResult UpdateEmployee(int id)
{
    return Ok($"Employee {id} updated");
}
```

Here:

```text
{id}
 ↓
Route parameter
```

---

# 12. PATCH

PATCH is generally used for a **partial update**.

Example:

```http
PATCH /api/employees/10
```

For example, an employee has:

```text
Name
Email
Department
Salary
```

You might update only:

```text
Salary
```

instead of sending the complete employee resource.

Example:

```csharp
[HttpPatch("{id}")]
public IActionResult UpdateEmployeePartially(int id)
{
    return Ok($"Employee {id} partially updated");
}
```

The exact PATCH request body and implementation depend on the API design. Later you can learn JSON Patch and other partial-update approaches.

---

# 13. DELETE

DELETE is used to delete a resource.

Request:

```http
DELETE /api/employees/10
```

Controller:

```csharp
[HttpDelete("{id}")]
public IActionResult DeleteEmployee(int id)
{
    return Ok($"Employee {id} deleted");
}
```

---

# 14. Complete CRUD Controller

Let's put everything together.

```csharp
using Microsoft.AspNetCore.Mvc;

namespace Day10ControllersRouting.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployees()
    {
        return Ok("All employees");
    }

    [HttpGet("{id}")]
    public IActionResult GetEmployee(int id)
    {
        return Ok($"Employee ID: {id}");
    }

    [HttpPost]
    public IActionResult CreateEmployee()
    {
        return Ok("Employee created");
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEmployee(int id)
    {
        return Ok($"Employee {id} updated");
    }

    [HttpPatch("{id}")]
    public IActionResult PartiallyUpdateEmployee(int id)
    {
        return Ok($"Employee {id} partially updated");
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEmployee(int id)
    {
        return Ok($"Employee {id} deleted");
    }
}
```

Now your API has:

```text
GET     /api/employees
GET     /api/employees/10

POST    /api/employees

PUT     /api/employees/10

PATCH   /api/employees/10

DELETE  /api/employees/10
```

---

# 15. Routing

Now let's understand **routing**.

Routing determines which endpoint should handle an incoming HTTP request.

For example:

```text
GET /api/employees/10
```

ASP.NET Core needs to determine:

```text
Which controller?
      ↓
EmployeesController

Which action?
      ↓
GetEmployee()

What value?
      ↓
id = 10
```

So routing is essentially:

> **Matching an incoming URL and HTTP method to an endpoint.**

---

# 16. Attribute Routing

In modern ASP.NET Core Web APIs, you will commonly use **attribute routing**.

Example:

```csharp
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
}
```

And:

```csharp
[HttpGet]
public IActionResult GetEmployees()
{
    return Ok();
}
```

Combined:

```text
GET /api/employees
```

Another:

```csharp
[HttpGet("{id}")]
public IActionResult GetEmployee(int id)
{
    return Ok();
}
```

Combined:

```text
GET /api/employees/10
```

This is called **attribute routing** because attributes define the routes.

---

# 17. Route Templates

Consider:

```csharp
[Route("api/[controller]")]
```

This is the controller-level route.

Then:

```csharp
[HttpGet("{id}")]
```

adds:

```text
/{id}
```

Together:

```text
/api/employees/{id}
```

Request:

```text
/api/employees/10
```

Result:

```text
id = 10
```

---

# 18. Route Parameters

A route parameter is a variable part of the URL.

Example:

```csharp
[HttpGet("{id}")]
public IActionResult GetEmployee(int id)
{
    return Ok($"Employee ID: {id}");
}
```

Request:

```text
GET /api/employees/10
```

Then:

```text
id = 10
```

Another request:

```text
GET /api/employees/25
```

Then:

```text
id = 25
```

---

# 19. Multiple Route Parameters

You can have multiple parameters.

Example:

```csharp
[HttpGet("{departmentId}/employees/{employeeId}")]
public IActionResult GetEmployee(
    int departmentId,
    int employeeId)
{
    return Ok(new
    {
        DepartmentId = departmentId,
        EmployeeId = employeeId
    });
}
```

URL:

```text
GET /api/employees/10/employees/50
```

Depending on your controller-level route, route composition needs to be designed carefully. A cleaner example is:

```csharp
[Route("api/departments")]
public class DepartmentsController : ControllerBase
{
    [HttpGet("{departmentId}/employees/{employeeId}")]
    public IActionResult GetEmployee(
        int departmentId,
        int employeeId)
    {
        return Ok();
    }
}
```

URL:

```text
GET /api/departments/10/employees/50
```

Values:

```text
departmentId = 10
employeeId   = 50
```

---

# 20. Query Parameters

Query parameters come after `?`.

Example:

```text
GET /api/employees?department=IT
```

Here:

```text
department=IT
```

is a query parameter.

Controller:

```csharp
[HttpGet]
public IActionResult GetEmployees(string? department)
{
    return Ok($"Department: {department}");
}
```

Request:

```text
GET /api/employees?department=IT
```

Result:

```text
department = "IT"
```

---

# 21. Multiple Query Parameters

Example:

```text
GET /api/employees?department=IT&minSalary=50000
```

Controller:

```csharp
[HttpGet]
public IActionResult GetEmployees(
    string? department,
    decimal? minSalary)
{
    return Ok(new
    {
        Department = department,
        MinSalary = minSalary
    });
}
```

Values:

```text
department = IT
minSalary = 50000
```

---

# 22. Route Parameter vs Query Parameter

This is an important interview question.

### Route parameter

```text
/api/employees/10
```

Example:

```csharp
[HttpGet("{id}")]
public IActionResult GetEmployee(int id)
```

Used when the value identifies a specific resource.

### Query parameter

```text
/api/employees?department=IT
```

Example:

```csharp
[HttpGet]
public IActionResult GetEmployees(string? department)
```

Commonly used for:

* Filtering
* Searching
* Sorting
* Pagination
* Optional parameters

Easy memory:

```text
Route
/api/employees/10
               ↑
            resource ID

Query
/api/employees?department=IT
               ↑
          filter/options
```

---

# 23. `[FromRoute]`

You can explicitly tell ASP.NET Core that a value comes from the route.

```csharp
[HttpGet("{id}")]
public IActionResult GetEmployee([FromRoute] int id)
{
    return Ok(id);
}
```

URL:

```text
/api/employees/10
```

---

# 24. `[FromQuery]`

You can explicitly specify query-string binding:

```csharp
[HttpGet]
public IActionResult GetEmployees(
    [FromQuery] string? department)
{
    return Ok(department);
}
```

Request:

```text
/api/employees?department=IT
```

For simple parameters, ASP.NET Core often infers the source, so `[FromQuery]` isn't always required.

---

# 25. Route Constraints

Route constraints restrict which values can match a route.

Suppose:

```csharp
[HttpGet("{id:int}")]
public IActionResult GetEmployee(int id)
{
    return Ok(id);
}
```

The `:int` means:

> The route parameter must be an integer.

This works:

```text
/api/employees/10
```

This does not match that route:

```text
/api/employees/abc
```

---

# 26. Common Route Constraints

Some useful constraints:

```text
:int
:long
:decimal
:double
:bool
:guid
:minlength(...)
:maxlength(...)
:min(...)
:max(...)
:range(...)
```

Examples:

```csharp
[HttpGet("{id:int}")]
```

```csharp
[HttpGet("{id:guid}")]
```

```csharp
[HttpGet("{age:int:min(18):max(100)}")]
```

---

# 27. GUID Route Example

Suppose employee IDs are GUIDs.

```csharp
[HttpGet("{id:guid}")]
public IActionResult GetEmployee(Guid id)
{
    return Ok(id);
}
```

Valid:

```text
/api/employees/550e8400-e29b-41d4-a716-446655440000
```

A normal string such as:

```text
/api/employees/abc
```

does not match this constrained route.

---

# 28. Conventional Routing

Now let's understand the other routing style.

**Conventional routing** defines a general route pattern rather than putting route attributes on every action.

A traditional MVC example:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

This means a URL can follow:

```text
/controller/action/id
```

For example:

```text
/Home/Index
/Home/Details/10
```

Conceptually:

```text
/Home/Details/10
   ↓
Controller = Home
Action     = Details
id         = 10
```

---

# 29. Attribute vs Conventional Routing

### Attribute routing

Routes are defined using attributes:

```csharp
[Route("api/employees")]
```

```csharp
[HttpGet("{id}")]
```

Common in Web APIs.

### Conventional routing

Routes are defined using a central pattern:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

Commonly associated with traditional MVC controller/action URL patterns.

For the Web API portion of your roadmap, focus strongly on **attribute routing**.

---

# 30. Route Combination

This is important.

Controller:

```csharp
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
```

Action:

```csharp
[HttpGet("{id}")]
public IActionResult GetEmployee(int id)
```

ASP.NET Core combines:

```text
api/[controller]
       +
      {id}
```

Result:

```text
/api/employees/{id}
```

Therefore:

```text
GET /api/employees/10
```

---

# 31. Different Action Routes

You can create:

```csharp
[HttpGet]
public IActionResult GetEmployees()
{
    return Ok();
}
```

and:

```csharp
[HttpGet("{id}")]
public IActionResult GetEmployee(int id)
{
    return Ok();
}
```

So:

```text
GET /api/employees
```

maps to:

```text
GetEmployees()
```

while:

```text
GET /api/employees/10
```

maps to:

```text
GetEmployee(10)
```

---

# 32. Route Naming

You can define explicit routes.

For example:

```csharp
[HttpGet("active")]
public IActionResult GetActiveEmployees()
{
    return Ok();
}
```

URL:

```text
GET /api/employees/active
```

Another:

```csharp
[HttpGet("search")]
public IActionResult SearchEmployees()
{
    return Ok();
}
```

URL:

```text
GET /api/employees/search
```

---

# 33. Route Constraints Help Avoid Ambiguity

Suppose you have:

```csharp
[HttpGet("{id}")]
public IActionResult GetEmployee(int id)
```

and:

```csharp
[HttpGet("active")]
public IActionResult GetActiveEmployees()
```

A more explicit constraint can make numeric routes clear:

```csharp
[HttpGet("{id:int}")]
public IActionResult GetEmployee(int id)
{
    return Ok();
}
```

Now:

```text
/api/employees/10
```

is clearly the employee ID route.

And:

```text
/api/employees/active
```

matches the `"active"` route.

---

# 34. `IActionResult`

You'll frequently see:

```csharp
public IActionResult GetEmployees()
```

`IActionResult` represents an HTTP action result.

Examples:

```csharp
return Ok();
```

```csharp
return NotFound();
```

```csharp
return BadRequest();
```

```csharp
return Created();
```

This gives your controller flexibility to return different HTTP results.

---

# 35. Important HTTP Result Methods

### `Ok()`

HTTP:

```text
200 OK
```

```csharp
return Ok();
```

### `Ok(data)`

```csharp
return Ok(employees);
```

Usually:

```text
200 OK
```

with JSON data.

### `BadRequest()`

```text
400 Bad Request
```

```csharp
return BadRequest();
```

### `NotFound()`

```text
404 Not Found
```

```csharp
return NotFound();
```

### `Unauthorized()`

```text
401 Unauthorized
```

```csharp
return Unauthorized();
```

### `Forbid()`

```text
403 Forbidden
```

```csharp
return Forbid();
```

### `Created()`

```text
201 Created
```

Used when a resource has been successfully created.

---

# 36. Real Employee API Example

Let's create a slightly more realistic controller.

```csharp
using Microsoft.AspNetCore.Mvc;

namespace Day10ControllersRouting.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployees()
    {
        var employees = new[]
        {
            new { Id = 1, Name = "Arun", Department = "IT" },
            new { Id = 2, Name = "Priya", Department = "HR" },
            new { Id = 3, Name = "Kumar", Department = "IT" }
        };

        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetEmployee(int id)
    {
        return Ok(new
        {
            Id = id,
            Name = "Arun",
            Department = "IT"
        });
    }

    [HttpGet("search")]
    public IActionResult SearchEmployees(
        [FromQuery] string? department)
    {
        return Ok(new
        {
            Department = department
        });
    }

    [HttpPost]
    public IActionResult CreateEmployee()
    {
        return Ok("Employee created");
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateEmployee(int id)
    {
        return Ok($"Employee {id} updated");
    }

    [HttpPatch("{id:int}")]
    public IActionResult PartiallyUpdateEmployee(int id)
    {
        return Ok($"Employee {id} partially updated");
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteEmployee(int id)
    {
        return Ok($"Employee {id} deleted");
    }
}
```

---

# 37. Test These URLs

### Get all employees

```text
GET /api/employees
```

### Get one employee

```text
GET /api/employees/10
```

### Search

```text
GET /api/employees/search?department=IT
```

### Create

```text
POST /api/employees
```

### Update

```text
PUT /api/employees/10
```

### Partial update

```text
PATCH /api/employees/10
```

### Delete

```text
DELETE /api/employees/10
```

---

# 38. Request Mapping Mental Model

Always think like this:

```text
HTTP Method + URL
       ↓
     Route
       ↓
Controller
       ↓
Action Method
```

Example:

```text
GET /api/employees/10
```

↓

```text
[HttpGet("{id:int}")]
```

↓

```text
EmployeesController
```

↓

```text
GetEmployee(int id)
```

↓

```text
id = 10
```

---

# 39. Complete Day 10 Project Structure

For your hands-on repository:

```text
DotNetHandsOn
│
├── Day01DotNetIntroduction
├── Day02CSharpFundamentals
├── Day03MethodsStringsArrays
├── Day04OOPFundamentals
├── Day05AdvancedOOP
├── Day06CollectionsGenerics
├── Day07LINQLambda
├── Day08ExceptionAsync
│
└── Day10ControllersRouting
    │
    ├── Controllers
    │   └── EmployeesController.cs
    │
    ├── Properties
    │   └── launchSettings.json
    │
    ├── appsettings.json
    ├── appsettings.Development.json
    ├── Program.cs
    └── Day10ControllersRouting.csproj
```

---

# 40. Program.cs

Your `Program.cs` can be:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

The important connection is:

```text
Program.cs
    ↓
MapControllers()
    ↓
Controller endpoints
    ↓
EmployeesController
```

---

# 41. Day 10 — Important Differences

## Route Parameter

```text
/api/employees/10
```

```csharp
[HttpGet("{id:int}")]
public IActionResult GetEmployee(int id)
```

Use it to identify a resource.

---

## Query Parameter

```text
/api/employees?department=IT
```

```csharp
[HttpGet]
public IActionResult GetEmployees(string? department)
```

Commonly used for filtering/searching/options.

---

## Route Constraint

```csharp
[HttpGet("{id:int}")]
```

Restricts the route parameter to an integer.

---

## Attribute Routing

```csharp
[Route("api/[controller]")]
[HttpGet("{id}")]
```

Route definitions are attached to controllers/actions.

---

## Conventional Routing

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

A general route pattern maps controller/action/id.

---

# 42. Interview Questions

### 1. What is a controller?

A class that handles HTTP requests and returns HTTP responses.

### 2. What is `ControllerBase`?

A base class providing common functionality for API controllers.

### 3. What does `[ApiController]` do?

It enables API-specific conventions and behaviors such as automatic model-validation responses and improved parameter binding.

### 4. What does `[Route("api/[controller]")]` mean?

It defines a controller route and replaces `[controller]` with the controller name without the `Controller` suffix.

### 5. What is routing?

The process of matching an incoming request to an endpoint/action.

### 6. What is attribute routing?

Defining routes using attributes such as:

```csharp
[Route]
[HttpGet]
[HttpPost]
```

### 7. What is conventional routing?

Defining a general route pattern, for example:

```text
{controller}/{action}/{id?}
```

### 8. What is a route parameter?

A variable segment of the URL:

```text
/api/employees/10
```

where:

```text
10
```

is the route value.

### 9. What is a query parameter?

A value supplied after `?`:

```text
/api/employees?department=IT
```

### 10. What is a route constraint?

A restriction on values that can match a route.

Example:

```csharp
{id:int}
```

### 11. GET vs POST?

```text
GET
→ retrieve

POST
→ create
```

### 12. PUT vs PATCH?

```text
PUT
→ generally replaces/updates the resource representation

PATCH
→ partially modifies a resource
```

### 13. What does `[HttpGet("{id}")]` mean?

It maps a GET request containing a route segment to the action.

### 14. What does `IActionResult` represent?

An HTTP action result that allows an action to return different response types/status codes.

---

# 43. Day 10 Cheat Sheet

```text
ASP.NET CORE CONTROLLER
        ↓
[ApiController]
        ↓
[Route("api/[controller]")]
        ↓
EmployeesController
```

### HTTP methods

```text
GET
   → Read

POST
   → Create

PUT
   → Replace/Update

PATCH
   → Partial Update

DELETE
   → Delete
```

### Routing

```text
Attribute Routing
    ↓
[Route]
[HttpGet]
[HttpPost]
[HttpPut]
[HttpPatch]
[HttpDelete]

Conventional Routing
    ↓
{controller}/{action}/{id?}
```

### Parameters

```text
Route parameter
/api/employees/10

Query parameter
/api/employees?department=IT

Route constraint
/api/employees/{id:int}
```

### Complete request flow

```text
Client
   ↓
GET /api/employees/10
   ↓
Kestrel
   ↓
Middleware
   ↓
Routing
   ↓
EmployeesController
   ↓
[HttpGet("{id:int}")]
   ↓
GetEmployee(int id)
   ↓
id = 10
   ↓
IActionResult
   ↓
HTTP Response
   ↓
Client
```

