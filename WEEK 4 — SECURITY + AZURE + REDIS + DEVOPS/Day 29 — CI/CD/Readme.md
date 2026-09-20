# Day 29 — CI/CD with Azure DevOps

Today you will connect your **ASP.NET Core API + Git + Azure DevOps** and create a basic **CI/CD pipeline**.

The main goal is to understand this complete flow:

```text
Developer
   ↓
Write Code
   ↓
Git Commit
   ↓
Git Push
   ↓
Azure DevOps
   ↓
Build
   ↓
Test
   ↓
Publish
   ↓
Artifact
   ↓
Deploy
   ↓
Environment
```

---

# 1. What is CI/CD?

## CI — Continuous Integration

CI means automatically checking your code whenever developers push changes.

Typical CI process:

```text
Git Push
   ↓
Restore
   ↓
Build
   ↓
Test
   ↓
Publish
```

Example:

You change:

```csharp
public string GetMessage()
{
    return "Hello .NET";
}
```

Then:

```bash
git add .
git commit -m "Update API"
git push
```

Azure DevOps automatically starts the pipeline.

---

# 2. What is CD?

CD means automatically delivering/deploying the application after successful CI.

```text
Build
  ↓
Test
  ↓
Publish
  ↓
Artifact
  ↓
Deploy
  ↓
Azure App Service
```

So:

### CI

> Is the code working?

### CD

> Can we deliver this working code to an environment?

---

# 3. CI/CD Pipeline

For your .NET project:

```text
Developer
    ↓
Visual Studio
    ↓
Git
    ↓
Azure Repos
    ↓
Azure Pipeline
    ↓
┌──────────────┐
│ Restore      │
│ Build        │
│ Test         │
│ Publish      │
└──────────────┘
    ↓
Artifact
    ↓
Deployment Stage
    ↓
Development Environment
    ↓
Azure App Service
```

---

# 4. What is YAML?

YAML stands for **YAML Ain't Markup Language**.

Azure DevOps uses YAML to define pipeline instructions.

Example:

```yaml
trigger:
- main

pool:
  vmImage: 'ubuntu-latest'

steps:

- task: UseDotNet@2
  inputs:
    packageType: 'sdk'
    version: '8.x'

- script: dotnet restore
  displayName: 'Restore'

- script: dotnet build --configuration Release --no-restore
  displayName: 'Build'

- script: dotnet test --configuration Release --no-build
  displayName: 'Test'
```

Instead of configuring everything manually through the UI, we keep the pipeline configuration in source control.

---

# 5. Why YAML Pipeline?

Suppose your project contains:

```text
EmployeeManagement/
│
├── Controllers/
├── Services/
├── DAL/
├── Models/
├── Tests/
├── Program.cs
├── appsettings.json
└── EmployeeManagement.csproj
```

Create:

```text
azure-pipelines.yml
```

Now the pipeline definition becomes part of your Git repository.

Advantages:

* Version controlled
* Easy to review
* Easy to reproduce
* Can be changed through pull requests
* Pipeline configuration stays with the code

---

# 6. Build Pipeline

A build pipeline normally performs:

```text
Restore
   ↓
Build
   ↓
Test
   ↓
Publish
```

### Restore

Downloads NuGet dependencies.

```bash
dotnet restore
```

### Build

Compiles the application.

```bash
dotnet build
```

### Test

Runs automated tests.

```bash
dotnet test
```

### Publish

Creates deployable application files.

```bash
dotnet publish
```

---

# 7. Artifact

An **artifact** is the output produced by your pipeline that can be consumed by later stages or deployment.

For example:

```text
Build
 ↓
dotnet publish
 ↓
publish/
 ├── EmployeeManagement.dll
 ├── appsettings.json
 ├── dependencies
 └── other published files
```

Then:

```text
Publish Output
      ↓
   Artifact
      ↓
 Deployment
```

Think of it as:

> **Artifact = packaged output of the build**

---

# 8. Environment

An environment represents a deployment target.

For example:

```text
Development
Testing
Staging
Production
```

You might have:

```text
Pipeline
   ↓
Development
   ↓
Staging
   ↓
Production
```

In Azure DevOps, environments can also provide deployment history and deployment controls.

---

# 9. Variables

