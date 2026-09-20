# 🗓️ WEEK 4 — SECURITY + AZURE + REDIS + DEVOPS

# Day 24 — Microsoft Entra ID

Learn:

**Microsoft Entra ID (formerly Azure Active Directory)**

Understand:

* Tenant
* User
* Group
* App Registration
* Client ID
* Tenant ID
* Client Secret
* Redirect URI
* Permissions
* Scopes
* Roles

Understand:

```text
Authentication
vs
Authorization
```

---

# Day 25 — MSAL Authentication

Learn:

**Microsoft Authentication Library (MSAL)**

Understand:

* Access tokens
* ID tokens
* Refresh tokens
* OAuth 2.0
* OpenID Connect
* JWT

Learn:

```text
Client
 ↓
Microsoft Entra ID
 ↓
Access Token
 ↓
ASP.NET Core API
 ↓
JWT validation
```

Implement:

```csharp
[Authorize]
```

Learn:

* Authentication middleware
* JWT Bearer
* Claims
* Roles
* Policies
* `[AllowAnonymous]`

---

# Day 26 — Managed Identity + Azure Security

Learn:

## Managed Identity

Understand:

* System-assigned identity
* User-assigned identity
* Why managed identity is used
* Azure resource authentication
* Passwordless authentication

Understand architecture:

```text
ASP.NET Core API
       ↓
Managed Identity
       ↓
Azure Resource
```

Learn how applications can authenticate to Azure services without storing credentials in source code.

Also learn:

* Key Vault concept
* Secrets
* Environment variables
* Secure configuration

---

# Day 27 — Redis

Learn Redis fundamentals.

Understand:

* What is Redis?
* In-memory database
* Key-value storage
* Cache
* TTL
* Expiration

Commands/concepts:

```text
SET
GET
DEL
EXPIRE
TTL
```

Learn caching patterns:

```text
API
 ↓
Redis Cache
 ↓
SQL Server
```

Understand:

### Cache-aside pattern

```text
Request
 ↓
Check Redis
 ↓
Found? → Return
 ↓
Not found
 ↓
SQL Server
 ↓
Store in Redis
 ↓
Return
```

Implement Redis caching in ASP.NET Core.

---

# Day 28 — Azure DevOps Basics

Learn:

## Azure DevOps

Understand:

* Organization
* Project
* Repository
* Boards
* Pipelines
* Artifacts
* Test Plans

## Git

Learn:

```bash
git init
git clone
git status
git add
git commit
git push
git pull
git branch
git checkout
git switch
git merge
git rebase
```

Understand:

```text
Working Directory
 ↓
Staging
 ↓
Local Repository
 ↓
Remote Repository
```

---

# Day 29 — CI/CD

Build a basic pipeline.

Understand:

```text
Developer
   ↓
Git
   ↓
Azure DevOps
   ↓
Build
   ↓
Test
   ↓
Publish
   ↓
Deploy
```

Learn:

* YAML pipelines
* Build pipeline
* Release/deployment
* Environment
* Variables
* Variable groups
* Secrets
* Artifacts
* Deployment stages

Create a basic ASP.NET Core API CI/CD pipeline.

---

# Day 30 — API Debugging + Troubleshooting + Final Project

This day brings everything together.

## API Debugging

Learn:

* Swagger
* Postman
* Browser DevTools
* Visual Studio debugger
* Breakpoints
* Watch
* Locals
* Call stack
* Exception details

---

## HTTP Troubleshooting

Understand:

```text
400 → Request problem
401 → Authentication problem
403 → Authorization problem
404 → Route/resource problem
409 → Conflict
500 → Server problem
502 → Gateway problem
503 → Service unavailable
```

---

## Common .NET Problems

Learn how to troubleshoot:

### API not starting

Check:

```text
Program.cs
Port
Configuration
Dependencies
Logs
```

### Database connection failure

Check:

```text
Connection string
SQL Server
Credentials
Firewall
Database name
```

### 401

Check:

```text
Token
Issuer
Audience
Expiration
Authentication configuration
```

### 403

Check:

```text
Role
Claim
Policy
Authorization
```

### 500

Check:

```text
Exception
Stack trace
Logs
Middleware
Database
Service layer
```

---

# 🔥 FINAL PROJECT — Full .NET 8 Enterprise API

For the final 30-day project, don't build another simple CRUD application.

Build:

# Employee Management & Reporting API

### Architecture

```text
                   ┌──────────────────┐
                   │     Client       │
                   │ Swagger/Postman  │
                   └────────┬─────────┘
                            │
                            ▼
                   ┌──────────────────┐
                   │ ASP.NET Core API │
                   └────────┬─────────┘
                            │
                   ┌────────▼─────────┐
                   │    Middleware    │
                   │ Logging / Errors │
                   └────────┬─────────┘
                            │
                   ┌────────▼─────────┐
                   │   Controllers    │
                   └────────┬─────────┘
                            │
                   ┌────────▼─────────┐
                   │ Service / BAL    │
                   └────────┬─────────┘
                            │
                   ┌────────▼─────────┐
                   │ Repository / DAL │
                   └──────┬─────┬─────┘
                          │     │
                 ┌────────▼─┐ ┌─▼──────────┐
                 │SQL Server│ │   Redis    │
                 └──────────┘ └────────────┘

                          │
                          ▼
                 ┌────────────────┐
                 │ ADOMD Client   │
                 └───────┬────────┘
                         ▼
                 ┌────────────────┐
                 │Analysis Service│
                 │     / MDX      │
                 └────────────────┘
```

### Features

Your project should contain:

**Employee**

```text
Create
Read
Update
Delete
Search
Filter
Sort
Pagination
```

**Department**

```text
Create
Read
Update
Delete
```

**Authentication**

```text
Microsoft Entra ID
MSAL
JWT
Roles
Claims
Authorization
```

**Database**

```text
SQL Server
Stored Procedures
Joins
CTEs
UNION
Indexes
Transactions
```

**Data Access**

```text
ADO.NET
DAL
BAL
Repository
```

**Analytics**

```text
ADOMD Client
MDX
Analysis Services
```

**Performance**

```text
Redis
Caching
Async/Await
Pagination
```

**API Infrastructure**

```text
Middleware
Global Exception Handling
Logging
Configuration
appsettings.json
DTOs
Validation
JSON Serialization
```

**DevOps**

```text
Git
Azure DevOps
CI/CD
YAML
Build
Test
Deploy
```

---


