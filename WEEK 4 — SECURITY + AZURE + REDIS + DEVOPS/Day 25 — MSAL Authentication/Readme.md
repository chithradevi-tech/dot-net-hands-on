# Day 25 — MSAL Authentication

Today we connect **Microsoft Entra ID + MSAL + ASP.NET Core Web API**.

The most important concept is:

```text
Client Application
       ↓
Microsoft Entra ID
       ↓
Authentication
       ↓
Access Token (JWT)
       ↓
ASP.NET Core API
       ↓
JWT Bearer Validation
       ↓
Claims / Roles / Policies
       ↓
Controller
```

---

# 1. What is MSAL?

**MSAL = Microsoft Authentication Library**

MSAL is Microsoft's library for applications to:

* Sign users in
* Obtain access tokens
* Obtain ID tokens
* Refresh tokens when applicable
* Call protected APIs
* Work with Microsoft Entra ID

MSAL is available for several platforms, including:

* Angular / JavaScript
* .NET
* Android
* iOS
* Python
* Java
* etc.

### Important distinction

MSAL is the **client-side library** used to obtain tokens.

ASP.NET Core JWT Bearer authentication is used by the **API to validate access tokens**.

So:

```text
Angular
   ↓
MSAL
   ↓
Microsoft Entra ID
   ↓
Access Token
   ↓
ASP.NET Core API
   ↓
JWT Bearer
   ↓
Authorize
```

---

# 2. OAuth 2.0

**OAuth 2.0** is an authorization framework.

It allows an application to obtain permission to access a protected resource/API.

For example:

```text
Angular Application
       ↓
Requests permission
       ↓
Microsoft Entra ID
       ↓
Access Token
       ↓
Employee API
```

The important point:

> OAuth 2.0 primarily deals with **authorization**.

It answers:

> "Can this application access this resource?"

---

# 3. OpenID Connect

**OpenID Connect (OIDC)** is an authentication protocol built on top of OAuth 2.0.

It provides information about the authenticated user.

So remember:

```text
OAuth 2.0
    ↓
Authorization

OpenID Connect
    ↓
Authentication + Identity
```

Example:

```text
User
 ↓
Microsoft Entra ID
 ↓
Login
 ↓
ID Token
 ↓
Application knows the user's identity
```

---

# 4. Access Token

An **access token** is used to access a protected API/resource.

Example HTTP request:

```http
GET /api/employees
Authorization: Bearer eyJhbGciOi...
```

The API validates this token.

```text
Client
   ↓
Bearer Access Token
   ↓
ASP.NET Core API
   ↓
Validate Token
   ↓
Allow / Reject
```

### Important

Do not think:

> Access token = login information only

Think:

> **Access token = permission to access a particular protected resource/API.**

---

# 5. ID Token

An **ID token** is primarily used by the client application to know that authentication occurred and to obtain identity information about the signed-in user.

Conceptually:

```text
ID Token
   ↓
Who is the user?
```

It can contain claims such as:

```text
name
preferred_username
oid
tid
```

The exact claims depend on the Microsoft Entra configuration and token.

### Very important

Do **not** normally send an ID token to your API as the API authorization credential.

For an API:

```text
Access Token → API
```

not:

```text
ID Token → API
```

---

# 6. Access Token vs ID Token

| Access Token                                                                    | ID Token                                          |
| ------------------------------------------------------------------------------- | ------------------------------------------------- |
| Used to access API/resource                                                     | Represents authentication/identity                |
| Intended for resource server                                                    | Intended for client application                   |
| API validates it                                                                | Client uses identity information                  |
| Contains authorization-related information such as scopes/roles when configured | Contains identity claims                          |
| Sent to protected API                                                           | Normally not used as API authorization credential |

### Easy memory

```text
Access Token → ACCESS API

ID Token → IDENTITY / LOGIN
```

---

# 7. Refresh Token

