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


