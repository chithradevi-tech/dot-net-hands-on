# Day 5 — Advanced OOP in C#

```text
Abstract Classes
Interfaces
Virtual / Override
Method Hiding
Sealed
Composition
Association
Aggregation
        ↓
Interface vs Abstract Class
        ↓
SOLID Principles
        ↓
Real-world Design
```

---

# 1. Abstract Classes

An **abstract class** is a class that is designed to be inherited from.

You **cannot directly create an object** of an abstract class.

```csharp
abstract class Employee
{
    public string Name { get; set; } = "";

    public abstract void Work();
}
```

This is invalid:

```csharp
Employee employee = new Employee();
```

Because `Employee` is abstract.

A derived class must provide the implementation of the abstract method:

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

---

# 2. Abstract Method

An abstract method has **no implementation** in the abstract class.

```csharp
abstract class Employee
{
    public abstract void Work();
}
```

Notice:

```text
abstract
void Work();
```

There is no `{ }`.

The derived class must implement it:

```csharp
class Developer : Employee
{
    public override void Work()
    {
        Console.WriteLine("Writing code");
    }
}
```

---

# 3. Abstract Class Can Have Normal Methods

This is an important point.

An abstract class can contain:

* Fields
* Properties
* Constructors
* Normal methods
* Virtual methods
* Abstract methods
* Static members

Example:

```csharp
abstract class Employee
{
    public string Name { get; set; } = "";

    public void Login()
    {
        Console.WriteLine($"{Name} logged in");
    }

    public abstract void Work();
}
```

Derived class:

```csharp
class Developer : Employee
{
    public override void Work()
    {
        Console.WriteLine($"{Name} is writing code");
    }
}
```

Usage:

```csharp
Developer developer = new Developer();

developer.Name = "Chitra";

developer.Login();
developer.Work();
```

Output:

```text
Chitra logged in
Chitra is writing code
```

---

# 4. Abstract Class with Constructor

An abstract class can have a constructor.

```csharp
abstract class Employee
{
    public string Name { get; }

    protected Employee(string name)
    {
        Name = name;
    }

    public abstract void Work();
}
```

Derived class:

```csharp
class Developer : Employee
{
    public Developer(string name)
        : base(name)
    {
    }

    public override void Work()
    {
        Console.WriteLine($"{Name} is developing software");
    }
}
```

Create:

```csharp
Developer developer = new Developer("Chitra");
```

The base constructor runs as part of creating the derived object.

---

# 5. Interfaces

An **interface** defines a contract.

Think:

> "Any class implementing this interface must provide these members."

Example:

```csharp
public interface IPaymentService
{
    void Pay();
}
```

Implementation:

```csharp
public class CreditCardPayment : IPaymentService
{
    public void Pay()
    {
        Console.WriteLine("Payment made using Credit Card");
    }
}
```

Another implementation:

```csharp
public class UpiPayment : IPaymentService
{
    public void Pay()
    {
        Console.WriteLine("Payment made using UPI");
    }
}
```

Usage:

```csharp
IPaymentService payment = new UpiPayment();

payment.Pay();
```

Output:

```text
Payment made using UPI
```

---

# 6. Why Do We Need Interfaces?

Imagine an application supports:

```text
Credit Card
UPI
PayPal
Bank Transfer
```

Without interfaces, your code may become tightly coupled.

With an interface:

```text
             IPaymentService
              /     |      \
             /      |       \
      CreditCard   UPI    PayPal
```

All payment classes follow the same contract.

```csharp
IPaymentService payment;
```

The application doesn't necessarily need to care about the concrete implementation.

---

# 7. Multiple Interfaces

A class can implement multiple interfaces.

```csharp
interface IPrintable
{
    void Print();
}

interface IExportable
{
    void Export();
}
```

A class can implement both:

```csharp
class Report : IPrintable, IExportable
{
    public void Print()
    {
        Console.WriteLine("Printing report");
    }

    public void Export()
    {
        Console.WriteLine("Exporting report");
    }
}
```

This is one important way C# supports multiple contracts without multiple class inheritance.

---

# 8. Interface Naming Convention

C# convention is:

```text
I + InterfaceName
```

Examples:

```csharp
IPaymentService
IEmployeeRepository
ILogger
IUserService
IEmailSender
```

