# Day 11 — REST APIs

Today we move from **Controllers & Routing** to understanding how a **proper REST API is designed and communicated over HTTP**.

By the end of Day 11, you should be able to look at an API request like:

```text
POST /api/employees
Content-Type: application/json
Authorization: Bearer <token>

{
    "name": "Arun",
    "department": "IT",
    "salary": 50000
}
```

and understand every part of it.

---

# 1. What is a REST API?

**REST** stands for:

> **Representational State Transfer**

REST is an architectural style for designing web APIs around **resources** and standard HTTP behavior.

For example, suppose your application manages employees.

The resource is:

```text
Employee
```

The API can expose:

```text
/api/employees
```

A REST-style API commonly uses HTTP methods to operate on that resource:

```text
GET     /api/employees
GET     /api/employees/10
POST    /api/employees
PUT     /api/employees/10
PATCH   /api/employees/10
DELETE  /api/employees/10
```

---

# 2. REST API Mental Model

Think:

```text
Resource
   ↓
URL
   ↓
HTTP Method
   ↓
Request
   ↓
Server
   ↓
Response
```

Example:

```text
GET /api/employees/10
```

means:

> Retrieve employee resource with ID 10.

---

# 3. REST Resource-Based URLs

REST APIs generally use **nouns** for resources rather than actions.

### Good resource-oriented URLs

```text
/api/employees
/api/employees/10
/api/departments
/api/departments/5
```

### Avoid action-style URLs when standard HTTP semantics already express the operation

Instead of:

```text
/api/getEmployees
```

use:

```text
GET /api/employees
```

Instead of:

```text
/api/deleteEmployee/10
```

use:

```text
DELETE /api/employees/10
```

The HTTP method tells us the operation.

---

# 4. Resource + HTTP Method

This is extremely important.

The same URL can represent different operations depending on the HTTP method.

```text
GET    /api/employees
POST   /api/employees
```

Same resource:

```text
employees
```

Different operation:

```text
GET
→ retrieve employees

POST
→ create employee
```

Similarly:

```text
GET    /api/employees/10
PUT    /api/employees/10
PATCH  /api/employees/10
DELETE /api/employees/10
```

---

# 5. REST CRUD Mapping

A common mapping is:

```text
CREATE
POST /api/employees

READ ALL
GET /api/employees

READ ONE
GET /api/employees/10

UPDATE
PUT /api/employees/10

PARTIAL UPDATE
PATCH /api/employees/10

DELETE
DELETE /api/employees/10
```

Visual:

```text
             Employee Resource
                    |
        +-----------+-----------+
        |           |           |
       GET         POST       DELETE
        |           |           |
       Read        Create      Delete
```

---

# 6. Statelessness

One of the important REST principles is **statelessness**.

Stateless means:

> Each request should contain the information necessary for the server to process that request.

The server should not depend on hidden client request state stored from a previous request in order to understand the current request.

Example:

```text
Request 1
GET /api/employees/10
Authorization: Bearer ...

Request 2
GET /api/employees/20
Authorization: Bearer ...
```

Each request contains the information needed for authentication/processing according to the API design.

---

# 7. Stateless Does NOT Mean "No Server-Side Data"

This is a common misunderstanding.

Statelessness does **not** mean:

> The server cannot have a database.

You can absolutely have:

```text
Client
  ↓
REST API
  ↓
Database
```

The important point is about **request interaction state**, not whether the application stores business data.

For example:

```text
Employee data
→ Database

Authentication token
→ Sent with request

Request-specific information
→ Included in the request
```

---

# 8. HTTP Methods

REST APIs use standard HTTP methods.

## GET

Retrieve data.

```http
GET /api/employees
```

Example response:

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

# 9. POST

Create a new resource.

```http
POST /api/employees
```

Request body:

```json
{
    "name": "Kumar",
    "department": "IT",
    "salary": 50000
}
```

Possible response:

```http
201 Created
```

---

# 10. PUT

Generally used to replace/update the representation of an existing resource.

```http
PUT /api/employees/10
```

Body:

```json
{
    "id": 10,
    "name": "Kumar",
    "department": "IT",
    "salary": 60000
}
```

---

# 11. PATCH

Used for a partial modification.

```http
PATCH /api/employees/10
```

For example, only salary:

```json
{
    "salary": 65000
}
```

The exact PATCH format depends on the API design.

---

# 12. DELETE

Delete a resource.

```http
DELETE /api/employees/10
```

Possible response:

```http
204 No Content
```

---

# 13. HTTP Status Codes

A REST API should communicate the result of an operation using appropriate HTTP status codes.

The important ones for your roadmap are:

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

Let's understand each.

---

# 14. 200 OK

```text
200 OK
```

Means the request was successfully processed.

Example:

```http
GET /api/employees/10
```

Response:

```http
200 OK
```

```json
{
    "id": 10,
    "name": "Arun",
    "department": "IT"
}
```

ASP.NET Core:

```csharp
return Ok(employee);
```

---

# 15. 201 Created

```text
201 Created
```

Generally used when a new resource has been successfully created.

Example:

```http
POST /api/employees
```

Response:

```http
201 Created
```

ASP.NET Core:

```csharp
return CreatedAtAction(
    nameof(GetEmployee),
    new { id = employee.Id },
    employee);
```

This is particularly useful because the response can identify the newly created resource.

---

# 16. 204 No Content

```text
204 No Content
```

Means the request succeeded but there is no response body to return.

Example:

```http
DELETE /api/employees/10
```

ASP.NET Core:

```csharp
return NoContent();
```

Commonly used for successful update/delete operations where no representation needs to be returned.

---

# 17. 400 Bad Request

```text
400 Bad Request
```

Means the server cannot process the request because the request is invalid.

Examples:

```text
Invalid input
Invalid JSON
Invalid parameter
Validation failure
```

Example:

```csharp
return BadRequest("Invalid employee data.");
```

Response:

```http
400 Bad Request
```

---

# 18. 401 Unauthorized

```text
401 Unauthorized
```

This generally means the request lacks valid authentication credentials.

For example:

```text
No access token
Invalid/expired authentication credentials
```

Conceptually:

```text
Client
  ↓
API
  ↓
Authentication required
  ↓
401
```

Important distinction:

```text
401
→ Authentication problem

403
→ Authorization/permission problem
```

---

# 19. 403 Forbidden

```text
403 Forbidden
```

The server understands the request, but the authenticated caller is not allowed to perform that operation.

Example:

```text
User is authenticated
        ↓
But doesn't have required permission
        ↓
403 Forbidden
```

For example:

```text
Employee
→ Can view employees

Admin
→ Can delete employees
```

An authenticated employee attempting an admin-only operation could receive:

```text
403 Forbidden
```

---

# 20. 404 Not Found

```text
404 Not Found
```

Usually means the requested resource or endpoint could not be found.

Example:

```http
GET /api/employees/9999
```

If employee `9999` does not exist:

```csharp
return NotFound();
```

Response:

```text
404 Not Found
```

---

# 21. 409 Conflict

```text
409 Conflict
```

Indicates that the request conflicts with the current state of the resource/system.

Example:

Suppose employee email addresses must be unique.

Client sends:

```http
POST /api/employees
```

with:

```json
{
    "name": "Arun",
    "email": "arun@example.com"
}
```

If that email already belongs to another employee, the API may return:

```text
409 Conflict
```

Example:

```csharp
return Conflict("Employee email already exists.");
```

---

# 22. 500 Internal Server Error

```text
500 Internal Server Error
```

Indicates an unexpected server-side failure.

For example:

```text
Unhandled exception
Unexpected application failure
Unexpected infrastructure failure
```

In production, don't return internal exception details such as:

```text
SQL connection string
Stack trace
Internal server paths
```

to clients.

Later you will learn centralized exception handling/middleware.

---

# 23. Status Code Cheat Sheet

```text
200 → Success / response returned

201 → Resource created

204 → Success, no response body

400 → Invalid request

401 → Authentication required/failed

403 → Authenticated but not permitted

404 → Resource/endpoint not found

409 → Conflict with current state

500 → Unexpected server-side error
```

---

# 24. Request Structure

An HTTP request can contain:

```text
Request
├── Method
├── URL
│   ├── Path
│   └── Query String
├── Headers
└── Body
```

