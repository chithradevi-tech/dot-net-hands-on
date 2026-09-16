# Day 4 — C# OOP Fundamentals

**OOP — Object-Oriented Programming** is one of the most important parts of C#.

---

# 1. What is OOP?

OOP means **Object-Oriented Programming**.

Instead of writing an application only as a collection of functions, we organize code around **objects**.

For example, in an Employee Management System, we might have:

```text
Employee
 ├── Name
 ├── Id
 ├── Salary
 ├── Department
 └── CalculateSalary()
```

Here:

* **Employee** → Class
* **Chitra** → Object
* **Name, Id, Salary** → Properties/Data
* **CalculateSalary()** → Method

---

# 2. Class

A **class** is a blueprint/template for creating objects.

```csharp
class Employee
{
    public string Name { get; set; }
}
```

The class describes what an employee should contain.

It does not represent one particular employee yet.

---

# 3. Object

An **object** is an instance of a class.

```csharp
Employee employee = new Employee();
```

Now we can assign values:

```csharp
employee.Name = "Chitra";

Console.WriteLine(employee.Name);
```

Output:

```text
Chitra
```

### Easy way to remember

```text
Class  = Blueprint
Object = Actual thing created from blueprint
```

For example:

```text
Class  → Employee
Object → Chitra
Object → Arun
Object → Priya
```

All three objects can be created from the same `Employee` class.

---

# 4. Complete Class Example

```csharp
class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public decimal Salary { get; set; }

    public void DisplayEmployee()
    {
        Console.WriteLine($"ID     : {Id}");
        Console.WriteLine($"Name   : {Name}");
        Console.WriteLine($"Salary : {Salary}");
    }
}
```

Create an object:

```csharp
Employee employee = new Employee();

employee.Id = 101;
employee.Name = "Chitra";
employee.Salary = 50000;

employee.DisplayEmployee();
```

Output:

```text
ID     : 101
Name   : Chitra
Salary : 50000
```

---

# 5. Fields

A **field** is a variable declared inside a class.

```csharp
class Employee
{
    private int id;
    private string name;
}
```

Here:

```text
id
name
```

are fields.

Fields normally store the internal state of an object.

---

# 6. Properties

A property provides controlled access to data.

```csharp
class Employee
{
    public string Name { get; set; }
}
```

Usage:

```csharp
Employee employee = new Employee();

employee.Name = "Chitra";

Console.WriteLine(employee.Name);
```

---

# 7. Field vs Property

This is an important interview question.

### Field

```csharp
private string name;
```

### Property

```csharp
public string Name { get; set; }
```

A property can provide additional control.

For example:

```csharp
public string Name
{
    get { return name; }
    set { name = value; }
}
```

This allows us to add validation or other logic.

---

# 8. Auto-Implemented Properties

Most modern C# code uses auto-properties.

```csharp
public string Name { get; set; }
```

C# automatically manages the underlying storage.

You don't need to explicitly declare the backing field.

---

# 9. Read-Only Property

You can create a property that can be read but not assigned externally.

```csharp
public string Name { get; private set; } = "";
```

Outside the class:

```csharp
employee.Name = "Chitra";
```

would not be allowed.

But the class itself can change it.

---

# 10. Constructors

A **constructor** is a special method that runs automatically when an object is created.

Example:

```csharp
class Employee
{
    public string Name { get; set; }

    public Employee()
    {
        Console.WriteLine("Employee object created");
    }
}
```

Create object:

```csharp
Employee employee = new Employee();
```

Output:

```text
Employee object created
```

---

# 11. Constructor Rules

A constructor:

* Has the same name as the class
* Has no return type
* Runs when an object is created

Example:

```csharp
class Employee
{
    public Employee()
    {
        // Constructor
    }
}
```

Not:

```csharp
public void Employee()
{
}
```

That would be a normal method, not a constructor.

---

# 12. Parameterized Constructor

A constructor can accept parameters.

```csharp
class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public Employee(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
```

Create object:

```csharp
Employee employee = new Employee(101, "Chitra");

Console.WriteLine(employee.Id);
Console.WriteLine(employee.Name);
```

Output:

```text
101
Chitra
```

---

# 13. Constructor Overloading

A class can have multiple constructors with different parameters.

```csharp
class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public decimal Salary { get; set; }

    // Constructor 1
    public Employee()
    {
    }

    // Constructor 2
    public Employee(int id, string name)
    {
        Id = id;
        Name = name;
    }

    // Constructor 3
    public Employee(int id, string name, decimal salary)
    {
        Id = id;
        Name = name;
        Salary = salary;
    }
}
```

Now all are valid:

```csharp
Employee employee1 = new Employee();

Employee employee2 = new Employee(101, "Chitra");

Employee employee3 = new Employee(102, "Arun", 60000);
```

This is **constructor overloading**.

---

# 14. Constructor Chaining

One constructor can call another constructor using `this`.

```csharp
class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public decimal Salary { get; set; }

    public Employee()
        : this(0, "Unknown", 0)
    {
    }

    public Employee(int id, string name, decimal salary)
    {
        Id = id;
        Name = name;
        Salary = salary;
    }
}
```

This avoids duplicate initialization logic.

---

# 15. The Four Pillars of OOP

The four major OOP concepts are:

```text
1. Encapsulation
2. Inheritance
3. Polymorphism
4. Abstraction
```

Remember:

```text
E → Encapsulation
I → Inheritance
P → Polymorphism
A → Abstraction
```

---

# 16. Encapsulation

Encapsulation means **bundling data and behavior together and controlling access to the internal state**.

Example:

```csharp
class BankAccount
{
    private decimal balance;

    public void Deposit(decimal amount)
    {
        if (amount > 0)
        {
            balance += amount;
        }
    }

    public decimal GetBalance()
    {
        return balance;
    }
}
```

Create object:

```csharp
BankAccount account = new BankAccount();

account.Deposit(10000);

Console.WriteLine(account.GetBalance());
```

We cannot directly do:

```csharp
account.balance = -5000;
```

because `balance` is private.

### Why?

The class controls how its internal data can be changed.

That is encapsulation.

---

# 17. Encapsulation Using Properties

Another common approach:

```csharp
class Employee
{
    public string Name { get; set; } = "";

    public decimal Salary { get; private set; }

    public void IncreaseSalary(decimal amount)
    {
        if (amount > 0)
        {
            Salary += amount;
        }
    }
}
```

External code can read:

```csharp
Console.WriteLine(employee.Salary);
```

But cannot directly assign:

```csharp
employee.Salary = -1000;
```

because the setter is private.

---

# 18. Inheritance

Inheritance allows one class to **reuse and extend another class**.

Example:

```csharp
class Employee
{
    public string Name { get; set; } = "";

    public void Work()
    {
        Console.WriteLine("Employee is working");
    }
}
```

Child class:

```csharp
class Manager : Employee
{
    public void ManageTeam()
    {
        Console.WriteLine("Manager is managing the team");
    }
}
```

Create object:

```csharp
Manager manager = new Manager();

manager.Name = "Chitra";

manager.Work();
manager.ManageTeam();
```

Output:

```text
Employee is working
Manager is managing the team
```

Here:

```text
Employee
   ↑
Manager
```

`Manager` inherits from `Employee`.

---

# 19. Base Class and Derived Class

Terminology:

```text
Employee → Base class / Parent class

Manager → Derived class / Child class
```

Syntax:

```csharp
class Manager : Employee
{
}
```

The `:` indicates inheritance.

---

# 20. Types of Inheritance in C#

Common forms:

```text
Single inheritance
Multilevel inheritance
Hierarchical inheritance
```

### Single

```text
Employee
   ↓
Manager
```

### Multilevel

```text
Employee
   ↓
Manager
   ↓
SeniorManager
```

### Hierarchical

```text
       Employee
       /      \
 Manager     Developer
```