The `I` is a naming convention.

---

# 9. Interface Members

Modern C# interfaces can contain more features than the traditional "method signatures only" model, including default implementations and static members.

But for your fundamentals, the most important pattern is:

```csharp
public interface IPaymentService
{
    void Pay();
}
```

The implementing class provides the behavior.

---

# 10. Interface Implementation

```csharp
interface IPaymentService
{
    void Pay();
}

class UpiPayment : IPaymentService
{
    public void Pay()
    {
        Console.WriteLine("UPI payment");
    }
}
```

The method must satisfy the interface contract.

---

# 11. Interface Reference

This is very important.

```csharp
IPaymentService payment = new UpiPayment();
```

Here:

```text
IPaymentService → reference/declared type
UpiPayment      → actual object type
```

Then:

```csharp
payment.Pay();
```

calls the implementation provided by `UpiPayment`.

This is a form of polymorphism.

---

# 12. Interface vs Abstract Class

This is one of the **most important C# interview questions**.

| Abstract Class                             | Interface                                                       |
| ------------------------------------------ | --------------------------------------------------------------- |
| Declared using `abstract class`            | Declared using `interface`                                      |
| Can contain fields                         | Cannot have instance fields                                     |
| Can have constructors                      | Cannot have instance constructors                               |
| Can contain implemented methods            | Can define contracts and, in modern C#, default implementations |
| Can contain abstract methods               | Can contain abstract contract members                           |
| Supports inheritance of one class          | A class can implement multiple interfaces                       |
| Can contain access modifiers on members    | Interface contract members are generally public                 |
| Represents shared base type/implementation | Represents a contract/capability                                |

### Simple rule

Use an **abstract class** when classes share:

* common identity
* common state
* common implementation

Use an **interface** when you mainly want to define:

* a contract
* a capability
* interchangeable implementations

---

# 13. Real-World Example

Suppose we have:

```text
Employee
Developer
Manager
Tester
```

They share common employee information:

```text
Id
Name
Salary
Login()
```

An abstract class can be useful:

```csharp
abstract class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public decimal Salary { get; set; }

    public void Login()
    {
        Console.WriteLine($"{Name} logged in");
    }

    public abstract void Work();
}
```

But suppose some employees can generate reports:

```csharp
interface IReportGenerator
{
    void GenerateReport();
}
```

Then:

```csharp
class Manager : Employee, IReportGenerator
{
    public override void Work()
    {
        Console.WriteLine("Manager is managing");
    }

    public void GenerateReport()
    {
        Console.WriteLine("Generating management report");
    }
}
```

Here:

```text
Employee
   ↓
Manager

Manager also implements
IReportGenerator
```

---

# 14. Virtual Methods

A `virtual` method provides a default implementation that derived classes **may override**.

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
        Console.WriteLine("Developer is coding");
    }
}
```

Usage:

```csharp
Employee employee = new Developer();

employee.Work();
```

Output:

```text
Developer is coding
```

---

# 15. `override`

`override` replaces the inherited virtual or abstract implementation.

Base:

```csharp
class Employee
{
    public virtual void Work()
    {
        Console.WriteLine("Employee working");
    }
}
```

Derived:

```csharp
class Developer : Employee
{
    public override void Work()
    {
        Console.WriteLine("Developer coding");
    }
}
```

---

# 16. Virtual vs Abstract

### Virtual

Provides default behavior.

```csharp
public virtual void Work()
{
    Console.WriteLine("Working");
}
```

Derived class:

```text
Can override
```

### Abstract

Provides no implementation.

```csharp
public abstract void Work();
```

Derived class:

```text
Must override
```

Easy memory:

```text
virtual  → optional override

abstract → required implementation
```

---

# 17. Method Hiding

Method hiding happens when a derived class declares a method with the same name as a base class method using `new`.

Example:

```csharp
class Employee
{
    public void Work()
    {
        Console.WriteLine("Employee working");
    }
}
```

Derived class:

```csharp
class Developer : Employee
{
    public new void Work()
    {
        Console.WriteLine("Developer coding");
    }
}
```

Now:

```csharp
Developer developer = new Developer();

developer.Work();
```

Output:

```text
Developer coding
```

But:

```csharp
Employee employee = new Developer();