Example:

```http
POST /api/employees?sendEmail=true HTTP/1.1
Host: localhost:7001
Content-Type: application/json
Authorization: Bearer <token>

{
    "name": "Arun",
    "department": "IT",
    "salary": 50000
}
```

Let's break it down.

---

# 25. HTTP Method

```text
POST
```

This tells the server the intended operation semantics.

---

# 26. URL Path

```text
/api/employees
```

This identifies the resource endpoint.

---

# 27. Query String

```text
?sendEmail=true
```

Query string parameters commonly represent:

* Filtering
* Searching
* Sorting
* Pagination
* Optional behavior

Example:

```text
/api/employees?department=IT
```

---

# 28. Request Headers

Headers provide metadata about the request.

Examples:

```http
Content-Type: application/json
Accept: application/json
Authorization: Bearer <token>
```

Common headers you'll encounter:

```text
Content-Type
Accept
Authorization
User-Agent
Cache-Control
```

---

# 29. `Content-Type`

`Content-Type` tells the server what format the request body uses.

Example:

```http
Content-Type: application/json
```

means:

> The request body is JSON.

Example:

```json
{
    "name": "Arun"
}
```

Other possible content types include:

```text
application/json
text/plain
multipart/form-data
application/x-www-form-urlencoded
```

---

# 30. `Accept`

`Accept` tells the server what response representation the client prefers.

Example:

```http
Accept: application/json
```

means:

> The client prefers JSON.

So remember:

```text
Content-Type
→ What format am I sending?

Accept
→ What response format do I want?
```

---

# 31. Authorization Header

Authentication tokens are commonly sent using:

```http
Authorization: Bearer <token>
```

Example:

```http
Authorization: Bearer eyJhbGciOi...
```

Later in your roadmap you will learn:

```text
Microsoft Entra ID
MSAL
JWT
Authentication
Authorization
```

For now, remember:

```text
Authorization header
        ↓
Credentials/token information
        ↓
Authentication/authorization processing
```

---

# 32. Request Body

The body contains data sent to the server.

Commonly used with:

```text
POST
PUT
PATCH
```

Example:

```json
{
    "name": "Arun",
    "department": "IT",
    "salary": 50000
}
```

In ASP.NET Core:

```csharp
[HttpPost]
public IActionResult CreateEmployee(Employee employee)
{
    return Ok(employee);
}
```

With `[ApiController]`, a complex type such as `Employee` is commonly inferred from the request body.

You can also explicitly write:

```csharp
[HttpPost]
public IActionResult CreateEmployee([FromBody] Employee employee)
{
    return Ok(employee);
}
```

---

# 33. Route Parameter

Example:

```text
GET /api/employees/10
```

The `10` is a route parameter.

Controller:

```csharp
[HttpGet("{id:int}")]
public IActionResult GetEmployee(int id)
{
    return Ok(id);
}
```

---

# 34. Query String

Example:

```text
GET /api/employees?department=IT
```

Controller:

```csharp
[HttpGet]
public IActionResult GetEmployees(string? department)
{
    return Ok(department);
}
```

Explicit version:

```csharp
[HttpGet]
public IActionResult GetEmployees(
    [FromQuery] string? department)
{
    return Ok(department);
}
```

---

# 35. Headers

Example:

```http
GET /api/employees
Accept: application/json
Authorization: Bearer <token>
```

ASP.NET Core can access headers through the request:

```csharp
Request.Headers
```

Example:

```csharp
[HttpGet]
public IActionResult GetEmployees()
{
    var userAgent = Request.Headers.UserAgent.ToString();

    return Ok(new
    {
        UserAgent = userAgent
    });
}
```

---

# 36. API Response Structure

A typical response contains:

```text
Response
├── Status Code
├── Headers
└── Body
```

Example:

```http
HTTP/1.1 200 OK
Content-Type: application/json

{
    "id": 10,
    "name": "Arun",
    "department": "IT"
}
```

---

# 37. Response Headers

Response headers provide metadata about the response.

Example:

```http
Content-Type: application/json
Content-Length: 72
```

The server can also return other headers depending on the application, authentication, caching, security, and infrastructure configuration.

---

# 38. API Response Format

A REST API commonly returns JSON.

Example:

```json
{
    "id": 10,
    "name": "Arun",
    "department": "IT",
    "salary": 50000
}
```

For multiple records:

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

ASP.NET Core automatically serializes many .NET objects to JSON when using the standard Web API configuration.

---

# 39. Request vs Response

Very important:

```text
REQUEST
----------------
Method
URL
Headers
Body
```

```text
RESPONSE
----------------
Status Code
Headers
Body
```

Example:

```text
CLIENT
  |
  | POST /api/employees
  | Content-Type: application/json
  |
  | {
  |   "name": "Arun"
  | }
  |
  ↓
SERVER
  |
  | 201 Created
  | Content-Type: application/json
  |
  | {
  |   "id": 10,
  |   "name": "Arun"
  | }
  ↓
CLIENT
```

---

# 40. Content Negotiation

This is an important REST/API concept.

**Content negotiation** is the process by which the client and server determine an appropriate representation format for the response.

The client can communicate its preferences using:

```http
Accept
```

For example:

```http
Accept: application/json
```

The server then attempts to provide a compatible representation.

---

# 41. `Accept` vs `Content-Type`

This is a common interview question.

### `Content-Type`

Describes the format of the **body being sent**.

```http
Content-Type: application/json
```

Meaning:

```text
My request body is JSON.
```

### `Accept`

Describes the **response representation the client can accept/prefer**.

```http
Accept: application/json
```

Meaning:

```text
I want/prefer JSON as the response representation.
```

Memory trick:

```text
Content-Type
→ What am I sending?

Accept
→ What can I receive/prefer?
```

---

# 42. Content Negotiation Example

Request:

```http
GET /api/employees
Accept: application/json
```

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
    }
]
```

The response's `Content-Type` tells the client what representation was actually returned.

---

# 43. ASP.NET Core Example — Request Body + Response

Let's create a model:

```csharp
public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }
}
```

Controller:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace Day11RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateEmployee(Employee employee)
    {
        employee.Id = 10;

        return CreatedAtAction(
            nameof(GetEmployee),
            new { id = employee.Id },
            employee);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetEmployee(int id)
    {
        var employee = new Employee
        {
            Id = id,
            Name = "Arun",
            Department = "IT",
            Salary = 50000
        };

        return Ok(employee);
    }
}
```

---

# 44. POST Request

Send:

```http
POST /api/employees
Content-Type: application/json
Accept: application/json
```

Body:

```json
{
    "name": "Kumar",
    "department": "IT",
    "salary": 60000
}
```

ASP.NET Core binds the JSON body to:

```csharp
Employee employee
```

Then:

```csharp
employee.Id = 10;
```

Response:

```http
201 Created
Content-Type: application/json
```

```json
{
    "id": 10,
    "name": "Kumar",
    "department": "IT",
    "salary": 60000
}
```

---

# 45. Why `CreatedAtAction()`?

Instead of simply:

```csharp
return Ok(employee);
```

after creating a resource, you can use:

```csharp
return CreatedAtAction(
    nameof(GetEmployee),
    new { id = employee.Id },
    employee);
```

This communicates:

```text
201 Created
```

and identifies where the created resource can be retrieved.

Conceptually:

```text
POST /api/employees
        ↓
Create employee
        ↓
201 Created
        ↓
Location / GET endpoint for new employee
```

This is a useful REST API pattern.

---

# 46. Complete REST Employee API Example