C# classes do **not** support multiple inheritance through classes.

For example, this is not allowed:

```csharp
class Manager : Employee, Person
{
}
```

Multiple contracts can instead be achieved using interfaces, which you'll learn later.

---

# 21. `base` Keyword

`base` refers to the base class.

Example:

```csharp
class Employee
{
    public string Name { get; set; } = "";

    public void Display()
    {
        Console.WriteLine($"Employee: {Name}");
    }
}

class Manager : Employee
{
    public void Show()
    {
        base.Display();
    }
}
```

---

# 22. Polymorphism

Polymorphism means:

> **One interface/base type, multiple possible implementations or behaviors.**

There are two major forms you'll learn in C#:

```text
Compile-time polymorphism
Runtime polymorphism
```

---

# 23. Compile-Time Polymorphism

Method overloading is a common example.

```csharp
class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }

    public int Add(int a, int b, int c)
    {
        return a + b + c;
    }
}
```

Same method name:

```text
Add()
```

but different parameter lists.

The compiler determines which method to call.

---

# 24. Runtime Polymorphism

Runtime polymorphism commonly uses:

```text
virtual
override
base class reference
```

Example:

```csharp
class Employee
{
    public virtual void Work()
    {
        Console.WriteLine("Employee is working");
    }
}
```

Derived class:

```csharp
class Developer : Employee
{
    public override void Work()
    {
        Console.WriteLine("Developer is writing code");
    }
}
```

Now:

```csharp
Employee employee = new Developer();

employee.Work();
```

Output:

```text
Developer is writing code
```

Even though the variable type is `Employee`, the actual object is `Developer`.

This is runtime polymorphism.

---

# 25. `virtual` and `override`

Base class:

```csharp
public virtual void Work()
{
}
```

Child class:

```csharp
public override void Work()
{
}
```

Think:

```text
virtual  → Child is allowed to change behavior

override → Child provides new implementation
```

---

# 26. Abstraction

Abstraction means **exposing essential behavior while hiding unnecessary implementation details**.

For example, when using an ATM:

```text
Insert card
Enter PIN
Withdraw money
```

You don't need to know exactly how the banking system processes the transaction internally.

In C#, abstraction is commonly achieved using:

```text
Abstract classes
Interfaces
```

Interfaces will become especially important in ASP.NET Core.

---

# 27. Abstract Class

Example:

```csharp id="qv8k3t"
abstract class Employee
{
    public string Name { get; set; } = "";

    public abstract void Work();
}
```

A child class must implement the abstract method:

```csharp id="y4v7fb"
class Developer : Employee
{
    public override void Work()
    {
        Console.WriteLine("Developer writes code");
    }
}
```

You cannot directly create an object of an abstract class:

```csharp
Employee employee = new Employee();
```

This is invalid.

But:

```csharp
Employee employee = new Developer();

employee.Work();
```

is valid.

---

# 28. Access Modifiers

Access modifiers control **where a class member can be accessed**.

Important ones for your roadmap:

```text
public
private
protected
internal
```

---

# 29. `public`

Accessible from anywhere that can access the containing type.

```csharp
class Employee
{
    public string Name { get; set; } = "";
}
```

Another class can access:

```csharp
Employee employee = new Employee();

employee.Name = "Chitra";
```

---

# 30. `private`

Accessible only inside the containing class.

```csharp
class Employee
{
    private decimal salary;

    public void SetSalary(decimal amount)
    {
        salary = amount;
    }
}
```

This is not allowed outside:

```csharp
employee.salary = 50000;
```

---

# 31. `protected`

Accessible inside:

* The containing class
* Derived classes

Example:

```csharp
class Employee
{
    protected string Department = "IT";
}

class Developer : Employee
{
    public void DisplayDepartment()
    {
        Console.WriteLine(Department);
    }
}
```

`Developer` can access `Department` because it inherits from `Employee`.

---

# 32. `internal`

Accessible within the **same assembly**.

```csharp
internal class Employee
{
}
```