employee.Work();
```

Output:

```text
Employee working
```

Why?

Because method hiding is based on the **reference type** when accessed through the base reference.

---

# 18. Method Hiding vs Override

Very important.

### Hiding

```csharp
class Developer : Employee
{
    public new void Work()
    {
    }
}
```

The base and derived methods remain separate.

### Overriding

```csharp
class Developer : Employee
{
    public override void Work()
    {
    }
}
```

Runtime polymorphism determines the implementation.

### Comparison

| Method Hiding                       | Override                                                |
| ----------------------------------- | ------------------------------------------------------- |
| Uses `new`                          | Uses `override`                                         |
| Hides base member                   | Replaces virtual behavior                               |
| Depends on reference type           | Runtime polymorphism                                    |
| Base method still exists separately | Derived implementation participates in virtual dispatch |

### Practical recommendation

When you genuinely want polymorphic behavior, use `virtual` + `override`.

Use method hiding only when you intentionally want to hide the inherited member.

---

# 19. Sealed Classes

A `sealed` class cannot be inherited.

```csharp
sealed class PaymentProcessor
{
    public void Process()
    {
        Console.WriteLine("Processing payment");
    }
}
```

This is invalid:

```csharp
class CustomPaymentProcessor : PaymentProcessor
{
}
```

because `PaymentProcessor` is sealed.

---

# 20. Why Use a Sealed Class?

A class may be sealed when its design should not permit inheritance.

Example:

```csharp
sealed class ConfigurationManager
{
}
```

The important point is:

> A sealed class prevents further inheritance.

---

# 21. Sealed Methods

A method can be sealed so that a derived class cannot override it further.

A method must already be an override to be sealed.

```csharp
class Employee
{
    public virtual void Work()
    {
        Console.WriteLine("Employee working");
    }
}
```

First derived class:

```csharp
class Developer : Employee
{
    public sealed override void Work()
    {
        Console.WriteLine("Developer coding");
    }
}
```

Now:

```csharp
class SeniorDeveloper : Developer
{
    // Cannot override Work()
}
```

Because `Developer.Work()` is sealed.

---

# 22. Sealed Class vs Sealed Method

```text
sealed class
     ↓
Cannot inherit from this class

sealed override method
     ↓
Cannot override this method further
```

---

# 23. Composition

Composition means creating a class using other objects as its components.

Think:

> **has-a relationship**

Example:

```text
Car
 └── Engine
```

A car **has an engine**.

```csharp
class Engine
{
    public void Start()
    {
        Console.WriteLine("Engine started");
    }
}

class Car
{
    private readonly Engine engine;

    public Car()
    {
        engine = new Engine();
    }

    public void Start()
    {
        engine.Start();
        Console.WriteLine("Car started");
    }
}
```

Usage:

```csharp
Car car = new Car();

car.Start();
```

---

# 24. Composition Example in .NET

Consider an order service.

```text
OrderService
 ├── PaymentService
 ├── EmailService
 └── OrderRepository
```

Instead of putting everything into one huge class, we compose the service from smaller components.

```csharp
class PaymentService
{
    public void Pay()
    {
        Console.WriteLine("Payment completed");
    }
}

class EmailService
{
    public void SendEmail()
    {
        Console.WriteLine("Email sent");
    }
}

class OrderService
{
    private readonly PaymentService paymentService;
    private readonly EmailService emailService;

    public OrderService(
        PaymentService paymentService,
        EmailService emailService)
    {
        this.paymentService = paymentService;
        this.emailService = emailService;
    }

    public void PlaceOrder()
    {
        paymentService.Pay();
        emailService.SendEmail();
    }
}
```

This is composition.

Later, ASP.NET Core's **Dependency Injection** system will make this style much more powerful.

---

# 25. Association

Association means two objects are related to each other.

It is a general relationship.

Example:

```text
Teacher ↔ Student
```

A teacher interacts with students.

```csharp
class Student
{
    public string Name { get; set; } = "";
}

class Teacher
{
    public void Teach(Student student)
    {
        Console.WriteLine($"Teaching {student.Name}");
    }
}
```

The `Teacher` and `Student` objects are associated.

---

# 26. Aggregation

Aggregation is a **has-a relationship** where the contained object can exist independently of the container.

Example:

```text
Department
   ↓