Here is a Day 11 practice controller:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace Day11RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployees(
        [FromQuery] string? department)
    {
        var employees = new[]
        {
            new Employee
            {
                Id = 1,
                Name = "Arun",
                Department = "IT",
                Salary = 50000
            },
            new Employee
            {
                Id = 2,
                Name = "Priya",
                Department = "HR",
                Salary = 45000
            }
        };

        if (!string.IsNullOrWhiteSpace(department))
        {
            employees = employees
                .Where(e =>
                    e.Department.Equals(
                        department,
                        StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }

        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetEmployee(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid employee ID.");

        var employee = new Employee
        {
            Id = id,
            Name = "Arun",
            Department = "IT",
            Salary = 50000
        };

        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployee(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.Name))
        {
            return BadRequest("Employee name is required.");
        }

        employee.Id = 10;

        return CreatedAtAction(
            nameof(GetEmployee),
            new { id = employee.Id },
            employee);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateEmployee(
        int id,
        Employee employee)
    {
        if (id <= 0)
            return BadRequest();

        employee.Id = id;

        return Ok(employee);
    }

    [HttpPatch("{id:int}")]
    public IActionResult PartiallyUpdateEmployee(
        int id,
        Employee employee)
    {
        if (id <= 0)
            return BadRequest();

        return Ok(new
        {
            Message = $"Employee {id} partially updated"
        });
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteEmployee(int id)
    {
        if (id <= 0)
            return BadRequest();

        return NoContent();
    }
}

public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }
}
```

This is still a **learning/demo API** because there is no database yet.

---

# 47. Test the API

You can test these endpoints using:

* Swagger UI
* Visual Studio tooling
* Postman
* curl
* Browser for simple GET requests

### GET all

```text
GET /api/employees
```

### GET by ID

```text
GET /api/employees/10
```

### Filter

```text
GET /api/employees?department=IT
```

### POST

```text
POST /api/employees
```

Body:

```json
{
    "name": "Kumar",
    "department": "IT",
    "salary": 60000
}
```

### PUT

```text
PUT /api/employees/10
```

### PATCH

```text
PATCH /api/employees/10
```

### DELETE

```text
DELETE /api/employees/10
```

---

# 48. REST API URL Design

For your Employee API, think in terms of resources:

```text
/api/employees
/api/employees/{id}
```

For departments:

```text
/api/departments
/api/departments/{id}
```

For employee-related sub-resources, depending on the API's domain model:

```text
/api/employees/{id}/addresses
/api/employees/{id}/leaves
```

The exact URL design should reflect the resources and relationships in your domain.

---

# 49. Don't Put Everything in the URL

Avoid unnecessarily long action-based URLs such as:

```text
/api/getAllEmployees
/api/createEmployee
/api/deleteEmployee/10
```

A resource-oriented design can instead use:

```text
GET    /api/employees
POST   /api/employees
DELETE /api/employees/10
```

The method communicates the operation.

---

# 50. REST API Request Anatomy

Memorize this diagram:

```text
                    HTTP REQUEST

┌─────────────────────────────────────┐
│ POST /api/employees?notify=true     │
│                                     │
│ Headers:                            │
│ Content-Type: application/json      │
│ Accept: application/json            │
│ Authorization: Bearer <token>       │
│                                     │
│ Body:                               │
│ {                                   │
│   "name": "Arun",                   │
│   "department": "IT"               │
│ }                                   │
└─────────────────────────────────────┘
                    ↓
              ASP.NET Core
                    ↓
              HTTP RESPONSE
┌─────────────────────────────────────┐
│ 201 Created                         │
│                                     │
│ Content-Type: application/json      │
│                                     │
│ {                                   │
│   "id": 10,                         │
│   "name": "Arun",                  │
│   "department": "IT"              │
│ }                                   │
└─────────────────────────────────────┘
```

---

# 51. REST Principles — Revision

For your class, remember these core concepts:

### 1. Resource-based URLs

```text
/api/employees
/api/employees/10
```

### 2. Statelessness

Each request contains the information required to process it; the API does not rely on hidden client interaction state from previous requests.

### 3. HTTP methods

```text
GET
POST
PUT
PATCH
DELETE
```

### 4. HTTP status codes

```text
200
201
204
400
401
403
404
409
500
```

### 5. Request headers

```text
Content-Type
Accept
Authorization
```

### 6. Response headers

```text
Content-Type
```

plus other metadata depending on the API/infrastructure.

### 7. Content negotiation

Client and server determine a suitable representation, commonly using:

```text
Accept
Content-Type
```

---

# 52. Day 11 Interview Questions

### What is REST?

REST is an architectural style for designing networked applications around resources, representations, and standard HTTP semantics.

### What is a REST API?

An API designed using REST principles and HTTP mechanisms to expose and manipulate resources.

### What does stateless mean?

Each request contains the information needed to process it; the server does not depend on stored client interaction state from previous requests.

### Why use nouns in REST URLs?

URLs identify resources, while HTTP methods express common operations.

### What is the difference between PUT and PATCH?

PUT is generally used to replace/update a resource representation, while PATCH is used for partial modification.

### What is the difference between 401 and 403?

```text
401
→ Authentication credentials are missing/invalid.

403
→ The caller is authenticated but isn't permitted to perform the operation.
```

### When would you use 201?

When a new resource has been successfully created.

### When would you use 204?

When an operation succeeds but there is no response body to return.

### When would you use 409?

When the request conflicts with the current state of the resource/system.

### What is `Content-Type`?

It describes the media type of the request or response body.

### What is `Accept`?

It communicates the response media types the client can accept/prefer.

### What is a request body?

The payload sent as part of an HTTP request, commonly used for POST, PUT, and PATCH.

### What is a route parameter?

A variable value embedded in the URL path:

```text
/api/employees/10
```

### What is a query parameter?

A parameter supplied after `?`:

```text
/api/employees?department=IT
```

---

# 53. Day 11 Final Cheat Sheet

```text
                    REST API

                      RESOURCE
                         ↓
                 /api/employees
                         ↓
        ┌────────────────────────────────┐
        │                                │
       GET                             POST
        ↓                                ↓
      READ                             CREATE

/api/employees/10

       GET
        ↓
     READ ONE

       PUT
        ↓
     REPLACE/UPDATE

      PATCH
        ↓
   PARTIAL UPDATE

     DELETE
        ↓
      DELETE
```

### Request

```text
Request
├── HTTP Method
├── URL
│   ├── Route
│   └── Query String
├── Headers
└── Body
```

### Response

```text
Response
├── Status Code
├── Headers
└── Body
```

### Most important status codes

```text
200 → Success
201 → Created
204 → Success / No Content

400 → Bad Request
401 → Authentication problem
403 → Permission problem
404 → Not Found
409 → Conflict

500 → Server Error
```

### Most important headers

```text
Content-Type
→ What format is the body?

Accept
→ What response format does the client prefer?

Authorization
→ Authentication credentials/token information
```

### Complete mental model

```text
Angular / React / Postman
          ↓
    HTTP Request
          ↓
Method + URL + Headers + Body
          ↓
     ASP.NET Core
          ↓
       Routing
          ↓
      Controller
          ↓
      HTTP Result
          ↓
    HTTP Response
          ↓
Status + Headers + Body
          ↓
        Client
```

---

# Phase 4: Web API Fundamentals — Complete Class Notes

Target: C# + ASP.NET Core Web API using .NET 10 Level: Beginner to intermediate Purpose: Teacher-ready notes with definitions, examples, practical implementation in Visual Studio, and Postman testing.

By the end of this phase, students should be able to create a RESTful Web API, understand HTTP requests and responses, build CRUD endpoints, validate incoming data, and test APIs using Swagger and Postman.

## 1. What is an API? What is REST?

### 1.1 What is an API?

API stands for Application Programming Interface. It allows two software applications to communicate with each other through a defined set of rules.

Example: An employee management application has an Angular frontend and a .NET backend.

Angular Frontend

User clicks “View Employees”

ASP.NET Core Web API

Receives request and processes it

Database

Stores employee information

The API sends the result back to the frontend, commonly as JSON.

Suppose the frontend sends:

```
GET /api/employees
```

The API might return:

```
[
  {
    "id": 1,
    "name": "Arun",
    "department": "IT"
  },
  {
    "id": 2,
    "name": "Priya",
    "department": "HR"
  }
]
```

The frontend can display this data in an employee table.

### 1.2 What is a Web API?

A Web API exposes functionality or data over HTTP or HTTPS so that other applications can access it.

Common examples include:

- Employee management APIs
- Banking APIs
- Payment APIs
- E-commerce APIs
- Authentication APIs
- Weather APIs

ASP.NET Core Web API is Microsoft's framework for building HTTP APIs using C# and .NET.

### 1.3 What is REST?

REST stands for Representational State Transfer. It is an architectural style for designing network APIs.

A REST-style API generally:

- Represents data as resources, such as employees or products.
- Uses resource URLs, such as `/api/employees`.
- Uses HTTP methods to describe operations.
- Uses standard HTTP status codes.
- Is stateless: each request carries the information needed to process it, rather than relying on stored client-session state.
- Commonly exchanges JSON.

Example:

| Operation                 | HTTP method | URL                |
| ------------------------- | ----------- | ------------------ |
| Get all employees         | GET         | `/api/employees`   |
| Get one employee          | GET         | `/api/employees/1` |
| Create employee           | POST        | `/api/employees`   |
| Replace employee          | PUT         | `/api/employees/1` |
| Partially update employee | PATCH       | `/api/employees/1` |
| Delete employee           | DELETE      | `/api/employees/1` |

Teaching point: A Web API is an interface exposed over the web. REST is one approach to designing that API. Not every Web API is RESTful.

## 2. HTTP Request and Response

HTTP is the protocol that clients and servers use to communicate.

An HTTP communication typically has two parts:

1. Request: Sent by the client to the server.
2. Response: Sent by the server back to the client.

### 2.1 Anatomy of an HTTP request

Example:

```
POST /api/employees HTTP/1.1
Host: localhost:7001
Content-Type: application/json
Accept: application/json

{
  "name": "Arun",
  "department": "IT",
  "email": "arun@example.com"
}
```

| Component        | Meaning                               |
| ---------------- | ------------------------------------- |
| `POST`           | HTTP method                           |
| `/api/employees` | Request path                          |
| `HTTP/1.1`       | HTTP protocol version                 |
| `Content-Type`   | Format of the request body            |
| `Accept`         | Response format the client can accept |
| JSON body        | Data sent to the server               |

The exact host and port depend on the application.

### 2.2 Anatomy of an HTTP response

```
HTTP/1.1 201 Created
Content-Type: application/json

{
  "id": 3,
  "name": "Arun",
  "department": "IT",
  "email": "arun@example.com"
}
```

The response contains:

- Status code: `201 Created`
- Headers: metadata about the response
- Body: the created employee data

Not every response contains a body. For example, `204 No Content` indicates successful processing with no response body.

### 2.3 Request-response flow

\#chatgpt-mermaid-\_r_5q3\_{font-family:-apple-system-body,ui-sans-serif,-apple-system,system-ui,"Segoe UI",Helvetica,"Apple Color Emoji",Arial,sans-serif,"Segoe UI Emoji","Segoe UI Symbol";font-size:16px;fill:rgb(237, 237, 237);}@keyframes edge-animation-frame{from{stroke-dashoffset:0;}}@keyframes dash{to{stroke-dashoffset:0;}}#chatgpt-mermaid-\_r_5q3\_ .edge-animation-slow{stroke-dasharray:9,5!important;stroke-dashoffset:900;animation:dash 50s linear infinite;stroke-linecap:round;}#chatgpt-mermaid-\_r_5q3\_ .edge-animation-fast{stroke-dasharray:9,5!important;stroke-dashoffset:900;animation:dash 20s linear infinite;stroke-linecap:round;}#chatgpt-mermaid-\_r_5q3\_ .error-icon{fill:rgb(48, 48, 48);}#chatgpt-mermaid-\_r_5q3\_ .error-text{fill:rgb(237, 237, 237);stroke:rgb(237, 237, 237);}#chatgpt-mermaid-\_r_5q3\_ .edge-thickness-normal{stroke-width:1px;}#chatgpt-mermaid-\_r_5q3\_ .edge-thickness-thick{stroke-width:3.5px;}#chatgpt-mermaid-\_r_5q3\_ .edge-pattern-solid{stroke-dasharray:0;}#chatgpt-mermaid-\_r_5q3\_ .edge-thickness-invisible{stroke-width:0;fill:none;}#chatgpt-mermaid-\_r_5q3\_ .edge-pattern-dashed{stroke-dasharray:3;}#chatgpt-mermaid-\_r_5q3\_ .edge-pattern-dotted{stroke-dasharray:2;}#chatgpt-mermaid-\_r_5q3\_ .marker{fill:rgb(175, 175, 175);stroke:rgb(175, 175, 175);}#chatgpt-mermaid-\_r_5q3\_ .marker.cross{stroke:rgb(175, 175, 175);}#chatgpt-mermaid-\_r_5q3\_ svg{font-family:-apple-system-body,ui-sans-serif,-apple-system,system-ui,"Segoe UI",Helvetica,"Apple Color Emoji",Arial,sans-serif,"Segoe UI Emoji","Segoe UI Symbol";font-size:16px;}#chatgpt-mermaid-\_r_5q3\_ p{margin:0;}#chatgpt-mermaid-\_r_5q3\_ .label{font-family:-apple-system-body,ui-sans-serif,-apple-system,system-ui,"Segoe UI",Helvetica,"Apple Color Emoji",Arial,sans-serif,"Segoe UI Emoji","Segoe UI Symbol";color:rgb(237, 237, 237);}#chatgpt-mermaid-\_r_5q3\_ .cluster-label text{fill:rgb(237, 237, 237);}#chatgpt-mermaid-\_r_5q3\_ .cluster-label span{color:rgb(237, 237, 237);}#chatgpt-mermaid-\_r_5q3\_ .cluster-label span p{background-color:transparent;}#chatgpt-mermaid-\_r_5q3\_ .label text,#chatgpt-mermaid-\_r_5q3\_ span{fill:rgb(237, 237, 237);color:rgb(237, 237, 237);}#chatgpt-mermaid-\_r_5q3\_ .node rect,#chatgpt-mermaid-\_r_5q3\_ .node circle,#chatgpt-mermaid-\_r_5q3\_ .node ellipse,#chatgpt-mermaid-\_r_5q3\_ .node polygon,#chatgpt-mermaid-\_r_5q3\_ .node path{fill:rgb(9, 23, 44);stroke:rgb(31, 78, 148);stroke-width:1px;}#chatgpt-mermaid-\_r_5q3\_ .rough-node .label text,#chatgpt-mermaid-\_r_5q3\_ .node .label text,#chatgpt-mermaid-\_r_5q3\_ .image-shape .label,#chatgpt-mermaid-\_r_5q3\_ .icon-shape .label{text-anchor:middle;}#chatgpt-mermaid-\_r_5q3\_ .node .katex path{fill:#000;stroke:#000;stroke-width:1px;}#chatgpt-mermaid-\_r_5q3\_ .rough-node .label,#chatgpt-mermaid-\_r_5q3\_ .node .label,#chatgpt-mermaid-\_r_5q3\_ .image-shape .label,#chatgpt-mermaid-\_r_5q3\_ .icon-shape .label{text-align:center;}#chatgpt-mermaid-\_r_5q3\_ .node.clickable{cursor:pointer;}#chatgpt-mermaid-\_r_5q3\_ .root .anchor path{fill:rgb(175, 175, 175)!important;stroke-width:0;stroke:rgb(175, 175, 175);}#chatgpt-mermaid-\_r_5q3\_ .arrowheadPath{fill:rgb(175, 175, 175);}#chatgpt-mermaid-\_r_5q3\_ .edgePath .path{stroke:rgb(175, 175, 175);stroke-width:1px;}#chatgpt-mermaid-\_r_5q3\_ .flowchart-link{stroke:rgb(175, 175, 175);fill:none;}#chatgpt-mermaid-\_r_5q3\_ .edgeLabel{background-color:rgb(0, 0, 0);text-align:center;}#chatgpt-mermaid-\_r_5q3\_ .edgeLabel p{background-color:rgb(0, 0, 0);}#chatgpt-mermaid-\_r_5q3\_ .edgeLabel rect{opacity:0.5;background-color:rgb(0, 0, 0);fill:rgb(0, 0, 0);}#chatgpt-mermaid-\_r_5q3\_ .labelBkg{background-color:rgba(0, 0, 0, 0.5);}#chatgpt-mermaid-\_r_5q3\_ .cluster rect{fill:rgb(48, 48, 48);stroke:rgba(255, 255, 255, 0.15);stroke-width:1px;}#chatgpt-mermaid-\_r_5q3\_ .cluster text{fill:rgb(237, 237, 237);}#chatgpt-mermaid-\_r_5q3\_ .cluster span{color:rgb(237, 237, 237);}#chatgpt-mermaid-\_r_5q3\_ div.mermaidTooltip{position:absolute;text-align:center;max-width:200px;padding:2px;font-family:-apple-system-body,ui-sans-serif,-apple-system,system-ui,"Segoe UI",Helvetica,"Apple Color Emoji",Arial,sans-serif,"Segoe UI Emoji","Segoe UI Symbol";font-size:12px;background:rgb(48, 48, 48);border:1px solid rgba(255, 255, 255, 0.15);border-radius:2px;pointer-events:none;z-index:100;}#chatgpt-mermaid-\_r_5q3\_ .flowchartTitleText{text-anchor:middle;font-size:18px;fill:rgb(237, 237, 237);}#chatgpt-mermaid-\_r_5q3\_ rect.text{fill:none;stroke-width:0;}#chatgpt-mermaid-\_r_5q3\_ .icon-shape,#chatgpt-mermaid-\_r_5q3\_ .image-shape{background-color:rgb(0, 0, 0);text-align:center;}#chatgpt-mermaid-\_r_5q3\_ .icon-shape p,#chatgpt-mermaid-\_r_5q3\_ .image-shape p{background-color:rgb(0, 0, 0);padding:2px;}#chatgpt-mermaid-\_r_5q3\_ .icon-shape .label rect,#chatgpt-mermaid-\_r_5q3\_ .image-shape .label rect{opacity:0.5;background-color:rgb(0, 0, 0);fill:rgb(0, 0, 0);}#chatgpt-mermaid-\_r_5q3\_ .label-icon{display:inline-block;height:1em;overflow:visible;vertical-align:-0.125em;}#chatgpt-mermaid-\_r_5q3\_ .node .label-icon path{fill:currentColor;stroke:revert;stroke-width:revert;}#chatgpt-mermaid-\_r_5q3\_ .node .neo-node{stroke:rgb(31, 78, 148);}#chatgpt-mermaid-\_r_5q3\_ [data-look="neo"].node rect,#chatgpt-mermaid-\_r_5q3\_ [data-look="neo"].cluster rect,#chatgpt-mermaid-\_r_5q3\_ [data-look="neo"].node polygon{stroke:url(#chatgpt-mermaid-\_r_5q3\_-gradient);filter:drop-shadow( 1px 2px 2px rgba(185,185,185,1));}#chatgpt-mermaid-\_r_5q3\_ [data-look="neo"].swimlane.cluster rect{filter:none;}#chatgpt-mermaid-\_r_5q3\_ [data-look="neo"].node path{stroke:url(#chatgpt-mermaid-\_r_5q3\_-gradient);stroke-width:1px;}#chatgpt-mermaid-\_r_5q3\_ [data-look="neo"].node .outer-path{filter:drop-shadow( 1px 2px 2px rgba(185,185,185,1));}#chatgpt-mermaid-\_r_5q3\_ [data-look="neo"].node .neo-line path{stroke:rgb(31, 78, 148);filter:none;}#chatgpt-mermaid-\_r_5q3\_ [data-look="neo"].node circle{stroke:url(#chatgpt-mermaid-\_r_5q3\_-gradient);filter:drop-shadow( 1px 2px 2px rgba(185,185,185,1));}#chatgpt-mermaid-\_r_5q3\_ [data-look="neo"].node circle .state-start{fill:#000000;}#chatgpt-mermaid-\_r_5q3\_ [data-look="neo"].icon-shape .icon{fill:url(#chatgpt-mermaid-\_r_5q3\_-gradient);filter:drop-shadow( 1px 2px 2px rgba(185,185,185,1));}#chatgpt-mermaid-\_r_5q3\_ [data-look="neo"].icon-shape .icon-neo path{stroke:url(#chatgpt-mermaid-\_r_5q3\_-gradient);filter:drop-shadow( 1px 2px 2px rgba(185,185,185,1));}#chatgpt-mermaid-\_r_5q3\_ .node text{font-size:14px;font-weight:600;letter-spacing:normal;fill:rgb(153, 206, 255);}#chatgpt-mermaid-\_r_5q3\_ .edgeLabels text{font-size:13px;font-weight:600;letter-spacing:-0.08px;fill:rgb(153, 206, 255);}#chatgpt-mermaid-\_r_5q3\_ .node tspan[font-weight="normal"],#chatgpt-mermaid-\_r_5q3\_ .edgeLabels tspan[font-weight="normal"]{font-weight:600;}#chatgpt-mermaid-\_r_5q3\_ .edgeLabel .label rect{opacity:1;rx:13px;ry:13px;fill:rgb(0, 14, 26);stroke:rgb(26, 62, 95);stroke-width:1px;}#chatgpt-mermaid-\_r_5q3\_ .node rect,#chatgpt-mermaid-\_r_5q3\_ .node circle,#chatgpt-mermaid-\_r_5q3\_ .node ellipse,#chatgpt-mermaid-\_r_5q3\_ .node polygon,#chatgpt-mermaid-\_r_5q3\_ .node path{fill:rgb(0, 40, 77);stroke:rgba(255, 255, 255, 0.1);stroke-width:1px;}#chatgpt-mermaid-\_r_5q3\_ .node rect{rx:16px;ry:16px;}#chatgpt-mermaid-\_r_5q3\_ .node.mermaid-decision .label-container{fill:rgb(0, 14, 26);stroke:rgb(26, 62, 95);stroke-dasharray:2,2;}#chatgpt-mermaid-\_r_5q3\_ .edgePaths .flowchart-link{stroke:rgb(175, 175, 175);stroke-width:1px;stroke-linecap:round;stroke-linejoin:round;}#chatgpt-mermaid-\_r_5q3\_ .marker{fill:rgb(175, 175, 175);stroke:rgb(175, 175, 175);}#chatgpt-mermaid-\_r_5q3\_ :root{--mermaid-font-family:-apple-system-body,ui-sans-serif,-apple-system,system-ui,"Segoe UI",Helvetica,"Apple Color Emoji",Arial,sans-serif,"Segoe UI Emoji","Segoe UI Symbol";}Angular / Postman ClientASP.NET Core MiddlewareRoutingController ActionBusiness LogicResponse DataHTTP Response + Status CodeHTTP Request

Class question: What is the difference between a request and a response?

Answer: The client sends a request to perform an operation or retrieve data. The server sends a response containing the outcome, status code, headers, and sometimes a body.

## 3. HTTP Methods: GET, POST, PUT, PATCH, DELETE

HTTP methods communicate the intended operation.

### GET

Retrieve data

`GET /api/employees`

Returns employees. It should not modify server data.

### POST

Create a resource or perform an operation

`POST /api/employees`

Usually sends new employee data in the request body.

### PUT

Replace a resource

`PUT /api/employees/1`

Usually supplies the complete editable representation of the employee.

### PATCH

Partially update a resource

`PATCH /api/employees/1`

Changes only specified fields. The exact body format depends on the API's PATCH contract.

### DELETE

Delete a resource

`DELETE /api/employees/1`

Removes the employee if the operation succeeds.

### PUT versus PATCH

Suppose an employee has these fields:

```
{
  "id": 1,
  "name": "Arun",
  "department": "IT",
  "email": "arun@example.com"
}
```

A PUT request generally provides the complete representation of the editable fields:

```
{
  "name": "Arun Kumar",
  "department": "IT",
  "email": "arun@example.com"
}
```

A PATCH request might provide only the field to change:

```
{
  "name": "Arun Kumar"
}
```

PATCH does not automatically know how to apply arbitrary JSON. The API must define the supported patch format and processing rules.

Important HTTP properties:

- GET is safe and idempotent.
- PUT and DELETE are defined as idempotent HTTP methods.
- POST is not generally idempotent.
- PATCH is not guaranteed to be idempotent.

Idempotent means repeating the same request has the same intended effect on server state as performing it once.

## 4. Controllers and Actions

### 4.1 What is a controller?

A controller is a C# class that handles incoming HTTP requests and produces HTTP responses.

In ASP.NET Core Web API, controllers commonly inherit from `ControllerBase`.

### 4.2 What is an action?

An action is a public method in a controller that handles a particular request.

Example:

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
        return Ok("Employee list");
    }
}
```

Explanation:

- `[ApiController]` enables API-specific behavior, including automatic validation responses for many invalid model-binding or validation cases.
- `[Route("api/[controller]")]` defines the base route.
- `[controller]` is replaced with the controller name minus `Controller`, so `EmployeesController` becomes `employees`.
- `[HttpGet]` maps GET requests to the action.
- `Ok(...)` returns HTTP `200 OK`.
- `ControllerBase` provides methods such as `Ok()`, `NotFound()`, `BadRequest()`, and `CreatedAtAction()`.

The endpoint is:

```
GET /api/employees
```

### 4.3 Why use controllers?

Controllers help organize API endpoints, separate request handling from business logic, and make routes and HTTP responses easier to understand.

For larger applications, keep business rules in services rather than placing all application logic directly inside controllers.

## 5. Routing and Route Parameters

Routing determines which controller action handles a request.

### 5.1 Attribute routing

Attribute routing uses attributes on controllers and actions to define URL patterns.

```
[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok("All employees");
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        return Ok($"Employee ID: {id}");
    }
}
```

Example URLs:

```
GET /api/employees
GET /api/employees/10
```

The `{id:int}` constraint means the route segment must match an integer.

### 5.2 Route parameters

Route parameters capture values from the URL path.

```
[HttpGet("{id:int}")]
public IActionResult GetById(int id)
{
    return Ok(new { EmployeeId = id });
}
```

Request:

```
GET /api/employees/25
```

Example JSON response:

```
{
  "employeeId": 25
}
```

If the route template requires an integer, a request such as `/api/employees/abc` will not match that route.

### 5.3 Route naming best practices

Prefer resource-oriented routes:

```
/api/employees
/api/employees/10
/api/departments
/api/products
```

Avoid unnecessarily action-oriented routes such as:

```
/api/getAllEmployees
/api/deleteEmployee/10
```

Use HTTP methods to express the operation, and routes to identify the resource.

## 6. Query Parameters and Request Headers

### 6.1 Query parameters

Query parameters appear after `?` in a URL and are often used for filtering, searching, sorting, and pagination.

Example:

```
GET /api/employees?department=IT&page=1&pageSize=10
```

Controller:

```
[HttpGet]
public IActionResult GetEmployees(
    [FromQuery] string? department,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
{
    return Ok(new
    {
        Department = department,
        Page = page,
        PageSize = pageSize
    });
}
```

For the example request, the response is:

```
{
  "department": "IT",
  "page": 1,
  "pageSize": 10
}
```

This example echoes the parameters; a real endpoint would use them to filter and paginate data.

### 6.2 Request headers

Headers contain metadata about a request.

Common headers include:

| Header          | Purpose                                           |
| --------------- | ------------------------------------------------- |
| `Content-Type`  | Format of the request body                        |
| `Accept`        | Response formats the client can accept            |
| `Authorization` | Credentials or access token                       |
| `User-Agent`    | Information about the client                      |
| `If-Match`      | Can support conditional updates using entity tags |

Read a header explicitly:

```
[HttpGet("client-info")]
public IActionResult GetClientInfo(
    [FromHeader(Name = "User-Agent")] string? userAgent)
{
    return Ok(new { UserAgent = userAgent });
}
```

Security note: Do not treat a custom header as proof of identity. Use properly configured authentication and authorization for protected endpoints.

## 7. Model Binding

Model binding converts incoming request data into .NET parameters and objects that a controller action can use.

ASP.NET Core can bind values from:

- Route parameters
- Query strings
- Request headers
- Request bodies
- Form fields

### 7.1 Example using multiple sources

```
[HttpGet("{id:int}")]
public IActionResult GetEmployee(
    [FromRoute] int id,
    [FromQuery] bool includeDepartment = false)
{
    return Ok(new
    {
        Id = id,
        IncludeDepartment = includeDepartment
    });
}
```

Request:

```
GET /api/employees/5?includeDepartment=true
```

Response:

```
{
  "id": 5,
  "includeDepartment": true
}
```

### 7.2 Common binding attributes

| Attribute      | Source       |
| -------------- | ------------ |
| `[FromRoute]`  | URL path     |
| `[FromQuery]`  | Query string |
| `[FromHeader]` | HTTP headers |
| `[FromBody]`   | Request body |
| `[FromForm]`   | Form fields  |

With `[ApiController]`, ASP.NET Core infers many binding sources automatically. Explicit attributes are useful for teaching and for making the API contract clear.

For example:

```
[HttpPost]
public IActionResult CreateEmployee(
    [FromBody] CreateEmployeeDto employee)
{
    return Ok(employee);
}
```

ASP.NET Core reads the JSON request body and attempts to deserialize it into `CreateEmployeeDto`.

If the body is invalid or cannot be converted, the request can produce a validation or model-binding error response.

## 8. DTOs — Data Transfer Objects

### 8.1 What is a DTO?

A DTO (Data Transfer Object) is a type used to transfer data between application boundaries, such as between a client and an API.

DTOs help control which fields clients can send or receive.

Consider an employee database entity:

```
public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public decimal Salary { get; set; }

    public bool IsAdmin { get; set; }
}
```

A public API should not automatically expose every field in the entity.

### 8.2 Create DTO

Create a folder called `DTOs` and add `CreateEmployeeDto.cs`.

```
using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.DTOs;

public class CreateEmployeeDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Department { get; set; } = string.Empty;
}
```

This DTO lets the client supply only the intended fields. The API decides the employee ID and any server-controlled fields.

### 8.3 Response DTO

Create `EmployeeResponseDto.cs`:

```
namespace EmployeeApi.DTOs;

public class EmployeeResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;
}
```

The response does not include `Salary` or `IsAdmin`.

### 8.4 Why are DTOs important?

- Prevent exposing internal database fields.
- Reduce accidental over-posting.
- Define separate request and response contracts.
- Apply validation rules.
- Allow database models to change without automatically changing the public API.

Teaching point: DTOs are not a security mechanism by themselves. The API still needs authentication, authorization, and appropriate validation.

## 9. JSON Serialization and Deserialization

JSON stands for JavaScript Object Notation. It is a common data format used by Web APIs.

### 9.1 Serialization

Serialization converts a .NET object into JSON.

```
using System.Text.Json;

var employee = new
{
    Id = 1,
    Name = "Arun",
    Department = "IT"
};

string json = JsonSerializer.Serialize(employee);

Console.WriteLine(json);
```

Output:

```
{"Id":1,"Name":"Arun","Department":"IT"}
```

### 9.2 Deserialization

Deserialization converts JSON into a .NET object.

```
using System.Text.Json;

string json = """
{
    "Name": "Arun",
    "Email": "arun@example.com",
    "Department": "IT"
}
""";

CreateEmployeeDto? employee =
    JsonSerializer.Deserialize<CreateEmployeeDto>(json);

Console.WriteLine(employee?.Name);
```

Output:

```
Arun
```

ASP.NET Core normally handles JSON serialization and deserialization for controller request bodies and responses using its configured formatters.

For example:

```
return Ok(employee);
```

ASP.NET Core serializes the returned object into JSON by default for a typical Web API response.

### 9.3 JSON naming conventions

A C# property named `FirstName` is commonly represented as `firstName` in ASP.NET Core's default web JSON configuration.

Example:

```
{
  "firstName": "Arun"
}
```

Do not confuse JSON serialization with database storage. JSON is the representation exchanged through the API; the application can store data in SQL Server or another data store.

## 10. HTTP Status Codes

A status code communicates the result of an HTTP request.

| Status | Name                  | Typical use                                                          |
| ------ | --------------------- | -------------------------------------------------------------------- |
| `200`  | OK                    | Successful request with a response                                   |
| `201`  | Created               | A new resource was created                                           |
| `204`  | No Content            | Successful request with no response body                             |
| `400`  | Bad Request           | Invalid input or request                                             |
| `401`  | Unauthorized          | Valid authentication credentials are missing or invalid              |
| `403`  | Forbidden             | The caller is authenticated but not allowed to perform the operation |
| `404`  | Not Found             | The requested resource was not found                                 |
| `409`  | Conflict              | Request conflicts with the current resource state                    |
| `500`  | Internal Server Error | Unexpected server-side failure                                       |

### 10.1 Examples in ASP.NET Core

```
return Ok(employee);                  // 200
return CreatedAtAction(...);          // 201
return NoContent();                   // 204
return BadRequest("Invalid data");    // 400
return Unauthorized();                // 401
return Forbid();                      // 403
return NotFound();                    // 404
return Conflict("Duplicate email");  // 409
```

For a `500` response, normally handle unexpected exceptions through centralized exception-handling middleware rather than returning `500` directly from every action.

### 10.2 Common scenarios

Scenario 1: The employee exists.

```
GET /api/employees/10
```

Possible response: `200 OK`.

Scenario 2: The employee does not exist.

Possible response: `404 Not Found`.

Scenario 3: The client sends an invalid email address.

Possible response: `400 Bad Request`.

Scenario 4: The caller has not provided a valid access token to a protected endpoint.

Possible response: `401 Unauthorized`.

Scenario 5: The caller is authenticated but lacks the required permission.

Possible response: `403 Forbidden`.

Scenario 6: The client attempts to create an employee using an email address that must be unique, and that email already exists.

Possible response: `409 Conflict`.

Important: A `500` response indicates a server failure. Do not expose stack traces, secrets, or database connection details in production error responses.

## 11. `IActionResult` and `ActionResult<T>`

These are return types commonly used in ASP.NET Core controller actions.

### 11.1 `IActionResult`

`IActionResult` allows an action to return different HTTP result types.

```
[HttpGet("{id:int}")]
public IActionResult GetEmployee(int id)
{
    if (id <= 0)
    {
        return BadRequest("ID must be positive.");
    }

    if (id != 1)
    {
        return NotFound();
    }

    return Ok(new
    {
        Id = 1,
        Name = "Arun"
    });
}
```

This action can return:

- `400 Bad Request`
- `404 Not Found`
- `200 OK`

### 11.2 `ActionResult<T>`

`ActionResult<T>` is useful when an action usually returns a specific data type but may also return HTTP error results.

```
[HttpGet("{id:int}")]
public ActionResult<EmployeeResponseDto> GetEmployee(int id)
{
    if (id <= 0)
    {
        return BadRequest();
    }

    var employee = new EmployeeResponseDto
    {
        Id = id,
        Name = "Arun",
        Email = "arun@example.com",
        Department = "IT"
    };

    return employee;
}
```

The successful response contains an `EmployeeResponseDto`; the action can also return `BadRequest()` or `NotFound()`.

### 11.3 Which should you use?

| Situation                                                  | Suggested return type                         |
| ---------------------------------------------------------- | --------------------------------------------- |
| Several different result types or no single response model | `IActionResult`                               |
| A defined response DTO plus possible HTTP errors           | `ActionResult<T>`                             |
| A method that returns only a simple fixed value            | Consider the clearest appropriate result type |

For typical CRUD controller actions, `ActionResult<T>` is often a good choice for GET operations that return a particular resource type.

## 12. Attribute Routing

Attribute routing explicitly defines routes with attributes.

Example:

```
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok("All products");
    }

    [HttpGet("{id:int}")]
    public IActionResult GetProductById(int id)
    {
        return Ok(new { Id = id });
    }

    [HttpGet("category/{categoryName}")]
    public IActionResult GetByCategory(string categoryName)
    {
        return Ok(new { Category = categoryName });
    }
}
```

Routes:

```
GET /api/products
GET /api/products/10
GET /api/products/category/electronics
```

### Route constraints

Constraints help distinguish route patterns.

```
[HttpGet("{id:int}")]
public IActionResult GetById(int id) => Ok(id);

[HttpGet("{name:alpha}")]
public IActionResult GetByName(string name) => Ok(name);
```

These examples constrain `id` to an integer and `name` to alphabetic characters. For more complex business validation, use model validation rather than relying solely on route constraints.

Teaching point: The route pattern and HTTP method work together to identify an action.

## 13. Swagger and OpenAPI

### 13.1 What is OpenAPI?

OpenAPI is a standard specification for describing HTTP APIs, including endpoints, parameters, request bodies, responses, and authentication schemes.

### 13.2 What is Swagger?

Swagger is a set of tools associated with working with OpenAPI documents. Swagger UI provides an interactive page for exploring and testing API endpoints.

In ASP.NET Core, the exact setup depends on the project template and the OpenAPI tooling package used.

### 13.3 Enable OpenAPI in a .NET 10 project

In a current Visual Studio ASP.NET Core Web API project, inspect `Program.cs` and confirm the project has the relevant OpenAPI package and services configured.

A common modern setup uses `Microsoft.AspNetCore.OpenApi`:

```
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
```

This exposes an OpenAPI document, commonly at:

```
/openapi/v1.json
```

Note: This code exposes the OpenAPI document, not Swagger UI itself. To get an interactive Swagger UI page, add and configure a compatible Swagger UI tool, such as Swashbuckle, according to that package's instructions. Templates and package defaults can differ, so don't assume every new .NET 10 project includes `/swagger`.

### 13.4 How to use Swagger UI

1. Run the API project in Visual Studio.
2. Confirm the API's configured HTTPS or HTTP URL.
3. Open the Swagger UI page if it has been configured.
4. Expand an endpoint.
5. Select Try it out.
6. Enter parameters or request JSON.
7. Select Execute.
8. Inspect the response code, headers, and body.

Important: Keep development documentation and test interfaces appropriately restricted in production. Swagger UI is a testing and documentation tool, not an authentication system.

## 14. Postman API Testing

Postman is an API client used to send HTTP requests and inspect responses.

### 14.1 Create a GET request

1. Run your API in Visual Studio.
2. Copy the base URL, for example `https://localhost:7001`.
3. Open Postman and create a new HTTP request.
4. Choose `GET`.
5. Enter `https://localhost:7001/api/employees`.
6. Click Send.
7. Inspect the status code and response body.

Use the actual port displayed by your application; the example port may differ.

### 14.2 Create a POST request

Select `POST` and enter:

```
https://localhost:7001/api/employees
```

Choose Body → raw → JSON and send:

```
{
  "name": "Priya",
  "email": "priya@example.com",
  "department": "HR"
}
```

If the endpoint accepts this DTO and creates the employee, a suitable response is `201 Created`.

### 14.3 Test other methods

| Method | URL                | What to test                         |
| ------ | ------------------ | ------------------------------------ |
| GET    | `/api/employees`   | Retrieve the list                    |
| GET    | `/api/employees/1` | Retrieve a single employee           |
| POST   | `/api/employees`   | Create an employee                   |
| PUT    | `/api/employees/1` | Replace editable employee data       |
| PATCH  | `/api/employees/1` | Change selected fields, if supported |
| DELETE | `/api/employees/1` | Delete an employee                   |

The API must implement each endpoint before Postman can test it successfully.

### 14.4 Useful Postman checks

- Test valid and invalid input.
- Check expected status codes.
- Check missing required fields.
- Check nonexistent resource IDs.
- Verify content types.
- Check authorization on protected endpoints.
- Test duplicate data and conflict scenarios.

Postman sends requests to the API. It does not create missing backend functionality automatically.

## 15. Content Types and Request Validation

### 15.1 What is `Content-Type`?

The `Content-Type` header tells the server the format of the request body.

Example:

```
Content-Type: application/json
```

Common content types include:

| Content type                        | Use                                 |
| ----------------------------------- | ----------------------------------- |
| `application/json`                  | JSON request data                   |
| `application/x-www-form-urlencoded` | URL-encoded form fields             |
| `multipart/form-data`               | Forms with files or mixed form data |
| `text/plain`                        | Plain text                          |

`Accept` is different: it indicates which response formats the client can accept.

### 15.2 What is request validation?

Request validation checks whether incoming data meets the API's rules before the application processes it.

Examples:

- Name is required.
- Email must be valid.
- Department must not exceed a maximum length.
- Age must fall within an allowed range.
- Numeric IDs must be valid.

### 15.3 Validation with Data Annotations

```
using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.DTOs;

public class CreateEmployeeDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Department { get; set; } = string.Empty;
}
```

With `[ApiController]`, invalid model state commonly triggers an automatic `400 Bad Request` response before the action executes.

### 15.4 Additional validation example

For a product DTO:

```
using System.ComponentModel.DataAnnotations;

public class CreateProductDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 100000)]
    public decimal Price { get; set; }
}
```

This requires a name between 2 and 100 characters and a price between `0.01` and `100000`.

Important: Validation attributes check basic input rules. Business rules—such as whether a department exists or an email is already registered—usually require additional application logic. Database constraints should also protect important data integrity rules.

# 16. Practical: Build an Employee CRUD Web API

Now combine the topics into a small working project.

## Step 1: Create the project in Visual Studio

1. Open Visual Studio.
2. Select Create a new project.
3. Search for ASP.NET Core Web API.
4. Select the template and click Next.
5. Set the project name to `EmployeeApi`.
6. Choose the location and click Next.
7. Select .NET 10.0 if installed.
8. Enable controller support if the template offers the option.
9. Configure OpenAPI support according to the selected template.
10. Click Create.

If .NET 10 is not listed, install the appropriate .NET 10 SDK and a Visual Studio version that supports it.

## Step 2: Create the DTOs

Add a folder named `DTOs`.

Create `CreateEmployeeDto.cs`:

```
using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.DTOs;

public class CreateEmployeeDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Department { get; set; } = string.Empty;
}
```

Create `UpdateEmployeeDto.cs`:

```
using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.DTOs;

public class UpdateEmployeeDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Department { get; set; } = string.Empty;
}
```

Create `EmployeeResponseDto.cs`:

```
namespace EmployeeApi.DTOs;

public class EmployeeResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;
}
```

## Step 3: Create the controller

Create `Controllers/EmployeesController.cs`:

```
using EmployeeApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private static readonly List<EmployeeResponseDto> Employees =
    [
        new EmployeeResponseDto
        {
            Id = 1,
            Name = "Arun",
            Email = "arun@example.com",
            Department = "IT"
        },
        new EmployeeResponseDto
        {
            Id = 2,
            Name = "Priya",
            Email = "priya@example.com",
            Department = "HR"
        }
    ];

    private static int _nextId = 3;

    // GET: api/employees
    [HttpGet]
    public ActionResult<IEnumerable<EmployeeResponseDto>> GetAll(
        [FromQuery] string? department)
    {
        IEnumerable<EmployeeResponseDto> result = Employees;

        if (!string.IsNullOrWhiteSpace(department))
        {
            result = result.Where(e =>
                e.Department.Equals(
                    department,
                    StringComparison.OrdinalIgnoreCase));
        }

        return Ok(result);
    }

    // GET: api/employees/1
    [HttpGet("{id:int}")]
    public ActionResult<EmployeeResponseDto> GetById(int id)
    {
        var employee = Employees.FirstOrDefault(e => e.Id == id);

        if (employee is null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    // POST: api/employees
    [HttpPost]
    public ActionResult<EmployeeResponseDto> Create(
        [FromBody] CreateEmployeeDto dto)
    {
        bool emailExists = Employees.Any(e =>
            e.Email.Equals(
                dto.Email,
                StringComparison.OrdinalIgnoreCase));

        if (emailExists)
        {
            return Conflict("An employee with this email already exists.");
        }

        var employee = new EmployeeResponseDto
        {
            Id = _nextId++,
            Name = dto.Name,
            Email = dto.Email,
            Department = dto.Department
        };

        Employees.Add(employee);

        return CreatedAtAction(
            nameof(GetById),
            new { id = employee.Id },
            employee);
    }

    // PUT: api/employees/1
    [HttpPut("{id:int}")]
    public IActionResult Update(
        int id,
        [FromBody] UpdateEmployeeDto dto)
    {
        var employee = Employees.FirstOrDefault(e => e.Id == id);

        if (employee is null)
        {
            return NotFound();
        }

        bool emailExists = Employees.Any(e =>
            e.Id != id &&
            e.Email.Equals(
                dto.Email,
                StringComparison.OrdinalIgnoreCase));

        if (emailExists)
        {
            return Conflict("Another employee already uses this email.");
        }

        employee.Name = dto.Name;
        employee.Email = dto.Email;
        employee.Department = dto.Department;

        return NoContent();
    }

    // DELETE: api/employees/1
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var employee = Employees.FirstOrDefault(e => e.Id == id);

        if (employee is null)
        {
            return NotFound();
        }

        Employees.Remove(employee);

        return NoContent();
    }
}
```

This project uses in-memory data for learning purposes. It is not connected to SQL Server, and the data resets when the application restarts. A production application should use an appropriate persistent data store and a service layer.

## Step 4: Test the endpoints

Run the project in Visual Studio. Use the actual base URL displayed in the launch output.

## API request builder

Prepare a request to copy into Postman. This does not send a request to your local API.

Base URL

HTTP method

GET

POST

PUT

DELETE

Path

Request URL

https\://localhost:7001/api/employees

&#x20; Copy request details

Try these requests:

| Request                                      | Expected result         |
| -------------------------------------------- | ----------------------- |
| `GET /api/employees`                         | `200 OK`, employee list |
| `GET /api/employees?department=IT`           | `200 OK`, filtered list |
| `GET /api/employees/1`                       | `200 OK`, Arun          |
| `GET /api/employees/999`                     | `404 Not Found`         |
| `POST /api/employees` with valid new data    | `201 Created`           |
| `POST /api/employees` with an existing email | `409 Conflict`          |
| `POST /api/employees` with invalid email     | `400 Bad Request`       |
| `PUT /api/employees/1` with valid data       | `204 No Content`        |
| `DELETE /api/employees/1`                    | `204 No Content`        |

For the invalid-email test, use an email value such as `"not-an-email"`. The `[ApiController]` behavior and data annotations should trigger a validation response.

## 17. Common mistakes to teach students

1. Using the wrong HTTP method: POST creates resources; GET retrieves them.
2. Returning `200 OK` for every scenario: Choose meaningful status codes.
3. Using database entities directly as public request models: DTOs provide better control over the API contract.
4. Forgetting `[ApiController]`: Without it, automatic API-specific validation behavior may differ.
5. Confusing route and query parameters: `/employees/5` is a path parameter; `/employees?page=2` uses a query parameter.
6. Confusing `Content-Type` and `Accept`: One describes the request body; the other indicates acceptable response formats.
7. Assuming in-memory data persists: It does not survive an application restart.
8. Assuming Swagger is always available: OpenAPI document generation and Swagger UI are related but distinct.
9. Returning sensitive exception details: Use appropriate exception-handling middleware and safe production error responses.
10. Trusting incoming data without validation: Validate input and enforce business rules server-side.

## 18. Suggested class schedule

| Session | Topics                                         | Practical                      |
| ------- | ---------------------------------------------- | ------------------------------ |
| 1       | API, REST, HTTP request/response               | Inspect an HTTP request        |
| 2       | HTTP methods and status codes                  | Design CRUD endpoint routes    |
| 3       | Controllers, actions, routing                  | Build GET endpoints            |
| 4       | Route/query parameters, headers, model binding | Search and filter employees    |
| 5       | DTOs and JSON                                  | Create and return DTOs         |
| 6       | `IActionResult`, `ActionResult<T>`, validation | Add POST, PUT and DELETE       |
| 7       | OpenAPI and Swagger UI                         | Explore documented endpoints   |
| 8       | Postman and content types                      | Test success and failure cases |

## 19. Interview and classroom questions

Use these questions to assess understanding.

1\. What is the difference between an API and REST?

2\. What is the difference between PUT and PATCH?

3\. What is model binding?

4\. Why use DTOs?

5\. When should an API return 201 Created?

6\. What is the difference between 401 and 403?

7\. What is the difference between route and query parameters?

8\. What is the difference between IActionResult and ActionResult\<T>?

9\. What is OpenAPI?

10\. What is the difference between Content-Type and Accept?

11\. What does [ApiController] provide?

12\. Why is the in-memory employee list unsuitable for production?

## Official learning resources

- [Microsoft Learn — Create web APIs with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
- [Microsoft Learn — Model binding in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding)
- [Microsoft Learn — Model validation in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation)
- [Microsoft Learn — OpenAPI support in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/overview)
- [Postman Learning Center](https://learning.postman.com/docs/introduction/overview/)

