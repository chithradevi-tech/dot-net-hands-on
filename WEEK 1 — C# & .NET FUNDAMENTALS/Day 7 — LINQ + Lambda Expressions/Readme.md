# Day 7 — LINQ + Lambda Expressions in C#

LINQ is used heavily with:

* `List<T>`
* Arrays
* Entity Framework Core
* SQL Server
* ASP.NET Core APIs
* Collections
* Database queries

Today's flow:

```text
Lambda Expressions
        ↓
Func / Action / Predicate
        ↓
LINQ
        ↓
Filtering
Projection
Sorting
Grouping
Joining
Aggregation
        ↓
IEnumerable vs IQueryable
```

---

# 1. What is Lambda Expression?

A **lambda expression** is a short way of writing a function.

Basic syntax:

```csharp
(parameters) => expression
```

Example:

```csharp
x => x.Age > 18
```

This means:

> Take `x` and check whether `x.Age` is greater than 18.

Another example:

```csharp
x => x * 2
```

Means:

> Take `x` and return `x * 2`.

---

# 2. Lambda Without Lambda

Suppose we have:

```csharp
List<int> numbers = new List<int>
{
    10, 20, 30, 40, 50
};
```

If we want numbers greater than 25, traditionally we might write:

```csharp
foreach (int number in numbers)
{
    if (number > 25)
    {
        Console.WriteLine(number);
    }
}
```

With LINQ + lambda:

```csharp
var result = numbers.Where(x => x > 25);
```

Much shorter.

---

# 3. Lambda Parameters

One parameter:

```csharp
x => x * 2
```

Two parameters:

```csharp
(x, y) => x + y
```

Multiple statements:

```csharp
x =>
{
    int result = x * 2;
    return result;
}
```

---

# 4. Lambda with Objects

Suppose:

```csharp
class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public int Age { get; set; }

    public decimal Salary { get; set; }

    public string Department { get; set; } = "";
}
```

Then:

```csharp
employees.Where(x => x.Age > 18);
```

Here:

```text
x
↓
Employee object
```

So:

```csharp
x.Age
```

means:

```text
Employee.Age
```

---

# 5. What is Func?

`Func` represents a method that:

> **Returns a value.**

General syntax:

```csharp
Func<input types, return type>
```

Example:

```csharp
Func<int, int> square = x => x * x;
```

Usage:

```csharp
int result = square(5);

Console.WriteLine(result);
```

Output:

```text
25
```

---

# 6. Func with Two Parameters

```csharp
Func<int, int, int> add =
    (a, b) => a + b;
```

Usage:

```csharp
int result = add(10, 20);

Console.WriteLine(result);
```

Output:

```text
30
```

The last generic type is always the **return type**.

```text
Func<int, int, int>
     ↑    ↑    ↑
     │    │    └── Return type
     │    └────── Parameter
     └─────────── Parameter
```

---

# 7. Func with Objects

```csharp
Func<Employee, bool> isSenior =
    employee => employee.Age >= 30;
```

Usage:

```csharp
bool result = isSenior(employee);
```

This is extremely similar to predicates used by LINQ.

---

# 8. What is Action?

`Action` represents a method that:

> **Does something but returns nothing.**

Example:

```csharp
Action<string> print =
    message => Console.WriteLine(message);
```

Usage:

```csharp
print("Hello");
```

Output:

```text
Hello
```

---

# 9. Action with Multiple Parameters

```csharp
Action<string, int> display =
    (name, age) =>
    {
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Age: {age}");
    };
```

Usage:

```csharp
display("Arun", 25);
```

`Action` always returns:

```text
void
```

---

# 10. Func vs Action

| Func                                  | Action                      |
| ------------------------------------- | --------------------------- |
| Returns a value                       | Doesn't return a value      |
| Last generic parameter is return type | No return type              |
| `Func<int, int>`                      | `Action<int>`               |
| `x => x * 2`                          | `x => Console.WriteLine(x)` |

Easy memory:

```text
Func
↓
Function → returns something

Action
↓
Does an action → void
```

