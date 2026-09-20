# 🗓️ WEEK 1 — C# & .NET FUNDAMENTALS

## Day 1 — .NET Introduction & Environment Setup

Learn:

* What is .NET?
* .NET Framework vs .NET Core vs modern .NET
* .NET 6/7/8 evolution
* What is .NET 8?
* CLR
* CTS
* CLS
* JIT
* Managed code vs unmanaged code
* SDK vs Runtime
* .NET CLI
* Visual Studio
* Visual Studio Code
* Solution vs Project
* `.csproj`
* NuGet
* Build
* Restore
* Run
* Publish

Practice:

```bash
dotnet --version
dotnet --info
dotnet new console
dotnet build
dotnet run
dotnet restore
dotnet publish
```

Understand a basic .NET 8 console application.

---

# Day 2 — C# Fundamentals

Learn:

### Variables

* `int`
* `long`
* `float`
* `double`
* `decimal`
* `bool`
* `char`
* `string`

### Constants

```csharp
const int age = 25;
```

### Operators

* Arithmetic
* Relational
* Logical
* Assignment
* Increment/decrement
* Null-coalescing
* Null-conditional

### Control Flow

* `if`
* `else`
* `else if`
* `switch`
* `for`
* `foreach`
* `while`
* `do while`

Practice:

* Calculator
* Number checker
* Even/odd
* Prime number
* Factorial
* Fibonacci

---

# Day 3 — C# Methods, Strings & Arrays

Learn:

* Methods
* Parameters
* Return values
* Optional parameters
* Named parameters
* `ref`
* `out`
* `in`
* Method overloading
* Recursion

### Strings

* String methods
* String interpolation
* `StringBuilder`
* String comparison
* Parsing
* Formatting

### Arrays

* Single-dimensional arrays
* Multi-dimensional arrays
* Jagged arrays

Practice:

```text
Employee Salary Calculator
Student Marks Calculator
String Analyzer
Array Sorting
Array Searching
```

---

# Day 4 — OOP Fundamentals

This is extremely important.

Learn:

## Classes & Objects

```csharp
class Employee
{
    public string Name { get; set; }
}
```

## Four pillars of OOP

### 1. Encapsulation

### 2. Inheritance

### 3. Polymorphism

### 4. Abstraction

Also learn:

* Constructors
* Constructor overloading
* Properties
* Fields
* Access modifiers
* `public`
* `private`
* `protected`
* `internal`
* `static`
* `readonly`
* `const`

---

# Day 5 — Advanced OOP

Learn:

* Abstract classes
* Interfaces
* Virtual methods
* Override
* Method hiding
* Sealed classes
* Sealed methods
* Composition
* Association
* Aggregation

### Interfaces

```csharp
public interface IPaymentService
{
    void Pay();
}
```

Understand:

**Interface vs Abstract Class**

Also learn:

* SOLID principles
* Single Responsibility
* Open/Closed
* Liskov Substitution
* Interface Segregation
* Dependency Inversion

---

# Day 6 — Collections & Generics

## Collections

Learn:

* Array
* `List<T>`
* `Dictionary<TKey,TValue>`
* `HashSet<T>`
* `Queue<T>`
* `Stack<T>`

Understand:

```text
IEnumerable
ICollection
IList
IReadOnlyCollection
IReadOnlyList
```

## Generics

Learn:

```csharp
List<T>
Dictionary<TKey,TValue>
```

Also:

* Generic classes
* Generic methods
* Generic interfaces
* Constraints
* `where T : class`
* `where T : new()`

---

# Day 7 — LINQ + Lambda Expressions

Very important for .NET interviews and development.

## Lambda

```csharp
x => x.Age > 18
```

Learn:

* Lambda expressions
* Func
* Action
* Predicate

## LINQ

Learn:

* `Where`
* `Select`
* `SelectMany`
* `OrderBy`
* `OrderByDescending`
* `ThenBy`
* `GroupBy`
* `Join`
* `Any`
* `All`
* `Contains`
* `First`
* `FirstOrDefault`
* `Single`
* `SingleOrDefault`
* `Count`
* `Sum`
* `Min`
* `Max`
* `Average`
* `Distinct`
* `Skip`
* `Take`

Understand:

**IEnumerable vs IQueryable**

### Week 1 Mini Project

Build:

**Employee Management Console Application**

Features:

```text
Add Employee
Update Employee
Delete Employee
Search Employee
Filter by Department
Sort Employees
Calculate Salary
Generate Reports
```

Use:

* OOP
* Collections
* Generics
* LINQ
* Lambda
* Exception handling

---