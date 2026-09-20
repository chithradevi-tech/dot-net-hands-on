# Day 24 — Microsoft Entra ID

```text
Day 13 → Dependency Injection
Day 14 → Middleware
Day 20 → Data Access
Day 21 → Layered Architecture
Day 23 → MDX + ADOMD
```

Now we add authentication and authorization using **Microsoft Entra ID**.

> **Microsoft Entra ID** is the current name for **Azure Active Directory (Azure AD)**.

The core architecture you should understand today is:

```text
Client
   ↓
Microsoft Entra ID
   ↓
Authentication
   ↓
Access Token
   ↓
ASP.NET Core API
   ↓
Authorization
   ↓
Controller
```

---

# 1. What is Microsoft Entra ID?

**Microsoft Entra ID** is Microsoft's cloud-based identity and access management service.

It can manage:

* Users
* Groups
* Applications
* Authentication
* Permissions
* Roles
* Access tokens
* Identity policies

For example, suppose your company has:

```text
Employee Management System
```

Users might be:

```text
Admin
Manager
Developer
HR
Employee
```

Entra ID can authenticate these users and provide identity information to applications.

---

# 2. Authentication vs Authorization

This is the **most important concept** of Day 24.

## Authentication

Authentication answers:

> **Who are you?**

Example:

```text
Username + Password
        ↓
Microsoft Entra ID
        ↓
Identity verified
```

Result:

```text
User = Arun
```

---

## Authorization

Authorization answers:

> **What are you allowed to do?**

For example:

```text
Arun
 ↓
Authenticated
 ↓
Role = Employee
 ↓
Can view own profile
 ↓
Cannot delete employees
```

Another user:

```text
Priya
 ↓
Authenticated
 ↓
Role = Admin
 ↓
Can create
Can update
Can delete
Can view
```

---

# 3. Easy Memory

Remember:

```text
Authentication
     ↓
WHO are you?

Authorization
     ↓
WHAT can you do?
```

Or:

```text
Authentication → Identity
Authorization  → Permission
```

---

# 4. Real-World Example

Suppose you open:

```text
https://employee-management-api.com
```

You try:

```text
GET /api/employees
```

The API checks:

```text
Do you have a valid access token?
```

If no:

```text
401 Unauthorized
```

If yes:

```text
Who is this user?
```

Then authorization checks:

```text
Does this user have permission?
```

If no:

```text
403 Forbidden
```

If yes:

```text
200 OK
```

---

# 5. 401 vs 403

Very important interview question.

### 401 Unauthorized

Means:

> Authentication is missing or invalid.

Examples:

```text
No token
Expired token
Invalid token
```

Think:

```text
401 → Who are you?
```

---

### 403 Forbidden

Means:

> You are authenticated, but you don't have permission.

Example:

```text
User = Employee
Required role = Admin
```

Token is valid, but access is denied.

Think:

```text
403 → I know who you are,
      but you can't do this.
```

---

# 6. Microsoft Entra ID Architecture

Basic architecture:

```text
                    Microsoft Entra ID
                           │
             ┌─────────────┼─────────────┐
             ↓             ↓             ↓
           Users         Groups       Applications
                                           │
                                           ↓
                                    App Registration
                                           │
                                           ↓
                                      Client ID
                                      Tenant ID
                                      Secrets
                                      Redirect URI
                                      Permissions
```

---

# 7. What is a Tenant?

A **tenant** is an isolated Microsoft Entra ID directory representing an organization or identity environment.

For example:

```text
Company
   ↓
Contoso
   ↓
Entra ID Tenant
   ├── Users
   ├── Groups
   ├── Applications
   └── Policies
```

A tenant has a unique:

```text
Tenant ID
```

which is a GUID.

Example format:

```text
12345678-abcd-1234-abcd-123456789abc
```

---

# 8. Tenant ID

**Tenant ID** identifies the Entra ID directory.

Example:

```text
Tenant ID:
12345678-abcd-1234-abcd-123456789abc
```

Think:

```text
Tenant ID
    ↓
Which Entra directory?
```

Your application uses tenant information to know which identity authority it should trust.

---

# 9. User

A user represents an identity in Entra ID.

Example:

```text
Arun
arun@company.com
```

The user can authenticate and receive tokens for applications they are allowed to access.

A user can belong to:

```text
Groups
Roles
Applications
```

---

# 10. Group

A group allows multiple users to be managed together.

Example:

```text
IT-Developers
    │
    ├── Arun
    ├── Priya
    ├── Kumar
    └── Divya
```

Instead of assigning something individually to every user, you can often manage access through groups.

Examples:

```text
HR-Team
IT-Team
Managers
Developers
Finance-Team
```

---

# 11. What is App Registration?

An **App Registration** represents an application in Microsoft Entra ID.

Suppose you create:

```text
Employee Management API
```

You can register the application in Entra ID.

The registration provides identity information and configuration for the application.

Conceptually:

```text
App Registration
       │
       ├── Client ID
       ├── Tenant ID
       ├── Redirect URIs
       ├── API permissions
       ├── Exposed scopes
       └── Certificates / secrets
```

---

# 12. Why App Registration?

Suppose you have:

```text
Angular Application
        ↓
ASP.NET Core API
        ↓
SQL Server
```

The Angular application needs to authenticate users.

The API needs to validate access tokens.

Entra ID acts as the identity provider:

```text
Angular
   ↓
Entra ID
   ↓
Token
   ↓
ASP.NET Core API
```

---

# 13. Client ID

When an application is registered, Entra ID gives it an identifier called:

```text
Application / Client ID
```

Example:

```text
Client ID:
aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee
```

Think:

```text
Client ID
    ↓
Which application is this?
```

---

# 14. Client ID vs Tenant ID

Very important:

```text
Client ID
    ↓
Identifies the application

Tenant ID
    ↓
Identifies the Entra directory
```

Memory:

```text
Client → Application
Tenant → Organization/Directory
```

---

# 15. Client Secret

A **client secret** is a credential that an application can use to authenticate itself when obtaining tokens in flows where a confidential client credential is appropriate.

Think of it as:

```text
Application
     +
Client Secret
     ↓
Application authentication
```

Example:

```text
Client ID:
aaaa-bbbb-cccc

Client Secret:
<secret value>
```

### Important security rule

**Never put a client secret in:**

```text
GitHub
Source code
Angular code
Public JavaScript
appsettings.json committed to Git
```

Instead use secure secret management such as:

```text
Azure Key Vault
Environment Variables
User Secrets (local development)
Managed Identity
```

---

# 16. Client Secret vs User Password

These are different.

### User password

Used to authenticate:

```text
User
```

### Client secret

Used by an application:

```text
Application
```

So:

```text
User
 ↓
User authentication

Application
 ↓
Client credentials
```

---

# 17. Redirect URI

A **Redirect URI** tells Entra ID where to send the browser after authentication.

Example:

```text
https://localhost:4200/auth/callback
```

Flow:

```text
Browser
   ↓
Entra ID
   ↓
User signs in
   ↓
Redirect URI
   ↓
Application
```

The redirect URI must be configured correctly in the app registration.

---

# 18. Why Redirect URI Matters

Suppose your Angular application runs at:

```text
https://localhost:4200
```

and your configured callback is:

```text
https://localhost:4200/auth/callback
```

After authentication:

```text
Entra ID
   ↓
https://localhost:4200/auth/callback
```

If the requested redirect URI doesn't match an allowed configuration, authentication can fail.

---

# 19. Permissions

Applications often need permission to access resources.

For example:

```text
Angular App
      ↓
Microsoft Graph
```

The application might request permissions to access specific Graph data.

Another example:

```text
Client
   ↓
Employee API
```

The client requests access to the API through configured permissions/scopes.

---

# 20. Scope

A **scope** represents a delegated permission that a client can request from a resource/API.

Imagine your Employee API exposes:

```text
Employee.Read
Employee.Write
```

These represent permissions the API makes available.

