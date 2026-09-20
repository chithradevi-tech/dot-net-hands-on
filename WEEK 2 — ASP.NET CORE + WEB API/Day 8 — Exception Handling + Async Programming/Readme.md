# 🗓️ WEEK 2 — ASP.NET CORE + WEB API

# Day 8 — Exception Handling + Async Programming

Day 8 is very important because these concepts appear constantly in real .NET applications.

Today's flow:

```text
EXCEPTION HANDLING
try
catch
finally
throw
   ↓
Custom Exceptions
Exception Hierarchy
Multiple catch
Exception Filters
Global Exception Handling

ASYNC PROGRAMMING
Synchronous
     ↓
Task
     ↓
async / await
     ↓
Task<T>
     ↓
WhenAll / WhenAny
     ↓
CancellationToken
     ↓
Thread / ThreadPool / Task
```

---

# PART 1 — EXCEPTION HANDLING

## 1. What is an Exception?

An **exception** is an unexpected condition that occurs while a program is running.

Example:

```csharp
int a = 10;
int b = 0;

int result = a / b;
```

This causes:

```text
System.DivideByZeroException
```

Without handling the exception, the application can terminate or propagate the error to a higher level.

---

# 2. Why Exception Handling?

Exception handling allows us to:

* Prevent unexpected application termination
* Handle errors gracefully
* Log errors
* Return meaningful messages
* Clean up resources
* Separate normal code from error-handling code

The basic structure is:

```csharp
try
{
    // Code that may throw an exception
}
catch
{
    // Handle exception
}
finally
{
    // Cleanup
}
```

---

# 3. try

Put code that may generate an exception inside `try`.

```csharp
try
{
    int number = 10;
    int result = number / 0;
}
```

But `try` normally needs a `catch` or `finally`.

---

# 4. catch

`catch` handles an exception.

```csharp
try
{
    int number = 10;
    int result = number / 0;
}
catch
{
    Console.WriteLine("An error occurred.");
}
```

Output:

```text
An error occurred.
```

---

# 5. Catch the Specific Exception

Instead of:

```csharp
catch
```

prefer a specific exception when you know what you're handling.

```csharp
try
{
    int number = 10;
    int result = number / 0;
}
catch (DivideByZeroException)
{
    Console.WriteLine("Cannot divide by zero.");
}
```

This is better because the handling is specific.

---

# 6. Exception Object

You can access the exception object:

```csharp
try
{
    int result = 10 / 0;
}
catch (DivideByZeroException ex)
{
    Console.WriteLine(ex.Message);
}
```

Important properties include:

```text
Message
StackTrace
InnerException
Source
```

Example:

```csharp
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
```

---

# 7. finally

`finally` executes whether an exception occurs or not, subject to abnormal process termination scenarios.

```csharp
try
{
    Console.WriteLine("Try");
}
catch (Exception)
{
    Console.WriteLine("Catch");
}
finally
{
    Console.WriteLine("Finally");
}
```

Output:

```text
Try
Finally
```

If an exception occurs:

```text
Try
Catch
Finally
```

---

# 8. Why Use finally?

It is commonly used for cleanup.

For example:

```csharp
FileStream? file = null;

try
{
    file = File.OpenRead("data.txt");

    // Work with file
}
catch (IOException ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    file?.Dispose();
}
```

However, in modern C#, prefer `using` / `using var` for disposable resources where appropriate:

```csharp
using FileStream file = File.OpenRead("data.txt");
```

---

# 9. throw

`throw` is used to explicitly throw an exception.

```csharp
throw new Exception("Something went wrong.");
```

Example:

```csharp
int age = 15;

if (age < 18)
{
    throw new Exception("Age must be 18 or above.");
}
```

---

# 10. `throw;` vs `throw ex;`

This is a very important interview question.

### Correct rethrow

```csharp
catch (Exception)
{
    throw;
}
```

This preserves the original stack trace.

### Avoid

```csharp
catch (Exception ex)
{
    throw ex;
}
```

This can reset the stack-trace information from the original throw location.

So remember:

```text
throw;
↓
Preserve original stack trace

throw ex;
↓
Avoid for rethrowing
```

---

# 11. Multiple catch Blocks

You can have multiple `catch` blocks.

```csharp
try
{
    int number = Convert.ToInt32(Console.ReadLine());

    int result = 100 / number;
}
catch (FormatException)
{
    Console.WriteLine("Invalid number format.");
}
catch (DivideByZeroException)
{
    Console.WriteLine("Cannot divide by zero.");
}
catch (Exception)
{
    Console.WriteLine("Unexpected error.");
}
```

---

# 12. Catch Order Matters

Put specific exceptions before general exceptions.

Correct:

```csharp
catch (FormatException)
{
}
catch (Exception)
{
}
```

Incorrect:

```csharp
catch (Exception)
{
}
catch (FormatException)
{
}
```

Why?

Because `FormatException` derives from `Exception`, so the first `catch` would already catch it.

---

# 13. Exception Hierarchy

The basic hierarchy you should understand:

```text
System.Object
    ↓
System.Exception
    ├── System.SystemException
    │      ├── NullReferenceException
    │      ├── ArgumentException
    │      ├── InvalidOperationException
    │      ├── IOException
    │      ├── DivideByZeroException
    │      └── ...
    │
    └── ApplicationException
```

There are many more branches in the actual .NET exception hierarchy.

The important point is:

> `Exception` is the common base class for exceptions.

---

# 14. System.Exception

`System.Exception` is the base type for exceptions in .NET.

Example:

```csharp
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
```

It can catch many exception types.

But don't automatically use:

```csharp
catch (Exception)
```

everywhere.

Prefer specific handling when you can meaningfully handle the specific exception.

---

# 15. SystemException

`SystemException` is a base class for many exceptions generated by the runtime or representing system-level/runtime-related failures.

Examples include types such as:

```text
NullReferenceException
InvalidOperationException
IndexOutOfRangeException
ArgumentException
```

You will generally catch the more specific exception rather than `SystemException`.

---

# 16. ApplicationException

`ApplicationException` exists in the .NET exception hierarchy, but it is **not the recommended base class for new custom exceptions**.

For new applications, create your custom exception by deriving directly from `Exception`.

Recommended:

```csharp
public class InvalidEmployeeException : Exception
{
    public InvalidEmployeeException(string message)
        : base(message)
    {
    }
}
```

Not:

```csharp
public class InvalidEmployeeException : ApplicationException
{
}
```

---

# 17. Custom Exceptions

Custom exceptions are useful when your application has domain-specific errors.

For example:

```text
EmployeeNotFoundException
InvalidEmployeeException
InsufficientBalanceException
OrderNotFoundException
```

Create:

```csharp
public class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException(string message)
        : base(message)
    {
    }
}
```

Use:

```csharp
Employee? employee = null;

if (employee == null)
{
    throw new EmployeeNotFoundException(
        "Employee was not found."
    );
}
```

Handle:

```csharp
try
{
    // Employee operation
}
catch (EmployeeNotFoundException ex)
{
    Console.WriteLine(ex.Message);
}
```

---

# 18. Custom Exception Best Practice

Usually:

```csharp
public class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException()
    {
    }

    public EmployeeNotFoundException(string message)
        : base(message)
    {
    }

    public EmployeeNotFoundException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
    }
}
```

This gives you common construction options.

For most beginner applications, the string constructor is enough.

---

# 19. InnerException

An exception can contain another exception.

```csharp
try
{
    try
    {
        throw new InvalidOperationException(
            "Database operation failed.");
    }
    catch (Exception ex)
    {
        throw new Exception(
            "Employee loading failed.",
            ex);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.InnerException?.Message);
}
```

Conceptually:

```text
Outer Exception
      ↓
Inner Exception
      ↓
Original problem
```

This is useful when adding context while preserving the original cause.

---

# 20. Exception Filters

Exception filters use:

```csharp
when
```

Example:

```csharp
try
{
    throw new Exception("Database error");
}
catch (Exception ex) when (ex.Message.Contains("Database"))
{
    Console.WriteLine("Database-related error.");
}
```