---

# 11. What is Predicate<T>?

`Predicate<T>` represents a method that:

> Takes one value of type `T` and returns `bool`.

Example:

```csharp
Predicate<int> isEven =
    x => x % 2 == 0;
```

Usage:

```csharp
Console.WriteLine(isEven(10));
```

Output:

```text
True
```

Another:

```csharp
Predicate<Employee> isSenior =
    employee => employee.Age >= 30;
```

---

# 12. Predicate vs Func

Both can return `bool`.

```csharp
Predicate<int> predicate =
    x => x > 10;
```

Equivalent conceptually:

```csharp
Func<int, bool> function =
    x => x > 10;
```

The difference is mainly the delegate type/signature:

```text
Predicate<T>
→ exactly one T input
→ bool return

Func<T, bool>
→ one T input
→ bool return
```

LINQ commonly uses `Func<...>`-style delegates.

---

# 13. What is LINQ?

LINQ stands for:

> **Language Integrated Query**

It allows us to query data using C# syntax.

LINQ can work with:

```text
Arrays
Lists
Collections
Objects
XML
Databases
Entity Framework Core
```

Example:

```csharp
var result = employees
    .Where(x => x.Age > 25)
    .OrderBy(x => x.Name);
```

---

# 14. LINQ Method Syntax

The style you will use most often is **method syntax**:

```csharp
var result = employees
    .Where(x => x.Age > 25)
    .OrderBy(x => x.Name);
```

It combines:

```text
LINQ method
+
Lambda expression
```

---

# 15. Where()

`Where()` filters data.

Example:

```csharp
var result = employees
    .Where(x => x.Age > 25);
```

Meaning:

```text
Give me employees whose age is greater than 25.
```

Example with salary:

```csharp
var result = employees
    .Where(x => x.Salary > 50000);
```

---

# 16. Where with Multiple Conditions

```csharp
var result = employees
    .Where(x =>
        x.Age > 25 &&
        x.Department == "IT");
```

You can also use OR:

```csharp
var result = employees
    .Where(x =>
        x.Department == "IT" ||
        x.Department == "HR");
```

---

# 17. Select()

`Select()` is used for **projection**.

It transforms each item into something else.

Suppose:

```csharp
var names = employees
    .Select(x => x.Name);
```

Now the result contains names rather than Employee objects.

```text
Employee
   ↓
Select
   ↓
Name
```

---

# 18. Select Multiple Properties

```csharp
var result = employees.Select(x => new
{
    x.Id,
    x.Name,
    x.Salary
});
```

This creates anonymous objects.

Example output:

```text
101 Arun 50000
102 Priya 45000
103 Kumar 60000
```

This concept is extremely important in Web API and Entity Framework.

---

# 19. Where + Select

Very common combination:

```csharp
var result = employees
    .Where(x => x.Department == "IT")
    .Select(x => x.Name);
```

Meaning:

```text
Employees
   ↓
Filter IT employees
   ↓
Select their names
```

---

# 20. SelectMany()

`SelectMany()` is used when each item contains another collection and you want to **flatten** them into one sequence.

Example:

```csharp
class Employee
{
    public string Name { get; set; } = "";

    public List<string> Skills { get; set; } = new();
}
```

Data:

```csharp
var employees = new List<Employee>
{
    new Employee
    {
        Name = "Arun",
        Skills = new List<string>
        {
            "C#",
            "SQL"
        }
    },

    new Employee
    {
        Name = "Priya",
        Skills = new List<string>
        {
            "Angular",
            "C#"
        }
    }
};
```

Using:

```csharp
var skills = employees
    .SelectMany(x => x.Skills);
```

Result:

```text
C#
SQL
Angular
C#
```

### Memory trick

```text
Select
→ one item → one result

SelectMany
→ one item → many results → flatten
```

---

# 21. OrderBy()

Sort ascending.

```csharp
var result = employees
    .OrderBy(x => x.Salary);
```

For names:

```csharp
var result = employees
    .OrderBy(x => x.Name);
```