A client could request:

```text
api://<api-client-id>/Employee.Read
```

Conceptually:

```text
Client
   ↓
Request Employee.Read
   ↓
Entra ID
   ↓
Access token
   ↓
Employee API
```

The API then validates that the token is intended for it and contains the required permission.

---

# 21. Scope vs Role

This is another important concept.

### Scope

Usually represents **delegated access**:

```text
Application acting on behalf of a user
```

Example:

```text
Employee.Read
Employee.Write
```

### Role

Represents an application-defined role or permission level.

Example:

```text
Admin
Manager
Employee
```

Memory:

```text
Scope → Permission to access something

Role  → Permission level / responsibility
```

The exact token claim used can differ depending on the OAuth flow and API design. For APIs, delegated scopes are commonly represented in the `scp` claim, while application roles are commonly represented in the `roles` claim.

---

# 22. Application Roles

Suppose your Employee API defines:

```text
Employee.Admin
Employee.Manager
Employee.User
```

Then users/applications can be assigned appropriate roles.

Example:

```text
Arun
 ↓
Employee.User

Priya
 ↓
Employee.Manager

Admin Account
 ↓
Employee.Admin
```

The API can then enforce authorization based on roles.

---

# 23. Scope vs Role — Practical Example

Suppose Angular calls:

```text
Employee API
```

The application needs:

```text
Employee.Read
```

That's a scope.

But inside the API:

```text
DELETE /api/employees/10
```

might require:

```text
Employee.Admin
```

That's a role.

So:

```text
Scope
 ↓
Can this client/user access this API capability?

Role
 ↓
What level of access does the identity have?
```

---

# 24. Access Token

After authentication/authorization, the client may receive an **access token**.

Conceptually:

```text
User
 ↓
Login
 ↓
Entra ID
 ↓
Access Token
 ↓
ASP.NET Core API
```

The client sends:

```http
Authorization: Bearer <access-token>
```

Example:

```text
GET /api/employees

Authorization:
Bearer eyJ...
```

The API validates the token.

---

# 25. What's Inside an Access Token?

An access token can contain claims about the token and its subject, depending on configuration.

Common claims include:

```text
iss  → issuer
aud  → audience
tid  → tenant ID
oid  → object ID
scp  → delegated scopes
roles → application roles
exp  → expiration
```

Important:

Don't assume every token contains every claim.

The exact claims depend on:

* Token type
* Flow
* Resource/API
* Configuration

---

# 26. Issuer

`iss` identifies who issued the token.

Conceptually:

```text
iss
 ↓
Microsoft Entra ID
```

The API uses the issuer configuration when validating the token.

---

# 27. Audience

`aud` identifies the resource/API the token is intended for.

This is extremely important.

Suppose:

```text
Token audience
     ↓
Employee API
```

The Employee API should reject a token intended for another resource.

Think:

```text
aud
 ↓
Who is this token meant for?
```

---

# 28. Token Validation

When a request reaches your API:

```text
HTTP Request
     ↓
Authorization: Bearer token
     ↓
JWT Bearer Authentication
     ↓
Validate token
     ├── Signature
     ├── Issuer
     ├── Audience
     ├── Lifetime
     └── Other configured validation
     ↓
Authenticated user
```

Then authorization occurs.

---

# 29. Authentication vs Authorization Flow

Complete flow:

```text
                 USER
                   │
                   ▼
           ┌───────────────┐
           │ Microsoft     │
           │ Entra ID      │
           └───────┬───────┘
                   │
             Authentication
                   │
                   ▼
             Access Token
                   │
                   ▼
           ┌───────────────┐
           │ ASP.NET Core  │
           │ API           │
           └───────┬───────┘
                   │
              Authentication
                validation
                   │
                   ▼
              Authorization
                   │
          ┌────────┴────────┐
          │                 │
       Allowed           Denied
          │                 │
          ▼                 ▼
    Controller             403
```

---

# 30. Microsoft Entra ID with ASP.NET Core

For an ASP.NET Core Web API, a common architecture is:

```text
Angular
   │
   │ Access Token
   ▼
ASP.NET Core API
   │
   │ JWT Bearer Authentication
   ▼
Microsoft Entra ID
   │
   └── Token issuer
```

The API doesn't normally redirect an API client to a login page.

Instead, the client obtains a token and sends it to the API.

---

# 31. Create App Registration for API

In the Microsoft Entra admin center, conceptually:

```text
Microsoft Entra ID
      ↓
App registrations
      ↓
New registration
```

Example:

```text
Name:
EmployeeManagementAPI
```

Then you get:

```text
Application (Client) ID
Directory (Tenant) ID
```

---

# 32. Expose an API

For an API application, you can configure:

```text
Expose an API
```

You can define an Application ID URI, conceptually:

```text
api://<client-id>
```

Then define scopes such as:

```text
Employee.Read
Employee.Write
```

For example:

```text
api://<client-id>/Employee.Read
```

The exact identifier depends on your app registration.

---

# 33. API Permission Model

Imagine:

```text
EmployeeManagementAPI
│
├── Employee.Read
└── Employee.Write
```

Client:

```text
EmployeeManagementWeb
```

requests:

```text
Employee.Read
```

Then:

```text
Web Application
      ↓
Entra ID
      ↓
Access Token
      ↓
EmployeeManagementAPI
```

The API validates the token and required scope.

---

# 34. ASP.NET Core Configuration

Install the JWT bearer authentication package:

```text id="4y1i1m"
Microsoft.AspNetCore.Authentication.JwtBearer
```

Then configuration can look like:

```csharp id="9j5jzj"
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

The exact `Audience` value should match the audience configured for your API/token model.

---

# 35. appsettings.json

For local development, you might have configuration such as:

```json id="v8p3fs"
{
  "AzureAd": {
    "TenantId": "YOUR-TENANT-ID",
    "ClientId": "YOUR-API-CLIENT-ID"
  }
}
```

Do not put client secrets into source-controlled configuration.

For local development, use:

```text
User Secrets
```

and for deployed applications consider:

```text
Azure Key Vault
Managed Identity
Environment configuration
```

---

# 36. Authentication Middleware Order

This is important:

```csharp id="r4tsgl"
app.UseAuthentication();

app.UseAuthorization();
```

Authentication must run before authorization.

Think:

```text id="d4kgc8"
Request
 ↓
Authentication
 ↓
Who is the user?
 ↓
Authorization
 ↓
Is the user allowed?
 ↓
Controller
```

---

# 37. Protect a Controller

Use:

```csharp id="2m4gpi"
[Authorize]
```

Example:

```csharp id="2p8f0j"
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployees()
    {
        return Ok("Authenticated user can access this API.");
    }
}
```

Now the endpoint requires authentication.

---

# 38. Public vs Protected Endpoint

Public:

```csharp id="uqc3zj"
[AllowAnonymous]
[HttpGet("public")]
public IActionResult PublicEndpoint()
{
    return Ok("Anyone can access this.");
}
```

Protected:

```csharp id="4fl2z4"
[Authorize]
[HttpGet("private")]
public IActionResult PrivateEndpoint()
{
    return Ok("Authentication required.");
}
```

---

# 39. Role-Based Authorization

Suppose we have:

```text
Employee.Admin
```

We can use:

```csharp id="o3q5do"
[Authorize(Roles = "Employee.Admin")]
```

Example:

```csharp id="j3y2w4"
[Authorize(Roles = "Employee.Admin")]
[HttpDelete("{id:int}")]
public IActionResult DeleteEmployee(int id)
{
    return NoContent();
}
```

Now the user must have the required role for that endpoint.

---

# 40. Policy-Based Authorization

For more complex rules, use policies.

Example:

```csharp id="0j85w5"
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "EmployeeRead",
        policy =>
        {
            policy.RequireClaim(
                "scp",
                "Employee.Read");
        });
});
```

Then:

```csharp id="j1qg5x"
[Authorize(Policy = "EmployeeRead")]
[HttpGet]
public IActionResult GetEmployees()
{
    return Ok();
}
```

This demonstrates:

```text
Access Token
      ↓
