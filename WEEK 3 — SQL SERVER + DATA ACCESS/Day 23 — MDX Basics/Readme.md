# Day 23 — MDX Basics

Today we move from traditional SQL Server querying into **SSAS / Analysis Services** and **MDX (Multidimensional Expressions)**.

The important thing to understand is:

```text
SQL
 ↓
Tables / Rows / Columns
```

whereas:

```text
MDX
 ↓
Cubes / Dimensions / Hierarchies / Members / Measures
```

And in your .NET architecture:

```text
ASP.NET Core API
        ↓
Service / BAL
        ↓
ADOMD Client
        ↓
SQL Server Analysis Services
        ↓
MDX
        ↓
Result
        ↓
ASP.NET Core Response
```

---

# 1. What is MDX?

**MDX = Multidimensional Expressions.**

MDX is a query language primarily used with **Analysis Services multidimensional cubes**.

SQL works mainly with relational tables:

```text
Employees
Departments
Sales
Products
```

MDX works with multidimensional analytical structures:

```text
Cube
 ├── Measures
 ├── Dimensions
 │    ├── Product
 │    ├── Date
 │    └── Region
 └── Hierarchies
```

The main purpose of MDX is analytical querying.

For example:

> What were total sales by product and year?

Instead of thinking:

```text
SELECT columns
FROM table
GROUP BY ...
```

you think:

```text
Measures
+
Dimensions
+
Members
+
Tuples
+
Sets
```

---

# 2. SQL vs MDX

### SQL

```sql
SELECT
    ProductName,
    SUM(SalesAmount)
FROM Sales
GROUP BY ProductName;
```

Conceptually:

```text
Table
 ↓
Rows
 ↓
Columns
 ↓
GROUP BY
 ↓
Aggregate
```

### MDX

Conceptually:

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Product].[Product].[Product].Members ON ROWS
FROM [Sales Cube];
```

Here:

```text
Measure
+
Dimension Members
```

are used to produce the analytical result.

---

# 3. What is a Cube?

A **cube** is a multidimensional analytical model containing measures and dimensions.

Imagine a sales cube:

```text
                    Time
                     │
                     │
                     ▼
              ┌─────────────┐
             /│             /│
            / │            / │
           ┌─────────────┐   │
           │  │          │   │
           │  └──────────│───┘
           │ /           │  /
           │/            │ /
           └─────────────┘
          Product       Region
```

We can analyze:

```text
Sales
 ↓
by Product
by Year
by Region
by Month
```

A cube does **not** mean that there are literally only three dimensions.

A cube can contain many dimensions.

For example:

```text
Sales Cube
│
├── Date
├── Product
├── Customer
├── Region
├── Store
└── Promotion
```

---

# 4. What is a Dimension?

A **dimension** describes the perspective from which you analyze data.

Examples:

```text
Date
Product
Region
Customer
Employee
Store
```

For sales:

```text
Measure:
Sales Amount

Dimensions:
Product
Date
Region
```

So we can ask:

```text
Sales by Product
Sales by Year
Sales by Region
```

---

# 5. What is a Hierarchy?

A hierarchy organizes members from higher-level categories to lower-level details.

For example, Date:

```text
Date
│
└── Calendar Hierarchy
     │
     ├── Year
     │    └── 2026
     │
     ├── Quarter
     │    └── Q1
     │
     ├── Month
     │    └── January
     │
     └── Day
          └── 15
```

Another example:

```text
Geography
│
└── Geography Hierarchy
     │
     ├── Country
     │
     ├── State
     │
     └── City
```

Example:

```text
India
 └── Karnataka
      └── Bengaluru
```

---

# 6. What is a Level?

A **level** is a particular position in a hierarchy.

For example:

```text
Date Hierarchy

Year
Quarter
Month
Day
```

These are levels.

Example:

```text
[Date].[Calendar].[Year]
[Date].[Calendar].[Quarter]
[Date].[Calendar].[Month]
[Date].[Calendar].[Day]
```

Think:

```text
Hierarchy = complete structure

Level = one stage in that structure
```

---

# 7. What is a Member?

A **member** is an individual value within a hierarchy level.

For example:

```text
Year Level
 ├── 2024
 ├── 2025
 └── 2026