---

# 22. OrderByDescending()

Sort descending.

```csharp
var result = employees
    .OrderByDescending(x => x.Salary);
```

Highest salary comes first.

---

# 23. ThenBy()

Used for secondary sorting.

Example:

```csharp
var result = employees
    .OrderBy(x => x.Department)
    .ThenBy(x => x.Name);
```

Meaning:

```text
First sort by Department
Then sort employees within each department by Name
```

---

# 24. ThenByDescending()

```csharp
var result = employees
    .OrderBy(x => x.Department)
    .ThenByDescending(x => x.Salary);
```

---

# 25. GroupBy()

`GroupBy()` groups data based on a key.

Suppose:

```text
Arun   → IT
Priya  → HR
Kumar  → IT
Meena  → HR
```

We can group by department:

```csharp
var groups = employees
    .GroupBy(x => x.Department);
```

Then:

```csharp
foreach (var group in groups)
{
    Console.WriteLine($"Department: {group.Key}");

    foreach (var employee in group)
    {
        Console.WriteLine(employee.Name);
    }
}
```

Output:

```text
Department: IT
Arun
Kumar

Department: HR
Priya
Meena
```

---

# 26. GroupBy with Count

Very common interview/development scenario:

```csharp
var result = employees
    .GroupBy(x => x.Department)
    .Select(group => new
    {
        Department = group.Key,
        EmployeeCount = group.Count()
    });
```

Result:

```text
IT → 2
HR → 2
```

---

# 27. Join()

`Join()` combines two collections based on a matching key.

Example:

```csharp
class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int DepartmentId { get; set; }
}

class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
```

Data:

```csharp
var employees = new List<Employee>
{
    new Employee
    {
        Id = 1,
        Name = "Arun",
        DepartmentId = 10
    },
    new Employee
    {
        Id = 2,
        Name = "Priya",
        DepartmentId = 20
    }
};

var departments = new List<Department>
{
    new Department
    {
        Id = 10,
        Name = "IT"
    },
    new Department
    {
        Id = 20,
        Name = "HR"
    }
};
```

Join:

```csharp
var result = employees.Join(
    departments,
    employee => employee.DepartmentId,
    department => department.Id,
    (employee, department) => new
    {
        EmployeeName = employee.Name,
        DepartmentName = department.Name
    });
```

Result:

```text
Arun  → IT
Priya → HR
```

This is conceptually similar to a SQL `INNER JOIN`.

---

# 28. Any()

`Any()` checks whether **at least one** element satisfies a condition.

```csharp
bool exists = employees
    .Any(x => x.Salary > 100000);
```

Result:

```text
true / false
```

Without a condition:

```csharp
bool hasEmployees = employees.Any();
```

This checks whether the collection contains at least one item.

---

# 29. All()

`All()` checks whether **every element** satisfies a condition.

```csharp
bool result = employees
    .All(x => x.Age >= 18);
```

Meaning:

> Are all employees at least 18?

Difference:

```text
Any()
→ At least one?

All()
→ Every one?
```

---

# 30. Contains()

Checks whether a collection contains a particular value.

```csharp
List<string> skills = new()
{
    "C#",
    "SQL",
    "Angular"
};

bool result = skills.Contains("C#");
```

Output:

```text
True
```

For objects, equality behavior depends on the type's equality implementation.

---

# 31. First()

Returns the first matching element.

```csharp
var employee = employees
    .First(x => x.Department == "IT");
```

Important:

If no matching element exists:

```text
InvalidOperationException
```

---

# 32. FirstOrDefault()

Returns the first matching element.

If nothing is found, it returns the default value.

```csharp
var employee = employees
    .FirstOrDefault(x => x.Department == "Finance");
```

For a reference type, the result will normally be:

```text
null
```

This is why `FirstOrDefault()` is often safer when "not found" is an expected possibility.

---

# 33. First vs FirstOrDefault

```text
First()
     ↓
Must find something
     ↓
Throws if no element

FirstOrDefault()
     ↓
May find nothing
     ↓
Returns default
```