In a normal .NET project, think of an assembly as the compiled output of the project, such as a `.dll` or executable.

`internal` is useful when you want something available throughout your project/assembly but not exposed publicly to other assemblies.

---

# 33. Access Modifier Summary

| Modifier    | Same Class | Derived Class | Same Assembly | Outside Assembly |
| ----------- | ---------: | ------------: | ------------: | ---------------: |
| `private`   |          ✅ |             ❌ |             ❌ |                ❌ |
| `protected` |          ✅ |             ✅ |            ❌* |               ❌* |
| `internal`  |          ✅ |             ✅ |             ✅ |                ❌ |
| `public`    |          ✅ |             ✅ |             ✅ |                ✅ |

* `protected` has additional accessibility rules in C# involving derived types; the simplified table is for first understanding.

---

# 34. `static`

`static` means a member belongs to the **type itself**, rather than to a particular object instance.

Example:

```csharp
class Calculator
{
    public static int Add(int a, int b)
    {
        return a + b;
    }
}
```

You don't need to create an object:

```csharp
int result = Calculator.Add(10, 20);
```

---

# 35. Static Variable

```csharp
class Employee
{
    public static int EmployeeCount = 0;
}
```

There is one shared `EmployeeCount` associated with the type, rather than one separate field for every employee object.

Example:

```csharp
Employee.EmployeeCount++;
```

---

# 36. Instance vs Static

### Instance member

```csharp
class Employee
{
    public string Name { get; set; } = "";
}
```

Use:

```csharp
Employee employee = new Employee();

employee.Name = "Chitra";
```

### Static member

```csharp
class Calculator
{
    public static int Add(int a, int b)
    {
        return a + b;
    }
}
```

Use:

```csharp
Calculator.Add(10, 20);
```

### Easy rule

```text
Object-specific data → instance

Shared/type-level behavior or data → static
```

---

# 37. `const`

`const` represents a compile-time constant.

```csharp
const int MaxEmployees = 100;
```

Once declared, it cannot be changed.

This is invalid:

```csharp
MaxEmployees = 200;
```

---

# 38. `const` Rules

A constant must be assigned when declared.

Correct:

```csharp
const int MaxAge = 100;
```

Not:

```csharp
const int MaxAge;

MaxAge = 100;
```

Common constant types include primitive/value types and `string`.

Example:

```csharp
const double Pi = 3.14159;
const string CompanyName = "ABC Technologies";
```

---

# 39. `readonly`

`readonly` means the field can be assigned:

* At declaration, or
* In a constructor

After construction, it cannot normally be reassigned.

```csharp
class Employee
{
    public readonly int Id;

    public Employee(int id)
    {
        Id = id;
    }
}
```

Create:

```csharp
Employee employee = new Employee(101);

Console.WriteLine(employee.Id);
```

You cannot later do:

```csharp
employee.Id = 200;
```

---

# 40. `const` vs `readonly`

Very important interview question.

| `const`                         | `readonly`                           |
| ------------------------------- | ------------------------------------ |
| Compile-time constant           | Runtime-assigned read-only field     |
| Must be assigned at declaration | Can be assigned in constructor       |
| Implicitly static               | Not automatically static             |
| Value cannot vary per object    | Can have different values per object |
| Common for true constants       | Common for immutable object state    |

Example:

```csharp
class Employee
{
    public const string Company = "ABC";

    public readonly int EmployeeId;

    public Employee(int id)
    {
        EmployeeId = id;
    }
}
```

Here:

```text
Company   → same constant for the type
EmployeeId → can differ between employee objects
```

---

# 41. Complete OOP Example

Let's combine today's concepts.

```csharp
class Employee
{
    private decimal salary;

    public int Id { get; }

    public string Name { get; private set; }

    public Employee(int id, string name, decimal salary)
    {
        Id = id;
        Name = name;
        this.salary = salary;
    }

    public decimal GetSalary()
    {
        return salary;
    }

    public virtual void Work()
    {
        Console.WriteLine($"{Name} is working");
    }
}
```

