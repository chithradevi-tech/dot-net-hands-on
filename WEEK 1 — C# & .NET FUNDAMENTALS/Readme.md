# 🗓️ WEEK 1 — DAY 1: .NET Introduction & Environment Setup

---

# 1. What is .NET?

**.NET** is a free, open-source development platform from Microsoft used to build and run different types of applications.

You can build:

* Console applications
* Web applications
* REST APIs
* Desktop applications
* Cloud applications
* Microservices
* Background services
* Mobile applications
* Games and other application types through supported frameworks/tools

.NET supports multiple programming languages:

* **C#**
* F#
* Visual Basic

For your roadmap, we will primarily use **C# + .NET 8 + ASP.NET Core**.

### Simple definition

> **.NET is a software development platform used to create, run, and deploy applications.**

---

# 2. .NET Framework vs .NET Core vs Modern .NET

This is important because you will see all three names in interviews and real projects.

## .NET Framework

.NET Framework is the original Microsoft .NET platform.

It was mainly designed for Windows.

It includes technologies such as:

```text
ASP.NET
Windows Forms
WPF
ADO.NET
```

Common versions include:

```text
.NET Framework 4.0
.NET Framework 4.5
.NET Framework 4.7
.NET Framework 4.8
```

### Important

.NET Framework is **not the same thing as modern .NET**.

---

# 3. What is .NET Core?

Microsoft later introduced **.NET Core**.

The major purpose was to provide a:

* Cross-platform
* Open-source
* High-performance
* Modular
* Cloud-friendly

.NET platform.

It could run on:

```text
Windows
Linux
macOS
```

Versions included:

```text
.NET Core 1.x
.NET Core 2.x
.NET Core 3.x
.NET Core 3.1
```

---

# 4. What is Modern .NET?

After .NET Core 3.1, Microsoft moved toward a unified platform.

The evolution was approximately:

```text
.NET Framework
      ↓
.NET Core
      ↓
.NET 5
      ↓
.NET 6
      ↓
.NET 7
      ↓
.NET 8
      ↓
.NET 9
      ↓
.NET 10
```

From **.NET 5 onward**, Microsoft dropped the word **Core** from the product name.

So today we normally say:

> **.NET**

rather than:

> .NET Core

For your course, we will use **.NET 8**.

---

# 5. .NET 6, .NET 7 and .NET 8 Evolution

## .NET 6

.NET 6 was a major unified .NET release.

Important areas:

* Cross-platform development
* Performance improvements
* Minimal APIs
* Improved developer experience
* Long-Term Support release

---

## .NET 7

.NET 7 continued the unified platform and focused heavily on:

* Performance
* Cloud-native development
* ASP.NET Core improvements
* Minimal APIs
* Developer productivity

.NET 7 was a **Standard Term Support** release.

---

## .NET 8

.NET 8 introduced further improvements in:

* Performance
* ASP.NET Core
* Cloud-native development
* Native AOT
* Runtime
* C# language support

.NET 8 is an **LTS — Long-Term Support** release.

### Simple timeline

```text
.NET 6  → LTS
.NET 7  → STS
.NET 8  → LTS
```

---

# 6. What is .NET 8?

.NET 8 is a modern version of Microsoft's .NET development platform.

You can use it to build:

```text
C# Console Applications
        ↓
ASP.NET Core Web Applications
        ↓
REST APIs
        ↓
Microservices
        ↓
Cloud Applications
        ↓
Background Services
```

### Simple definition

> **.NET 8 is a modern, cross-platform, high-performance version of the .NET platform.**

---

# 7. .NET Architecture

You should understand this basic flow:

```text
                C# Code
                   ↓
              C# Compiler
                   ↓
          Intermediate Language
                 (IL)
                   ↓
            .NET Runtime
                   ↓
              JIT Compiler
                   ↓
             Machine Code
                   ↓
                  CPU
```

Let's understand each part.

---

# 8. What is CLR?

**CLR = Common Language Runtime**

CLR is the runtime environment responsible for executing and managing .NET applications.

It provides services such as:

* Memory management
* Garbage collection
* Exception handling
* Thread management
* Type safety
* Code execution
* JIT compilation

### Example

You write:

```csharp
Console.WriteLine("Hello");
```

The CLR is part of the runtime environment that helps your application execute.

### Simple definition

> **CLR is the execution environment that manages and runs .NET applications.**

---

# 9. What is CTS?