Employees
```

Employees can exist even if the department is removed.

```csharp
class Employee
{
    public string Name { get; set; } = "";
}

class Department
{
    public List<Employee> Employees { get; }

    public Department(List<Employee> employees)
    {
        Employees = employees;
    }
}
```

Employees are supplied from outside.

```csharp
Employee employee1 = new Employee
{
    Name = "Chitra"
};

Employee employee2 = new Employee
{
    Name = "Arun"
};

List<Employee> employees = new()
{
    employee1,
    employee2
};

Department department = new Department(employees);
```

The `Employee` objects can continue to exist independently of `Department`.

---

# 27. Composition vs Aggregation

This distinction is useful for interviews.

### Composition

Strong ownership.

```text
House
 └── Room
```

The containing object manages the component as part of its own lifecycle.

### Aggregation

Weaker relationship.

```text
Department
 └── Employee
```

The employee can exist independently.

Easy memory:

```text
Composition → Strong has-a

Aggregation → Weak has-a
```

---

# 28. Association vs Aggregation vs Composition

Think about the strength of the relationship:

```text
Association
    ↓
General relationship

Aggregation
    ↓
Has-a
Objects can exist independently

Composition
    ↓
Strong has-a
Contained object is part of the owner's structure/lifecycle
```

These terms describe **object relationships/design concepts**; C# does not have separate keywords called `association`, `aggregation`, and `composition`.

---

# 29. SOLID Principles

Now we reach one of the most important professional OOP topics.

**SOLID** is a collection of five design principles.

```text
S → Single Responsibility Principle
O → Open/Closed Principle
L → Liskov Substitution Principle
I → Interface Segregation Principle
D → Dependency Inversion Principle
```

These principles help create code that is easier to:

* maintain
* test
* extend
* understand
* modify

---

# 30. S — Single Responsibility Principle

### Definition

> A class should have one responsibility and therefore one main reason to change.

Bad example:

```csharp
class EmployeeService
{
    public void CalculateSalary()
    {
    }

    public void SaveToDatabase()
    {
    }

    public void SendEmail()
    {
    }

    public void GeneratePdf()
    {
    }
}
```

This class has too many responsibilities.

```text
Salary calculation
Database
Email
PDF
```

Better:

```csharp
class SalaryCalculator
{
    public void Calculate()
    {
    }
}

class EmployeeRepository
{
    public void Save()
    {
    }
}

class EmailService
{
    public void Send()
    {
    }
}

class ReportGenerator
{
    public void Generate()
    {
    }
}
```

Each class has a focused responsibility.

---

# 31. O — Open/Closed Principle

### Definition

> Software entities should be open for extension but closed for modification.

Suppose you have payment methods.

A problematic design:

```csharp
class PaymentService
{
    public void Pay(string type)
    {
        if (type == "UPI")
        {
            // UPI
        }
        else if (type == "Card")
        {
            // Card
        }
        else if (type == "PayPal")
        {
            // PayPal
        }
    }
}
```

Every time a new payment method is added, you keep modifying this class.

Instead:

```csharp
interface IPaymentService
{
    void Pay();
}
```

Implementations:

```csharp
class UpiPayment : IPaymentService
{
    public void Pay()
    {
        Console.WriteLine("UPI payment");
    }
}

class CardPayment : IPaymentService
{
    public void Pay()
    {
        Console.WriteLine("Card payment");
    }
}
```

Now a new payment type can be added by creating another implementation.

```csharp
class PayPalPayment : IPaymentService
{
    public void Pay()
    {
        Console.WriteLine("PayPal payment");
    }
}
```

The existing payment contract remains unchanged.

---

# 32. L — Liskov Substitution Principle

### Definition

> Objects of a derived type should be usable wherever objects of the base type are expected without breaking the expected behavior.

Consider:

```csharp
class Bird
{
    public virtual void Fly()
    {
        Console.WriteLine("Flying");
    }
}
```

If we create:

```csharp
class Penguin : Bird
{
    public override void Fly()
    {
        throw new NotSupportedException();
    }
}
```

we have a design problem.

Code expecting a `Bird` that can `Fly()` may break when given a `Penguin`.

The deeper problem is that the base abstraction promises behavior that isn't appropriate for every derived type.

A better design can separate capabilities:

```csharp
class Bird
{
    public void Eat()
    {
        Console.WriteLine("Eating");
    }
}