```

`2026` is a member.

For month:

```text
Month Level
 ├── January
 ├── February
 ├── March
 └── ...
```

`January` is a member.

Example MDX member:

```text
[Date].[Calendar].[Year].&[2026]
```

The exact unique-member syntax depends on how the SSAS dimension is modeled.

---

# 8. What is a Measure?

A **measure** is a numeric value that we analyze.

Examples:

```text
Sales Amount
Order Quantity
Profit
Cost
Discount
```

For example:

```text
[Measures].[Sales Amount]
```

If the cube contains:

```text
Sales Amount = 5,000,000
```

that is a measure.

---

# 9. Measure Group

A **measure group** organizes related measures around a fact table/fact source in a multidimensional SSAS model.

For example:

```text
Sales Measure Group
│
├── Sales Amount
├── Order Quantity
├── Discount Amount
└── Cost
```

Another:

```text
Inventory Measure Group
│
├── Inventory Quantity
├── Inventory Value
└── Units Sold
```

Think:

```text
Measure
    ↓
Individual metric

Measure Group
    ↓
Collection of related metrics
```

---

# 10. Complete MDX Vocabulary

You should remember this hierarchy:

```text
Cube
 │
 ├── Dimensions
 │     │
 │     └── Hierarchies
 │            │
 │            └── Levels
 │                   │
 │                   └── Members
 │
 └── Measure Groups
        │
        └── Measures
```

And MDX queries use:

```text
Members
Tuples
Sets
```

---

# 11. What is a Tuple?

A **tuple** identifies one cell in a multidimensional cube.

A tuple is a combination of members from dimensions.

For example:

```text
Sales Amount
+
2026
+
India
+
Laptop
```

Conceptually:

```text
(
    [Measures].[Sales Amount],
    [Date].[Calendar].[Year].&[2026],
    [Region].[Region].[India],
    [Product].[Product].[Laptop]
)
```

This identifies a specific point/cell in the cube.

### Important

A tuple generally contains **one member from each relevant hierarchy**.

---

# 12. What is a Set?

A **set** is a collection of tuples.

For example:

```text
2024
2025
2026
```

can form a set:

```text
{
    2024,
    2025,
    2026
}
```

In MDX:

```text
[Date].[Calendar].[Year].Members
```

returns the members of the Year level/hierarchy depending on the exact hierarchy structure.

---

# 13. Tuple vs Set

This is a very common interview question.

### Tuple

One point:

```text
(Product = Laptop, Year = 2026)
```

### Set

Multiple points:

```text
{
    (Laptop, 2024),
    (Laptop, 2025),
    (Laptop, 2026)
}
```

Memory:

```text
Tuple = one combination

Set = collection of tuples
```

---

# 14. Basic MDX SELECT

The basic structure is:

```text
SELECT
    <expression> ON COLUMNS,
    <expression> ON ROWS
FROM
    [Cube]
WHERE
    <expression>
```

Example:

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Product].[Product].[Product].Members ON ROWS
FROM
    [Sales Cube];
```

Conceptually:

```text
             Sales Amount
                  │
                  ▼
Product       Sales
─────────     ───────
Laptop         50000
Mobile         70000
Tablet         30000
```

---

# 15. ON COLUMNS

`ON COLUMNS` defines what appears as columns in the result.

Example:

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS
FROM
    [Sales Cube];
```

Result conceptually:

```text
Sales Amount
────────────
5000000
```

---

# 16. ON ROWS

`ON ROWS` defines what appears as rows.

Example:

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Product].[Product].[Product].Members ON ROWS
FROM
    [Sales Cube];
```

Conceptually:

```text
Product       Sales Amount
--------------------------
Laptop          50000
Mobile          70000
Tablet          30000
```

---

# 17. FROM

`FROM` identifies the cube.

Example:

```text
FROM [Sales Cube]
```

Important:

This is **not** necessarily the same idea as a SQL table.

You are querying a cube.

---

# 18. WHERE

`WHERE` is used for slicing/filtering the cube context.

Example:

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Product].[Product].[Product].Members ON ROWS
FROM
    [Sales Cube]
WHERE
    [Date].[Calendar].[Year].&[2026];