Derived class:

```csharp
class Developer : Employee
{
    public Developer(
        int id,
        string name,
        decimal salary)
        : base(id, name, salary)
    {
    }

    public override void Work()
    {
        Console.WriteLine($"{Name} is developing software");
    }
}
```

Use:

```csharp
Employee employee = new Developer(
    101,
    "Chitra",
    60000
);

Console.WriteLine(employee.Id);
Console.WriteLine(employee.Name);
Console.WriteLine(employee.GetSalary());

employee.Work();
```

Output:

```text
101
Chitra
60000
Chitra is developing software
```

### Concepts present here

```text
Class
Object
Properties
Field
Constructor
Inheritance
Encapsulation
Polymorphism
private
public
protected/base concepts
virtual
override
```

---

# 42. `this` Keyword

You should also know `this` while learning constructors and classes.

`this` refers to the **current object**.

Example:

```csharp
class Employee
{
    private string name;

    public Employee(string name)
    {
        this.name = name;
    }
}
```

Here there are two variables named `name`:

```text
parameter → name
field     → this.name
```

So:

```csharp
this.name = name;
```

means:

```text
current object's name = parameter name
```

---

# 43. Property Validation

Properties can contain logic.

```csharp
class Employee
{
    private decimal salary;

    public decimal Salary
    {
        get
        {
            return salary;
        }
        set
        {
            if (value >= 0)
            {
                salary = value;
            }
        }
    }
}
```

Now:

```csharp
Employee employee = new Employee();

employee.Salary = 50000;
```

works.

But:

```csharp
employee.Salary = -5000;
```

will not update the salary because of the validation.

This is a practical example of **encapsulation**.

---

# 44. Primary Constructors — Modern C# Note

Since you're learning modern .NET 8/C#, you may encounter primary constructors.

Example:

```csharp
class Employee(string name, decimal salary)
{
    public string Name { get; } = name;

    public decimal Salary { get; } = salary;
}
```

Create:

```csharp
Employee employee = new Employee("Chitra", 50000);
```

This is modern C# syntax.

For your fundamentals, first become comfortable with traditional constructors:

```csharp
public Employee(string name, decimal salary)
{
    Name = name;
    Salary = salary;
}
```

Then learn primary constructors as modern syntax.

---

# 45. OOP Four Pillars — Real-World Example

Imagine an Employee Management System.

### Encapsulation

Protect salary:

```csharp
private decimal salary;
```

Only controlled methods/properties modify it.

### Inheritance

```text
Employee
   ↓
Developer
```

Developer gets common Employee functionality.

### Polymorphism

```csharp
Employee employee = new Developer();

employee.Work();
```

Different employee types can provide different `Work()` implementations.

### Abstraction

```csharp
abstract class Employee
{
    public abstract void Work();
}
```

We define what must be done without specifying the complete implementation.

---

# 46. Important Difference — Encapsulation vs Abstraction

This is frequently asked.

### Encapsulation

Focuses on:

> **Protecting and controlling data/state**

Example:

```csharp
private decimal salary;
```

### Abstraction

Focuses on:

> **Hiding implementation details and exposing essential behavior**

Example:

```csharp
public abstract void Work();
```

Easy memory:

```text
Encapsulation → How to protect data?

Abstraction   → What should be exposed?
```

---

# 47. Important Difference — Overloading vs Overriding

### Overloading

Same class usually:

```csharp
Add(int a, int b)

Add(int a, int b, int c)
```

Different parameter lists.

Compile-time polymorphism.

### Overriding

Parent/child relationship:

```csharp
virtual Work()
override Work()
```

Child changes inherited behavior.

Runtime polymorphism.

---

# 48. Important Difference — Class vs Object

```text
Class:
Blueprint/template

Object:
Actual instance
```

Example:

```csharp
class Employee
{
}
```

Class.

```csharp
Employee employee = new Employee();
```

Object.

---

# 49. Important Difference — Field vs Property