The catch executes only when the condition is true.

---

# 21. Practical Exception Filter

Suppose:

```csharp
int statusCode = 404;

try
{
    throw new Exception("Request failed");
}
catch (Exception ex) when (statusCode == 404)
{
    Console.WriteLine("Resource not found.");
}
```

The `when` condition decides whether that catch block is applicable.

---

# 22. Exception Filters vs Nested if

Instead of:

```csharp
catch (Exception ex)
{
    if (statusCode == 404)
    {
        // ...
    }
}
```

you can use:

```csharp
catch (Exception ex) when (statusCode == 404)
{
    // ...
}
```

Exception filters can make exception handling more precise.

---

# 23. Don't Use Exceptions for Normal Control Flow

Avoid:

```csharp
try
{
    int number = int.Parse(input);
}
catch
{
    // Used just to check whether input is valid
}
```

For normal user input validation, prefer:

```csharp
if (int.TryParse(input, out int number))
{
    Console.WriteLine(number);
}
else
{
    Console.WriteLine("Invalid number.");
}
```

Exceptions should generally represent exceptional conditions, not routine branching.

---

# 24. Global Exception Handling

In a console application, we often use `try/catch` around operations.

In ASP.NET Core Web API, we don't want every controller method to contain:

```csharp
try
{
}
catch
{
}
```

Instead, we can implement **global exception handling**.

Conceptually:

```text
HTTP Request
     ↓
Middleware
     ↓
Controller
     ↓
Service
     ↓
Exception
     ↓
Global Exception Handler
     ↓
HTTP Error Response
```

This becomes especially important when you reach ASP.NET Core middleware.

---

# 25. ASP.NET Core Global Exception Handling

Modern ASP.NET Core applications can use centralized exception handling.

For example:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandler("/error");

app.MapControllers();

app.Run();
```

In a production application, you would normally create a proper error endpoint/exception handler and return a consistent error response rather than exposing internal exception details.

---

# 26. Developer Exception Page

During development, ASP.NET Core can use:

```csharp
app.UseDeveloperExceptionPage();
```

But don't expose detailed exception information to production users.

Sensitive information such as:

```text
Stack traces
Database details
Internal file paths
Connection details
```

should not be returned to clients.

---

# PART 2 — ASYNC PROGRAMMING

# 27. What is Synchronous Programming?

Synchronous code executes operations sequentially.

Example:

```csharp
DoTask1();
DoTask2();
DoTask3();
```

Conceptually:

```text
Task 1
  ↓
Task 2
  ↓
Task 3
```

Task 2 starts after Task 1 completes.

---

# 28. What is Asynchronous Programming?

Asynchronous programming allows an operation to be started without requiring the current thread to sit blocked waiting for its completion.

Example:

```csharp
await GetEmployeeAsync();
```

For I/O operations, while the operation is waiting on external resources, the thread can return to useful work rather than simply blocking.

Typical I/O:

```text
Database
HTTP API
File
Network
Cloud service
```

---

# 29. Why Async is Important in ASP.NET Core

Imagine an API receives:

```text
1000 requests
```

Each request needs to wait for a database call.

Blocking a thread for every I/O wait can reduce scalability.

Async I/O allows the application to avoid unnecessarily blocking request threads while waiting for the external operation.

Conceptually:

```text
Request
  ↓
Database call
  ↓
Waiting...
  ↓
Thread can be returned to ThreadPool
  ↓
Database completes
  ↓
Continuation resumes
```

---

# 30. What is Task?

`Task` represents an asynchronous operation.

Example:

```csharp
Task task = DoWorkAsync();
```

A `Task` can represent:

> An operation that will complete in the future.

---

# 31. Task<T>

`Task<T>` represents an asynchronous operation that eventually produces a result of type `T`.

Example:

```csharp
Task<int> task = GetNumberAsync();
```

Eventually:

```text
Task<int>
    ↓
Result
    ↓