```

This means:

> Show product sales for 2026.

---

# 19. Sales by Product

Suppose our cube contains:

```text
[Sales Cube]

Measures:
    Sales Amount

Product:
    Laptop
    Mobile
    Tablet
```

Query:

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Product].[Product].[Product].Members ON ROWS
FROM
    [Sales Cube];
```

Conceptual result:

```text
Product       Sales Amount
--------------------------
Laptop          500000
Mobile          750000
Tablet          300000
```

---

# 20. Sales by Year

Query:

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Date].[Calendar].[Year].Members ON ROWS
FROM
    [Sales Cube];
```

Result:

```text
Year       Sales Amount
-----------------------
2024         800000
2025        1200000
2026        1500000
```

---

# 21. Sales by Region

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Region].[Region].[Region].Members ON ROWS
FROM
    [Sales Cube];
```

Conceptually:

```text
Region       Sales
------------------
India        900000
USA          700000
UK           400000
```

---

# 22. Sales by Month

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Date].[Calendar].[Month].Members ON ROWS
FROM
    [Sales Cube];
```

Result:

```text
Month        Sales
------------------
January      100000
February     120000
March        150000
April        140000
```

---

# 23. Multiple Measures

You can put multiple measures on columns.

For example:

```text
Sales Amount
Order Quantity
Profit
```

Conceptually:

```text
SELECT
{
    [Measures].[Sales Amount],
    [Measures].[Order Quantity],
    [Measures].[Profit]
} ON COLUMNS,
[Product].[Product].[Product].Members ON ROWS
FROM [Sales Cube];
```

Result:

```text
Product     Sales     Quantity     Profit
-----------------------------------------
Laptop      500000       1000      100000
Mobile      750000       1500      180000
Tablet      300000        600       70000
```

Notice the use of:

```text
{ ... }
```

That represents a **set**.

---

# 24. Filtering with WHERE

Suppose:

```text
Year:
2024
2025
2026
```

To get only 2026:

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Product].[Product].[Product].Members ON ROWS
FROM
    [Sales Cube]
WHERE
    [Date].[Calendar].[Year].&[2026];
```

---

# 25. Sales by Product for One Year

This combines concepts.

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Product].[Product].[Product].Members ON ROWS
FROM
    [Sales Cube]
WHERE
    [Date].[Calendar].[Year].&[2026];
```

Meaning:

```text
Rows    → Products
Columns → Sales Amount
Filter  → 2026
```

---

# 26. Sales by Region for One Year

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Region].[Region].[Region].Members ON ROWS
FROM
    [Sales Cube]
WHERE
    [Date].[Calendar].[Year].&[2026];
```

Meaning:

```text
Rows    → Regions
Columns → Sales
Filter  → 2026
```

---

# 27. Sales by Year and Product

Now we want two dimensions in the result.

Conceptually:

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    (
        [Date].[Calendar].[Year].Members,
        [Product].[Product].[Product].Members
    ) ON ROWS
FROM [Sales Cube];
```

However, for practical MDX, combining member sets from different hierarchies generally uses a **crossjoin**.

Example:

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    CrossJoin(
        [Date].[Calendar].[Year].Members,
        [Product].[Product].[Product].Members
    ) ON ROWS
FROM [Sales Cube];
```

Conceptually:

```text
Year    Product     Sales
-------------------------
2025    Laptop      200000
2025    Mobile      300000
2025    Tablet      100000
2026    Laptop      300000
2026    Mobile      450000
2026    Tablet      200000
```

---

# 28. Hierarchies Matter

You may see MDX expressions such as:

```text
[Date].[Calendar].[Year]
```

Break it down:

```text
[Date]
   ↓
Dimension

[Calendar]
   ↓
Hierarchy

[Year]
   ↓
Level
```

Then a specific member might be:

```text
[Date].[Calendar].[Year].&[2026]
```

So:

```text
Dimension
   ↓
Hierarchy
   ↓
Level
   ↓
Member
```

is an extremely important MDX concept.

---

# 29. `.Members`

`.Members` is commonly used to obtain members of a hierarchy/level.

Example:

```text
[Product].[Product].[Product].Members
```

means conceptually:

> Give me the members of this Product level.

Then those members can be placed on rows:

```text
... ON ROWS
```

---

# 30. Measures vs Dimensions

This distinction is extremely important.

### Measure

Usually something numeric that you calculate/analyze:

```text
Sales Amount
Quantity
Profit
Cost
```

### Dimension

Provides the context:

```text
Product
Date
Region
Customer
```

Think:

```text
Dimension → "By what?"

Measure   → "What value?"
```

For:

> Sales by Region

```text
Measure   = Sales
Dimension = Region
```

For:

> Profit by Product

```text
Measure   = Profit
Dimension = Product
```

---

# 31. MDX Mental Model

Suppose someone asks:

> Show sales by product for 2026.

Think:

```text
WHAT?
 ↓
Sales Amount
 ↓
[Measures].[Sales Amount]

BY WHAT?
 ↓
Product
 ↓
[Product].[Product].[Product].Members

FILTER?
 ↓
2026
 ↓
[Date].[Calendar].[Year].&[2026]
```

Then construct:

```text
SELECT
    Sales ON COLUMNS,
    Products ON ROWS
FROM
    Sales Cube
WHERE
    2026;
```

This mental model makes MDX much easier.

---

# 32. MDX vs SQL GROUP BY

SQL:

```sql
SELECT
    Product,
    SUM(SalesAmount)
FROM Sales
GROUP BY Product;
```

MDX:

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Product].[Product].[Product].Members ON ROWS
FROM [Sales Cube];
```

The conceptual difference:

```text
SQL
 ↓
GROUP BY

MDX
 ↓
Dimension members on an axis
```

---

# 33. MDX Axes

The most common axes you will initially use are:

```text
ON COLUMNS
ON ROWS
```

You can think:

```text
COLUMNS → X-axis
ROWS    → Y-axis
```

For example:

```text
             Sales
              ↓
        ┌─────────────┐
Product │             │
   ↓    │    Values   │
        │             │
        └─────────────┘
```

---

# 34. A Simple Cube Example

Imagine:

```text
Sales Cube
│
├── Measures
│   ├── Sales Amount
│   ├── Quantity
│   └── Profit
│
├── Product
│   └── Product
│       └── Product
│
├── Date
│   └── Calendar
│       ├── Year
│       ├── Quarter
│       ├── Month
│       └── Day
│
└── Region
    └── Region
```

Then:

```text
Sales by Product
```

uses:

```text
Measure = Sales Amount
Dimension = Product
```

while:

```text
Sales by Year
```

uses:

```text
Measure = Sales Amount
Dimension = Date
Level = Year
```

---

# 35. ADOMD Client

Now we connect the .NET API to Analysis Services.

**ADOMD.NET** provides client APIs for communicating with Analysis Services.

The common namespace is:

```csharp
Microsoft.AnalysisServices.AdomdClient
```

The main object you'll work with is:

```text
AdomdConnection
```

and queries are executed through:

```text
AdomdCommand
```

---

# 36. Install ADOMD Client

In Visual Studio:

```text
Project
→ Manage NuGet Packages
→ Browse
→ Microsoft.AnalysisServices.AdomdClient
```

Install the package appropriate for your project/runtime and Analysis Services environment.

---

# 37. ADOMD Architecture

Your application becomes:

```text
Angular / Client
       ↓
ASP.NET Core Controller
       ↓
Service
       ↓
ADOMD Client
       ↓
AdomdConnection
       ↓
Analysis Services
       ↓
MDX
       ↓
AdomdDataReader
       ↓
Service
       ↓
JSON Response
```

---

# 38. Connection String

The exact connection string depends on whether you are connecting to:

* SQL Server Analysis Services
* Azure Analysis Services
* another supported Analysis Services environment
* specific authentication configuration

Conceptually:

```text
Provider=MSOLAP;
Data Source=<analysis-services-server>;
Initial Catalog=<database>;
```

For example, the application configuration might contain:

```json
{
  "ConnectionStrings": {
    "AnalysisServices": "Provider=MSOLAP;Data Source=YOUR_SERVER;Initial Catalog=YOUR_DATABASE;"
  }
}
```

**Do not hard-code credentials or secrets in source code.**

---

# 39. Create MDX Service Interface

In your ASP.NET Core project:

```text
Services
├── Interfaces
│   └── IMdxService.cs
└── Implementations
    └── MdxService.cs