---

# 34. Single()

`Single()` expects **exactly one** matching element.

```csharp
var employee = employees
    .Single(x => x.Id == 101);
```

There must be exactly one matching employee.

If:

```text
0 matches → exception
2+ matches → exception
```

---

# 35. SingleOrDefault()

```csharp
var employee = employees
    .SingleOrDefault(x => x.Id == 101);
```

Rules:

```text
1 match
→ return item

0 matches
→ return default

More than 1 match
→ exception
```

---

# 36. First vs Single — Very Important

Suppose:

```text
101 → Arun
102 → Priya
103 → Kumar
```

### First

```csharp
employees.First(x => x.Department == "IT");
```

If multiple IT employees exist, it returns the first one.

### Single

```csharp
employees.Single(x => x.Id == 101);
```

Use when business logic says:

> There should be exactly one matching record.

### Interview memory

```text
First
→ Give me the first one.

Single
→ There must be exactly one.
```

---

# 37. Count()

Counts elements.

```csharp
int count = employees.Count();
```

With condition:

```csharp
int itCount = employees.Count(
    x => x.Department == "IT");
```

---

# 38. Sum()

```csharp
decimal totalSalary = employees
    .Sum(x => x.Salary);
```

Example:

```text
50000 + 45000 + 60000
= 155000
```

---

# 39. Min()

```csharp
decimal minimumSalary = employees
    .Min(x => x.Salary);
```

---

# 40. Max()

```csharp
decimal maximumSalary = employees
    .Max(x => x.Salary);
```

---

# 41. Average()

```csharp
double averageSalary = employees
    .Average(x => (double)x.Salary);
```

For numeric collections:

```csharp
List<int> marks = new()
{
    80, 90, 70, 85
};

double average = marks.Average();
```

---

# 42. Distinct()

Removes duplicate values from a sequence.

```csharp
List<string> departments = new()
{
    "IT",
    "HR",
    "IT",
    "Finance",
    "HR"
};

var uniqueDepartments = departments
    .Distinct();
```

Result:

```text
IT
HR
Finance
```

---

# 43. Skip()

Skips a specified number of items.

```csharp
var result = employees
    .Skip(2);
```

If:

```text
1 2 3 4 5
```

then:

```text
Skip(2)
↓
3 4 5
```

---

# 44. Take()

Takes a specified number of items.

```csharp
var result = employees
    .Take(2);
```

If:

```text
1 2 3 4 5
```

then:

```text
Take(2)
↓
1 2
```

---

# 45. Skip + Take — Pagination

This is extremely important in Web API development.

Suppose:

```text
Page size = 10
Page number = 3
```

Formula:

```text
Skip = (pageNumber - 1) × pageSize
```

So:

```text
(3 - 1) × 10
= 20
```

LINQ:

```csharp
var page = employees
    .Skip(20)
    .Take(10)
    .ToList();
```

This concept will appear later when you build APIs.

---

# 46. LINQ Method Chaining

LINQ becomes powerful when methods are chained.

```csharp
var result = employees
    .Where(x => x.Age >= 25)
    .Where(x => x.Salary > 50000)
    .OrderByDescending(x => x.Salary)
    .Select(x => new
    {
        x.Name,
        x.Salary
    })
    .ToList();
```

Read it from top to bottom:

```text
Employees
   ↓
Age >= 25
   ↓
Salary > 50000
   ↓
Highest salary first
   ↓
Select Name + Salary
   ↓
Convert to List
```

---

# 47. ToList() and ToArray()

LINQ often returns an enumerable sequence.

You can materialize it:

```csharp
var result = employees
    .Where(x => x.Age > 25)
    .ToList();
```

Or:

```csharp
var result = employees
    .Where(x => x.Age > 25)
    .ToArray();
```

This distinction becomes especially important with `IEnumerable` and `IQueryable`.

---

# 48. Deferred Execution

This is an important LINQ concept.

Consider:

```csharp
var result = employees
    .Where(x => x.Salary > 50000);
```