interface IFlyingBird
{
    void Fly();
}

class Eagle : Bird, IFlyingBird
{
    public void Fly()
    {
        Console.WriteLine("Eagle flying");
    }
}

class Penguin : Bird
{
}
```

Now we don't force `Penguin` to implement flying behavior it doesn't support.

---

# 33. I — Interface Segregation Principle

### Definition

> Clients should not be forced to depend on interfaces they do not use.

Bad:

```csharp
interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
}
```

Suppose a class only needs `Work()`.

It shouldn't be forced to implement everything.

Better:

```csharp
interface IWorkable
{
    void Work();
}

interface IEatable
{
    void Eat();
}

interface ISleepable
{
    void Sleep();
}
```

A class can implement only what it needs:

```csharp
class Robot : IWorkable
{
    public void Work()
    {
        Console.WriteLine("Robot working");
    }
}
```

This is a major reason interfaces should be **small and focused**.

---

# 34. D — Dependency Inversion Principle

### Definition

> High-level modules should not depend directly on low-level concrete implementations. Both should depend on abstractions.

Instead of:

```csharp
class OrderService
{
    private readonly MySqlOrderRepository repository =
        new MySqlOrderRepository();
}
```

the high-level service is tightly coupled to a specific implementation.

Better:

```csharp
interface IOrderRepository
{
    void Save();
}
```

Implementation:

```csharp
class SqlOrderRepository : IOrderRepository
{
    public void Save()
    {
        Console.WriteLine("Saving order to SQL Server");
    }
}
```

Service:

```csharp
class OrderService
{
    private readonly IOrderRepository repository;

    public OrderService(IOrderRepository repository)
    {
        this.repository = repository;
    }

    public void PlaceOrder()
    {
        repository.Save();
    }
}
```

Now:

```csharp
IOrderRepository repository =
    new SqlOrderRepository();

OrderService service =
    new OrderService(repository);

service.PlaceOrder();
```

The important idea is:

```text
OrderService
     ↓
IOrderRepository
     ↑
SqlOrderRepository
```

rather than:

```text
OrderService
     ↓
SqlOrderRepository
```

This concept directly leads into **Dependency Injection in ASP.NET Core**.

---

# 35. SOLID — Easy Memory

```text
S → One responsibility

O → Extend without repeatedly modifying existing code

L → Derived types should behave correctly wherever the base type is expected

I → Keep interfaces small and focused

D → Depend on abstractions, not concrete implementations
```

---

# 36. One Real-World Example Combining SOLID

Imagine an online shopping system.

We need:

```text
Order
Payment
Email
Database
```

A poor design might put everything into one class.

Instead:

```text
                OrderService
                    │
        ┌───────────┼───────────┐
        ↓           ↓           ↓
 IOrderRepository IPayment   IEmailService
                     │
              ┌──────┼──────┐
              ↓      ↓      ↓
             UPI    Card   PayPal
```

Example:

```csharp
interface IPaymentService
{
    void Pay(decimal amount);
}

interface IEmailService
{
    void Send(string message);
}

interface IOrderRepository
{
    void Save();
}
```

Implementations:

```csharp
class UpiPayment : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"UPI payment: {amount}");
    }
}

class EmailService : IEmailService
{
    public void Send(string message)
    {
        Console.WriteLine($"Email: {message}");
    }
}

class OrderRepository : IOrderRepository
{
    public void Save()
    {
        Console.WriteLine("Order saved");
    }
}
```

Order service:

```csharp
class OrderService
{
    private readonly IPaymentService paymentService;
    private readonly IEmailService emailService;
    private readonly IOrderRepository orderRepository;

    public OrderService(
        IPaymentService paymentService,
        IEmailService emailService,
        IOrderRepository orderRepository)
    {
        this.paymentService = paymentService;
        this.emailService = emailService;
        this.orderRepository = orderRepository;
    }

    public void PlaceOrder(decimal amount)
    {
        paymentService.Pay(amount);
        orderRepository.Save();
        emailService.Send("Order placed successfully");
    }
}
```

Usage:

```csharp
IPaymentService payment = new UpiPayment();
IEmailService email = new EmailService();
IOrderRepository repository = new OrderRepository();