**CTS = Common Type System**

CTS defines the data types that are supported by the .NET platform.

For example:

```csharp
int age = 25;
string name = "Chitra";
bool isActive = true;
double salary = 50000.50;
```

.NET has a common type system so that different .NET languages can understand compatible types.

### Simple definition

> **CTS defines the common data types and rules for using types in .NET.**

### Easy memory trick

```text
CTS → Types
```

---

# 10. What is CLS?

**CLS = Common Language Specification**

CLS is a set of rules that helps different .NET programming languages work together.

For example:

```text
C#
 ↓
.NET Library
 ↓
VB.NET / F# / other .NET languages
```

A developer can create a library in one .NET language and make it consumable by another language when the relevant public API follows CLS-compatible rules.

### Simple definition

> **CLS defines common rules that .NET languages can follow to improve interoperability.**

### Easy memory trick

```text
CTS → Types
CLS → Language rules
```

---

# 11. CTS vs CLS

| CTS                       | CLS                                      |
| ------------------------- | ---------------------------------------- |
| Common Type System        | Common Language Specification            |
| Defines types             | Defines interoperability rules           |
| Concerned with data types | Concerned with language compatibility    |
| Broader type system       | Common subset/rules for interoperability |

### Remember

```text
CTS = What types exist?

CLS = What common rules should languages follow?
```

---

# 12. What is JIT?

**JIT = Just-In-Time Compiler**

C# source code is compiled into **Intermediate Language (IL)**.

At runtime, JIT compiles the required IL into native machine code that the processor can execute.

### Flow

```text
C# Source Code
       ↓
C# Compiler
       ↓
IL
       ↓
JIT
       ↓
Native Machine Code
       ↓
CPU
```

### Example

You write:

```csharp
int a = 10;
int b = 20;
int result = a + b;
```

The compiler produces IL.

When the application executes, the runtime/JIT compiles the relevant IL into native code.

### Simple definition

> **JIT converts Intermediate Language into native machine code at runtime.**

---

# 13. Managed Code vs Unmanaged Code

## Managed Code

Code that executes under the management of the .NET runtime is called **managed code**.

Example:

```csharp
string name = "John";
int age = 25;
```

The .NET runtime manages many runtime services, including memory management and garbage collection.

---

## Unmanaged Code

Unmanaged code executes outside the .NET runtime's managed execution environment.

Examples include native code written in:

```text
C
C++
Operating-system APIs
Native libraries
```

### Comparison

| Managed Code                       | Unmanaged Code                                             |
| ---------------------------------- | ---------------------------------------------------------- |
| Runs under .NET runtime management | Runs outside .NET managed runtime                          |
| Garbage Collection available       | No .NET GC management                                      |
| Runtime provides services          | Developer/native environment manages resources differently |
| Common example: C#/.NET            | Common example: native C/C++                               |

### Interview answer

> Managed code is executed under the control of the .NET runtime, whereas unmanaged code executes outside the .NET managed runtime.

---

# 14. What is .NET SDK?

**SDK = Software Development Kit**

The .NET SDK contains the tools required to **develop .NET applications**.

It allows you to:

```text
Create
Build
Run
Test
Publish
```

applications.

For example:

```bash
dotnet new
dotnet build
dotnet run
dotnet test
dotnet publish
```

### Simple definition

> **The .NET SDK provides the tools required to develop .NET applications.**

---

# 15. What is .NET Runtime?

The **.NET Runtime** provides the components needed to run a .NET application.

It includes the runtime environment needed for application execution.

### Simple difference

```text
SDK
 ↓
Develop applications

Runtime
 ↓
Run applications
```

---

# 16. SDK vs Runtime

| SDK                        | Runtime                                     |
| -------------------------- | ------------------------------------------- |
| Used for development       | Used for execution                          |
| Can create projects        | Cannot create projects as a development SDK |
| Can build applications     | Runs applications                           |
| Can publish applications   | Provides runtime components                 |
| Contains development tools | Contains runtime components                 |

### Easy interview answer

> **SDK is for developing .NET applications; Runtime is for running them.**

---

# 17. What is .NET CLI?

**CLI = Command-Line Interface**

The .NET CLI allows you to create and manage .NET applications using a terminal.

Main command:

```bash
dotnet
```

Examples:

```bash
dotnet --version
dotnet --info
dotnet new
dotnet restore
dotnet build
dotnet run
dotnet test
dotnet publish
```