The query may not execute immediately.

The filtering happens when you enumerate it:

```csharp
foreach (var employee in result)
{
    Console.WriteLine(employee.Name);
}
```

or materialize it:

```csharp
var list = result.ToList();
```

This is called:

> **Deferred execution**

---

# 49. Immediate Execution

Some LINQ operations force execution.

Examples:

```csharp
ToList()
ToArray()
Count()
Sum()
Min()
Max()
Average()
First()
Single()
```

For example:

```csharp
var result = employees
    .Where(x => x.Salary > 50000)
    .ToList();
```

`ToList()` immediately materializes the results.

---

# 50. IEnumerable vs IQueryable

This is one of the most important interview topics.

## IEnumerable<T>

Typically used for **in-memory collections**.

Example:

```csharp
List<Employee> employees = GetEmployees();

IEnumerable<Employee> result =
    employees.Where(x => x.Salary > 50000);
```

The filtering is performed in the application process over the in-memory data.

---

# 51. IQueryable<T>

`IQueryable<T>` represents a query that can be translated by a query provider.

The most important example is **Entity Framework Core**.

Example:

```csharp
IQueryable<Employee> employees =
    dbContext.Employees;

var result = employees
    .Where(x => x.Salary > 50000)
    .OrderBy(x => x.Name);
```

With EF Core, this expression can be translated into SQL and executed by the database when the query is materialized.

Conceptually:

```text
C# LINQ
   ↓
IQueryable
   ↓
EF Core
   ↓
SQL
   ↓
SQL Server
```

---

# 52. IEnumerable vs IQueryable Example

Suppose the database contains:

```text
1,000,000 employees
```

With an appropriate `IQueryable` EF Core query:

```csharp
var employees = dbContext.Employees
    .Where(x => x.Salary > 50000)
    .ToList();
```

The filtering can be translated to SQL so the database does the filtering before the results are materialized.

Conceptually:

```sql
SELECT *
FROM Employees
WHERE Salary > 50000;
```

Instead, if you first materialize everything:

```csharp
var employees = dbContext.Employees.ToList();

var result = employees
    .Where(x => x.Salary > 50000);
```

the entire result set has already been loaded into memory before the `Where()` executes.

So:

> **Where you execute the query matters.**

---

# 53. Important Difference

| `IEnumerable<T>`                                            | `IQueryable<T>`                                   |
| ----------------------------------------------------------- | ------------------------------------------------- |
| Commonly works with in-memory data                          | Designed for queryable data sources               |
| LINQ operations run against objects after data is available | Query expressions can be translated by a provider |
| Common with `List<T>`                                       | Common with EF Core                               |
| Uses delegates such as `Func<T,...>` for LINQ-to-Objects    | Uses expression trees for provider translation    |
| Good for in-memory collections                              | Useful for database queries                       |

---

# 54. Expression Trees — Interview Concept

With `IEnumerable<T>`, a predicate is commonly represented as a delegate:

```csharp
Func<Employee, bool>
```

With `IQueryable<T>`, the provider generally receives an expression tree:

```csharp
Expression<Func<Employee, bool>>
```

The provider can inspect that expression and translate it.

Conceptually:

```text
IEnumerable
     ↓
Delegate
     ↓
Execute C# code

IQueryable
     ↓
Expression Tree
     ↓
Query Provider
     ↓
Translate/Execute
```

This is why you should not think of `IQueryable` simply as "faster IEnumerable."

They serve different purposes.

---

# 55. Important Warning with IQueryable

Not every C# method can necessarily be translated to SQL by every LINQ provider.

For example, a provider may not be able to translate an arbitrary custom C# method into SQL.

So with database queries, you should understand:

```text
What can the provider translate?
```

This is particularly important with Entity Framework Core.

---

# 56. Complete Day 7 Example

Create:

```text
DotNetHandsOn
│
└── Day07LinqLambda
    └── Program.cs
```

Use:

```csharp
class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public int Age { get; set; }

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }
}
```

Data:

```csharp
List<Employee> employees = new()
{
    new Employee
    {
        Id = 101,
        Name = "Arun",
        Age = 25,
        Department = "IT",
        Salary = 50000
    },

    new Employee
    {
        Id = 102,
        Name = "Priya",
        Age = 30,
        Department = "HR",
        Salary = 45000
    },

    new Employee
    {
        Id = 103,
        Name = "Kumar",
        Age = 35,
        Department = "IT",
        Salary = 70000
    },

    new Employee
    {
        Id = 104,
        Name = "Meena",
        Age = 28,
        Department = "Finance",
        Salary = 55000
    },

    new Employee
    {
        Id = 105,
        Name = "Ravi",
        Age = 40,
        Department = "IT",
        Salary = 80000
    }
};
```

---

## Filter

```csharp
var itEmployees = employees
    .Where(x => x.Department == "IT");

foreach (var employee in itEmployees)
{
    Console.WriteLine(employee.Name);
}
```

---

## Select

```csharp
var names = employees
    .Select(x => x.Name);

foreach (string name in names)
{
    Console.WriteLine(name);
}
```

---

## Sort

```csharp
var sortedEmployees = employees
    .OrderByDescending(x => x.Salary);

foreach (var employee in sortedEmployees)
{
    Console.WriteLine(
        $"{employee.Name} - {employee.Salary}");
}
```

---

## Group

```csharp
var departmentGroups = employees
    .GroupBy(x => x.Department);

foreach (var group in departmentGroups)
{
    Console.WriteLine(group.Key);

    foreach (var employee in group)
    {
        Console.WriteLine($"  {employee.Name}");
    }
}
```

---

## Aggregation

```csharp
Console.WriteLine(
    $"Count: {employees.Count()}");

Console.WriteLine(
    $"Total Salary: {employees.Sum(x => x.Salary)}");

Console.WriteLine(
    $"Minimum Salary: {employees.Min(x => x.Salary)}");

Console.WriteLine(
    $"Maximum Salary: {employees.Max(x => x.Salary)}");

Console.WriteLine(
    $"Average Salary: {employees.Average(x => x.Salary)}");
```

---

# 57. Real-World Employee Query

A very useful practice:

```csharp
var result = employees
    .Where(x => x.Age >= 25)
    .Where(x => x.Salary >= 50000)
    .OrderByDescending(x => x.Salary)
    .Select(x => new
    {
        x.Id,
        x.Name,
        x.Department,
        x.Salary
    })
    .ToList();
```

This is the kind of LINQ you will frequently encounter in .NET projects.

---

# 58. LINQ Query Syntax

There is another LINQ syntax called **query syntax**.

Method syntax:

```csharp
var result = employees
    .Where(x => x.Age > 25)
    .Select(x => x.Name);
```

Query syntax:

```csharp
var result =
    from employee in employees
    where employee.Age > 25
    select employee.Name;
```

Both can represent LINQ queries.

For modern .NET development, **method syntax + lambdas** is extremely common.

---

# 59. Most Important LINQ Cheat Sheet

```text
Where()
→ Filter

Select()
→ Transform / project

SelectMany()
→ Flatten nested collections

OrderBy()
→ Ascending

OrderByDescending()
→ Descending

ThenBy()
→ Secondary ascending sort

ThenByDescending()
→ Secondary descending sort

GroupBy()
→ Group data

Join()
→ Combine collections by matching key

Any()
→ At least one?

All()
→ Every item?

Contains()
→ Does it contain this value?

First()
→ First matching item; throws if none

FirstOrDefault()
→ First matching item or default

Single()
→ Exactly one; throws if 0 or >1

SingleOrDefault()
→ 0 or 1 allowed; >1 throws

Count()
→ Number of items

Sum()
→ Total

Min()
→ Minimum

Max()
→ Maximum

Average()
→ Average

Distinct()
→ Remove duplicates

Skip()
→ Skip N items

Take()
→ Take N items

ToList()
→ Materialize as List

ToArray()
→ Materialize as Array
```

---

# 60. Lambda Cheat Sheet