OrderService orderService =
    new OrderService(payment, email, repository);

orderService.PlaceOrder(1500);
```

This example demonstrates:

```text
Interfaces
Abstraction
Polymorphism
Composition
Dependency Injection style
Dependency Inversion
Single Responsibility
Open/Closed
Interface Segregation
```

---

# 37. Dependency Injection Preview

You will study Dependency Injection properly later in **ASP.NET Core**.

For now understand this pattern:

Instead of:

```csharp
class OrderService
{
    private readonly UpiPayment payment = new UpiPayment();
}
```

we do:

```csharp
class OrderService
{
    private readonly IPaymentService payment;

    public OrderService(IPaymentService payment)
    {
        this.payment = payment;
    }
}
```

Then:

```csharp
OrderService service =
    new OrderService(new UpiPayment());
```

The dependency is **provided from outside**.

That is Dependency Injection.

ASP.NET Core has a built-in DI container that can automatically create and supply these dependencies.

---

# 38. Day 5 Practical Project

Create a Visual Studio project:

```text
DotNetHandsOn
│
└── Day05AdvancedOOP
```

Create:

```text
Day05AdvancedOOP
│
├── Program.cs
│
├── IPaymentService.cs
├── UpiPayment.cs
├── CardPayment.cs
├── IEmailService.cs
├── EmailService.cs
├── IOrderRepository.cs
├── OrderRepository.cs
└── OrderService.cs
```

---

## IPaymentService.cs

```csharp
public interface IPaymentService
{
    void Pay(decimal amount);
}
```

---

## UpiPayment.cs

```csharp
public class UpiPayment : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"UPI payment completed: {amount:C}");
    }
}
```

---

## CardPayment.cs

```csharp
public class CardPayment : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Card payment completed: {amount:C}");
    }
}
```

---

## IEmailService.cs

```csharp
public interface IEmailService
{
    void Send(string message);
}
```

---

## EmailService.cs

```csharp
public class EmailService : IEmailService
{
    public void Send(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}
```

---

## IOrderRepository.cs

```csharp
public interface IOrderRepository
{
    void Save();
}
```

---

## OrderRepository.cs

```csharp
public class OrderRepository : IOrderRepository
{
    public void Save()
    {
        Console.WriteLine("Order saved to database");
    }
}
```

---

## OrderService.cs

```csharp
public class OrderService
{
    private readonly IPaymentService paymentService;
    private readonly IEmailService emailService;
    private readonly IOrderRepository orderRepository;

    public OrderService(
        IPaymentService paymentService,
        IEmailService emailService,
        IOrderRepository orderRepository)
    {
        this.paymentService = paymentService;
        this.emailService = emailService;
        this.orderRepository = orderRepository;
    }