```

Interface:

```csharp
using System.Data;

namespace EmployeeManagement.Services.Interfaces;

public interface IMdxService
{
    Task<DataTable> GetSalesByProductAsync();

    Task<DataTable> GetSalesByYearAsync();

    Task<DataTable> GetSalesByRegionAsync();

    Task<DataTable> GetSalesByMonthAsync();
}
```

---

# 40. MDX Service

```csharp
using System.Data;
using Microsoft.AnalysisServices.AdomdClient;
using EmployeeManagement.Services.Interfaces;

namespace EmployeeManagement.Services.Implementations;

public class MdxService : IMdxService
{
    private readonly string _connectionString;

    public MdxService(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString(
                "AnalysisServices")
            ?? throw new InvalidOperationException(
                "Analysis Services connection string is missing.");
    }

    public async Task<DataTable>
        GetSalesByProductAsync()
    {
        const string mdx = """
            SELECT
                [Measures].[Sales Amount] ON COLUMNS,
                [Product].[Product].[Product].Members ON ROWS
            FROM [Sales Cube];
            """;

        return await ExecuteMdxAsync(mdx);
    }

    public async Task<DataTable>
        GetSalesByYearAsync()
    {
        const string mdx = """
            SELECT
                [Measures].[Sales Amount] ON COLUMNS,
                [Date].[Calendar].[Year].Members ON ROWS
            FROM [Sales Cube];
            """;

        return await ExecuteMdxAsync(mdx);
    }

    public async Task<DataTable>
        GetSalesByRegionAsync()
    {
        const string mdx = """
            SELECT
                [Measures].[Sales Amount] ON COLUMNS,
                [Region].[Region].[Region].Members ON ROWS
            FROM [Sales Cube];
            """;

        return await ExecuteMdxAsync(mdx);
    }

    public async Task<DataTable>
        GetSalesByMonthAsync()
    {
        const string mdx = """
            SELECT
                [Measures].[Sales Amount] ON COLUMNS,
                [Date].[Calendar].[Month].Members ON ROWS
            FROM [Sales Cube];
            """;

        return await ExecuteMdxAsync(mdx);
    }

    private async Task<DataTable>
        ExecuteMdxAsync(string mdx)
    {
        var table = new DataTable();

        await using var connection =
            new AdomdConnection(_connectionString);

        await connection.OpenAsync();

        await using var command =
            new AdomdCommand(mdx, connection);

        await using var reader =
            await command.ExecuteReaderAsync();

        table.Load(reader);

        return table;
    }
}
```

> The exact cube, dimension, hierarchy, level, and measure names must match your actual SSAS model. Names such as `[Sales Cube]` and `[Measures].[Sales Amount]` above are examples.

---

# 41. Controller for MDX

Create:

```text
Controllers
└── AnalyticsController.cs
```

```csharp
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IMdxService _mdxService;

    public AnalyticsController(
        IMdxService mdxService)
    {
        _mdxService = mdxService;
    }

    [HttpGet("sales-by-product")]
    public async Task<IActionResult> GetSalesByProduct()
    {
        var result =
            await _mdxService.GetSalesByProductAsync();

        return Ok(result);
    }

    [HttpGet("sales-by-year")]
    public async Task<IActionResult> GetSalesByYear()
    {
        var result =
            await _mdxService.GetSalesByYearAsync();

        return Ok(result);
    }

    [HttpGet("sales-by-region")]
    public async Task<IActionResult> GetSalesByRegion()
    {
        var result =
            await _mdxService.GetSalesByRegionAsync();

        return Ok(result);
    }

    [HttpGet("sales-by-month")]
    public async Task<IActionResult> GetSalesByMonth()
    {
        var result =
            await _mdxService.GetSalesByMonthAsync();

        return Ok(result);
    }
}
```

---

# 42. Register the Service

In `Program.cs`:

```csharp
builder.Services.AddScoped<
    IMdxService,
    MdxService>();
```

So your DI becomes:

```text
AnalyticsController
        ↓
IMdxService
        ↓
MdxService
        ↓
AdomdConnection
        ↓