A refresh token can be used by a client to obtain a new access token without requiring the user to authenticate interactively again, subject to the identity platform's rules and token flow.

Conceptually:

```text
Access Token expires
       ↓
Refresh Token
       ↓
Microsoft Entra ID
       ↓
New Access Token
```

### Important

Refresh-token handling depends on:

* Client type
* OAuth flow
* MSAL platform
* Browser/app environment
* Entra ID configuration

You should **not manually implement token-refresh logic blindly**. MSAL handles token acquisition/caching according to the platform.

---

# 8. JWT

Most access tokens used to call your ASP.NET Core API are JWTs when configured as JWT bearer tokens.

JWT = **JSON Web Token**

A JWT conceptually has three parts:

```text
Header.Payload.Signature
```

Example:

```text
xxxxx.yyyyy.zzzzz
```

---

## JWT Header

Contains metadata such as:

```json
{
  "alg": "RS256",
  "typ": "JWT",
  "kid": "..."
}
```

---

## JWT Payload

Contains claims.

Example conceptually:

```json
{
  "iss": "https://login.microsoftonline.com/...",
  "aud": "api-client-id",
  "tid": "tenant-id",
  "oid": "user-object-id",
  "scp": "Employee.Read",
  "exp": 1780000000
}
```

Don't memorize the exact claims yet.

Understand:

```text
JWT
 ├── Header
 ├── Payload → Claims
 └── Signature
```

---

# 9. JWT Claims

A **claim** is a piece of information about the token subject/context.

Common claims include:

### `iss`

Issuer.

```text
Who issued this token?
```

### `aud`

Audience.

```text
Which API/resource is this token intended for?
```

### `tid`

Tenant ID.

```text
Which Entra tenant?
```

### `oid`

Object ID of the identity.

### `exp`

Expiration time.

### `scp`

Scopes.

Example:

```text
Employee.Read Employee.Write
```

### `roles`

Application roles, when applicable.

Example:

```text
Employee.Admin
```

---

# 10. OAuth + OIDC + JWT Relationship

This is important for interviews.

```text
OAuth 2.0
    ↓
Authorization framework

OpenID Connect
    ↓
Authentication / identity layer

JWT
    ↓
Token format commonly used for tokens
```

They are **not the same thing**.

---

# 11. MSAL Authentication Flow

Let's imagine we have:

```text
Angular Employee Application
ASP.NET Core Employee API
Microsoft Entra ID
```

The flow is:

```text
        ┌─────────────────┐
        │ Angular Client  │
        └────────┬────────┘
                 │
                 │ Login
                 ↓
        ┌─────────────────┐
        │ Microsoft       │
        │ Entra ID        │
        └────────┬────────┘
                 │
                 │ Authentication
                 ↓
        ┌─────────────────┐
        │ Access Token    │
        └────────┬────────┘
                 │
                 │ Authorization:
                 │ Bearer <token>
                 ↓
        ┌─────────────────┐
        │ ASP.NET Core    │
        │ Web API         │
        └────────┬────────┘
                 │
                 ↓
        JWT Validation
                 │
        ┌────────┴────────┐
        │                 │
      Valid             Invalid
        │                 │
        ↓                 ↓
   Controller            401
```

---

# 12. ASP.NET Core JWT Bearer Authentication

Now let's implement this in your **.NET 8 Web API**.

Create a project in Visual Studio:

```text
Create a new project
        ↓
ASP.NET Core Web API
        ↓
Project Name:
Day25MsalAuthentication
        ↓
Framework:
.NET 8
```

---

# 13. Install JWT Bearer Package

In Visual Studio:

```text
Tools
 ↓
NuGet Package Manager
 ↓
Manage NuGet Packages
```

Install:

```text
Microsoft.AspNetCore.Authentication.JwtBearer
```

Or Package Manager Console:

```powershell
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
```

---

# 14. appsettings.json

Create configuration:

```json
{
  "AzureAd": {
    "TenantId": "YOUR-TENANT-ID",
    "ClientId": "YOUR-API-CLIENT-ID"
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

### Important

Don't put a client secret into source-controlled configuration.

For development, sensitive values can be stored using:

```text
User Secrets
```

or appropriate environment/secret-management services.

---

# 15. Configure Authentication

In `Program.cs`:

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority =
            $"https://login.microsoftonline.com/{builder.Configuration["AzureAd:TenantId"]}/v2.0";

        options.Audience =
            builder.Configuration["AzureAd:ClientId"];
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

---

# 16. Understand the Middleware

These two lines are extremely important:

```csharp
app.UseAuthentication();

app.UseAuthorization();
```

### Authentication

Determines:

> Who is the caller?

It validates the authentication credentials/token and establishes the user's identity.

### Authorization

Determines:

> Is this authenticated user allowed to perform this action?

So:

```text
Authentication
       ↓
WHO ARE YOU?

Authorization
       ↓
WHAT ARE YOU ALLOWED TO DO?
```

---

# 17. Middleware Order

Use:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

in this order.

Think:

```text
Request
  ↓
Authentication
  ↓
Authorization
  ↓
Controller
```

Authentication needs to establish the user before authorization evaluates access.

---

# 18. `[Authorize]`

Now create a controller:

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Day25MsalAuthentication.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployees()
    {
        return Ok(new
        {
            Message = "You are authenticated!",
            User = User.Identity?.Name
        });
    }
}
```

Now:

```http
GET /api/employees
```

requires authentication.

---

# 19. What happens without a token?

Request:

```http
GET /api/employees
```

without:

```http
Authorization: Bearer <access-token>
```

The API rejects the request.

Typically:

```text
401 Unauthorized
```

Flow:

```text
Request
  ↓
[Authorize]
  ↓
No valid authentication
  ↓
401
```

---

# 20. Request with Access Token

The client sends:

```http
GET /api/employees
Authorization: Bearer eyJhbGciOi...
```

Then:

```text
Request
 ↓
JWT Bearer Authentication
 ↓
Validate token
 ↓
Create ClaimsPrincipal
 ↓
Authorization
 ↓
[Authorize]
 ↓
Controller
```

---

# 21. `[AllowAnonymous]`

Sometimes a controller is protected:

```csharp
[Authorize]
public class EmployeesController : ControllerBase
{
}
```

But you want one endpoint to be public.

Use:

```csharp
[AllowAnonymous]
```

Example:

```csharp
[HttpGet("public")]
[AllowAnonymous]
public IActionResult PublicEndpoint()
{
    return Ok("Anyone can access this endpoint.");
}
```

So:

```text
[Authorize]
      ↓
Protected

[AllowAnonymous]
      ↓
Public
```

---

# 22. Controller-Level Authorization

You can protect the entire controller:

```csharp
[Authorize]
public class EmployeesController : ControllerBase
{
}
```

All actions require authentication unless overridden with `[AllowAnonymous]`.

---

# 23. Action-Level Authorization

You can also protect individual actions:

```csharp
public class EmployeesController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult GetEmployees()
    {
        return Ok();
    }

    [HttpGet("public")]
    [AllowAnonymous]
    public IActionResult Public()
    {
        return Ok();
    }
}
```

---

# 24. Claims

After successful authentication, ASP.NET Core creates a `ClaimsPrincipal`.

You can inspect claims:

```csharp
[Authorize]
[HttpGet("claims")]
public IActionResult GetClaims()
{
    var claims = User.Claims.Select(c => new
    {
        c.Type,
        c.Value
    });

    return Ok(claims);
}
```

This is very useful while debugging authentication.

---

# 25. Accessing Individual Claims

Example:

```csharp
var tenantId = User.FindFirst("tid")?.Value;

var objectId = User.FindFirst("oid")?.Value;

var userName = User.FindFirst("preferred_username")?.Value;
```

Example:

```csharp
[Authorize]
[HttpGet("me")]
public IActionResult GetCurrentUser()
{
    var tenantId = User.FindFirst("tid")?.Value;
    var objectId = User.FindFirst("oid")?.Value;

    return Ok(new
    {
        TenantId = tenantId,
        ObjectId = objectId
    });
}
```

The exact available claims depend on the token and Entra configuration.

---

# 26. Scopes

Suppose your API exposes:

```text
Employee.Read
Employee.Write
```

A token might contain:

```text
scp = Employee.Read
```

You can create a policy:

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployeeRead", policy =>
    {
        policy.RequireClaim("scp", "Employee.Read");
    });
});
```

Then:

```csharp
[Authorize(Policy = "EmployeeRead")]
[HttpGet]
public IActionResult GetEmployees()
{
    return Ok();
}
```

---

# 27. Important Scope Nuance

The `scp` claim can contain multiple space-separated scopes.

For example:

```text
Employee.Read Employee.Write
```

Therefore, for production applications, scope-based policies should account for the fact that multiple scopes may be present.

A more explicit custom requirement/handler can be used when you need precise scope checking.

For basic learning:

```text
scp → delegated permissions/scopes
```

is the key concept.

---

# 28. Roles

Roles are another authorization mechanism.

Suppose your application has:

```text
Employee.Admin
Employee.Manager
Employee.User
```

A token can contain:

```text
roles:
    Employee.Admin
```

Then:

```csharp
[Authorize(Roles = "Employee.Admin")]
[HttpDelete("{id}")]
public IActionResult DeleteEmployee(int id)
{
    return Ok($"Employee {id} deleted.");
}
```

Now only users with the required role can access the endpoint.

---

# 29. Roles vs Scopes

Remember:

```text
Scopes
 ↓
What permissions are being delegated?

Roles
 ↓
What application role does the identity have?
```

Typical examples:

```text
Employee.Read
Employee.Write
```

versus:

```text
Employee.Admin
Employee.Manager
Employee.User
```

---

# 30. Policies

Policies provide flexible authorization rules.

Instead of only:

```csharp
[Authorize]
```

you can define:

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole("Employee.Admin");
    });
});
```

Then:

```csharp
[Authorize(Policy = "AdminOnly")]
public IActionResult DeleteEmployee(int id)
{
    return Ok();
}
```

---

# 31. Policy with Multiple Requirements

A policy can combine requirements.

For example:

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployeeManager", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Employee.Manager");
    });
});
```

Then:

```csharp
[Authorize(Policy = "EmployeeManager")]
public IActionResult ManageEmployees()
{
    return Ok();
}
```

---

# 32. `[Authorize]` vs `[AllowAnonymous]`

### `[Authorize]`

Requires authentication.

```csharp
[Authorize]
public IActionResult GetEmployees()
{
    return Ok();
}
```

### `[AllowAnonymous]`

Allows unauthenticated access.

```csharp
[AllowAnonymous]
public IActionResult LoginInfo()
{
    return Ok();
}
```

---

# 33. Authentication vs Authorization

This is a very common interview question.

### Authentication

```text
WHO ARE YOU?
```

Example:

```text
Microsoft Entra ID
```

### Authorization

```text
WHAT CAN YOU DO?
```

Example:

```text
Employee.Admin
Employee.Read
```

Flow:

```text
User
 ↓
Authentication
 ↓
Identity established
 ↓
Authorization
 ↓
Permission checked
 ↓
Resource
```

---

# 34. 401 vs 403

Very important.

### 401 Unauthorized

Authentication failed or credentials are missing/invalid.

```text
No valid access token
       ↓
401
```

### 403 Forbidden

The caller is authenticated but isn't authorized for that resource.

```text
Valid token
    ↓
Authenticated
    ↓
Insufficient permission
    ↓