scp claim
      ↓
Employee.Read
      ↓
Policy
      ↓
Controller
```

---

# 41. Claims

A **claim** is a piece of information about an identity or token.

For example:

```text id="c9i3ne"
Name
Email
Object ID
Tenant ID
Scope
Role
```

In ASP.NET Core:

```csharp id="3a7s4q"
User.Claims
```

can be used to inspect claims associated with the authenticated principal.

Example:

```csharp id="4y9z0v"
var userId =
    User.FindFirst("oid")?.Value;
```

Don't blindly assume a particular claim name for every scenario; verify the token and configuration for your application.

---

# 42. User vs Object ID

You may encounter:

```text
oid
```

This commonly identifies the object associated with the identity in the tenant.

For application design, don't confuse:

```text
Tenant ID
```

with:

```text
Object ID
```

or:

```text
Client ID
```

They serve different purposes.

---

# 43. The Main IDs to Remember

This is a common interview area.

### Tenant ID

```text
Identifies the Entra tenant/directory
```

### Client ID

```text
Identifies an application registration
```

### Object ID

```text
Identifies a specific directory object
```

### Client Secret

```text
Credential for a confidential application
```

Memory:

```text
Tenant ID  → Directory
Client ID  → Application
Object ID  → Directory object
Secret     → Application credential
```

---

# 44. Important Security Rule

Never expose:

```text
Client Secret
```

in:

```text
Angular
React
JavaScript
Mobile client source
GitHub
Public repository
```

A browser application is a public client and cannot safely keep a client secret.

---

# 45. Typical Enterprise Architecture

For your .NET learning roadmap, imagine:

```text
                    ┌─────────────────┐
                    │   Angular App   │
                    └────────┬────────┘
                             │
                             │ Login
                             ▼
                    ┌─────────────────┐
                    │ Microsoft       │
                    │ Entra ID        │
                    └────────┬────────┘
                             │
                       Access Token
                             │
                             ▼
                    ┌─────────────────┐
                    │ ASP.NET Core    │
                    │ Web API         │
                    └────────┬────────┘
                             │
                   Authentication
                             │
                    Authorization
                             │
                             ▼
                    ┌─────────────────┐
                    │ Service / BAL   │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ Repository/DAL  │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ SQL Server      │
                    └─────────────────┘
```

---

# 46. Where Entra ID Fits in Your Day 21 Architecture

Previously:

```text
Client
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

Now:

```text
Client
 ↓
Microsoft Entra ID
 ↓
Access Token
 ↓
ASP.NET Core Middleware
 ↓
Authentication
 ↓
Authorization
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

Security sits **before your controller logic**.

---

# 47. Authentication Flow Example

Suppose Arun accesses the application.

```text id="xjcw6j"
1. Arun opens Angular application
             ↓
2. Angular redirects/signs in through Entra ID
             ↓
3. Entra ID authenticates Arun
             ↓
4. Token is issued for the requested resource
             ↓
5. Angular sends access token to API
             ↓
6. API validates token
             ↓
7. User is authenticated
             ↓
8. Authorization checks permission
             ↓
9. Controller executes
```

---

# 48. If Token Is Missing

```text id="t6rjkh"
Client
 ↓
GET /api/employees
 ↓
No Authorization header
 ↓
Authentication fails
 ↓
401 Unauthorized
```

---

# 49. If Token Is Valid but Role Is Wrong

```text id="syjzgg"
Client
 ↓
Valid Access Token
 ↓
Authenticated
 ↓
Role = Employee
 ↓
Endpoint requires Admin
 ↓
403 Forbidden
```

---

# 50. Important Entra ID Terminology

| Term             | Meaning                                              |
| ---------------- | ---------------------------------------------------- |
| Tenant           | Entra directory                                      |
| Tenant ID        | Identifier of the tenant                             |
| User             | Identity                                             |
| Group            | Collection of users/service principals               |
| App Registration | Application identity/configuration                   |
| Client ID        | Application identifier                               |
| Client Secret    | Application credential                               |
| Redirect URI     | Authentication callback address                      |
| Permission       | Access granted/requested                             |
| Scope            | Delegated permission exposed/requested by a resource |
| Role             | Application-defined authorization role               |
| Claim            | Information in identity/token                        |
| Access Token     | Credential presented to an API                       |

---

# 51. Common Interview Questions

### Q1. What is Microsoft Entra ID?

Microsoft's cloud identity and access management service used for authentication, authorization, users, groups, applications and access control.

### Q2. What was Azure AD called before?

Microsoft Entra ID was formerly called **Azure Active Directory (Azure AD)**.

### Q3. Authentication vs Authorization?

```text
Authentication → Who are you?

Authorization → What are you allowed to do?
```

### Q4. What is a tenant?

An isolated Microsoft Entra directory containing identities, groups, applications and related configuration.

### Q5. What is Client ID?

An identifier for an application registration.

### Q6. What is Tenant ID?

An identifier for the Entra directory.

### Q7. What is a Client Secret?

A credential used by a confidential application to authenticate itself in supported authentication flows.

### Q8. What is Redirect URI?

The URI where an identity provider redirects the browser after an authentication flow.

### Q9. What is a scope?

A delegated permission exposed by a resource/API that a client can request.

### Q10. What is a role?

An application-defined authorization role that can be assigned to users or applications depending on the scenario.

### Q11. What is an access token?

A token presented to a protected resource/API to demonstrate authorized access.

### Q12. What is 401?

Authentication is missing or invalid.

### Q13. What is 403?

The caller is authenticated but doesn't have sufficient permission.

### Q14. Why shouldn't client secrets be stored in Angular?

Because browser/client-side code is accessible to users and cannot securely protect confidential credentials.

### Q15. What is `[Authorize]`?

An ASP.NET Core authorization attribute used to require authorization for an endpoint/controller.

---

# 52. Day 24 — Final Mental Model

Remember these four layers:

```text id="4obm3c"
IDENTITY
   ↓
Microsoft Entra ID

AUTHENTICATION
   ↓
Who are you?

TOKEN
   ↓
Access Token

AUTHORIZATION
   ↓
What can you do?
```

And the complete .NET flow:

```text id="qzllm9"
                ┌───────────────────┐
                │       User        │
                └─────────┬─────────┘
                          │
                          ▼
                ┌───────────────────┐
                │ Microsoft Entra   │
                │       ID          │
                └─────────┬─────────┘
                          │
                    Authentication
                          │
                          ▼
                    Access Token
                          │
                          ▼
                ┌───────────────────┐
                │  ASP.NET Core API │
                └─────────┬─────────┘
                          │
                    Authentication
                          │
                    Authorization
                          │
                          ▼
                ┌───────────────────┐
                │    Controller     │
                └─────────┬─────────┘
                          │
                          ▼
                     Service
                          │
                          ▼
                    Repository
                          │
                          ▼
                    SQL Server
```

### ⭐ Day 24 must-remember points

```text
Microsoft Entra ID
        ↓
Identity Provider

Tenant
        ↓
Directory

Tenant ID
        ↓
Identifies directory

App Registration
        ↓
Application identity/configuration

Client ID
        ↓
Identifies application

Client Secret
        ↓
Confidential application credential

Redirect URI
        ↓
Authentication callback

Scope
        ↓
Delegated permission

Role
        ↓
Authorization level

Access Token
        ↓
Credential sent to API

Authentication
        ↓
WHO?

Authorization
        ↓
WHAT?

401
        ↓
Authentication problem

403
        ↓
Authorization problem
```

The key connection to your previous days is:

```text
Day 21 Layered Architecture
            +
Day 24 Microsoft Entra ID
            ↓
Secure ASP.NET Core API
            ↓
Authentication Middleware
            ↓
Authorization
            ↓
Controller
            ↓
Service
            ↓
DAL
            ↓
SQL Server
```