```text
Field:
private string name;

Property:
public string Name { get; set; }
```

Fields directly store state.

Properties provide an interface for accessing or controlling state.

---

# 50. PRACTICE PROJECT — EMPLOYEE MANAGEMENT

Create a console project:

```text
Day04OOP
```

Create these classes:

```text
Day04OOP
│
├── Program.cs
│
├── Employee.cs
├── Developer.cs
└── Manager.cs
```

---

## Employee.cs

```csharp
class Employee
{
    public int Id { get; }

    public string Name { get; private set; }

    public decimal Salary { get; private set; }

    public Employee(int id, string name, decimal salary)
    {
        Id = id;
        Name = name;
        Salary = salary;
    }

    public void IncreaseSalary(decimal amount)
    {
        if (amount > 0)
        {
            Salary += amount;
        }
    }

    public virtual void Work()
    {
        Console.WriteLine($"{Name} is working");
    }

    public void Display()
    {
        Console.WriteLine($"ID     : {Id}");
        Console.WriteLine($"Name   : {Name}");
        Console.WriteLine($"Salary : {Salary:C}");
    }
}
```

---

## Developer.cs

```csharp
class Developer : Employee
{
    public Developer(
        int id,
        string name,
        decimal salary)
        : base(id, name, salary)
    {
    }

    public override void Work()
    {
        Console.WriteLine($"{Name} is writing C# code");
    }
}
```

---

## Manager.cs

```csharp
class Manager : Employee
{
    public Manager(
        int id,
        string name,
        decimal salary)
        : base(id, name, salary)
    {
    }

    public override void Work()
    {
        Console.WriteLine($"{Name} is managing the team");
    }
}
```

---

## Program.cs

```csharp
Employee developer = new Developer(
    101,
    "Chitra",
    60000
);

Employee manager = new Manager(
    102,
    "Arun",
    80000
);

developer.Display();
developer.Work();

Console.WriteLine();

manager.Display();
manager.Work();

Console.WriteLine();

developer.IncreaseSalary(5000);

Console.WriteLine(
    $"Updated Developer Salary: {developer.Salary:C}"
);
```

---

# 51. What This Project Demonstrates

```text
Employee
   │
   ├── Developer
   │
   └── Manager
```

### Encapsulation

```csharp
public decimal Salary { get; private set; }
```

### Inheritance

```csharp
class Developer : Employee
```

### Polymorphism

```csharp
Employee developer = new Developer();

developer.Work();
```

### Abstraction-like design

The base class defines common employee behavior and derived classes specialize it.

### Constructor

```csharp
public Developer(...) : base(...)
```

### Properties

```csharp
public int Id { get; }
public string Name { get; private set; }
```

---

# 52. Visual Studio Practice

Since you're using **Visual Studio**, create the Day 4 project like this:

### Step 1

Open your existing solution:

```text
DotNetHandsOn
```

### Step 2

Right-click the solution in **Solution Explorer**.

Choose:

```text
Add
→ New Project
```

### Step 3

Select:

```text
Console App
C#
```

Project name:

```text
Day04OOP
```

Framework:

```text
.NET 8.0
```

### Step 4

Right-click the project:

```text
Add
→ Class
```

Create:

```text
Employee.cs
```

Repeat for:

```text
Developer.cs
Manager.cs
```

### Step 5

Put the corresponding class code into each file.

### Step 6

Put the object creation code in:

```text
Program.cs
```

### Step 7

Run:

```text
Ctrl + F5
```

or click the green **Start** button.

---

# 53. DAY 4 INTERVIEW QUESTIONS

## Basic

**1. What is OOP?**

Object-Oriented Programming is a programming paradigm that organizes software around objects containing data and behavior.

**2. What is a class?**

A blueprint/template used to create objects.

**3. What is an object?**

An instance of a class.

**4. What are the four pillars of OOP?**

```text
Encapsulation
Inheritance
Polymorphism
Abstraction
```

---

## Intermediate

**5. What is encapsulation?**