403
```

Memory trick:

```text
401 → "Who are you?"
403 → "I know who you are, but you can't do this."
```

---

# 35. Complete ASP.NET Core Flow

Your Day 25 architecture should look like this:

```text
┌───────────────────────┐
│ Angular / Client      │
└───────────┬───────────┘
            │
            │ MSAL
            ↓
┌───────────────────────┐
│ Microsoft Entra ID    │
└───────────┬───────────┘
            │
            │ Access Token
            ↓
┌───────────────────────┐
│ ASP.NET Core API      │
└───────────┬───────────┘
            │
            ↓
┌───────────────────────┐
│ JWT Bearer            │
│ Authentication        │
└───────────┬───────────┘
            │
            ↓
       Token Valid?
       /         \
     No           Yes
     ↓             ↓
    401       ClaimsPrincipal
                    │
                    ↓
             Authorization
                    │
             ┌──────┴──────┐
             │             │
          Allowed        Denied
             │             │
             ↓             ↓
        Controller        403
             │
             ↓
          Service
             │
             ↓
         Repository
             │
             ↓
        SQL Server
```

---

# 36. MSAL's Role in This Architecture

Don't confuse these two:

### MSAL

Runs in the **client application**.

```text
Client
 ↓
MSAL
 ↓
Entra ID
 ↓
Token
```

### JWT Bearer

Runs in the **ASP.NET Core API**.

```text
Access Token
 ↓
JWT Bearer Authentication
 ↓
Validation
 ↓
User.Claims
 ↓
Authorization
```

---

# 37. Client → API Request

Once MSAL obtains the access token, the client sends:

```http
GET https://localhost:7001/api/employees
Authorization: Bearer <access-token>
```

The API:

```text
Receives token
      ↓
Reads JWT
      ↓
Validates signature
      ↓
Validates issuer
      ↓
Validates audience
      ↓
Checks expiration
      ↓
Creates authenticated identity
      ↓
Authorization
      ↓
Controller
```

---

# 38. What JWT Validation Checks

Conceptually, the API validates things such as:

### Signature

Was the token issued/signed by the trusted identity provider?

### Issuer

Is the token from the expected tenant/issuer?

### Audience

Is the token intended for **this API**?

### Expiration

Is the token still valid?

### Claims

Does the caller have the required scope/role/claim?

So:

```text
JWT
 ↓
Signature ✓
Issuer ✓
Audience ✓
Expiration ✓
Claims ✓
 ↓
Authenticated
```

---

# 39. Why Audience Is Important

Imagine:

```text
Token intended for API A
```

and the user sends it to:

```text
API B
```

API B should not simply accept it.

The `aud` claim helps ensure the token is intended for the correct resource.

This is one reason you must configure the API's authentication correctly.

---

# 40. Example Complete Controller

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Day25MsalAuthentication.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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

    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst("oid")?.Value;
        var tenantId = User.FindFirst("tid")?.Value;

        return Ok(new
        {
            UserId = userId,
            TenantId = tenantId
        });
    }

    [HttpGet("public")]
    [AllowAnonymous]
    public IActionResult PublicEndpoint()
    {
        return Ok("This endpoint does not require authentication.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Employee.Admin")]
    public IActionResult DeleteEmployee(int id)
    {
        return Ok($"Employee {id} deleted.");
    }
}
```

This single controller demonstrates:

```text
[Authorize]
[AllowAnonymous]
Claims
Roles
```

---

# 41. Project Structure

For your Day 25 Visual Studio project:

```text
Day25MsalAuthentication
│
├── Controllers
│   └── EmployeesController.cs
│
├── Properties
│
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
│
└── Day25MsalAuthentication.csproj
```

Later, when you combine this with your previous architecture:

```text
EmployeeManagement
│
├── Controllers
│
├── Services
│   ├── Interfaces
│   └── Implementations
│
├── DAL
│   ├── Interfaces
│   └── Implementations
│
├── DTOs
│
├── Models
│
├── Middleware
│
├── Authentication
│
└── Program.cs
```