Analysis Services
```

---

# 43. API Endpoints

Now you can expose:

```text
GET /api/analytics/sales-by-product
```

```text
GET /api/analytics/sales-by-year
```

```text
GET /api/analytics/sales-by-region
```

```text
GET /api/analytics/sales-by-month
```

---

# 44. Complete MDX API Flow

For:

```text
GET /api/analytics/sales-by-product
```

the flow is:

```text
Client
   ↓
GET /api/analytics/sales-by-product
   ↓
AnalyticsController
   ↓
IMdxService
   ↓
MdxService
   ↓
MDX Query
   ↓
AdomdCommand
   ↓
AdomdConnection
   ↓
Analysis Services
   ↓
Cube
   ↓
Measures + Dimensions
   ↓
Result
   ↓
DataReader
   ↓
DataTable
   ↓
JSON
   ↓
Client
```

---

# 45. SQL Server vs Analysis Services

This distinction is very important for interviews.

### SQL Server Database

```text
Database
 ↓
Tables
 ↓
Rows
 ↓
Columns
```

Queries:

```text
SQL
```

Example:

```sql
SELECT *
FROM Sales;
```

### Analysis Services Multidimensional

```text
Cube
 ↓
Dimensions
 ↓
Hierarchies
 ↓
Levels
 ↓
Members
 ↓
Measures
```

Queries:

```text
MDX
```

Example:

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Product].[Product].[Product].Members ON ROWS
FROM [Sales Cube];
```

---

# 46. Important MDX Terms — Easy Memory

Remember this:

```text
CUBE
 ↓
Contains analytical data

DIMENSION
 ↓
Perspective / category

HIERARCHY
 ↓
Organization within a dimension

LEVEL
 ↓
Position in hierarchy

MEMBER
 ↓
Individual value

MEASURE
 ↓
Numeric business value

MEASURE GROUP
 ↓
Group of related measures

TUPLE
 ↓
One coordinate/cell

SET
 ↓
Collection of tuples
```

---

# 47. Real-World Example

Suppose the business asks:

> Show total sales for laptops in Karnataka during 2026.

Break the requirement down:

```text
Measure
    ↓
Sales Amount

Product Member
    ↓
Laptop

Region Member
    ↓
Karnataka

Year Member
    ↓
2026
```

Conceptually, you're identifying a particular cube cell using multiple dimensions.

That is the power of multidimensional analysis.

---

# 48. MDX Practice Queries

Practice these in order.

### Query 1 — Total Sales

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS
FROM [Sales Cube];
```

### Query 2 — Sales by Product

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Product].[Product].[Product].Members ON ROWS
FROM [Sales Cube];
```

### Query 3 — Sales by Year

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Date].[Calendar].[Year].Members ON ROWS
FROM [Sales Cube];
```

### Query 4 — Sales by Region

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Region].[Region].[Region].Members ON ROWS
FROM [Sales Cube];
```

### Query 5 — Sales by Month

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Date].[Calendar].[Month].Members ON ROWS
FROM [Sales Cube];
```

### Query 6 — Product Sales for 2026

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    [Product].[Product].[Product].Members ON ROWS
FROM [Sales Cube]
WHERE
    [Date].[Calendar].[Year].&[2026];
```

### Query 7 — Multiple Measures

```text
SELECT
{
    [Measures].[Sales Amount],
    [Measures].[Order Quantity],
    [Measures].[Profit]
} ON COLUMNS,
[Product].[Product].[Product].Members ON ROWS
FROM [Sales Cube];
```

### Query 8 — Product × Year

```text
SELECT
    [Measures].[Sales Amount] ON COLUMNS,
    CrossJoin(
        [Date].[Calendar].[Year].Members,
        [Product].[Product].[Product].Members
    ) ON ROWS
FROM [Sales Cube];
```

---

# 49. Common Beginner Mistakes

### Mistake 1 — Thinking MDX is SQL

Don't think:

```text
MDX = SQL with different syntax
```

Instead:

```text
SQL → relational data

MDX → multidimensional analytical model
```

---

### Mistake 2 — Confusing dimension and measure

Wrong mental model:

```text
Sales = Dimension
```

Normally:

```text
Sales = Measure
```