Variables allow you to avoid hardcoding values in your YAML.

Example:

```yaml
variables:
  buildConfiguration: 'Release'
```

Then:

```yaml
- script: dotnet build --configuration $(buildConfiguration)
```

`$(buildConfiguration)` becomes:

```text
Release
```

---

# 10. Why Variables?

Instead of:

```yaml
dotnet build --configuration Release
```

you can use:

```yaml
variables:
  buildConfiguration: 'Release'
```

and:

```yaml
dotnet build --configuration $(buildConfiguration)
```

Now changing the configuration is easier.

---

# 11. Variable Groups

Suppose multiple pipelines use:

```text
DatabaseServer
DatabaseName
ApiEnvironment
RedisConnection
```

Instead of defining them separately in every pipeline, you can create an Azure DevOps **Variable Group**.

Example:

```text
Variable Group: EmployeeManagement-Dev
```

Containing:

```text
DatabaseServer
DatabaseName
RedisConnection
ApiEnvironment
```

Pipeline:

```yaml
variables:
- group: EmployeeManagement-Dev
```

Then:

```yaml
- script: echo $(ApiEnvironment)
```

---

# 12. Secrets

Never put passwords directly into YAML.

❌ Bad:

```yaml
variables:
  dbPassword: 'MyPassword123'
```

Especially don't commit this to Git.

Instead, use a secret variable.

For example:

```text
DatabasePassword
```

Mark it as:

```text
Keep this value secret
```

Azure DevOps masks secret values in pipeline logs.

For production systems, use an appropriate secret-management solution such as **Azure Key Vault** rather than storing production secrets directly in YAML.

---

# 13. Pipeline Stages

A pipeline can have multiple stages.

Example:

```text
Stage 1
Build
   ↓
Stage 2
Test
   ↓
Stage 3
Deploy Dev
   ↓
Stage 4
Deploy Staging
   ↓
Stage 5
Deploy Production
```

A simple pipeline:

```text
Build
  ↓
Deploy
```

A more realistic pipeline:

```text
Build
  ↓
Test
  ↓
Development
  ↓
Staging
  ↓
Production
```

---

# 14. Jobs vs Steps vs Stages

This is important for interviews.

### Stage

A major phase.

```text
Build
Deploy
```

### Job

A group of work executed on an agent.

```text
Build Job
```

### Step

An individual task/command.

```text
dotnet restore
dotnet build
dotnet test
```

Think:

```text
Stage
  ↓
 Job
  ↓
 Steps
```

---

# 15. Create the ASP.NET Core API

For Day 29, create a project:

```text
Day29CICD
```

In Visual Studio:

```text
Create a new project
        ↓
ASP.NET Core Web API
        ↓
Project Name:
Day29CICD
        ↓
Framework:
.NET 8
        ↓
Create
```

Your project might look like:

```text
Day29CICD
│
├── Controllers
│   └── WeatherForecastController.cs
│
├── Properties
│
├── Program.cs
├── appsettings.json
├── Day29CICD.csproj
└── Day29CICD.sln
```

---

# 16. Add a Simple Controller

Create:

```text
Controllers/EmployeeController.cs
```

```csharp
using Microsoft.AspNetCore.Mvc;

namespace Day29CICD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Message = "Employee API is working",
            Status = "Success"
        });
    }
}
```

Run the API.

You should be able to access:

```text
GET /api/employee
```

---

# 17. Add Unit Tests

For a real CI pipeline, we should have tests.

In Visual Studio:

```text
Solution
   ↓
Add
   ↓
New Project
   ↓
xUnit Test Project
```

Name it:

```text
Day29CICD.Tests
```

Solution:

```text
Day29CICD
│
├── Day29CICD
│   ├── Controllers
│   ├── Program.cs
│   └── Day29CICD.csproj
│
└── Day29CICD.Tests
    └── UnitTest1.cs
```

---

# 18. Simple Unit Test

You can start with:

```csharp
namespace Day29CICD.Tests;

public class BasicTests
{
    [Fact]
    public void Addition_ShouldReturnCorrectResult()
    {
        int result = 10 + 20;

        Assert.Equal(30, result);
    }
}
```

Run locally:

```bash
dotnet test
```

Expected:

```text
Passed!  - Failed: 0, Passed: 1
```

---

# 19. Create azure-pipelines.yml

At the **solution root**, create:

```text
azure-pipelines.yml
```

Structure:

```text
Day29CICD
│
├── Day29CICD
│
├── Day29CICD.Tests
│
├── Day29CICD.sln
│
└── azure-pipelines.yml
```

---

# 20. Basic CI YAML

Start with this:

```yaml
trigger:
- main

pool:
  vmImage: 'ubuntu-latest'

variables:
  buildConfiguration: 'Release'

steps:

- task: UseDotNet@2
  displayName: 'Install .NET 8 SDK'
  inputs:
    packageType: 'sdk'
    version: '8.x'

- script: dotnet restore
  displayName: 'Restore NuGet Packages'

- script: dotnet build --configuration $(buildConfiguration) --no-restore
  displayName: 'Build Application'

- script: dotnet test --configuration $(buildConfiguration) --no-build
  displayName: 'Run Unit Tests'
```

This is your basic **CI pipeline**.

---

# 21. What Happens?

When you push to `main`:

```text
git push
   ↓
Azure DevOps
   ↓
Pipeline starts
   ↓
Install .NET 8
   ↓
Restore
   ↓
Build
   ↓
Test
```

If build/test fails:

```text
Pipeline ❌
```

If everything succeeds:

```text
Pipeline ✅
```

---

# 22. Add Publish

Now extend the pipeline:

```yaml
trigger:
- main

pool:
  vmImage: 'ubuntu-latest'

variables:
  buildConfiguration: 'Release'

steps:

- task: UseDotNet@2
  displayName: 'Install .NET 8 SDK'
  inputs:
    packageType: 'sdk'
    version: '8.x'

- script: dotnet restore
  displayName: 'Restore NuGet Packages'

- script: dotnet build --configuration $(buildConfiguration) --no-restore
  displayName: 'Build Application'

- script: dotnet test --configuration $(buildConfiguration) --no-build
  displayName: 'Run Unit Tests'

- script: dotnet publish Day29CICD/Day29CICD.csproj \
    --configuration $(buildConfiguration) \
    --output $(Build.ArtifactStagingDirectory)
  displayName: 'Publish Application'

- task: PublishBuildArtifacts@1
  displayName: 'Publish Artifact'
  inputs:
    PathtoPublish: '$(Build.ArtifactStagingDirectory)'
    ArtifactName: 'drop'
    publishLocation: 'Container'
```

Now the flow becomes:

```text
Restore
   ↓
Build
   ↓
Test
   ↓
Publish
   ↓
Artifact
```

---

# 23. Better Modern Artifact Task

For Azure DevOps pipelines, you can also use:

```yaml
- task: PublishPipelineArtifact@1
  displayName: 'Publish Pipeline Artifact'
  inputs:
    targetPath: '$(Build.ArtifactStagingDirectory)'
    artifact: 'drop'
```

This produces:

```text
drop
```

which later deployment stages can consume.

---

# 24. Recommended Day 29 CI YAML

For your hands-on project, I recommend learning this version:

```yaml
trigger:
- main

pool:
  vmImage: 'ubuntu-latest'

variables:
  buildConfiguration: 'Release'

stages:

- stage: Build
  displayName: 'Build and Test'

  jobs:

  - job: BuildJob

    steps:

    - task: UseDotNet@2
      displayName: 'Install .NET 8 SDK'
      inputs:
        packageType: 'sdk'
        version: '8.x'

    - script: dotnet restore
      displayName: 'Restore NuGet Packages'

    - script: dotnet build --configuration $(buildConfiguration) --no-restore
      displayName: 'Build Application'

    - script: dotnet test --configuration $(buildConfiguration) --no-build
      displayName: 'Run Unit Tests'

    - script: dotnet publish Day29CICD/Day29CICD.csproj --configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory)
      displayName: 'Publish Application'

    - task: PublishPipelineArtifact@1
      displayName: 'Publish Artifact'
      inputs:
        targetPath: '$(Build.ArtifactStagingDirectory)'
        artifact: 'drop'
```

Notice the hierarchy:

```text
stages
  ↓
stage
  ↓
jobs
  ↓
steps
```

