# Day 27 — Redis + Caching

Today we learn **Redis**, why it is used in .NET applications, and how to implement the **Cache-Aside pattern** in ASP.NET Core.

The main architecture is:

```text
Client
  ↓
ASP.NET Core API
  ↓
Redis Cache
  ↓
If cache miss
  ↓
SQL Server
```

The main goal is to reduce repeated database queries and improve API response time.

---

# 1. What is Redis?

**Redis** is an in-memory data store commonly used for:

* Caching
* Session/state storage
* Counters
* Distributed locks
* Pub/Sub
* Queues and other fast data-access scenarios

For today's lesson, focus on:

> **Redis as a distributed cache.**

Redis primarily stores data in memory, which makes access very fast.

---

# 2. Why Do We Need a Cache?

Imagine your API has:

```text
GET /api/employees/10
```

Without caching:

```text
Request 1 → SQL Server
Request 2 → SQL Server
Request 3 → SQL Server
Request 4 → SQL Server
Request 5 → SQL Server
```

If the same employee information is requested repeatedly, you're repeatedly querying the database.

With Redis:

```text
Request
  ↓
Redis
  ↓
Found
  ↓
Return
```

SQL Server isn't queried for every request.

---

# 3. Basic Architecture

Without Redis:

```text
Client
  ↓
ASP.NET Core API
  ↓
SQL Server
```

With Redis:

```text
Client
  ↓
ASP.NET Core API
  ↓
Redis
  ↓
SQL Server
```

More accurately:

```text
                   ┌──────────────┐
                   │   Redis      │
                   │    Cache     │
                   └──────▲───────┘
                          │
Client → API ─────────────┤
                          │
                          ↓
                    ┌──────────┐
                    │ SQL      │
                    │ Server   │
                    └──────────┘
```

---

# 4. Redis Is an In-Memory Data Store

Traditional SQL Server:

```text
Application
    ↓
SQL Server
    ↓
Disk + Memory
```

Redis:

```text
Application
    ↓
Redis
    ↓
Memory
```

Because Redis keeps data in memory, reading frequently accessed data can be extremely fast.

However:

> Redis should not automatically be treated as your permanent source of truth.

For an employee management application:

```text
SQL Server
    ↓
Source of truth

Redis
    ↓
Fast temporary copy
```

---

# 5. Redis Key-Value Model

Redis fundamentally works with keys and values.

Example:

```text
Key:
employee:10

Value:
{"id":10,"name":"Arun","department":"IT"}
```

Conceptually:

```text
employee:10
     ↓
Employee JSON
```

Another:

```text
product:1001
     ↓
Product JSON
```

Another:

```text
department:5
     ↓
Department JSON
```

---

# 6. Redis Basic Commands

You should know these commands for the fundamentals.

```text
SET
GET
DEL
EXPIRE
TTL
```

---

# 7. SET

Stores a value.

```text
SET employee:10 "Arun"
```

Conceptually:

```text
employee:10 → Arun
```

Then:

```text
GET employee:10
```

returns:

```text
Arun
```

---

# 8. GET

Retrieves a value.

```text
GET employee:10
```

If the key exists:

```text
Arun
```

If the key doesn't exist:

```text
(nil)
```

---

# 9. DEL

Deletes a key.

```text
DEL employee:10
```

Then:

```text
GET employee:10
```

returns nothing because the key was removed.

---

# 10. EXPIRE

Sets an expiration time.

Example:

```text
SET employee:10 "Arun"
EXPIRE employee:10 60
```

The key expires after **60 seconds**.

Architecture:

```text
SET
 ↓
Value stored
 ↓
60 seconds
 ↓
Key expires
```

---

# 11. TTL

`TTL` tells you how many seconds remain before the key expires.

```text
TTL employee:10
```

Possible result:

```text
42
```

Meaning approximately 42 seconds remain.

If the key doesn't have an expiration:

```text
-1
```

If the key doesn't exist:

```text
-2
```

---

# 12. Complete Redis Command Example

```text
SET employee:10 "Arun"

GET employee:10

EXPIRE employee:10 60

TTL employee:10

DEL employee:10
```

Mental model:

```text
SET    → Store
GET    → Read
DEL    → Delete
EXPIRE → Set expiration
TTL    → Check expiration
```

---

# 13. What is TTL?

TTL = **Time To Live**

It determines how long cached data should remain available.

Example:

```text
employee:10
TTL = 300 seconds
```

After five minutes:

```text
employee:10
     ↓
Expired
     ↓
Redis returns cache miss
```