while:

```text
Product = Dimension
Region = Dimension
Date = Dimension
```

---

### Mistake 3 — Confusing member and level

```text
Year
```

is a level.

```text
2026
```

is a member of that level.

---

### Mistake 4 — Confusing tuple and set

```text
Tuple → one coordinate
Set   → collection of tuples
```

---

### Mistake 5 — Assuming cube means only 3 dimensions

A cube can have many dimensions:

```text
Date
Product
Customer
Region
Store
Promotion
```

The term "cube" represents the multidimensional model, not a literal three-dimensional geometric object.

---

# 50. Day 23 Interview Questions

### 1. What is MDX?

MDX stands for Multidimensional Expressions and is used to query multidimensional Analysis Services cubes.

### 2. What is a cube?

A multidimensional analytical model containing dimensions and measures.

### 3. What is a dimension?

A perspective or category used to analyze measures.

### 4. What is a hierarchy?

An organizational structure within a dimension.

Example:

```text
Year
 ↓
Quarter
 ↓
Month
 ↓
Day
```

### 5. What is a level?

A particular stage within a hierarchy.

### 6. What is a member?

An individual value belonging to a level.

Example:

```text
Level  → Year
Member → 2026
```

### 7. What is a measure?

A numeric analytical value such as:

```text
Sales
Profit
Quantity
Cost
```

### 8. What is a measure group?

A logical group of related measures, usually associated with a fact table/fact source.

### 9. What is a tuple?

A coordinate consisting of members from dimensions/hierarchies that identifies a cube cell.

### 10. What is a set?

A collection of tuples.

### 11. What is `ON COLUMNS`?

It specifies the expressions displayed on the column axis.

### 12. What is `ON ROWS`?

It specifies the expressions displayed on the row axis.

### 13. What does `FROM` specify?

The cube being queried.

### 14. What does `WHERE` do?

It establishes a filter/slicer context for the query.

### 15. What is ADOMD.NET?

A .NET client API used to communicate with Analysis Services and execute analytical queries such as MDX.

---

# 51. Day 23 Final Cheat Sheet

```text
MDX
│
├── Cube
│
├── Dimensions
│   ├── Product
│   ├── Date
│   └── Region
│
├── Hierarchies
│   └── Calendar
│       ├── Year
│       ├── Quarter
│       ├── Month
│       └── Day
│
├── Levels
│
├── Members
│
├── Measure Groups
│   └── Sales
│
└── Measures
    ├── Sales Amount
    ├── Quantity
    └── Profit
```

MDX query pattern:

```text
SELECT
    <MEASURE> ON COLUMNS,
    <DIMENSION MEMBERS> ON ROWS
FROM
    [CUBE]
WHERE
    <FILTER>;
```

And your .NET architecture:

```text
┌───────────────────────────┐
│          Client           │
└─────────────┬─────────────┘
              ↓
┌───────────────────────────┐
│     ASP.NET Core API      │
│       Controller          │
└─────────────┬─────────────┘
              ↓
┌───────────────────────────┐
│        Service/BAL        │
└─────────────┬─────────────┘
              ↓
┌───────────────────────────┐
│       ADOMD Client        │
│     AdomdConnection       │
│       AdomdCommand        │
└─────────────┬─────────────┘
              ↓
┌───────────────────────────┐
│   SQL Server Analysis     │
│        Services           │
└─────────────┬─────────────┘
              ↓
┌───────────────────────────┐
│          Cube             │
│ Measures + Dimensions     │
└─────────────┬─────────────┘
              ↓
             MDX
              ↓
         Result Set
              ↓
          JSON API
```

### The 6 concepts you should be able to explain without notes

```text
Cube       → Analytical model

Dimension  → What we analyze by

Hierarchy  → How a dimension is organized

Level      → One stage in a hierarchy

Member     → One value at a level

Measure    → Numeric value being analyzed
```

And the most important MDX mental model:

```text
WHAT VALUE?
    ↓
MEASURE

BY WHAT?
    ↓
DIMENSION / MEMBERS

WHICH TIME/REGION/PRODUCT?
    ↓
MEMBER / WHERE

RESULT LAYOUT?
    ↓
ON COLUMNS / ON ROWS
```