    public void PlaceOrder(decimal amount)
    {
        Console.WriteLine("Placing order...");

        paymentService.Pay(amount);

        orderRepository.Save();

        emailService.Send("Order placed successfully.");

        Console.WriteLine("Order completed.");
    }
}
```

---

## Program.cs

```csharp
IPaymentService paymentService = new UpiPayment();

IEmailService emailService = new EmailService();

IOrderRepository orderRepository = new OrderRepository();

OrderService orderService = new OrderService(
    paymentService,
    emailService,
    orderRepository
);

orderService.PlaceOrder(1500);
```

Expected output:

```text
Placing order...
UPI payment completed: ₹1,500.00
Order saved to database
Email sent: Order placed successfully.
Order completed.
```

The exact currency formatting can vary depending on your system culture.

---

# 39. Change Payment Without Changing OrderService

Now replace:

```csharp
IPaymentService paymentService = new UpiPayment();
```

with:

```csharp
IPaymentService paymentService = new CardPayment();
```

Nothing in `OrderService` needs to change.

This is one of the most important ideas behind programming to an abstraction.

---

# 40. Day 5 Interview Questions

### 1. What is an abstract class?

A class that can provide shared implementation and abstract members and cannot itself be instantiated.

### 2. Can an abstract class have a constructor?

Yes.

### 3. Can an abstract class contain normal methods?

Yes.

### 4. Can an abstract class contain fields?

Yes.

### 5. Can we create an object of an abstract class?

No.

### 6. What is an interface?

An interface defines a contract that implementing types agree to fulfill.

### 7. Can a class implement multiple interfaces?

Yes.

```csharp
class Employee : IWorkable, IReportable
{
}
```

### 8. Can a class inherit multiple classes?

No.

### 9. What is `virtual`?

It allows a derived class to override a base implementation.

### 10. What is `override`?

It provides a new implementation of an inherited virtual or abstract member.

### 11. What is method hiding?

Using `new` to hide an inherited member rather than override its virtual behavior.

### 12. What is a sealed class?

A class that cannot be inherited.

### 13. What is a sealed method?

A method marked `sealed` after an `override`, preventing further overriding.

### 14. What is composition?

Building a class using other objects as its components — a strong "has-a" relationship.

### 15. What is aggregation?

A weaker "has-a" relationship where the contained objects can exist independently.

### 16. What is association?

A general relationship between objects.

### 17. What does SOLID stand for?

```text
S → Single Responsibility
O → Open/Closed
L → Liskov Substitution
I → Interface Segregation
D → Dependency Inversion
```

### 18. Which SOLID principle says "one reason to change"?

**Single Responsibility Principle.**

### 19. Which principle says "open for extension, closed for modification"?

**Open/Closed Principle.**

### 20. Which principle deals with substituting derived types for base types?

**Liskov Substitution Principle.**

### 21. Which principle says interfaces should be small and focused?

**Interface Segregation Principle.**

### 22. Which principle encourages depending on abstractions?

**Dependency Inversion Principle.**

---

# 41. Very Important Comparisons

## Abstract Class vs Interface

```text
Abstract Class
      ↓
Shared identity + state + implementation

Interface
      ↓
Contract / capability
```

---

## Virtual vs Abstract

```text
virtual
   ↓
Has implementation
Override is optional

abstract
   ↓
No implementation
Override is required
```

---

## Override vs Hiding

```text
override
   ↓
Runtime polymorphism

new
   ↓
Method hiding
```

---

## Composition vs Inheritance

```text
Inheritance
    ↓
"is-a"

Developer IS-A Employee


Composition
    ↓
"has-a"

Car HAS-A Engine
OrderService HAS-A PaymentService
```

A useful design guideline is to consider composition when you need to assemble behavior from independent components rather than forcing everything into an inheritance hierarchy.

---

# 42. Day 5 Final Revision Map

```text
                         ADVANCED OOP
                              │
          ┌───────────────────┼───────────────────┐
          │                   │                   │
     ABSTRACT             INTERFACE          RELATIONSHIPS
      CLASS                   │                   │
          │                   │             ┌─────┼─────┐
     ┌────┴────┐              │         Association
     │         │              │         Aggregation
  abstract   normal      Contract       Composition
  members    members
                              │
                         Polymorphism
                              │
                    ┌─────────┴─────────┐
                  virtual              override
                                         │
                                      sealed
                                         │
                                   method hiding
                                         │
                                      new
```

Then:

```text
                         SOLID
                           │
       ┌─────────┬─────────┼─────────┬─────────┐
       │         │         │         │         │
       S         O         L         I         D
       │         │         │         │         │
 Single      Open/     Liskov    Interface  Dependency
Responsibility Closed   Substitution Segregation Inversion
```

---

# Day 5 — What You Should Be Able to Explain in Class


```csharp
public interface IPaymentService
{
    void Pay(decimal amount);
}

public class UpiPayment : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"UPI payment: {amount}");
    }
}

public class OrderService
{
    private readonly IPaymentService paymentService;

    public OrderService(IPaymentService paymentService)
    {
        this.paymentService = paymentService;
    }

    public void PlaceOrder(decimal amount)
    {
        paymentService.Pay(amount);
    }
}
```

And explain:

```text
IPaymentService
    ↓
Interface / abstraction

UpiPayment
    ↓
Concrete implementation

OrderService
    ↓
High-level service

Constructor
    ↓
Dependency is injected

private readonly
    ↓
Dependency is stored and cannot be reassigned

IPaymentService reference
    ↓
Loose coupling

New payment type
    ↓
Can be added without changing OrderService
```