---

# 14. Why Use Expiration?

Suppose employee information is cached forever.

SQL Server changes:

```text
Salary: 50,000 → 60,000
```

But Redis still contains:

```text
Salary: 50,000
```

Your API could return stale data.

TTL helps ensure cached data eventually expires.

```text
Database
   ↓
Updated data

Redis
   ↓
Old data

TTL
   ↓
Eventually expires
```

---

# 15. What is Cache?

A **cache** is a temporary copy of data stored closer to the application so that frequently requested data can be retrieved faster.

Example:

```text
SQL Server
     ↓
Employee data
     ↓
Redis
     ↓
Frequently requested API data
```

---

# 16. Cache Hit

A **cache hit** occurs when the requested data exists in Redis.

```text
Request
  ↓
Redis
  ↓
Found
  ↓
Cache HIT
  ↓
Return data
```

SQL Server isn't required for that request.

---

# 17. Cache Miss

A **cache miss** occurs when the requested data isn't in Redis.

```text
Request
  ↓
Redis
  ↓
Not Found
  ↓
Cache MISS
  ↓
SQL Server
```

Then we usually store the database result in Redis.

---

# 18. Cache-Aside Pattern

This is the most important caching pattern for today's lesson.

```text
Request
   ↓
Check Redis
   ↓
Found?
  / \
Yes  No
 |    |
 ↓    ↓
Return SQL Server
      ↓
   Store Redis
      ↓
    Return
```

This is called:

> **Cache-Aside**

It is also called **Lazy Loading Cache** in many explanations.

---

# 19. Cache-Aside Step by Step

Suppose:

```text
GET /api/employees/10
```

### Step 1

API creates the Redis key:

```text
employee:10
```

### Step 2

Check Redis.

```text
GET employee:10
```

### Step 3

If found:

```text
Cache HIT
```

Return it.

### Step 4

If not found:

```text
Cache MISS
```

### Step 5

Query SQL Server.

```sql
SELECT *
FROM Employees
WHERE EmployeeId = 10;
```

### Step 6

Serialize the result to JSON.

### Step 7

Store it in Redis.

```text
SET employee:10 <json>
EXPIRE employee:10 300
```

### Step 8

Return the employee to the client.

---

# 20. Real Example

First request:

```text
GET /api/employees/10
```

```text
API
 ↓
Redis
 ↓
MISS
 ↓
SQL Server
 ↓
Employee
 ↓
Redis
 ↓
Client
```

Second request:

```text
GET /api/employees/10
```

```text
API
 ↓
Redis
 ↓
HIT
 ↓
Client
```

SQL Server isn't needed for the second request.

---

# 21. Redis in ASP.NET Core

For ASP.NET Core, Microsoft provides distributed caching abstractions.

The important interface is:

```csharp
IDistributedCache
```

This is useful because your application can work against a common caching abstraction instead of directly coupling every service to Redis APIs.

---

# 22. Redis Package

For a .NET 8 ASP.NET Core project, install the Redis distributed-cache provider:

```text
Microsoft.Extensions.Caching.StackExchangeRedis
```

In Visual Studio:

```text
Tools
 ↓
NuGet Package Manager
 ↓
Manage NuGet Packages
 ↓
Microsoft.Extensions.Caching.StackExchangeRedis
```

Or Package Manager Console:

```powershell
Install-Package Microsoft.Extensions.Caching.StackExchangeRedis
```

---

# 23. StackExchange.Redis

Under the hood, a commonly used .NET Redis client is:

```text
StackExchange.Redis
```

The ASP.NET Core distributed cache integration uses it for Redis-backed caching.

You can work at two levels:

### Application caching abstraction

```csharp
IDistributedCache
```

### Direct Redis functionality

```csharp
IConnectionMultiplexer
```

For your first ASP.NET Core caching implementation, start with:

```text
IDistributedCache
```

---

# 24. Configure Redis

Suppose Redis is running at:

```text
localhost:6379
```

Add to `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Redis": "localhost:6379"
  }
}
```

Then in `Program.cs`:

```csharp
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration.GetConnectionString("Redis");
});
```

Now ASP.NET Core can inject:

```csharp
IDistributedCache
```

into your services.

---

# 25. Complete Program.cs

For today's practice project:

```csharp
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration.GetConnectionString("Redis");
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
```

---

# 26. Create Employee Model

```csharp
public class Employee
{
    public int EmployeeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public decimal Salary { get; set; }
}
```

---

# 27. Create Employee Repository

For today's caching example, assume SQL Server access is already available through your repository layer.

```csharp
public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(int id);
}
```

Implementation:

```csharp
public class EmployeeRepository : IEmployeeRepository
{
    public async Task<Employee?> GetByIdAsync(int id)
    {
        // SQL Server / ADO.NET implementation
        // Example only

        await Task.Delay(100);

        return new Employee
        {
            EmployeeId = id,
            Name = "Arun",
            Department = "IT",
            Salary = 50000
        };
    }
}
```

The `Task.Delay` is only simulating database latency for this demonstration.

---

# 28. Cache Service

Create:

```text
Services
└── EmployeeService.cs
```

Then:

```csharp
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

public class EmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IDistributedCache _cache;

    public EmployeeService(
        IEmployeeRepository repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        string cacheKey = $"employee:{id}";

        var cachedData =
            await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            Console.WriteLine("CACHE HIT");

            return JsonSerializer.Deserialize<Employee>(
                cachedData);
        }

        Console.WriteLine("CACHE MISS");

        var employee =
            await _repository.GetByIdAsync(id);

        if (employee is null)
        {
            return null;
        }

        var json =
            JsonSerializer.Serialize(employee);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow =
                TimeSpan.FromMinutes(5)
        };

        await _cache.SetStringAsync(
            cacheKey,
            json,
            options);

        return employee;
    }
}
```

---

# 29. Understand the Code

This is the key line:

```csharp
string cacheKey = $"employee:{id}";
```

For:

```text
id = 10
```

the key becomes:

```text
employee:10
```

---

# 30. Check Redis

```csharp
var cachedData =
    await _cache.GetStringAsync(cacheKey);
```

If Redis contains:

```text
employee:10
```

we get the JSON.

Then:

```csharp
if (!string.IsNullOrEmpty(cachedData))
```

means:

```text
Cache HIT
```

---

# 31. Deserialize

Redis stores the employee as JSON:

```json
{
  "employeeId": 10,
  "name": "Arun",
  "department": "IT",
  "salary": 50000
}
```

We convert JSON back into C#:

```csharp
JsonSerializer.Deserialize<Employee>(
    cachedData);
```

---

# 32. Cache Miss

If:

```csharp
cachedData
```

is null:

```text
CACHE MISS
```

Then:

```csharp
var employee =
    await _repository.GetByIdAsync(id);
```

SQL Server is queried.

---

# 33. Store in Redis

After getting data from SQL Server:

```csharp
var json =
    JsonSerializer.Serialize(employee);
```

Then:

```csharp
await _cache.SetStringAsync(
    cacheKey,
    json,
    options);
```

Redis now contains the employee.

---

# 34. Configure Expiration

We used:

```csharp
var options = new DistributedCacheEntryOptions
{
    AbsoluteExpirationRelativeToNow =
        TimeSpan.FromMinutes(5)
};
```

Meaning:

```text
Employee cached
      ↓
5 minutes
      ↓
Cache expires
```

---

# 35. Absolute vs Sliding Expiration

Two important concepts:

### Absolute expiration

Expires after a fixed duration.

```text
Cached
  ↓
5 minutes
  ↓
Expires
```

Example:

```csharp
AbsoluteExpirationRelativeToNow =
    TimeSpan.FromMinutes(5);
```

### Sliding expiration

Expiration is extended whenever the cached item is accessed, depending on the cache provider/semantics.

Example:

```csharp
SlidingExpiration =
    TimeSpan.FromMinutes(5);
```

Conceptually:

```text
Request
 ↓
Access cache
 ↓
Expiration window refreshed
```

Use carefully: frequently accessed data can remain cached much longer than expected with sliding expiration.

---

# 36. Create Controller

```csharp
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly EmployeeService _employeeService;

    public EmployeesController(
        EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployee(int id)
    {
        var employee =
            await _employeeService.GetByIdAsync(id);

        if (employee is null)
        {
            return NotFound();
        }

        return Ok(employee);
    }
}
```

---

# 37. Register Services

In `Program.cs`:

```csharp
builder.Services.AddScoped<IEmployeeRepository,
                           EmployeeRepository>();

builder.Services.AddScoped<EmployeeService>();
```

Complete:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddScoped<IEmployeeRepository,
                           EmployeeRepository>();

builder.Services.AddScoped<EmployeeService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
```

---

# 38. Complete Request Flow

First request:

```text
GET /api/employees/10
        ↓
EmployeesController
        ↓
EmployeeService
        ↓
Redis GET employee:10
        ↓
     NOT FOUND
        ↓
EmployeeRepository
        ↓
SQL Server
        ↓
Employee
        ↓
Serialize JSON
        ↓
Redis SET employee:10
        ↓
Return Employee
```

Second request:

```text
GET /api/employees/10
        ↓
EmployeesController
        ↓
EmployeeService
        ↓
Redis GET employee:10
        ↓
      FOUND
        ↓
Deserialize JSON
        ↓
Return Employee
```

---

# 39. Cache Hit vs Cache Miss

### First request

```text
Redis
 ↓
MISS
 ↓
SQL
 ↓
Redis
 ↓
Response
```

### Subsequent request

```text
Redis
 ↓
HIT
 ↓
Response
```

This is the heart of Cache-Aside.

---

# 40. Why Redis Improves Performance

Imagine:

```text
SQL Server query = 100 ms
Redis lookup = much faster
```

If the same data is requested many times:

Without cache:

```text
100 requests
 ×
Database query
```

With cache:

```text
First request
 ↓
Database

Remaining requests
 ↓
Redis
```

The actual performance depends on network, workload, query complexity, Redis deployment, serialization, and many other factors. Redis isn't automatically faster in every scenario, but it is designed for very fast data access.

---

# 41. Redis Should Not Replace SQL Server

This is important.

Don't think:

```text
SQL Server → old
Redis → new database
```

Instead:

```text
SQL Server
 ↓
Primary data store

Redis
 ↓
Cache / fast-access store
```

Example:

```text
Employee record
      ↓
SQL Server = source of truth
      ↓
Redis = cached representation
```

---

# 42. Cache Invalidation

One of the hardest caching problems is:

> What happens when the underlying data changes?

Suppose:

```text
SQL:
Salary = 50,000
```

Redis:

```text
Salary = 50,000
```

Now update SQL:

```text
Salary = 60,000
```

Redis might still contain:

```text
Salary = 50,000
```

This is **stale cache data**.

---

# 43. Delete Cache After Update

A simple strategy:

```text
Update SQL
    ↓
Delete Redis key
```

Example:

```csharp
await _repository.UpdateAsync(employee);

await _cache.RemoveAsync(
    $"employee:{employee.EmployeeId}");
```

Next GET:

```text
Redis
 ↓
MISS
 ↓
SQL Server
 ↓
Latest data
 ↓
Redis
```

---

# 44. Create Employee

After creating:

```text
POST /api/employees
```

You usually don't need to cache the new object immediately unless your design calls for it.

```text
INSERT SQL
 ↓
Return created employee
```

---

# 45. Update Employee

Example:

```text
PUT /api/employees/10
```

Flow:

```text
Request
 ↓
SQL UPDATE
 ↓
Remove employee:10 from Redis
 ↓
Response
```

---

# 46. Delete Employee

Example:

```text
DELETE /api/employees/10
```

Flow:

```text
SQL DELETE
 ↓
Redis DEL employee:10
 ↓
Response
```

This prevents Redis from returning an employee that no longer exists.

---

# 47. Cache Invalidation Strategy

A common pattern:

```text
CREATE
 ↓
SQL INSERT
 ↓
Cache strategy depends on application

UPDATE
 ↓
SQL UPDATE
 ↓
Invalidate cache

DELETE
 ↓
SQL DELETE
 ↓
Invalidate cache
```

For Cache-Aside, invalidating the relevant cache entry after successful database modification is a common approach.

---

# 48. Cache Key Design

Don't use random keys.

Bad:

```text
10
```

Better:

```text
employee:10
```

Even better for larger applications:

```text
employee:v1:10
```

For lists:

```text
employees:page:1:size:20
```

For filtered data:

```text
employees:department:IT
```

Good cache keys should be:

* Predictable
* Unique
* Consistent
* Easy to invalidate

---

# 49. JSON Serialization

Redis doesn't understand your C# object directly.

You can serialize:

```csharp
Employee
```

into:

```json
{
  "employeeId": 10,
  "name": "Arun",
  "department": "IT",
  "salary": 50000
}
```

Then store it.

When retrieving:

```text
JSON
 ↓
Employee object
```

---

# 50. `IDistributedCache`

The important interface:

```csharp
IDistributedCache
```

provides methods such as:

```csharp
GetAsync()
SetAsync()
RemoveAsync()
RefreshAsync()
GetStringAsync()
SetStringAsync()
```

This gives your application a standard abstraction for distributed caching.

---

# 51. Why "Distributed" Cache?

Imagine your API has multiple instances:

```text
              Load Balancer
                    ↓
          ┌─────────┼─────────┐
          ↓         ↓         ↓
       API #1     API #2    API #3
          │         │         │
          └─────────┼─────────┘
                    ↓
                  Redis
```

If each API instance has its own in-memory cache:

```text
API #1 → Cache #1
API #2 → Cache #2
API #3 → Cache #3
```

the cached data isn't shared.

With Redis:

```text
API #1 ──┐
API #2 ──┼──→ Redis
API #3 ──┘
```

all instances can use the same distributed cache.

This is a major reason Redis is useful in scalable web applications.

---

# 52. In-Memory Cache vs Redis

ASP.NET Core also has:

```csharp
IMemoryCache
```

### IMemoryCache

```text
API Instance
 ↓
Local memory
```

### Redis

```text
API Instance 1 ──┐
API Instance 2 ──┼──→ Redis
API Instance 3 ──┘
```

So:

```text
IMemoryCache
→ Local to application instance

Redis
→ Shared/distributed cache
```

---

# 53. When to Use Redis

Good examples:

### Frequently accessed employee details

```text
GET /api/employees/10
```

### Product catalog

```text
GET /api/products
```

### Reference data

```text
Countries
Departments
Categories
```

### Expensive queries

```text
Complex reporting query
        ↓
Cache result
```

### Session/state scenarios

Redis can also support distributed session/state depending on architecture.

---

# 54. When Not to Cache

Avoid blindly caching everything.

Be careful with:

* Highly sensitive information
* Frequently changing data
* Very large objects
* Data requiring strict real-time consistency
* Data with low reuse

Caching adds complexity.

You need to think about:

```text
Consistency
Expiration
Invalidation
Memory usage
Serialization
Failure handling
```

---

# 55. What If Redis Is Down?

Very important production question.

Suppose:

```text
API
 ↓
Redis
 ↓
Unavailable
```

Should the entire application fail?

Not necessarily.

For many cache-aside scenarios, Redis is an optimization rather than the source of truth.

A resilient approach can be:

```text
Redis available
    ↓
Use cache

Redis unavailable
    ↓
Query SQL Server
```

However, the exact fallback strategy depends on the application's requirements.

Don't blindly swallow every Redis exception; monitor and handle failures appropriately.

---

# 56. Cache Stampede

Imagine the cache expires:

```text
employee:10
      ↓
Expires
```

Then 1,000 requests arrive at almost the same time.

All 1,000 requests may do:

```text
Redis MISS
 ↓
SQL Server
```

Now SQL Server receives a huge burst of identical queries.

This is called a **cache stampede** or **thundering herd** problem.

More advanced solutions include:

* Request coalescing
* Locks
* Jittered expiration
* Background refresh
* Pre-warming

You don't need to implement these today, but you should know the concept.

---

# 57. Cache-Aside vs Write-Through

For your roadmap, focus on Cache-Aside.

### Cache-Aside

```text
Read
 ↓
Cache
 ↓
Miss
 ↓
Database
 ↓
Cache
```

Application controls the cache.

### Write-Through

Conceptually:

```text
Application
 ↓
Cache
 ↓
Database
```

Writes go through the cache layer to the backing store according to the implementation.

The exact semantics depend on the caching technology/design.

---

# 58. Practical Redis Testing

If you have Redis running locally, you can test:

```text
SET employee:10 "Arun"
```

Then:

```text
GET employee:10
```

Then:

```text
EXPIRE employee:10 30
```

Then:

```text
TTL employee:10
```

Finally:

```text
DEL employee:10
```

You should observe:

```text
SET
 ↓
GET
 ↓
EXPIRE
 ↓
TTL
 ↓
DEL
```

---

# 59. Visual Studio Project

For your one-month .NET roadmap, create:

```text
DotNetHandsOn
│
└── Day27Redis
    │
    ├── Controllers
    │   └── EmployeesController.cs
    │
    ├── Models
    │   └── Employee.cs
    │
    ├── Services
    │   ├── IEmployeeService.cs
    │   └── EmployeeService.cs
    │
    ├── Repositories
    │   ├── IEmployeeRepository.cs
    │   └── EmployeeRepository.cs
    │
    ├── appsettings.json
    └── Program.cs
```

This fits nicely with the layered architecture you learned earlier.

---

# 60. Day 27 Architecture With Your Existing Layers

You previously learned:

```text
Controller
    ↓
Service / BAL
    ↓
Repository / DAL
    ↓
ADO.NET
    ↓
SQL Server
```

Now Redis sits between the Service and Repository:

```text
                  ┌─────────────┐
                  │   Redis     │
                  │   Cache     │
                  └──────▲──────┘
                         │
Controller
     ↓
Service / BAL
     │
     ├────────→ Redis
     │
     ↓
Repository / DAL
     ↓
ADO.NET
     ↓
SQL Server
```

This is a very important real-world architecture.

---

# 61. Recommended Service Flow

Your service should roughly do:

```csharp
public async Task<Employee?> GetByIdAsync(int id)
{
    // 1. Create cache key

    // 2. Check Redis

    // 3. If found → return cached data

    // 4. If not found → repository

    // 5. Serialize database result

    // 6. Store in Redis with TTL

    // 7. Return result
}
```

The Controller shouldn't contain Redis logic.

Bad:

```text
Controller
 ↓
Redis
 ↓
SQL
```

Better:

```text
Controller
 ↓
Service
 ↓
Redis / Repository
```

---

# 62. Important Cache Rules

### Rule 1

SQL Server remains the source of truth.

### Rule 2

Redis contains cached data.

### Rule 3

Always define an appropriate expiration strategy.

### Rule 4

Invalidate cache when underlying data changes.

### Rule 5

Design predictable cache keys.

### Rule 6

Don't put unnecessary sensitive data into cache.

### Rule 7

Don't assume Redis is always available.

### Rule 8

Don't cache everything.

---

# 63. Day 27 Interview Questions

### 1. What is Redis?

An in-memory data store commonly used for caching and other fast data-access scenarios.

### 2. Why is Redis fast?

It primarily operates on data in memory and is optimized for fast data access.

### 3. What is a cache?

A temporary copy of frequently accessed data used to reduce expensive data retrieval operations.

### 4. What is TTL?

**Time To Live** — how long a cached key remains valid before expiration.

### 5. What is a cache hit?

Requested data is found in the cache.

```text
Redis → Found
```

### 6. What is a cache miss?

Requested data isn't found in the cache.

```text
Redis → Not Found
```

### 7. What is Cache-Aside?

The application first checks the cache. If data isn't present, it reads from the database and then stores the result in the cache.

### 8. Why use Redis instead of `IMemoryCache`?

Redis can act as a shared distributed cache across multiple application instances.

### 9. What is cache invalidation?

Removing or updating stale cached data after the underlying data changes.

### 10. What happens when Redis data expires?

The next request becomes a cache miss and the application can retrieve fresh data from the database.

### 11. What is `IDistributedCache`?

An ASP.NET Core abstraction for distributed caching providers.

### 12. Why serialize objects before storing them?

A Redis string/value cache needs a representable serialized form such as JSON.

### 13. Should Redis replace SQL Server?

Generally no. In a typical cache-aside architecture, SQL Server remains the source of truth and Redis stores temporary cached representations.

### 14. What is a cache stampede?

A situation where many requests simultaneously miss/expire the same cache entry and overwhelm the backing database with duplicate requests.

---

# 64. Day 27 Final Cheat Sheet

```text
REDIS
 ↓
In-memory data store
 ↓
Fast access
 ↓
Commonly used as cache
```

```text
SET
 ↓
Store

GET
 ↓
Read

DEL
 ↓
Delete

EXPIRE
 ↓
Set expiration

TTL
 ↓
Check remaining lifetime
```

### Cache-Aside

```text
Request
   ↓
Redis
   ↓
HIT?
 ┌─┴─┐
YES  NO
 │    │
 ↓    ↓
Return SQL
      ↓
    Redis
      ↓
    Return
```

### ASP.NET Core

```text
Controller
    ↓
Service
    ↓
IDistributedCache
    ↓
Redis
```

Cache miss:

```text
Service
   ↓
Redis MISS
   ↓
Repository
   ↓
ADO.NET
   ↓
SQL Server
   ↓
Service
   ↓
Redis SET
   ↓
Response
```

### Most important concepts for Day 27

```text
Redis
In-memory
Key-value
Cache
TTL
Expiration
Cache Hit
Cache Miss
Cache-Aside
IDistributedCache
Serialization
Cache Invalidation
Distributed Cache
IMemoryCache vs Redis
```

**One-line memory:**

> **Cache-Aside means: check Redis first → if found, return it → if not found, get data from SQL Server → put it into Redis with an expiration → return it.**