---

# 42. MSAL vs JWT Bearer

A common interview question:

### MSAL

```text
Client-side token acquisition
```

### JWT Bearer

```text
API-side token validation
```

Example:

```text
Angular
  ↓
MSAL
  ↓
Entra ID
  ↓
Access Token
  ↓
ASP.NET Core
  ↓
JWT Bearer
```

---

# 43. Complete Day 25 Mental Model

Memorize this:

```text
MSAL
 ↓
Helps client authenticate and acquire tokens

Microsoft Entra ID
 ↓
Identity Provider

OAuth 2.0
 ↓
Authorization framework

OpenID Connect
 ↓
Authentication/identity protocol

Access Token
 ↓
Used to access API

ID Token
 ↓
Identity information for client

Refresh Token
 ↓
Used to obtain new access tokens when applicable

JWT
 ↓
Token format

JWT Bearer
 ↓
ASP.NET Core authentication handler

Claims
 ↓
Information about authenticated identity/token

Roles
 ↓
Application roles

Policies
 ↓
Flexible authorization rules

[Authorize]
 ↓
Protect endpoint

[AllowAnonymous]
 ↓
Allow unauthenticated access
```

---

# 44. Day 25 Interview Questions

### 1. What is MSAL?

Microsoft Authentication Library used by applications to authenticate users and acquire tokens from Microsoft Entra ID.

### 2. What is OAuth 2.0?

An authorization framework for obtaining access to protected resources.

### 3. What is OpenID Connect?

An authentication/identity protocol built on OAuth 2.0.

### 4. What is an access token?

A token used to access a protected resource/API.

### 5. What is an ID token?

A token representing authentication/identity information for the client application.

### 6. What is JWT?

JSON Web Token, a compact token format consisting conceptually of header, payload, and signature.

### 7. What is `[Authorize]`?

An ASP.NET Core authorization attribute used to protect controllers/actions.

### 8. What is `[AllowAnonymous]`?

Allows an endpoint to be accessed without authentication even when authorization is applied at a higher level.

### 9. What is JWT Bearer authentication?

An ASP.NET Core authentication scheme that extracts and validates bearer tokens, commonly JWT access tokens.

### 10. What is a claim?

A piece of information associated with an identity/token.

### 11. What is the difference between authentication and authorization?

```text
Authentication → Who are you?
Authorization → What can you do?
```

### 12. Difference between 401 and 403?

```text
401 → Authentication problem
403 → Authorization problem
```

### 13. What is `scp`?

A claim commonly used to represent delegated scopes in an access token.

### 14. What is `roles`?

A claim commonly used to represent application roles assigned to the identity.

### 15. Why do we use `UseAuthentication()`?

To authenticate the incoming request and establish the authenticated user.

### 16. Why do we use `UseAuthorization()`?

To evaluate whether the authenticated user is authorized to access the requested resource.

### 17. Why must authentication come before authorization?

Because authorization needs an authenticated identity/claims to evaluate access.

---

# Day 25 — Final Revision

```text
                    MSAL
                     ↓
             Microsoft Entra ID
                     ↓
              Authentication
                     ↓
               Access Token
                     ↓
              ASP.NET Core API
                     ↓
             JWT Bearer Handler
                     ↓
              Token Validation
                     ↓
              ClaimsPrincipal
                     ↓
               Authorization
                /          \
             Allowed       Denied
                ↓            ↓
          Controller         403
                ↓
             Service
                ↓
           Repository
                ↓
           SQL Server
```

### The 10 things you should be able to explain after Day 25

1. **MSAL**
2. **OAuth 2.0**
3. **OpenID Connect**
4. **Access Token**
5. **ID Token**
6. **Refresh Token**
7. **JWT**
8. **JWT Bearer authentication**
9. **Claims / Roles / Policies**
10. **`[Authorize]` / `[AllowAnonymous]`**

And the most important distinction:


