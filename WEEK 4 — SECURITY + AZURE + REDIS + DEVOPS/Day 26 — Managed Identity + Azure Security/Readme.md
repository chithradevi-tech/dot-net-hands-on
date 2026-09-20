# Day 26 — Managed Identity + Azure Security

Today we move from **user authentication** to **application-to-Azure-resource authentication**.

This is an important distinction from Day 25.

### Day 25

```text
User / Client
      ↓
Microsoft Entra ID
      ↓
Access Token
      ↓
ASP.NET Core API
```

### Day 26

```text
ASP.NET Core API
      ↓
Managed Identity
      ↓
Microsoft Entra ID
      ↓
Azure Resource
```

The main goal is:

> **Your application should access Azure resources without storing passwords, client secrets, or connection credentials in source code.**

---

# 1. What is Managed Identity?

**Managed Identity** is an Azure feature that gives an Azure resource an identity in **Microsoft Entra ID**.

That identity can then be granted permissions to other Azure resources.

For example:

```text
ASP.NET Core API
       ↓
Managed Identity
       ↓
Microsoft Entra ID
       ↓
Azure Key Vault
```

Your application doesn't need:

```text
Username
Password
Client Secret
Certificate
```

Instead, Azure manages the application's identity.

---

# 2. Why Do We Need Managed Identity?

Suppose your ASP.NET Core API needs to access Azure Key Vault.

A traditional approach might be:

```text
API
 ↓
Client ID
Client Secret
 ↓
Key Vault
```

You now have a secret that needs to be stored somewhere.

Problems:

* Secret can expire.
* Secret can be leaked.
* Secret needs rotation.
* Developers may accidentally commit it.
* Deployment configuration becomes more complicated.

With Managed Identity:

```text
API
 ↓
Managed Identity
 ↓
Key Vault
```

No application-managed password or client secret is required.

---

# 3. Passwordless Authentication

Managed Identity is commonly described as **passwordless authentication** for Azure resource-to-resource access.

Instead of:

```text
Application
   ↓
Username + Password
   ↓
Azure Resource
```

you have:

```text
Application
   ↓
Managed Identity
   ↓
Microsoft Entra ID
   ↓
Access Token
   ↓
Azure Resource
```

---

# 4. Basic Architecture

Your Day 26 architecture:

```text
┌─────────────────────────┐
│ ASP.NET Core API        │
│ Running in Azure        │
└────────────┬────────────┘
             │
             │ Managed Identity
             ↓
┌─────────────────────────┐
│ Microsoft Entra ID      │
└────────────┬────────────┘
             │
             │ Access Token
             ↓
┌─────────────────────────┐
│ Azure Resource          │
│                         │
│ Key Vault               │
│ Storage                  │
│ SQL Database             │
│ Service Bus              │
│ etc.                     │
└─────────────────────────┘
```

---

# 5. Two Types of Managed Identity

Azure provides two main types:

```text
Managed Identity
       │
       ├── System-assigned
       │
       └── User-assigned
```

---

# 6. System-Assigned Managed Identity

A **system-assigned identity** is tied directly to an Azure resource.

For example:

```text
App Service
     ↓
System-assigned identity
```

or:

```text
Azure VM
     ↓
System-assigned identity
```

### Important characteristics

The identity:

* Is created for that Azure resource.
* Has its own identity in Entra ID.
* Has the same lifecycle as the resource.
* Is automatically removed when the resource is deleted.

---

# 7. System-Assigned Example

Suppose you have:

```text
EmployeeManagement API
       ↓
Azure App Service
```

Enable:

```text
Identity
 ↓
System assigned
 ↓
On
```

Azure creates an identity for that App Service.

Then:

```text
App Service
     ↓
System Identity
     ↓
Microsoft Entra ID
```

You can grant this identity access to Key Vault.

---

# 8. User-Assigned Managed Identity

A **user-assigned identity** is created as a separate Azure resource.

For example:

```text
UserAssignedIdentity
       │
       ├──────── App Service
       │
       ├──────── Azure Function
       │
       └──────── VM
```

The identity can be reused by multiple Azure resources.

---

# 9. System vs User Assigned

| System-Assigned                     | User-Assigned                         |
| ----------------------------------- | ------------------------------------- |
| Created with Azure resource         | Created separately                    |
| Tied to one resource lifecycle      | Can be assigned to multiple resources |
| Deleted with resource               | Exists independently                  |
| Simple setup                        | More reusable                         |
| Good for resource-specific identity | Good for shared identity scenarios    |

### Easy memory

```text
System Assigned
      ↓
Identity belongs to resource

User Assigned
      ↓
Identity is separate and reusable
```

---

# 10. Real-World Example

Suppose your company has:

```text
Employee API
Payment API
Reporting API
```

All three need access to a particular Azure resource.

You could have:

```text
Employee API
     ↓
User-Assigned Identity
     ↑
Payment API
     ↑
Reporting API
```

The same managed identity can be assigned to multiple supported Azure resources.

However, shared identities should be used deliberately because all assigned resources receive the permissions granted to that identity.

---

# 11. Managed Identity Does Not Mean "Access to Everything"

This is extremely important.

Creating a Managed Identity does **not** automatically give it access to Azure resources.

You still need to assign permissions.

Example:

```text
ASP.NET Core API
      ↓
Managed Identity
      ↓
Key Vault
```

You need to grant the identity the appropriate Key Vault permissions.

Think:

```text
Identity
   +
Permission
   =
Access
```

---

# 12. Managed Identity + RBAC

Azure commonly uses **Role-Based Access Control (RBAC)** to control what an identity can do.

For example, you might assign a role that allows an identity to read secrets from a Key Vault.

Conceptually:

```text
Managed Identity
      ↓
Role Assignment
      ↓
Key Vault
      ↓
Read Secrets
```

Use the **least privilege** principle:

> Give an application only the permissions it actually needs.

For example, if an API only needs to read secrets, don't give it administrative access to the entire Key Vault.

---

# 13. Managed Identity Authentication Flow

Let's understand what happens behind the scenes.

```text
ASP.NET Core API
       ↓
Requests Azure credential
       ↓
Managed Identity
       ↓
Microsoft Entra ID
       ↓
Access Token
       ↓
Azure Resource
```

The application doesn't need to know the secret/password of the managed identity.

---

# 14. How Does the Application Get the Token?

In .NET applications, Azure SDK libraries commonly use:

```csharp
DefaultAzureCredential
```

from:

```text
Azure.Identity
```

Example:

```csharp
using Azure.Identity;

var credential = new DefaultAzureCredential();
```

This credential can use available authentication mechanisms depending on the environment.

For an Azure-hosted application with Managed Identity enabled, it can use the application's managed identity.

---

# 15. Install Azure.Identity

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
Azure.Identity
```

You can also use Package Manager Console:

```powershell
Install-Package Azure.Identity
```

---

# 16. DefaultAzureCredential

Example:

```csharp
using Azure.Identity;

var credential = new DefaultAzureCredential();
```

One reason this is convenient is that the same application code can work differently depending on the environment.

For example:

```text
Developer machine
       ↓
Developer authentication

Azure
       ↓
Managed Identity
```

This makes local development and Azure deployment easier to manage.

---

# 17. Local Development vs Azure

This is a very important practical concept.

### Local machine

Your laptop doesn't automatically have the same Managed Identity as an Azure App Service.

So during local development, `DefaultAzureCredential` can use developer authentication mechanisms available to you, such as an authenticated Azure CLI/Visual Studio environment, depending on your setup.

### Azure

After deploying to a supported Azure resource and enabling Managed Identity:

```text
ASP.NET Core API
       ↓
DefaultAzureCredential
       ↓
Managed Identity
       ↓
Azure Resource
```

This means the application code can remain the same while the credential source changes by environment.

---

# 18. Key Vault

Now let's understand **Azure Key Vault**.

Azure Key Vault is a service designed to securely store and manage sensitive information and cryptographic material.

Examples include:

```text
Secrets
Keys
Certificates
```

For today's topic, focus mainly on **secrets**.

---

# 19. What Is a Secret?

A secret is sensitive configuration information.

Examples:

```text
Database password
API key
Third-party service credential
Connection information
Client secret
```

Instead of:

```json
{
  "Password": "MySuperSecretPassword"
}
```

inside your application source code, you can store sensitive values in a secure secret store such as Key Vault.

---

# 20. Bad Approach

Never do this:

```csharp
string password = "MyPassword123";
```

Or:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Password=MyPassword123;"
  }
}
```

and commit it to Git.

Why?

```text
Source Code
     ↓
Git
     ↓
Repository
     ↓
Potential secret exposure
```

Even private repositories require careful secret handling.

---

# 21. Better Approach

Use:

```text
ASP.NET Core
      ↓
Managed Identity
      ↓
Azure Key Vault
      ↓
Secret
```

Now the application doesn't need a Key Vault password stored in source code.

---

# 22. Key Vault Architecture

```text
                 Azure
┌───────────────────────────────────┐
│                                   │
│  ASP.NET Core API                 │
│        │                          │
│        │ Managed Identity         │
│        ↓                          │
│  Microsoft Entra ID               │
│        │                          │
│        │ Access Token             │
│        ↓                          │
│  Azure Key Vault                  │
│        │                          │
│        ├── DatabasePassword       │
│        ├── ExternalApiKey         │
│        └── OtherSecret            │
│                                   │
└───────────────────────────────────┘
```

---

# 23. Using Key Vault from .NET

The Azure SDK provides:

```text
Azure.Security.KeyVault.Secrets
```

Install:

```powershell
Install-Package Azure.Security.KeyVault.Secrets
```

Then:

```csharp
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var client = new SecretClient(
    new Uri("https://my-key-vault.vault.azure.net/"),
    new DefaultAzureCredential());

KeyVaultSecret secret =
    await client.GetSecretAsync("DatabasePassword");

string password = secret.Value;
```

Notice what isn't present:

```text
Client Secret
Password
Certificate
```

The application uses its Azure identity.

---

# 24. What Is Happening Here?

This:

```csharp
new DefaultAzureCredential()
```

provides credentials.

Then:

```csharp
new SecretClient(...)
```

creates a Key Vault client.

Then:

```csharp
await client.GetSecretAsync("DatabasePassword");
```

requests the secret.

Architecture:

```text
ASP.NET Core
     ↓
DefaultAzureCredential
     ↓
Managed Identity
     ↓
Microsoft Entra ID
     ↓
Access Token
     ↓
Key Vault
     ↓
Secret
```

---

# 25. Important: Identity vs Permission

Suppose:

```text
Managed Identity = EmployeeApiIdentity
```

and Key Vault contains:

```text
DatabasePassword
```

The identity alone isn't enough.

You need:

```text
EmployeeApiIdentity
       ↓
Key Vault Role Assignment
       ↓
Permission to read secret
```

Otherwise the request can fail with an authorization error.

---

# 26. Environment Variables

Another important secure configuration mechanism is **environment variables**.

For example:

```text
ConnectionStrings__DefaultConnection
```

could be supplied through the hosting environment rather than being hard-coded into source code.

ASP.NET Core configuration can combine values from sources such as:

```text
appsettings.json
        ↓
appsettings.{Environment}.json
        ↓
Environment Variables
        ↓
Command-line arguments
        ↓
Other providers
```

The exact precedence depends on the configured providers; the standard ASP.NET Core builder adds several providers with later sources overriding earlier ones for the same key.

---

# 27. Configuration Example

`appsettings.json`:

```json
{
  "Application": {
    "Name": "EmployeeManagement"
  },
  "Azure": {
    "KeyVaultUrl": "https://my-key-vault.vault.azure.net/"
  }
}
```

Read it:

```csharp
var keyVaultUrl =
    builder.Configuration["Azure:KeyVaultUrl"];
```

---

# 28. Environment Variable Naming

Nested configuration:

```json
{
  "Azure": {
    "KeyVaultUrl": "..."
  }
}
```

can be represented as:

```text
Azure__KeyVaultUrl
```

Double underscore:

```text
__
```

represents the configuration hierarchy separator for environment variables.

So:

```text
Azure:KeyVaultUrl
```

becomes:

```text
Azure__KeyVaultUrl
```

---

# 29. appsettings.json vs Secrets

A useful rule:

### `appsettings.json`

Good for:

```text
Application settings
Feature flags
Non-sensitive configuration
URLs
Logging configuration
```

### User Secrets

Good for:

```text
Local development secrets
```

### Environment variables

Good for:

```text
Deployment-specific configuration
Secrets supplied by hosting/CI/CD systems
```

### Azure Key Vault

Good for:

```text
Production sensitive secrets
Keys
Certificates
Credentials
```

---

# 30. Never Commit Secrets

Suppose:

```json
{
  "ApiKey": "abc123-secret-value"
}
```

is committed to Git.

Even if you later delete it from the latest commit, the value may remain in Git history or other systems.

Better:

```text
Source Code
    ↓
Configuration reference
    ↓
Key Vault / secure environment
    ↓
Actual secret
```

---

# 31. Secure Configuration Architecture

A production-style architecture:

```text
┌────────────────────────┐
│ ASP.NET Core API       │
└────────────┬───────────┘
             │
             │ Managed Identity
             ↓
┌────────────────────────┐
│ Microsoft Entra ID     │
└────────────┬───────────┘
             │
             │ Access Token
             ↓
┌────────────────────────┐
│ Azure Key Vault        │
│                        │
│ Secrets                │
│ Certificates           │
│ Keys                   │
└────────────────────────┘
```

---

# 32. Connection String Security

Suppose your API needs SQL Server.

Bad:

```json
{
  "ConnectionStrings": {
    "DefaultConnection":
      "Server=server;Database=EmployeeDB;User Id=admin;Password=Secret123;"
  }
}
```

Better possibilities depend on the Azure SQL setup:

```text
ASP.NET Core
      ↓
Managed Identity
      ↓
Azure SQL
```

This can provide passwordless authentication to supported Azure SQL scenarios.

Or, when a secret is genuinely required:

```text
ASP.NET Core
      ↓
Managed Identity
      ↓
Key Vault
      ↓
Database credential
```

The preferred architecture depends on the Azure service and security requirements.

---

# 33. Managed Identity vs Client Secret

### Client Secret approach

```text
Application
    ↓
Client ID
    +
Client Secret
    ↓
Microsoft Entra ID
```

You have to manage the secret.

### Managed Identity

```text
Azure Application
    ↓
Managed Identity
    ↓
Microsoft Entra ID
```

Azure manages the identity credentials.

### Easy memory

```text
Client Secret
→ You manage the secret

Managed Identity
→ Azure manages the identity
```

---

# 34. System-Assigned Example

Imagine:

```text
Azure App Service
Name:
employee-api
```

Enable:

```text
Identity
 ↓
System assigned
 ↓
On
```

Now:

```text
employee-api
      ↓
System Managed Identity
```

Then assign the required role on Key Vault.

For example, conceptually:

```text
employee-api identity
      ↓
Key Vault Secrets User
      ↓
Key Vault
```

Now the application can request secrets using its identity.

---

# 35. User-Assigned Example

Create:

```text
employee-api-identity
```

Then:

```text
employee-api-identity
       │
       ├── Employee API
       ├── Reporting API
       └── Background Worker
```

Assign appropriate permissions to that identity.

This is useful when multiple Azure resources need the same identity/permission model.

---

# 36. Least Privilege

One of the most important security principles:

> Give an application only the permissions it requires.

Bad:

```text
API
 ↓
Owner
 ↓
Entire Azure Subscription
```

Better:

```text
API
 ↓
Specific Managed Identity
 ↓
Specific Role
 ↓
Specific Resource
```

For example:

```text
Employee API
    ↓
Managed Identity
    ↓
Key Vault Secrets User
    ↓
Specific Key Vault
```

---

# 37. Secrets vs Keys vs Certificates

Azure Key Vault can manage several types of security material.

### Secrets

Sensitive values.

```text
DatabasePassword
ApiKey
ClientSecret
```

### Keys

Cryptographic keys.

Used for encryption/signing scenarios.

### Certificates

Digital certificates.

Used for TLS and other certificate-based scenarios.

For Day 26, focus mainly on:

```text
Managed Identity → Key Vault → Secrets
```

---

# 38. ASP.NET Core Example

A simple service:

```csharp
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

public class KeyVaultService
{
    private readonly SecretClient _client;

    public KeyVaultService(IConfiguration configuration)
    {
        var keyVaultUrl =
            configuration["Azure:KeyVaultUrl"];

        _client = new SecretClient(
            new Uri(keyVaultUrl!),
            new DefaultAzureCredential());
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        KeyVaultSecret secret =
            await _client.GetSecretAsync(secretName);

        return secret.Value;
    }
}
```

Register:

```csharp
builder.Services.AddSingleton<KeyVaultService>();
```

Controller:

```csharp
[ApiController]
[Route("api/[controller]")]
public class ConfigurationController : ControllerBase
{
    private readonly KeyVaultService _keyVaultService;

    public ConfigurationController(
        KeyVaultService keyVaultService)
    {
        _keyVaultService = keyVaultService;
    }

    [HttpGet("secret")]
    public async Task<IActionResult> GetSecret()
    {
        var value =
            await _keyVaultService.GetSecretAsync(
                "DatabasePassword");

        return Ok(new
        {
            Message = "Secret retrieved successfully"
        });
    }
}
```

### Important security improvement

Don't return the actual secret from an API endpoint like this in a real application.

The example is only demonstrating the retrieval flow.

A real service would use the secret internally without exposing it to clients.

---

# 39. Better Key Vault Integration

Instead of manually fetching every configuration secret throughout your application, you can integrate Key Vault into ASP.NET Core configuration.

Conceptually:

```text
ASP.NET Core Configuration
          ↓
Azure Key Vault Provider
          ↓
Managed Identity
          ↓
Key Vault
```

Then application services can consume configuration through `IConfiguration` or the Options pattern.

This keeps secret retrieval centralized.

---

# 40. Local Development

Suppose you're developing on Windows with Visual Studio.

You can use:

```text
Visual Studio
      ↓
Developer authentication
      ↓
DefaultAzureCredential
      ↓
Azure
```

For local-only secrets, you can also use:

```text
ASP.NET Core User Secrets
```

Example:

```text
Right-click project
 ↓
Manage User Secrets
```

This creates a local secrets store outside the normal project source files.

---

# 41. User Secrets vs Key Vault

### User Secrets

Best for:

```text
Local development
```

### Azure Key Vault

Best for:

```text
Azure / production secret management
```

Architecture:

```text
Development
    ↓
User Secrets

Production
    ↓
Managed Identity
    ↓
Key Vault
```

---

# 42. What Should NOT Be Stored in Git?

Avoid committing:

```text
❌ Passwords
❌ Client secrets
❌ API keys
❌ Database credentials
❌ Private keys
❌ Certificates/private certificate material
❌ Production connection strings containing credentials
```

Safe examples may include:

```text
✓ Application name
✓ Non-sensitive API URL
✓ Logging configuration
✓ Feature flags
✓ Key Vault URI
```

Even a URI should be reviewed for whether it contains sensitive information.

---

# 43. Day 26 Practical Project

Create this Visual Studio project:

```text
Day26ManagedIdentity
│
├── Controllers
│   └── ConfigurationController.cs
│
├── Services
│   ├── IKeyVaultService.cs
│   └── KeyVaultService.cs
│
├── appsettings.json
├── Program.cs
└── Day26ManagedIdentity.csproj
```

Install:

```text
Azure.Identity
Azure.Security.KeyVault.Secrets
```

Configuration:

```json
{
  "Azure": {
    "KeyVaultUrl": "https://your-key-vault.vault.azure.net/"
  }
}
```

Flow:

```text
HTTP Request
      ↓
Controller
      ↓
KeyVaultService
      ↓
DefaultAzureCredential
      ↓
Managed Identity
      ↓
Microsoft Entra ID
      ↓
Access Token
      ↓
Azure Key Vault
      ↓
Secret
```

---

# 44. Day 25 vs Day 26

This distinction is very important for your .NET class.

### Day 25 — User Authentication

```text
User
 ↓
MSAL
 ↓
Microsoft Entra ID
 ↓
Access Token
 ↓
ASP.NET Core API
```

Purpose:

> Authenticate users and authorize their API access.

### Day 26 — Application Authentication

```text
ASP.NET Core
 ↓
Managed Identity
 ↓
Microsoft Entra ID
 ↓
Access Token
 ↓
Azure Resource
```

Purpose:

> Allow an Azure-hosted application to securely access Azure resources without managing application secrets.

---

# 45. Managed Identity vs MSAL

| MSAL                                            | Managed Identity                                                   |
| ----------------------------------------------- | ------------------------------------------------------------------ |
| Library used by applications to acquire tokens  | Azure-managed identity for Azure resources                         |
| Commonly used for user/app authentication flows | Mainly used for Azure resource-to-resource authentication          |
| Application participates in token acquisition   | Azure manages identity credentials                                 |
| Used on supported client/server platforms       | Requires an Azure resource/environment supporting Managed Identity |
| Can involve interactive user authentication     | Designed for workload identity                                     |
| Often used with user sign-in                    | Commonly used for application authentication                       |

A useful way to remember:

```text
MSAL
→ "My application needs to authenticate/acquire tokens."

Managed Identity
→ "My Azure application needs to authenticate to another Azure resource without storing credentials."
```

---

# 46. Managed Identity + Key Vault — Most Important Flow

Memorize this:

```text
ASP.NET Core API
        ↓
DefaultAzureCredential
        ↓
Managed Identity
        ↓
Microsoft Entra ID
        ↓
Access Token
        ↓
Azure Key Vault
        ↓
Secret
```

No:

```text
❌ Hard-coded password
❌ Hard-coded API key
❌ Client secret in source code
```

---

# 47. Interview Questions

### 1. What is Managed Identity?

An Azure feature that provides an identity for an Azure resource in Microsoft Entra ID so the resource can authenticate to supported services without application-managed credentials.

### 2. What are the types of Managed Identity?

```text
System-assigned
User-assigned
```

### 3. What is system-assigned identity?

An identity tied to the lifecycle of a specific Azure resource.

### 4. What is user-assigned identity?

A separately created managed identity that can be assigned to one or more supported Azure resources.

### 5. Why use Managed Identity?

To avoid storing and managing application credentials such as client secrets when authenticating to supported Azure resources.

### 6. Does Managed Identity automatically have access to Azure resources?

No.

You must grant the identity appropriate permissions.

### 7. What is Azure Key Vault?

An Azure service for securely managing secrets, keys, and certificates.

### 8. What is `DefaultAzureCredential`?

An Azure Identity credential that attempts available authentication mechanisms according to the environment, including managed identity when running in supported Azure environments.

### 9. What is passwordless authentication?

Authentication that doesn't require your application to manage and store a traditional password/secret credential.

### 10. What is least privilege?

Granting only the permissions required for the application's task.

### 11. Should secrets be stored in `appsettings.json`?

Production secrets should not be committed to source-controlled configuration files. Use appropriate secret-management mechanisms such as Key Vault or secure deployment configuration.

### 12. What is the difference between User Secrets and Key Vault?

```text
User Secrets
→ Local development

Key Vault
→ Azure/production secret management
```

### 13. What is RBAC?

**Role-Based Access Control** controls access to Azure resources through role assignments.

### 14. Managed Identity vs Client Secret?

```text
Client Secret
→ Application must manage secret

Managed Identity
→ Azure manages identity credentials
```

---

# 48. Day 26 Final Cheat Sheet

```text
MANAGED IDENTITY
        ↓
Azure-managed application identity
```

```text
SYSTEM-ASSIGNED
        ↓
Tied to Azure resource
        ↓
Deleted with resource
```

```text
USER-ASSIGNED
        ↓
Separate Azure resource
        ↓
Reusable across supported resources
```

```text
MANAGED IDENTITY
        ↓
Microsoft Entra ID
        ↓
Access Token
        ↓
Azure Resource
```

```text
KEY VAULT
        ↓
Secrets
Keys
Certificates
```

```text
SECURE CONFIGURATION
        ↓
User Secrets → Local development
Environment Variables → Deployment configuration
Key Vault → Production secrets
Managed Identity → Passwordless Azure authentication
```

### One line to remember:

> **Managed Identity lets an Azure-hosted application authenticate to supported Azure resources using an Azure-managed identity instead of storing credentials in your source code.**

### Your Day 25 → Day 26 connection

```text
DAY 25
User
 ↓
MSAL
 ↓
Entra ID
 ↓
Access Token
 ↓
ASP.NET Core API
 ↓
[Authorize]


DAY 26
ASP.NET Core API
 ↓
Managed Identity
 ↓
Entra ID
 ↓
Access Token
 ↓
Azure Key Vault / Azure SQL / Storage / other resource
```