---

# 18. Visual Studio

[Visual Studio](https://visualstudio.microsoft.com/?utm_source=chatgpt.com) is Microsoft's full-featured IDE.

**IDE = Integrated Development Environment**

It provides:

* Code editor
* IntelliSense
* Debugger
* Project management
* Git integration
* NuGet integration
* Testing tools
* Profiling tools

For enterprise .NET development, Visual Studio is commonly used.

---

# 19. Visual Studio Code

[Visual Studio Code](https://code.visualstudio.com/?utm_source=chatgpt.com) is a lightweight source-code editor.

It supports .NET development through extensions and the .NET CLI.

It provides:

* Code editing
* IntelliSense
* Extensions
* Git integration
* Integrated terminal
* Debugging support

### Visual Studio vs VS Code

| Visual Studio                  | VS Code                        |
| ------------------------------ | ------------------------------ |
| Full IDE                       | Lightweight code editor        |
| More integrated .NET tooling   | Uses extensions/tooling        |
| Heavier                        | Lightweight                    |
| Strong enterprise .NET tooling | Flexible multi-language editor |

**Important:** Visual Studio and Visual Studio Code are different applications.

---

# 20. What is a Solution?

A **Solution** is a container used to organize one or more related projects.

Example:

```text
EmployeeManagement.sln
│
├── EmployeeManagement.API
├── EmployeeManagement.Service
├── EmployeeManagement.Data
└── EmployeeManagement.Tests
```

One solution can contain multiple projects.

---

# 21. What is a Project?

A **Project** represents an application or library.

For example:

```text
EmployeeManagement.API
```

could be one project.

Another could be:

```text
EmployeeManagement.Data
```

### Simple definition

> **A project contains source code, dependencies, configuration, and build information for an application or library.**

---

# 22. Solution vs Project

```text
Solution
│
├── Project 1
├── Project 2
├── Project 3
└── Project 4
```

| Solution                      | Project                                    |
| ----------------------------- | ------------------------------------------ |
| Organizes projects            | Contains application/library code          |
| Can contain multiple projects | Usually represents one application/library |
| `.sln`                        | `.csproj`                                  |

---

# 23. What is `.csproj`?

`.csproj` means **C# project file**.

It contains project configuration.

Example:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

Let's understand it.

### `TargetFramework`

```xml
<TargetFramework>net8.0</TargetFramework>
```

This means the project targets .NET 8.

### `OutputType`

```xml
<OutputType>Exe</OutputType>
```

This indicates an executable application.

### `Nullable`

```xml
<Nullable>enable</Nullable>
```

Enables nullable reference type analysis.

### `ImplicitUsings`

```xml
<ImplicitUsings>enable</ImplicitUsings>
```

Enables commonly used namespaces automatically.

---

# 24. What is NuGet?

**NuGet** is the package manager for .NET.

It allows us to install external libraries/packages.

For example, a project may need:

```text
Entity Framework Core
Newtonsoft.Json
Serilog
xUnit
```

These can be obtained through NuGet.

Example:

```bash
dotnet add package Newtonsoft.Json
```

The package reference is then added to the project.

### Simple definition

> **NuGet is the package management system used to install and manage .NET libraries.**

---

# 25. What is Restore?

When your project has dependencies, .NET needs to obtain those packages.

Command:

```bash
dotnet restore
```

Conceptually:

```text
.csproj
   ↓
Package References
   ↓
dotnet restore
   ↓
Packages restored
```

---

# 26. What is Build?

Build compiles your project and produces build output.

Command:

```bash
dotnet build
```

Conceptually:

```text
C# Source Code
       ↓
Compiler
       ↓
Compilation
       ↓
Assembly / Build Output
```

If there are compilation errors, the build fails.

---

# 27. What is Run?

Command:

```bash
dotnet run
```

This runs the application from the project.

For a console application:

```bash
dotnet run
```

may produce:

```text
Hello, World!
```

---

# 28. What is Publish?

`dotnet publish` prepares your application for deployment.

Command:

```bash
dotnet publish
```

A typical framework-dependent publish output for a .NET 8 project may be located under:

```text
bin/Release/net8.0/publish/
```

depending on configuration.

### Development flow

```text
Write Code
    ↓
Restore
    ↓
Build
    ↓
Test
    ↓
Publish
    ↓
Deploy
```

---

# 29. Important .NET CLI Commands

## Check version

```bash
dotnet --version
```

Shows the active .NET SDK version.

---

## Detailed information

```bash
dotnet --info
```

Shows:

* SDK versions
* Runtime versions
* OS
* Architecture
* .NET installation information

---

## List installed SDKs

```bash
dotnet --list-sdks
```

Example:

```text
8.0.xxx
9.0.xxx
```

---

## List installed runtimes

```bash
dotnet --list-runtimes
```

---

# 30. Create a Console Application

```bash
dotnet new console
```

This creates a console application in the current directory.

Or specify a project name:

```bash
dotnet new console -n Day01DotNetIntroduction
```

---

# 31. Create a Solution

You can create a solution using:

```bash
dotnet new sln -n DotNetHandsOn
```

Then create a project:

```bash
dotnet new console -n Day01DotNetIntroduction
```

Add the project to the solution:

```bash
dotnet sln add Day01DotNetIntroduction/Day01DotNetIntroduction.csproj
```

Structure:

```text
DotNetHandsOn
│
├── DotNetHandsOn.sln
│
└── Day01DotNetIntroduction
    ├── Program.cs
    └── Day01DotNetIntroduction.csproj
```

---

# 32. DAY 1 HANDS-ON SETUP

Since you are practicing .NET from beginner to advanced, create a dedicated folder.

### Step 1 — Create folder

```bash
mkdir dotnet-hands-on
```

### Step 2 — Enter folder

```bash
cd dotnet-hands-on
```

### Step 3 — Check .NET

```bash
dotnet --version
```

Then:

```bash
dotnet --info
```

---

# 33. Create Your First .NET 8 Console Project

Run:

```bash
dotnet new console -n Day01DotNetIntroduction --framework net8.0
```

Enter the project:

```bash
cd Day01DotNetIntroduction
```

---

# 34. Understand the Project Structure

You should see something similar to:

```text
Day01DotNetIntroduction/
│
├── Program.cs
├── Day01DotNetIntroduction.csproj
├── bin/
└── obj/
```

### Program.cs

Contains your C# source code.

### `.csproj`

Contains project configuration.

### bin/

Contains build output.

### obj/

Contains intermediate build files generated during build/restore.

You normally don't manually edit `bin` or `obj`.

---

# 35. Your First Program

Open:

```text
Program.cs
```

Put:

```csharp
Console.WriteLine("Welcome to .NET 8!");
Console.WriteLine("This is my first .NET application.");

string name = "Chitra";
int age = 25;

Console.WriteLine($"Name: {name}");
Console.WriteLine($"Age: {age}");
```

Run:

```bash
dotnet run
```

Expected output:

```text
Welcome to .NET 8!
This is my first .NET application.
Name: Chitra
Age: 25
```

---

# 36. Run Restore

```bash
dotnet restore
```

You should see that the project is restored successfully if there are no dependency problems.

---

# 37. Build

```bash
dotnet build
```

Expected result:

```text
Build succeeded.
```

---

# 38. Run

```bash
dotnet run
```

You should see:

```text
Welcome to .NET 8!
This is my first .NET application.
Name: Chitra
Age: 25
```

---

# 39. Publish

Run:

```bash
dotnet publish
```

Then check:

```text
bin/
└── Release/
    └── net8.0/
        └── publish/
```

The exact contents depend on the project and publish options.

---

# 40. Complete Hands-On Command Flow

For your class, practice these commands in this order:

```bash
dotnet --version

dotnet --info

mkdir dotnet-hands-on

cd dotnet-hands-on

dotnet new console -n Day01DotNetIntroduction --framework net8.0

cd Day01DotNetIntroduction

dotnet restore

dotnet build

dotnet run

dotnet publish
```

---

# 41. Understand What Happens Internally

When you execute:

```bash
dotnet run
```

the simplified flow is:

```text
Program.cs
    ↓
C# Compiler
    ↓
Intermediate Language (IL)
    ↓
.NET Runtime / CLR
    ↓
JIT Compiler
    ↓
Native Machine Code
    ↓
CPU
    ↓
Output
```

For example:

```csharp
Console.WriteLine("Hello");
```

doesn't simply go directly from C# source code to CPU instructions.

It goes through the .NET compilation/runtime pipeline.

---

# 42. Important Difference: Compiler vs JIT

This is a common interview question.

### C# Compiler

The C# compiler converts:

```text
C# → IL
```

### JIT

JIT converts:

```text
IL → Native Machine Code
```

So remember:

```text
C# Compiler
C# → IL

JIT
IL → Machine Code
```

---

# 43. What is an Assembly?

An **assembly** is a compiled unit in .NET.

Depending on the project/output, assemblies can commonly be:

```text
.dll
.exe
```

For example, a library project commonly produces:

```text
MyLibrary.dll
```

A runnable application may have executable-related output depending on its deployment configuration.

Assemblies contain compiled .NET code and associated metadata/resources.

---

# 44. What is Garbage Collection?

The .NET runtime provides **Garbage Collection (GC)**.

When managed objects are no longer reachable by the application, the GC can reclaim their memory.

Example:

```csharp
Customer customer = new Customer();
```

The object is allocated in managed memory.

When it is no longer reachable and eligible for collection, the GC can eventually reclaim that memory.

### Important

You normally don't manually free ordinary managed objects using `free()` as you would in C.

### Simple definition

> **Garbage Collection automatically manages the lifetime of many managed objects and reclaims memory that is no longer reachable.**

---

# 45. Why is .NET Popular?

Important reasons include:

### Cross-platform

```text
Windows
Linux
macOS
```

### Performance

Modern .NET provides high-performance runtime and web technologies.

### Large ecosystem

You have:

```text
.NET
ASP.NET Core
Entity Framework Core
NuGet
Azure integrations
```

### Open source

Modern .NET is developed openly through Microsoft's repositories and the broader .NET community.

### Enterprise adoption

.NET is widely used for:

* Business applications
* APIs
* Banking systems
* E-commerce
* Enterprise software
* Cloud applications

---

# 46. Day 1 Important Terms

You should know these before moving to Day 2:

```text
.NET
.NET Framework
.NET Core
Modern .NET
.NET 8
CLR
CTS
CLS
JIT
Managed Code
Unmanaged Code
SDK
Runtime
CLI
Visual Studio
VS Code
Solution
Project
.csproj
NuGet
Restore
Build
Run
Publish
Assembly
Garbage Collection
IL
```

---

### Q1. What is .NET?

> .NET is a free, open-source development platform used to build and run applications across different platforms.

### Q2. What is .NET Framework?

> .NET Framework is Microsoft's original .NET implementation, primarily associated with Windows application development.

### Q3. What is .NET Core?

> .NET Core was Microsoft's cross-platform, open-source successor to the older .NET Framework and evolved into the unified modern .NET platform.

### Q4. What happened after .NET Core 3.1?

> Microsoft unified the platform under the name .NET, starting with .NET 5.

### Q5. What is .NET 8?

> .NET 8 is a modern LTS release of the unified .NET platform.

### Q6. What is CLR?

> CLR stands for Common Language Runtime. It provides the runtime environment and services required to execute managed .NET applications.

### Q7. What is CTS?

> CTS stands for Common Type System and defines the common type system used by .NET languages.

### Q8. What is CLS?

> CLS stands for Common Language Specification and defines common interoperability rules for .NET languages.

### Q9. What is JIT?

> JIT stands for Just-In-Time compiler. It compiles IL into native machine code at runtime.

### Q10. What is managed code?

> Managed code executes under the control of the .NET runtime.

### Q11. What is unmanaged code?

> Unmanaged code executes outside the .NET managed runtime environment, such as native code.

### Q12. What is SDK?

> SDK provides the tools required to develop, build, test, and publish .NET applications.

### Q13. What is Runtime?

> Runtime provides the components required to execute .NET applications.

### Q14. What is NuGet?

> NuGet is the package manager used to install and manage .NET packages.

### Q15. What is `.csproj`?

> `.csproj` is the C# project configuration file containing information such as the target framework, package references, and project settings.

### Q16. What is the difference between Solution and Project?

> A solution organizes one or more projects, while a project represents an application or library.

### Q17. What does `dotnet restore` do?

> It restores the project's NuGet dependencies.

### Q18. What does `dotnet build` do?

> It compiles the project and generates build output.

### Q19. What does `dotnet run` do?

> It runs the application, building it first when necessary.

### Q20. What does `dotnet publish` do?

> It creates deployment-ready output for the application based on the selected publish configuration.

---

# 48. Very Important Interview Flow

If the interviewer asks:

**"Explain how a C# application executes."**

Answer:

> First, we write C# source code. The C# compiler compiles the source code into Intermediate Language (IL). The .NET runtime loads and manages the application, and the JIT compiler converts the required IL into native machine code that the CPU can execute.

Short version:

```text
C#
 ↓
Compiler
 ↓
IL
 ↓
CLR / Runtime
 ↓
JIT
 ↓
Machine Code
 ↓
CPU
```

---

# 49. Day 1 Assignment

Create:

```text
DotNetHandsOn/
└── Day01DotNetIntroduction/
```

Your application should display:

```text
================================
      .NET 8 INTRODUCTION
================================

Name       : Your Name
Age        : Your Age
Technology : C#
Framework  : .NET 8
Goal       : Become a .NET Developer
```

Then execute:

```bash
dotnet restore
dotnet build
dotnet run
dotnet publish
```

Also inspect:

```text
Program.cs
Day01DotNetIntroduction.csproj
bin/
obj/
```

---

# 🎯 DAY 1 — ONE-PAGE REVISION

```text
.NET
→ Development platform

.NET Framework
→ Older Windows-focused .NET platform

.NET Core
→ Cross-platform predecessor to modern unified .NET

Modern .NET
→ Unified platform starting with .NET 5

.NET 8
→ Modern LTS .NET release

CLR
→ Runtime environment

CTS
→ Common type system

CLS
→ Common language interoperability rules

JIT
→ IL → Native Machine Code

Managed Code
→ Runs under .NET runtime management

Unmanaged Code
→ Runs outside .NET managed runtime

SDK
→ Develop

Runtime
→ Run

CLI
→ Command-line tools

Solution
→ Container for projects

Project
→ Application/library

.csproj
→ Project configuration

NuGet
→ Package manager

Restore
→ Restore dependencies

Build
→ Compile

Run
→ Execute

Publish
→ Prepare deployment output
```

---

**Day 1 .NET 8 Console Application** using the Visual Studio GUI.

# 🟣 Create .NET 8 Console Project in Visual Studio

## 1. Open Visual Studio

Open **Visual Studio** from Windows Start Menu.

You should see:

**Visual Studio → Create a new project**

Click:

> **Create a new project**

---

## 2. Select Console App

In the project template screen, search:

```text
Console App
```

You will see something like:

> **Console App**
> Create a console application that can run on .NET and Windows.

Select **Console App**.

⚠️ Make sure you select the **C#** template.

Click:

> **Next**

---

## 3. Configure Your Project

You will get the **Configure your new project** screen.

Enter:

### Project name

```text
Day01DotNetIntroduction
```

### Location

For example:

```text
D:\dotnet-hands-on
```

or:

```text
C:\Users\YourName\source\repos
```

### Solution name

```text
DotNetHandsOn
```

I recommend using:

```text
Solution name: DotNetHandsOn
Project name: Day01DotNetIntroduction
```

Then click:

> **Next**

---

# 4. Select .NET 8

You will now see:

> **Additional information**

Find:

### Framework

Select:

```text
.NET 8.0 (Long Term Support)
```

You may see options such as:

```text
.NET 8.0
.NET 9.0
.NET 10.0
```

For your current roadmap, select:

> **.NET 8.0 (Long Term Support)**

---

### Authentication Type

For this console project:

```text
None
```

### Configure for HTTPS

This option is not relevant to a console application.

### Enable native AOT

For this beginner project, leave it at the default/off setting.

### Do not use top-level statements

Leave it **unchecked** for now.

Then click:

> **Create**

---

# 5. Your Visual Studio Project

Visual Studio will create something similar to:

```text
DotNetHandsOn
│
├── Day01DotNetIntroduction
│   │
│   ├── Dependencies
│   ├── Program.cs
│   └── Day01DotNetIntroduction.csproj
│
└── Solution Items
```

The right side of Visual Studio contains:

> **Solution Explorer**

This is where you manage your project.

---

# 6. Understand Solution Explorer

You should see:

```text
Solution 'DotNetHandsOn'
│
└── Day01DotNetIntroduction
    │
    ├── Dependencies
    │
    ├── Program.cs
    │
    └── Day01DotNetIntroduction.csproj
```

### Solution

```text
DotNetHandsOn
```

The solution can contain multiple projects.

### Project

```text
Day01DotNetIntroduction
```

This is your actual console application.

### Program.cs

This contains your C# code.

### `.csproj`

This contains your project configuration.

### Dependencies

Contains project/package/framework references used by the project.

---

# 7. Open Program.cs

In **Solution Explorer**, double-click:

```text
Program.cs
```

You will probably see:

```csharp
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
```

Replace it with:

```csharp
Console.WriteLine("================================");
Console.WriteLine("      .NET 8 INTRODUCTION");
Console.WriteLine("================================");

string name = "Your Name";
int age = 25;

Console.WriteLine($"Name       : {name}");
Console.WriteLine($"Age        : {age}");
Console.WriteLine("Technology : C#");
Console.WriteLine("Framework  : .NET 8");
Console.WriteLine("Goal       : Become a .NET Developer");
```

---

# 8. Run the Project

At the top of Visual Studio, you will see a green ▶ button.

Click:

> **Start**

or press:

```text
Ctrl + F5
```

This runs the application without attaching the debugger.

You should see:

```text
================================
      .NET 8 INTRODUCTION
================================

Name       : Your Name
Age        : 25
Technology : C#
Framework  : .NET 8
Goal       : Become a .NET Developer
```

---

# 9. Run with Debugging

You can also press:

```text
F5
```

This starts the application with the Visual Studio debugger attached.

For now, you can use:

```text
Ctrl + F5
```

for normal execution.

Later, you will learn:

```text
Breakpoints
Step Over
Step Into
Step Out
Watch
Locals
Call Stack
Debug Console
```

These become very important when you start real .NET development.

---

# 10. Check Your `.csproj`

In Solution Explorer, right-click:

```text
Day01DotNetIntroduction
```

You can select:

> **Edit Project File**

You should see something similar to:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

The most important line for today is:

```xml
<TargetFramework>net8.0</TargetFramework>
```

This tells you that your project targets **.NET 8**.

---

# 11. Build the Project

In Visual Studio, select:

```text
Build
   ↓
Build Solution
```

Or press:

```text
Ctrl + Shift + B
```

Visual Studio will compile your project.

At the bottom, check:

> **Output**

You should see something similar to:

```text
Build succeeded.
```

---

# 12. Restore NuGet Packages

Visual Studio normally restores dependencies automatically when required.

You can also manually restore them.

Right-click the solution:

```text
Solution 'DotNetHandsOn'
```

Then:

```text
Restore NuGet Packages
```

You can also use:

```text
Tools
   ↓
NuGet Package Manager
   ↓
Package Manager Settings
```

You don't need to install any external package for this first console application.

---

# 13. Publish the Application

Right-click the project:

```text
Day01DotNetIntroduction
```

Select:

```text
Publish
```

You will get:

> **Publish**

Choose a target such as:

```text
Folder
```

Then click:

> **Next**

Select a folder, for example:

```text
bin\Release\net8.0\publish
```

Then click:

> **Finish**

Finally click:

> **Publish**

Visual Studio will create deployment output.

---

# 14. Your Final Project Structure

After building/publishing, your project will look roughly like:

```text
D:\dotnet-hands-on
│
└── DotNetHandsOn
    │
    ├── Day01DotNetIntroduction
    │   │
    │   ├── Dependencies
    │   ├── bin
    │   ├── obj
    │   ├── Program.cs
    │   └── Day01DotNetIntroduction.csproj
    │
    └── DotNetHandsOn.sln
```

The important files for today are:

```text
DotNetHandsOn.sln
Day01DotNetIntroduction.csproj
Program.cs
```

---

# 15. CLI Commands vs Visual Studio

Since you're learning both concepts, understand that the GUI actions correspond roughly to CLI commands:

| Visual Studio      | .NET CLI             |
| ------------------ | -------------------- |
| Create Console App | `dotnet new console` |
| Restore            | `dotnet restore`     |
| Build Solution     | `dotnet build`       |
| Start              | `dotnet run`         |
| Publish            | `dotnet publish`     |
| Add NuGet Package  | `dotnet add package` |

So when you create the project through Visual Studio, you are still using the **same .NET project system** underneath.

---

# 🎯 For Your Day 1 Class

I recommend you create exactly this:

```text
Solution
└── DotNetHandsOn

Project
└── Day01DotNetIntroduction
```

Target framework:

```text
.NET 8.0
```

Main file:

```text
Program.cs
```

Project file:

```text
Day01DotNetIntroduction.csproj
```

Then practice:

```text
1. Create project
2. Understand Solution
3. Understand Project
4. Open Program.cs
5. Understand .csproj
6. Write C# code
7. Build
8. Run
9. Restore
10. Publish
```