int
```

---

# 32. async

The `async` keyword indicates that a method contains asynchronous operations and can use `await`.

Example:

```csharp
public async Task DoWorkAsync()
{
    await Task.Delay(1000);

    Console.WriteLine("Work completed");
}
```

---

# 33. await

`await` asynchronously waits for an awaitable operation to complete.

```csharp
await Task.Delay(1000);
```

Important:

> `await` does not mean "create a new thread."

This is a very common interview question.

---

# 34. Basic Async Example

```csharp
static async Task DoWorkAsync()
{
    Console.WriteLine("Starting...");

    await Task.Delay(2000);

    Console.WriteLine("Completed.");
}
```

Call:

```csharp
await DoWorkAsync();
```

Output:

```text
Starting...
(wait)
Completed.
```

`Task.Delay()` is only a simple demonstration; it does not represent actual useful I/O.

---

# 35. Task<T> Example

```csharp
static async Task<int> GetNumberAsync()
{
    await Task.Delay(1000);

    return 100;
}
```

Call:

```csharp
int number = await GetNumberAsync();

Console.WriteLine(number);
```

Output:

```text
100
```

---

# 36. Async Naming Convention

Asynchronous methods generally end with:

```text
Async
```

Examples:

```csharp
GetEmployeeAsync()
SaveEmployeeAsync()
DeleteEmployeeAsync()
SendEmailAsync()
GetDataAsync()
```

This is a standard .NET naming convention.

---

# 37. Async Return Types

Common return types:

### No result

```csharp
Task
```

Example:

```csharp
public async Task SaveAsync()
{
    await SomeOperationAsync();
}
```

### Result

```csharp
Task<T>
```

Example:

```csharp
public async Task<Employee?> GetEmployeeAsync(int id)
{
    ...
}
```

### Synchronous result

```csharp
Employee
```

for a normal synchronous method.

---

# 38. Avoid `async void`

Normally:

```csharp
public async Task SaveAsync()
```

is preferred over:

```csharp
public async void SaveAsync()
```

`async void` is mainly appropriate for event handlers.

Why?

Because callers cannot await an `async void` method and exception handling/composition becomes harder.

---

# 39. Task.Delay()

```csharp
await Task.Delay(2000);
```

means:

> Asynchronously wait for approximately 2 seconds.

It does **not** mean:

```text
Create a new thread and sleep it.
```

It is useful for demonstrations, timing, and some retry/backoff scenarios, but it isn't itself a real database/network operation.

---

# 40. Sequential Async Operations

Suppose:

```csharp
Task<string> task1 = GetEmployeeAsync();
Task<string> task2 = GetDepartmentAsync();
```

If you do:

```csharp
string employee = await task1;
string department = await task2;
```

the operations may already have been started before the first `await`, but the exact behavior depends on how the methods initiate their work.

A clearer sequential pattern is:

```csharp
string employee = await GetEmployeeAsync();

string department = await GetDepartmentAsync();
```

Here the second operation is started only after the first has completed.

---

# 41. Task.WhenAll()

`Task.WhenAll()` is used when multiple asynchronous operations can run concurrently.

Example:

```csharp
Task<string> employeeTask = GetEmployeeAsync();
Task<string> departmentTask = GetDepartmentAsync();

await Task.WhenAll(employeeTask, departmentTask);

string employee = await employeeTask;
string department = await departmentTask;
```

Conceptually:

```text
Employee API ────────┐
                     ├── WhenAll
Department API ──────┘
```

This is often useful when operations are independent.

---

# 42. Better WhenAll Pattern

You can also write:

```csharp
Task<string> employeeTask = GetEmployeeAsync();
Task<string> departmentTask = GetDepartmentAsync();

string[] results =
    await Task.WhenAll(employeeTask, departmentTask);
```

Now:

```text
results[0] → employee
results[1] → department
```

---

# 43. Real-World WhenAll Example

Imagine an API needs:

```text
Employee data
Department data
Project data
```

They don't depend on each other.

Instead of:

```csharp
var employees = await GetEmployeesAsync();
var departments = await GetDepartmentsAsync();
var projects = await GetProjectsAsync();
```

you can start them together:

```csharp
Task<List<Employee>> employeesTask =
    GetEmployeesAsync();

Task<List<Department>> departmentsTask =
    GetDepartmentsAsync();

Task<List<Project>> projectsTask =
    GetProjectsAsync();

await Task.WhenAll(
    employeesTask,
    departmentsTask,
    projectsTask);

List<Employee> employees = await employeesTask;
List<Department> departments = await departmentsTask;
List<Project> projects = await projectsTask;
```

For independent I/O operations, this can reduce total waiting time.

---

# 44. Task.WhenAny()

`Task.WhenAny()` completes when **any one** of the supplied tasks completes.

Example:

```csharp
Task<string> task1 = GetEmployeeAsync();
Task<string> task2 = GetDepartmentAsync();

Task completedTask =
    await Task.WhenAny(task1, task2);
```

Conceptually:

```text
Task 1 ────────────────┐
                       ├── First completed
Task 2 ────────┐       │
               └───────┘
```

---

# 45. WhenAny Use Cases

Useful scenarios include:

```text
First response wins
Timeout patterns
Race multiple independent sources
Wait for whichever task finishes first
```

Example:

```csharp
Task apiTask = GetDataAsync();
Task timeoutTask = Task.Delay(5000);

Task completed =
    await Task.WhenAny(apiTask, timeoutTask);

if (completed == timeoutTask)
{
    Console.WriteLine("Operation timed out.");
}
else
{
    Console.WriteLine("API completed.");
}
```

For production code, prefer APIs designed for cancellation/timeouts when available, but understanding `WhenAny` is important.

---

# 46. CancellationToken

Sometimes we need to cancel an asynchronous operation.

Example:

```text
User cancels request
        ↓
CancellationToken
        ↓
Long-running operation stops
```

Create:

```csharp
CancellationTokenSource cts =
    new CancellationTokenSource();
```

Get token:

```csharp
CancellationToken token = cts.Token;
```

Cancel:

```csharp
cts.Cancel();
```

---

# 47. CancellationToken Example

```csharp
static async Task DoWorkAsync(
    CancellationToken cancellationToken)
{
    for (int i = 1; i <= 10; i++)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Console.WriteLine($"Processing {i}");

        await Task.Delay(1000, cancellationToken);
    }
}
```

Call:

```csharp
CancellationTokenSource cts =
    new CancellationTokenSource();

try
{
    await DoWorkAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation cancelled.");
}
```

---

# 48. Cancel After a Timeout

Very useful:

```csharp
using CancellationTokenSource cts =
    new CancellationTokenSource();

cts.CancelAfter(TimeSpan.FromSeconds(5));

try
{
    await DoWorkAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation cancelled or timed out.");
}
```

---

# 49. Cancellation is Cooperative

This is important.

Calling:

```csharp
cts.Cancel();
```

does not magically kill arbitrary code.

The operation needs to observe the token.

For example:

```csharp
cancellationToken.ThrowIfCancellationRequested();
```

or pass it to an API that supports cancellation:

```csharp
await Task.Delay(
    1000,
    cancellationToken);
```

So:

> Cancellation in .NET is generally cooperative.

---

# 50. Thread vs Task

This is one of the most important Day 8 interview topics.

## Thread

A thread is an execution path managed by the operating system/runtime.

You can explicitly create one:

```csharp
Thread thread = new Thread(() =>
{
    Console.WriteLine("Running on thread");
});

thread.Start();
```

---

# 51. Task

A `Task` represents an asynchronous operation.

```csharp
Task task = Task.Run(() =>
{
    Console.WriteLine("Running work");
});
```

A Task is **not synonymous with a thread**.

A Task can represent:

* CPU-bound work
* I/O-bound asynchronous work
* Other asynchronous operations

---

# 52. ThreadPool

.NET maintains a pool of reusable worker threads called the **ThreadPool**.

Instead of constantly creating new threads:

```text
Create
Destroy
Create
Destroy
```

the runtime can reuse ThreadPool threads.

This reduces thread creation overhead.

---

# 53. Task.Run()

`Task.Run()` is commonly used to schedule CPU-bound work on the ThreadPool.

Example:

```csharp
Task<int> task = Task.Run(() =>
{
    return CalculateSomething();
});

int result = await task;
```

But don't use `Task.Run()` automatically for every async method.

---

# 54. CPU-bound vs I/O-bound

This distinction is extremely important.

## I/O-bound

Examples:

```text
Database call
HTTP request
File read
Network operation
```

Usually prefer genuine async APIs:

```csharp
await httpClient.GetAsync(url);
await command.ExecuteReaderAsync();
await File.ReadAllTextAsync(path);
```

Don't wrap these unnecessarily in:

```csharp
Task.Run(...)
```

---

## CPU-bound

Examples:

```text
Large calculations
Image processing
Complex data processing
CPU-heavy algorithms
```

`Task.Run()` can sometimes be appropriate to move CPU-bound work to a ThreadPool thread, depending on the application's architecture and responsiveness requirements.

---

# 55. Parallel vs Async

These are often confused.

### Async

Main goal:

> Don't block while waiting for asynchronous operations, especially I/O.

Example:

```csharp
await httpClient.GetAsync(url);
```

### Parallel

Main goal:

> Perform independent CPU work concurrently.

Examples:

```csharp
Parallel.For(...)
Parallel.ForEach(...)
```

or carefully designed task-based CPU parallelism.

---

# 56. Async Does NOT Automatically Mean Parallel

This:

```csharp
await GetEmployeeAsync();
await GetDepartmentAsync();
```

does not mean both operations execute simultaneously.

The code awaits one before starting the next if the second method hasn't been started.

For independent operations:

```csharp
Task<Employee> employeeTask = GetEmployeeAsync();
Task<Department> departmentTask = GetDepartmentAsync();

await Task.WhenAll(
    employeeTask,
    departmentTask);
```

allows them to be in flight concurrently.

---

# 57. Parallel Does NOT Mean Async

You can have:

```csharp
Parallel.ForEach(...)
```

without using `async/await`.

Parallelism and asynchrony solve related but different problems.

Remember:

```text
ASYNC
→ Efficient waiting / non-blocking I/O

PARALLEL
→ Concurrent execution of work
```

---

# 58. Thread → ThreadPool → Task → async/await

Understand the relationship:

```text
Thread
  ↓
Actual execution path

ThreadPool
  ↓
Reusable pool of worker threads

Task
  ↓
Represents an asynchronous operation

async/await
  ↓
Language syntax for composing asynchronous operations
```

Important:

> `async/await` does not mean "create a new thread."

---

# 59. What Happens with await?

Consider:

```csharp
public async Task<string> GetDataAsync()
{
    string result =
        await httpClient.GetStringAsync(url);

    return result;
}
```

Conceptually:

```text
Call method
    ↓
Start asynchronous I/O
    ↓
Await incomplete operation
    ↓
Method yields
    ↓
Thread is not blocked waiting for I/O
    ↓
I/O completes
    ↓
Continuation resumes
    ↓
Return result
```

The compiler transforms the async method into a state machine behind the scenes.

You don't normally need to implement that state machine yourself.

---

# 60. Common Async Mistakes

## Mistake 1 — `.Result`

Avoid:

```csharp
string result =
    GetDataAsync().Result;
```

## Mistake 2 — `.Wait()`

Avoid:

```csharp
GetDataAsync().Wait();
```

These synchronously block the current thread and can cause scalability problems and, in some synchronization-context environments, deadlock scenarios.

Prefer:

```csharp
string result =
    await GetDataAsync();
```

---

# 61. Don't Mix Sync and Async Unnecessarily

Bad pattern:

```csharp
public string GetEmployee()
{
    return GetEmployeeAsync().Result;
}
```

Prefer making the entire call chain asynchronous:

```csharp
public async Task<string> GetEmployeeAsync()
{
    return await GetEmployeeDataAsync();
}
```

Then:

```csharp
string employee =
    await GetEmployeeAsync();