```text
x => x * 2
→ One parameter

(x, y) => x + y
→ Two parameters

x => x > 10
→ Returns bool

x =>
{
    return x * 2;
}
→ Multiple statements
```

Delegates:

```text
Func<T>
→ Returns a value

Action<T>
→ Returns void

Predicate<T>
→ Takes T and returns bool
```

---

# 61. Interview Questions You Must Know

### Lambda

**1. What is a lambda expression?**

A concise syntax for representing an anonymous function.

```csharp
x => x > 10
```

**2. What is Func?**

A generic delegate that represents a method returning a value.

**3. What is Action?**

A generic delegate that represents a method returning `void`.

**4. What is Predicate<T>?**

A delegate that accepts one `T` and returns `bool`.

---

### LINQ

**5. What is LINQ?**

Language Integrated Query — a C# feature/API set for querying data sources.

**6. What is Where()?**

Filters elements based on a condition.

**7. What is Select()?**

Projects/transforms each element into another form.

**8. Difference between Select and SelectMany?**

```text
Select
→ one source item produces one result

SelectMany
→ nested results are flattened into one sequence
```

**9. Difference between First and Single?**

```text
First
→ returns the first matching item

Single
→ expects exactly one matching item
```

**10. Difference between First and FirstOrDefault?**

```text
First
→ throws if no item exists

FirstOrDefault
→ returns default if no item exists
```

**11. Difference between Any and All?**

```text
Any
→ at least one satisfies condition

All
→ every item satisfies condition
```

**12. What is deferred execution?**

A LINQ query can be evaluated when the sequence is enumerated rather than when the query is created.

**13. What does ToList() do?**

It materializes the query into a `List<T>`.

**14. What is IEnumerable?**

An interface representing a sequence that can be enumerated.

**15. What is IQueryable?**

An interface representing a query that can be interpreted by a query provider, such as Entity Framework Core.

**16. Difference between IEnumerable and IQueryable?**

The key difference is where/how the query is executed:

```text
IEnumerable
→ LINQ-to-Objects / in-memory execution

IQueryable
→ expression tree + query provider
→ can translate query to a data-source language such as SQL
```

---

# 62. Day 7 Final Mental Model

You should understand this flow before moving forward:

```text
              LAMBDA
                 │
                 ↓
       x => x.Age > 25
                 │
                 ↓
        FUNC / ACTION /
          PREDICATE
                 │
                 ↓
               LINQ
                 │
      ┌──────────┼──────────┐
      ↓          ↓          ↓
   Where      Select      GroupBy
      ↓          ↓          ↓
   Filter     Project     Group
      │          │          │
      └──────────┼──────────┘
                 ↓
          Order / Join /
       Aggregate / Paging
                 │
                 ↓
       IEnumerable / IQueryable
                 │
          ┌──────┴──────┐
          ↓             ↓
       Memory       Database
       Objects       EF Core
```

### The 10 things I would make sure you can write without looking at notes

```csharp
// 1. Where
employees.Where(x => x.Age > 25);

// 2. Select
employees.Select(x => x.Name);

// 3. SelectMany
employees.SelectMany(x => x.Skills);

// 4. Sorting
employees.OrderBy(x => x.Name);

// 5. Grouping
employees.GroupBy(x => x.Department);

// 6. Any
employees.Any(x => x.Salary > 100000);

// 7. FirstOrDefault
employees.FirstOrDefault(x => x.Id == 101);

// 8. Aggregation
employees.Sum(x => x.Salary);

// 9. Pagination
employees.Skip(10).Take(10);

// 10. Chained LINQ
employees
    .Where(x => x.Age >= 25)
    .OrderByDescending(x => x.Salary)
    .Select(x => new
    {
        x.Name,
        x.Salary
    })
    .ToList();
```

**The most important Day 7 connection:** `Lambda → LINQ → IEnumerable/IQueryable → Entity Framework Core → SQL`. Once you understand this chain, the LINQ concepts become much easier to apply in ASP.NET Core projects.