Bundling state and behavior together while controlling access to internal state.

**6. What is inheritance?**

A mechanism where a derived class reuses and extends a base class.

**7. What is polymorphism?**

The ability to use a common type/interface while obtaining different behavior depending on the actual implementation.

**8. What is abstraction?**

Exposing essential behavior while hiding implementation details.

**9. What is a constructor?**

A special member that runs when an object is created and is commonly used to initialize the object.

**10. Can a class have multiple constructors?**

Yes. This is constructor overloading.

**11. Can a constructor have a return type?**

No.

**12. Can constructors be inherited?**

No.

**13. What is method overloading?**

Multiple methods with the same name but different parameter lists.

**14. What is method overriding?**

A derived class provides a new implementation of an inherited virtual/abstract member.

---

# 54. Advanced Interview Questions

### What is the difference between `virtual`, `override`, and `abstract`?

```text
virtual
→ Base class provides implementation
→ Derived class may override it

abstract
→ Base class does not provide implementation
→ Derived non-abstract class must implement it

override
→ Derived class provides/replaces implementation
```

Example:

```csharp
class Employee
{
    public virtual void Work()
    {
        Console.WriteLine("Working");
    }
}

class Developer : Employee
{
    public override void Work()
    {
        Console.WriteLine("Writing code");
    }
}
```

---

### Can you create an object of an abstract class?

No.

```csharp
abstract class Employee
{
}
```

This is invalid:

```csharp
Employee employee = new Employee();
```

But this is valid:

```csharp
Employee employee = new Developer();
```

---

### Can C# support multiple inheritance?

A C# class cannot inherit from multiple classes.

Invalid:

```csharp
class C : A, B
{
}
```

But a class can implement multiple interfaces:

```csharp
class Employee : IWorkable, IReportable
{
}
```

Interfaces will be covered in more detail later.

---

# 55. DAY 4 CHEAT SHEET

```text
CLASS
    ↓
Blueprint

OBJECT
    ↓
Instance of class

CONSTRUCTOR
    ↓
Initializes object

FIELD
    ↓
Variable inside class

PROPERTY
    ↓
Controlled access to data

ENCAPSULATION
    ↓
Protect/control internal state

INHERITANCE
    ↓
Reuse/extend base class

POLYMORPHISM
    ↓
Different behavior through a common type

ABSTRACTION
    ↓
Hide implementation details

PUBLIC
    ↓
Accessible broadly

PRIVATE
    ↓
Containing class only

PROTECTED
    ↓
Containing class + derived classes

INTERNAL
    ↓
Same assembly

STATIC
    ↓
Belongs to type

CONST
    ↓
Compile-time constant

READONLY
    ↓
Assigned at declaration/constructor
```

---

# Day 4 — What You Must Be Able to Write

By the end of today's class, you should be able to write this without looking at notes:

```csharp
class Employee
{
    private decimal salary;

    public int Id { get; }

    public string Name { get; private set; }

    public Employee(int id, string name, decimal salary)
    {
        Id = id;
        Name = name;
        this.salary = salary;
    }

    public virtual void Work()
    {
        Console.WriteLine($"{Name} is working");
    }

    public decimal GetSalary()
    {
        return salary;
    }
}

class Developer : Employee
{
    public Developer(int id, string name, decimal salary)
        : base(id, name, salary)
    {
    }

    public override void Work()
    {
        Console.WriteLine($"{Name} is developing software");
    }
}

Employee employee = new Developer(
    101,
    "Chitra",
    60000
);

employee.Work();
```

If you understand **why every line exists** in this example, you have covered the core of Day 4.

### Day 4 learning flow

```text
Class & Object
      ↓
Fields & Properties
      ↓
Constructors
      ↓
Constructor Overloading
      ↓
Access Modifiers
      ↓
Encapsulation
      ↓
Inheritance
      ↓
Polymorphism
      ↓
Abstraction
      ↓
static
      ↓
readonly
      ↓
const
      ↓
Employee OOP Project
```