```

This is commonly called:

> **Async all the way**

---

# 62. Async Exception Handling

Exceptions from an awaited Task can be handled with normal `try/catch`.

```csharp
try
{
    string result = await GetDataAsync();

    Console.WriteLine(result);
}
catch (HttpRequestException ex)
{
    Console.WriteLine(ex.Message);
}
```

This is one reason `Task` is preferable to `async void`: callers can await the operation and observe its completion/failure.

---

# 63. Async + Cancellation + Exception

A practical pattern:

```csharp
try
{
    await ProcessAsync(cancellationToken);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation was cancelled.");
}
catch (Exception ex)
{
    Console.WriteLine(
        $"Unexpected error: {ex.Message}");
}
```

In ASP.NET Core, you would generally avoid swallowing exceptions silently; log them or allow centralized exception handling to process them appropriately.

---

# 64. Practical Day 8 Console Project

For Visual Studio:

```text id="2dxj2k"
DotNetHandsOn
│
└── Day08ExceptionAsync
    └── Program.cs
```

Use this complete example:

```csharp
using System.Net.Http;

try
{
    Console.WriteLine("Starting application...");

    string result = await GetDataAsync();

    Console.WriteLine(result);
}
catch (HttpRequestException ex)
{
    Console.WriteLine(
        $"HTTP Error: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine(
        $"Unexpected Error: {ex.Message}");
}
finally
{
    Console.WriteLine("Application finished.");
}

static async Task<string> GetDataAsync()
{
    await Task.Delay(1000);

    return "Data received successfully.";
}
```

---

# 65. Practical `WhenAll` Example

```csharp
static async Task<string> GetEmployeeAsync()
{
    await Task.Delay(2000);
    return "Employee data";
}

static async Task<string> GetDepartmentAsync()
{
    await Task.Delay(2000);
    return "Department data";
}

Task<string> employeeTask = GetEmployeeAsync();
Task<string> departmentTask = GetDepartmentAsync();

await Task.WhenAll(
    employeeTask,
    departmentTask);

Console.WriteLine(await employeeTask);
Console.WriteLine(await departmentTask);
```

Conceptually:

```text
Sequential:
2 sec + 2 sec
≈ 4 sec

Concurrent I/O:
2 sec
≈ maximum of the two waits
```

The exact elapsed time depends on the operations and environment.

---

# 66. Practical Cancellation Example

```csharp
using CancellationTokenSource cts =
    new CancellationTokenSource();

cts.CancelAfter(TimeSpan.FromSeconds(3));

try
{
    await ProcessDataAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine(
        "Processing was cancelled.");
}

static async Task ProcessDataAsync(
    CancellationToken cancellationToken)
{
    for (int i = 1; i <= 10; i++)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Console.WriteLine($"Processing item {i}");

        await Task.Delay(
            1000,
            cancellationToken);
    }
}
```

You should understand exactly what is happening:

```text
Start processing
      ↓
Token has 3-second timeout
      ↓
Process item 1
      ↓
Process item 2
      ↓
Process item 3
      ↓
Cancellation requested
      ↓
OperationCanceledException
```

---

# 67. Exception Handling — Quick Revision

```text
try
→ Code that may throw

catch
→ Handle exception

finally
→ Cleanup

throw
→ Explicitly throw/rethrow
```

Hierarchy:

```text
Exception
├── SystemException
│   ├── NullReferenceException
│   ├── InvalidOperationException
│   ├── ArgumentException
│   └── ...
│
└── ApplicationException
```

For new custom exceptions:

```csharp
class EmployeeNotFoundException : Exception
{
}
```

Prefer:

```csharp
throw;
```

over:

```csharp
throw ex;
```

when rethrowing the current exception.

---

# 68. Async Quick Revision

```text
Task
→ Represents asynchronous operation

Task<T>
→ Async operation returning T

async
→ Allows await in method

await
→ Asynchronously waits for operation

Task.WhenAll()
→ Wait for all

Task.WhenAny()
→ Wait for first completed

CancellationToken
→ Cooperative cancellation
```

---

# 69. Thread / Task / ThreadPool Cheat Sheet

```text
THREAD
→ Execution path

THREADPOOL
→ Reusable pool of worker threads

TASK
→ Represents an operation

async/await
→ Syntax/model for composing asynchronous operations
```

Don't say in an interview:

> "`async` creates a new thread."

That's incorrect.

---

# 70. Most Important Interview Questions

### Exception Handling

**1. What is an exception?**

An object representing an error or exceptional condition occurring during program execution.

**2. What is the difference between `throw` and `throw ex`?**

```text
throw
→ preserves original stack trace

throw ex
→ should generally be avoided when rethrowing
```

**3. Why use finally?**

For cleanup code that should normally execute regardless of whether an exception occurs.

**4. Can we have multiple catch blocks?**

Yes.

**5. Which catch should come first?**

More specific exception types should come before broader types.

**6. What is a custom exception?**

An application-specific exception type derived from `Exception`.

**7. What is an exception filter?**

A condition using `when` that determines whether a catch block handles the exception.

Example:

```csharp
catch (Exception ex) when (ex is IOException)
{
}
```

**8. What is global exception handling?**

Centralized handling of unhandled application exceptions, commonly through ASP.NET Core exception-handling middleware/handlers.

---

### Async

**9. What is a Task?**

An object representing an asynchronous operation.

**10. What is `Task<T>`?**

An asynchronous operation that eventually produces a value of type `T`.

**11. Does async create a new thread?**

No.

**12. Difference between Task and Thread?**

A Thread is an execution path; a Task represents an asynchronous operation and may or may not involve a dedicated thread.

**13. What is ThreadPool?**

A pool of reusable worker threads managed by .NET.

**14. What does await do?**

It asynchronously waits for an awaitable operation and resumes the method when the operation completes.

**15. What is Task.WhenAll?**

Waits for multiple tasks to complete.

**16. What is Task.WhenAny?**

Completes when one of the supplied tasks completes.

**17. What is CancellationToken?**

A mechanism for cooperative cancellation of asynchronous operations.

**18. Difference between async and parallel programming?**

```text
Async
→ especially useful for non-blocking I/O

Parallel
→ concurrent execution of independent work,
  particularly CPU-bound work
```

**19. Why avoid `.Result` and `.Wait()`?**

They synchronously block and can cause scalability issues and, in some environments, deadlock risks. Prefer `await`.

**20. What is `async void`?**

An asynchronous method returning no Task; mainly intended for event handlers. Application/service methods should normally return `Task` or `Task<T>`.

---

# 71. Day 8 — Final Mental Model

```text
                 DAY 8
                   │
        ┌──────────┴──────────┐
        ↓                     ↓
   EXCEPTIONS              ASYNC
        │                     │
   try/catch              Task
   finally                Task<T>
   throw                  async
   filters                await
   custom exceptions      WhenAll
   global handling        WhenAny
        │                 Cancellation
        │                     │
        ↓                     ↓
    Safe error          Non-blocking I/O
    handling            + scalability
        │                     │
        └──────────┬──────────┘
                   ↓
             ASP.NET CORE
                   ↓
              WEB API
                   ↓
        Controllers / Services
                   ↓
       Database / HTTP / Redis
```

### The key concepts to be able to explain without notes

```text
1. try / catch / finally
2. throw vs throw ex
3. Exception hierarchy
4. Custom exception
5. Exception filter
6. Global exception handling
7. Synchronous vs asynchronous
8. Task vs Task<T>
9. async / await
10. Task.WhenAll
11. Task.WhenAny
12. CancellationToken
13. Thread vs Task
14. ThreadPool
15. CPU-bound vs I/O-bound
16. Async vs Parallel
17. IEnumerable vs IQueryable from Day 7
```

The most important connection to **Day 9 — ASP.NET Core fundamentals** is:

```text
HTTP Request
     ↓
ASP.NET Core
     ↓
Controller
     ↓
Service
     ↓
Async Database/API call
     ↓
await
     ↓
Result
     ↓
HTTP Response
```

And when something fails:

```text
Exception
     ↓
Service / Controller
     ↓
Global Exception Handler
     ↓
Consistent HTTP error response
```

This is the foundation you will use when you start building **ASP.NET Core Web APIs**.