---

# 25. Push to Azure DevOps

First:

```bash
git status
```

Then:

```bash
git add .
```

Commit:

```bash
git commit -m "Add CI CD pipeline"
```

Push:

```bash
git push origin main
```

Now Azure DevOps can detect the change.

---

# 26. Create Pipeline in Azure DevOps

Go to your Azure DevOps project.

Navigate:

```text
Azure DevOps
   ↓
Pipelines
   ↓
New Pipeline
```

Choose:

```text
Where is your code?
```

If using Azure Repos:

```text
Azure Repos Git
```

Select your repository.

Then choose:

```text
Existing Azure Pipelines YAML file
```

Select:

```text
/main/azure-pipelines.yml
```

Then:

```text
Run
```

---

# 27. Pipeline Execution

You will see something similar to:

```text
Build and Test
      │
      └── BuildJob
            │
            ├── Install .NET 8 SDK
            ├── Restore NuGet Packages
            ├── Build Application
            ├── Run Unit Tests
            ├── Publish Application
            └── Publish Artifact
```

Each step will show:

```text
✓ Success
```

or:

```text
✗ Failed
```

---

# 28. Deployment Stage

Now we move from CI to CD.

Architecture:

```text
                CI
                 ↓
        ┌─────────────────┐
        │ Restore         │
        │ Build           │
        │ Test            │
        │ Publish         │
        └─────────────────┘
                 ↓
              Artifact
                 ↓
                CD
                 ↓
        ┌─────────────────┐
        │ Development     │
        │ Environment     │
        └─────────────────┘
                 ↓
          Azure App Service
```

---

# 29. Deployment Environment

In Azure DevOps:

```text
Pipelines
   ↓
Environments
   ↓
New environment
```

Create:

```text
Development
```

Later you might have:

```text
Development
Staging
Production
```

---

# 30. Multi-Stage Pipeline

Conceptually:

```yaml
stages:

- stage: Build
  jobs:
  - job: Build
    steps:
      # restore
      # build
      # test
      # publish
      # artifact


- stage: Deploy_Dev
  dependsOn: Build
  condition: succeeded()

  jobs:
  - deployment: Deploy
    environment: Development

    strategy:
      runOnce:
        deploy:
          steps:
            # deployment steps
```

The important concept is:

```text
Build
 ↓
dependsOn: Build
 ↓
Deploy
```

If Build fails:

```text
Build ❌
   ↓
Deploy ⛔
```

If Build succeeds:

```text
Build ✅
   ↓
Deploy ✅
```

---

# 31. Variables in Real Projects

For example:

```yaml
variables:
  buildConfiguration: 'Release'
  environmentName: 'Development'
```

Use:

```yaml
$(buildConfiguration)
```

and:

```yaml
$(environmentName)
```

---

# 32. Development vs Production Variables

You might have:

```text
EmployeeManagement-Dev
EmployeeManagement-Prod
```

Variable groups:

```text
EmployeeManagement-Dev
│
├── ApiEnvironment = Development
├── DatabaseName = EmployeeDB_Dev
└── ...

EmployeeManagement-Prod
│
├── ApiEnvironment = Production
├── DatabaseName = EmployeeDB_Prod
└── ...
```

The deployment stage can select the appropriate group.

---

# 33. Secrets Architecture

Do **not** do this:

```text
azure-pipelines.yml
        ↓
SQL Password
        ↓
Git
```

Instead:

```text
Azure DevOps / Key Vault
        ↓
Secret
        ↓
Pipeline
        ↓
Application
```

For Azure-hosted applications, a common production architecture is:

```text
ASP.NET Core
     ↓
Managed Identity
     ↓
Microsoft Entra ID
     ↓
Azure Key Vault
     ↓
Secret
```

This connects directly with your **Day 26 — Managed Identity** topic.

---

# 34. Complete CI/CD Architecture

By the end of Day 29, you should understand:

```text
Developer
    │
    │ git add
    ↓
Staging
    │
    │ git commit
    ↓
Local Repository
    │
    │ git push
    ↓
Azure Repos
    │
    ↓
Azure Pipeline
    │
    ├── Restore
    │
    ├── Build
    │
    ├── Test
    │
    └── Publish
           │
           ↓
        Artifact
           │
           ↓
      Deploy Stage
           │
           ↓
      Development
           │
           ↓
    Azure App Service
```

---

# 35. Build vs Release vs Deploy

These terms can be confusing.

### Build

Compile and package the application.

```text
Source Code
    ↓
Build
    ↓
Artifact
```

### Release

Take a specific artifact/version and move it through environments.

```text
Artifact
   ↓
Development
   ↓
Staging
   ↓
Production
```

### Deploy

Actually install/update the application in the target environment.

```text
Artifact
   ↓
Azure App Service
```

Modern Azure DevOps YAML pipelines commonly represent CI and CD as **multi-stage pipelines**, rather than requiring the older classic Release pipeline model.

---

# 36. CI/CD Interview Questions

### 1. What is CI?

Continuous Integration — automatically building and testing code changes.

### 2. What is CD?

Continuous Delivery/Deployment — automatically preparing and/or deploying validated software to environments.

### 3. What is a YAML pipeline?

A pipeline defined as YAML configuration stored with the source code.

### 4. What is an artifact?

The output of a build that can be consumed by later deployment stages.

### 5. What is a pipeline stage?

A major phase such as:

```text
Build
Deploy
```

### 6. What is a job?

A group of steps executed together by an agent.

### 7. What is a step?

An individual command/task.

### 8. What is an environment?

A deployment target such as:

```text
Development
Staging
Production
```

### 9. What is a variable?

A configurable value used by the pipeline.

### 10. What is a variable group?

A reusable collection of pipeline variables.

### 11. How should secrets be handled?

Use secret variables or an appropriate secret store such as Azure Key Vault; don't commit secrets to Git.

### 12. What happens if tests fail?

Normally the pipeline fails and later dependent stages don't run.

### 13. Why use artifacts?

To separate **building** from **deployment** and deploy a known build output.

### 14. What is `dependsOn`?

It specifies a dependency between stages/jobs.

Example:

```yaml
dependsOn: Build
```

### 15. What is `condition: succeeded()`?

It allows the stage/job to run only when its dependency has succeeded.

---

# 37. Day 29 Practical Task

Create:

```text
Day29CICD
│
├── Day29CICD
│   ├── Controllers
│   ├── Program.cs
│   └── Day29CICD.csproj
│
├── Day29CICD.Tests
│   └── BasicTests.cs
│
├── Day29CICD.sln
│
└── azure-pipelines.yml
```

Then perform:

```text
1. Create ASP.NET Core Web API
             ↓
2. Create xUnit test project
             ↓
3. Write at least one test
             ↓
4. Test locally
             ↓
5. Create azure-pipelines.yml
             ↓
6. git add .
             ↓
7. git commit
             ↓
8. git push
             ↓
9. Create Azure Pipeline
             ↓
10. Restore
             ↓
11. Build
             ↓
12. Test
             ↓
13. Publish
             ↓
14. Create Artifact
             ↓
15. Create Development Environment
             ↓
16. Add Deployment Stage
```

---

# 38. Day 29 Final Cheat Sheet

```text
CI/CD
│
├── CI
│   ├── Restore
│   ├── Build
│   └── Test
│
└── CD
    ├── Artifact
    ├── Environment
    └── Deploy
```

### YAML hierarchy

```text
Pipeline
   ↓
Stages
   ↓
Jobs
   ↓
Steps
```

### Important concepts

```text
YAML
    → Pipeline definition

Build
    → Compile application

Test
    → Verify application

Publish
    → Create deployable output

Artifact
    → Store build output

Environment
    → Deployment target

Variable
    → Configuration value

Variable Group
    → Reusable variables

Secret
    → Protected sensitive value

Stage
    → Major pipeline phase
```

### Most important mental model

```text
Git Push
   ↓
Azure DevOps Pipeline
   ↓
Restore
   ↓
Build
   ↓
Test
   ↓
Publish
   ↓
Artifact
   ↓
Deploy
   ↓
Development
   ↓
Staging
   ↓
Production
```

**Day 29 goal:** you should now be able to explain and build a basic **ASP.NET Core + Azure DevOps YAML CI/CD pipeline**, including **build, test, artifact, variables, secrets, environments, stages, and deployment flow**.
