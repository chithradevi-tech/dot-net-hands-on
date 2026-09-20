# Day 6 — Collections & Generics in C#

We will cover your complete list:

```text
COLLECTIONS
├── Array
├── List<T>
├── Dictionary<TKey, TValue>
├── HashSet<T>
├── Queue<T>
└── Stack<T>

COLLECTION INTERFACES
├── IEnumerable<T>
├── ICollection<T>
├── IList<T>
├── IReadOnlyCollection<T>
└── IReadOnlyList<T>

GENERICS
├── Generic classes
├── Generic methods
├── Generic interfaces
├── Generic constraints
├── where T : class
└── where T : new()
```

---

# 1. What is a Collection?

A **collection** is an object used to store and manage multiple values.

Instead of:

```csharp
string student1 = "Arun";
string student2 = "Priya";
string student3 = "Kumar";
```

we can use:

```csharp
List<string> students = new List<string>();

students.Add("Arun");
students.Add("Priya");
students.Add("Kumar");
```

Now we have one object containing multiple students.

### Why do we need collections?

Collections help us:

* Store multiple values
* Add/remove items dynamically
* Search items
* Sort items
* Iterate through items
* Avoid creating many individual variables
* Work efficiently with data

---

# 2. Array

You already learned arrays in Day 3, but we need to compare arrays with collections.

An array has a **fixed size**.

```csharp
int[] numbers = new int[5];

numbers[0] = 10;
numbers[1] = 20;
numbers[2] = 30;
numbers[3] = 40;
numbers[4] = 50;
```

Or:

```csharp
int[] numbers = { 10, 20, 30, 40, 50 };
```

Access:

```csharp
Console.WriteLine(numbers[0]);
```

Output:

```text
10
```

### Problem with arrays

The size is fixed.

```csharp
int[] numbers = new int[3];
```

You cannot simply do:

```csharp
numbers.Add(40); // ❌
```

For dynamically changing collections, we usually use `List<T>`.

---

# 3. List<T>

`List<T>` is one of the most commonly used collections in C#.

`T` represents the type of data.

For example:

```csharp
List<int> numbers = new List<int>();
```

Only integers can be stored.

```csharp
numbers.Add(10);
numbers.Add(20);
numbers.Add(30);
```

You can also use:

```csharp
List<string> names = new List<string>();

names.Add("Arun");
names.Add("Priya");
names.Add("Kumar");
```

---

## 3.1 Common List methods

### Add()

```csharp
List<string> names = new List<string>();

names.Add("Arun");
names.Add("Priya");
```

---

### AddRange()

```csharp
names.AddRange(new string[]
{
    "Kumar",
    "Ravi",
    "Meena"
});
```

---

### Count

```csharp
Console.WriteLine(names.Count);
```

`Count` tells us how many elements exist.

---

### Contains()

```csharp
if (names.Contains("Priya"))
{
    Console.WriteLine("Priya found");
}
```

---

### Remove()

```csharp
names.Remove("Priya");
```

---

### RemoveAt()

```csharp
names.RemoveAt(0);
```

---

### Clear()

```csharp
names.Clear();
```

---

### Sort()

```csharp
numbers.Sort();
```

---

### Reverse()

```csharp
numbers.Reverse();
```

---

### Insert()

```csharp
names.Insert(1, "Kumar");
```

---

### IndexOf()

```csharp
int index = names.IndexOf("Arun");
```

---

# 4. Complete List Example

```csharp
List<string> employees = new List<string>();

employees.Add("Arun");
employees.Add("Priya");
employees.Add("Kumar");

Console.WriteLine($"Total Employees: {employees.Count}");

foreach (string employee in employees)
{
    Console.WriteLine(employee);
}

if (employees.Contains("Priya"))
{
    Console.WriteLine("Priya exists");
}

employees.Remove("Kumar");

Console.WriteLine("After removing Kumar:");

foreach (string employee in employees)
{
    Console.WriteLine(employee);
}
```

---

# 5. Dictionary<TKey, TValue>

A `Dictionary` stores data as:

```text
Key → Value
```

Example:

```text
101 → Arun
102 → Priya
103 → Kumar
```

C#:

```csharp
Dictionary<int, string> employees = new Dictionary<int, string>();

employees.Add(101, "Arun");
employees.Add(102, "Priya");
employees.Add(103, "Kumar");
```

Here:

```text
TKey   = int
TValue = string
```

---

# 6. Access Dictionary Values

```csharp
Console.WriteLine(employees[101]);
```

Output:

```text
Arun
```

---

# 7. Dictionary ContainsKey()

Before accessing a key, you can check:

```csharp
if (employees.ContainsKey(101))
{
    Console.WriteLine(employees[101]);
}
```

---

# 8. Dictionary ContainsValue()

```csharp
if (employees.ContainsValue("Priya"))
{
    Console.WriteLine("Priya found");
}
```

---

# 9. Dictionary Remove()

```csharp
employees.Remove(102);
```

---

# 10. Dictionary Count

```csharp
Console.WriteLine(employees.Count);
```

---

# 11. Dictionary TryGetValue()

This is very useful in real applications.

Instead of:

```csharp
if (employees.ContainsKey(101))
{
    Console.WriteLine(employees[101]);
}
```

we can use:

```csharp
if (employees.TryGetValue(101, out string? employee))
{
    Console.WriteLine(employee);
}
```

`TryGetValue()` safely tries to retrieve a value.

---

# 12. Loop Through Dictionary

```csharp
foreach (var employee in employees)
{
    Console.WriteLine($"ID: {employee.Key}");
    Console.WriteLine($"Name: {employee.Value}");
}
```

Output:

```text
ID: 101
Name: Arun

ID: 102
Name: Priya
```

You can also write:

```csharp
foreach (KeyValuePair<int, string> employee in employees)
{
    Console.WriteLine($"{employee.Key} - {employee.Value}");
}
```

---

# 13. Important Dictionary Rule

Dictionary keys must be **unique**.

This works:

```csharp
employees.Add(101, "Arun");
employees.Add(102, "Priya");
```

This causes an exception:

```csharp
employees.Add(101, "Kumar");
```

because key `101` already exists.

You can instead use:

```csharp
employees[101] = "Kumar";
```

This updates the existing value.

---

# 14. HashSet<T>

`HashSet<T>` stores **unique values**.

Example:

```csharp
HashSet<int> numbers = new HashSet<int>();

numbers.Add(10);
numbers.Add(20);
numbers.Add(10);
numbers.Add(30);
```

The result contains:

```text
10
20
30
```

The duplicate `10` is ignored.

---

# 15. HashSet Example

```csharp
HashSet<string> skills = new HashSet<string>();

skills.Add("C#");
skills.Add("ASP.NET Core");
skills.Add("SQL");
skills.Add("C#");

foreach (string skill in skills)
{
    Console.WriteLine(skill);
}
```

`C#` appears only once.

---

# 16. HashSet Contains()

```csharp
if (skills.Contains("C#"))
{
    Console.WriteLine("C# exists");
}
```

---

# 17. HashSet Set Operations

One important advantage of `HashSet<T>` is set operations.

### Union

```csharp
HashSet<int> set1 = new HashSet<int> { 1, 2, 3 };
HashSet<int> set2 = new HashSet<int> { 3, 4, 5 };

set1.UnionWith(set2);
```

Result:

```text
1 2 3 4 5
```

### Intersection

```csharp
set1.IntersectWith(set2);
```

Gets common elements.

### Difference

```csharp
set1.ExceptWith(set2);
```

Gets elements that exist in `set1` but not `set2`.

---

# 18. Queue<T>

A `Queue<T>` follows:

> **FIFO — First In, First Out**

Think about a real-world queue at a ticket counter.

```text
First person → First served
```

Example:

```csharp
Queue<string> customers = new Queue<string>();

customers.Enqueue("Arun");
customers.Enqueue("Priya");
customers.Enqueue("Kumar");
```

Queue:

```text
Arun → Priya → Kumar
```

---

# 19. Queue Operations

### Enqueue()

Adds an item.

```csharp
customers.Enqueue("Meena");
```

### Dequeue()

Removes the first item.

```csharp
string customer = customers.Dequeue();

Console.WriteLine(customer);
```

Output:

```text
Arun
```

### Peek()

Views the first item without removing it.

```csharp
Console.WriteLine(customers.Peek());
```

### Count

```csharp
Console.WriteLine(customers.Count);
```

---

# 20. Queue Example

```csharp
Queue<string> customers = new Queue<string>();

customers.Enqueue("Customer 1");
customers.Enqueue("Customer 2");
customers.Enqueue("Customer 3");

Console.WriteLine($"Next: {customers.Peek()}");

while (customers.Count > 0)
{
    Console.WriteLine($"Serving: {customers.Dequeue()}");
}
```

Output:

```text
Next: Customer 1
Serving: Customer 1
Serving: Customer 2
Serving: Customer 3
```

---

# 21. Stack<T>

A `Stack<T>` follows:

> **LIFO — Last In, First Out**

Think of a stack of plates.

```text
Top → Plate 3
      Plate 2
      Plate 1
```

The last plate placed is the first one removed.

---

# 22. Stack Operations

### Push()

Adds an item.

```csharp
Stack<string> pages = new Stack<string>();

pages.Push("Home");
pages.Push("Products");
pages.Push("Details");
```

### Pop()

Removes the top item.

```csharp
string page = pages.Pop();

Console.WriteLine(page);
```

Output:

```text
Details
```

### Peek()

Views the top item.

```csharp
Console.WriteLine(pages.Peek());
```

### Count

```csharp
Console.WriteLine(pages.Count);
```

---

# 23. Real-world Stack Example

Browser history is a common conceptual example.

```csharp
Stack<string> browserHistory = new Stack<string>();

browserHistory.Push("Google");
browserHistory.Push("YouTube");
browserHistory.Push("GitHub");

Console.WriteLine($"Current Page: {browserHistory.Peek()}");

Console.WriteLine($"Back: {browserHistory.Pop()}");
Console.WriteLine($"Back: {browserHistory.Pop()}");
```

---

# 24. Collection Comparison

| Collection                | Main Purpose               | Duplicate Values    | Access   |
| ------------------------- | -------------------------- | ------------------- | -------- |
| Array                     | Fixed-size data            | Yes                 | Index    |
| `List<T>`                 | Dynamic ordered collection | Yes                 | Index    |
| `Dictionary<TKey,TValue>` | Key-value lookup           | Values yes, keys no | Key      |
| `HashSet<T>`              | Unique values              | No                  | No index |
| `Queue<T>`                | FIFO                       | Yes                 | Front    |
| `Stack<T>`                | LIFO                       | Yes                 | Top      |

### Easy memory trick

```text
Array      → Fixed size
List       → Dynamic list
Dictionary → Key → Value
HashSet    → Unique
Queue      → FIFO
Stack      → LIFO
```

---

# 25. What is IEnumerable<T>?

Now we move to **collection interfaces**.

`IEnumerable<T>` represents something that can be **enumerated/iterated**.

Example:

```csharp
IEnumerable<int> numbers = new List<int>
{
    10, 20, 30
};
```

We can use:

```csharp
foreach (int number in numbers)
{
    Console.WriteLine(number);
}
```

The important capability is:

> "I can iterate through this collection."

---

# 26. IEnumerable<T> Example

```csharp
IEnumerable<string> names = new List<string>
{
    "Arun",
    "Priya",
    "Kumar"
};

foreach (string name in names)
{
    Console.WriteLine(name);
}
```

But:

```csharp
names.Add("Meena");
```

is not available because the reference is `IEnumerable<string>`.

Why?

Because `IEnumerable<T>` doesn't expose `Add()`.

---

# 27. ICollection<T>

`ICollection<T>` provides more collection capabilities.

It supports operations such as:

```text
Count
Add
Remove
Clear
Contains
```

Example:

```csharp
ICollection<string> names = new List<string>();

names.Add("Arun");
names.Add("Priya");

Console.WriteLine(names.Count);

names.Remove("Arun");
```

---

# 28. IList<T>

`IList<T>` adds list/index-based capabilities.

```csharp
IList<string> names = new List<string>();

names.Add("Arun");
names.Add("Priya");

Console.WriteLine(names[0]);

names.Insert(1, "Kumar");

names.RemoveAt(0);
```

So `IList<T>` supports indexing.

---

# 29. IReadOnlyCollection<T>

`IReadOnlyCollection<T>` is useful when you want to expose a collection **without allowing modification through that reference**.

```csharp
IReadOnlyCollection<string> names =
    new List<string>
    {
        "Arun",
        "Priya"
    };
```

You can:

```csharp
Console.WriteLine(names.Count);
```

You can iterate:

```csharp
foreach (var name in names)
{
    Console.WriteLine(name);
}
```

But you cannot:

```csharp
names.Add("Kumar"); // ❌
```

---

# 30. IReadOnlyList<T>

`IReadOnlyList<T>` provides read-only access **with indexing**.

```csharp
IReadOnlyList<string> names =
    new List<string>
    {
        "Arun",
        "Priya",
        "Kumar"
    };
```

You can:

```csharp
Console.WriteLine(names[0]);
Console.WriteLine(names.Count);
```

But cannot:

```csharp
names.Add("Meena"); // ❌
```

---

# 31. Interface Hierarchy

A useful simplified understanding is:

```text
IEnumerable<T>
      ↑
ICollection<T>
      ↑
IList<T>
```

And separately:

```text
IReadOnlyCollection<T>
        ↑
IReadOnlyList<T>
```

Think about capabilities:

```text
IEnumerable<T>
    ↓
Can iterate

ICollection<T>
    ↓
Can iterate + Count + Add/Remove/Clear

IList<T>
    ↓
Collection + index access

IReadOnlyCollection<T>
    ↓
Read + Count, no mutation through interface

IReadOnlyList<T>
    ↓
Read + Count + index, no mutation through interface
```

---

# 32. Why Use Interfaces Instead of List<T>?

Consider:

```csharp
public List<string> GetEmployees()
{
    return employees;
}
```

This exposes a concrete implementation.

Instead:

```csharp
public IReadOnlyList<string> GetEmployees()
{
    return employees;
}
```

Now callers can read the employees but cannot directly modify the collection through that returned interface.

This is very common in well-designed applications.

---

# 33. What are Generics?

Generics allow us to write **type-safe reusable code**.

Instead of creating separate classes for:

```text
int
string
double
Employee
Product
Customer
```

we can create one generic class.

The type is supplied when we use it.

---

# 34. Why Generics?

Without generics, older code might use `object`:

```csharp
class Box
{
    public object Value { get; set; }
}
```

Usage:

```csharp
Box box = new Box();

box.Value = 100;

int number = (int)box.Value;
```

We need casting.

With generics:

```csharp
class Box<T>
{
    public T Value { get; set; }
}
```

Now:

```csharp
Box<int> box = new Box<int>();

box.Value = 100;

int number = box.Value;
```

No explicit cast.

---

# 35. Generic Class

Example:

```csharp
class Box<T>
{
    public T Value { get; set; }

    public void Display()
    {
        Console.WriteLine(Value);
    }
}
```

Use with `int`:

```csharp
Box<int> intBox = new Box<int>();

intBox.Value = 100;
intBox.Display();
```

Use with `string`:

```csharp
Box<string> stringBox = new Box<string>();

stringBox.Value = "Hello";
stringBox.Display();
```

Same class, different types.

---

# 36. Generic Method

A method can also have a type parameter.

```csharp
static void Display<T>(T value)
{
    Console.WriteLine(value);
}
```

Usage:

```csharp
Display<int>(100);
Display<string>("Hello");
Display<double>(10.5);
```

C# can usually infer the type:

```csharp
Display(100);
Display("Hello");
Display(10.5);
```

---

# 37. Generic Method with Two Types

```csharp
static void DisplayPair<TKey, TValue>(TKey key, TValue value)
{
    Console.WriteLine($"Key: {key}");
    Console.WriteLine($"Value: {value}");
}
```

Usage:

```csharp
DisplayPair(101, "Arun");
DisplayPair("EMP01", 50000);
```

---

# 38. Generic Interface

You can create generic interfaces.

```csharp
interface IRepository<T>
{
    void Add(T item);

    T GetById(int id);
}
```

Implementation:

```csharp
class EmployeeRepository : IRepository<Employee>
{
    public void Add(Employee item)
    {
        Console.WriteLine($"Adding {item.Name}");
    }

    public Employee GetById(int id)
    {
        return new Employee
        {
            Id = id,
            Name = "Arun"
        };
    }
}
```

Model:

```csharp
class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
}
```

---

# 39. Why Generic Repository?

Instead of:

```text
EmployeeRepository
ProductRepository
CustomerRepository
OrderRepository
```

we can create reusable generic functionality:

```csharp
IRepository<T>
```

For example:

```csharp
IRepository<Employee>
IRepository<Product>
IRepository<Customer>
```

The same generic contract can work with different types.

---

# 40. Generic Constraints

Sometimes we don't want to allow **every possible type**.

For example:

```csharp
class Repository<T>
{
}
```

`T` could be almost anything.

We can restrict it using:

```csharp
where T : ...
```

These are called **generic constraints**.

---

# 41. `where T : class`

This means:

> `T` must be a reference type.

Example:

```csharp
class Repository<T> where T : class
{
    public void Save(T item)
    {
        Console.WriteLine("Saving item");
    }
}
```

Allowed:

```csharp
Repository<Employee> repository =
    new Repository<Employee>();
```

because `Employee` is a class.

---

# 42. `where T : class` Example

```csharp
class Employee
{
    public string Name { get; set; } = "";
}

class Repository<T> where T : class
{
    public void Save(T item)
    {
        Console.WriteLine("Saved");
    }
}
```

Usage:

```csharp
Repository<Employee> repository =
    new Repository<Employee>();

repository.Save(new Employee
{
    Name = "Arun"
});
```

---

# 43. `where T : new()`

This means:

> `T` must have a public parameterless constructor.

Example:

```csharp
class Factory<T> where T : new()
{
    public T Create()
    {
        return new T();
    }
}
```

Now:

```csharp
class Employee
{
    public Employee()
    {
    }
}
```

We can do:

```csharp
Factory<Employee> factory =
    new Factory<Employee>();

Employee employee = factory.Create();
```

---

# 44. Why `new()` Constraint?

Because inside:

```csharp
return new T();
```

C# needs to know that `T` can be created using a parameterless constructor.

Without:

```csharp
where T : new()
```

this would not be allowed.

---

# 45. `class` vs `new()` Constraint

They solve different problems.

### `where T : class`

```csharp
class Repository<T> where T : class
```

Means:

```text
T must be a reference type.
```

### `where T : new()`

```csharp
class Factory<T> where T : new()
```

Means:

```text
T must have a public parameterless constructor.
```

You can combine them:

```csharp
class Factory<T> where T : class, new()
{
    public T Create()
    {
        return new T();
    }
}
```

Now `T` must:

1. Be a reference type
2. Have a public parameterless constructor

---

# 46. Other Generic Constraints You Should Know

Your syllabus specifically mentions `class` and `new()`, but for interviews you should know the other common constraints too.

### Struct

```csharp
where T : struct
```

`T` must be a value type.

---

### Base class

```csharp
where T : Employee
```

`T` must derive from `Employee`.

---

### Interface

```csharp
where T : IDisposable
```

`T` must implement `IDisposable`.

---

### Multiple constraints

```csharp
where T : Employee, IDisposable, new()
```

---

# 47. Important Generic Constraint Order

When multiple constraints are used, the usual order is:

```csharp
where T : class, InterfaceName, new()
```

or:

```csharp
where T : BaseClass, InterfaceName, new()
```

`new()` must appear last.

---

# 48. Real-World Generic Example

This is closer to what you will see later in ASP.NET Core.

```csharp
public interface IRepository<T> where T : class
{
    void Add(T entity);

    T? GetById(int id);
}
```

Implementation:

```csharp
public class Repository<T> : IRepository<T>
    where T : class, new()
{
    public void Add(T entity)
    {
        Console.WriteLine("Entity added");
    }

    public T? GetById(int id)
    {
        return new T();
    }
}
```

Usage:

```csharp
Repository<Employee> repository =
    new Repository<Employee>();

repository.Add(new Employee
{
    Name = "Arun"
});
```

This concept becomes very useful when you learn:

```text
ASP.NET Core
      ↓
Dependency Injection
      ↓
Services
      ↓
Repositories
      ↓
Entity Framework Core
```

---

# 49. Collection + Generics Together

Most modern C# collections are generic.

For example:

```csharp
List<int>
```

means:

```text
List of integers
```

```csharp
List<Employee>
```

means:

```text
List of Employee objects
```

```csharp
Dictionary<int, Employee>
```

means:

```text
Employee ID → Employee object
```

Example:

```csharp
Dictionary<int, Employee> employees =
    new Dictionary<int, Employee>();

employees.Add(
    101,
    new Employee
    {
        Name = "Arun"
    }
);
```

---

# 50. Complete Employee Collection Example

Create an `Employee` class:

```csharp
class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }
}
```

Create a list:

```csharp
List<Employee> employees = new List<Employee>
{
    new Employee
    {
        Id = 101,
        Name = "Arun",
        Department = "IT",
        Salary = 50000
    },

    new Employee
    {
        Id = 102,
        Name = "Priya",
        Department = "HR",
        Salary = 45000
    },

    new Employee
    {
        Id = 103,
        Name = "Kumar",
        Department = "IT",
        Salary = 60000
    }
};
```

Loop:

```csharp
foreach (Employee employee in employees)
{
    Console.WriteLine(
        $"{employee.Id} - {employee.Name} - " +
        $"{employee.Department} - {employee.Salary}"
    );
}
```

---

# 51. Dictionary of Employees

```csharp
Dictionary<int, Employee> employeeDictionary =
    new Dictionary<int, Employee>();
```

Add:

```csharp
foreach (Employee employee in employees)
{
    employeeDictionary.Add(employee.Id, employee);
}
```

Find employee:

```csharp
if (employeeDictionary.TryGetValue(102, out Employee? employee))
{
    Console.WriteLine(employee.Name);
}
```

This is a very common real-world pattern.

---

# 52. When Should You Use Which Collection?

### Use Array

When:

```text
Size is fixed
```

Example:

```csharp
int[] months = new int[12];
```

---

### Use List<T>

When:

```text
You need an ordered collection
You frequently add/remove items
You need index-based access
```

Example:

```csharp
List<Employee>
```

---

### Use Dictionary<TKey,TValue>

When:

```text
You need fast lookup using a key
```

Example:

```csharp
Dictionary<int, Employee>
```

---

### Use HashSet<T>

When:

```text
You need unique values
```

Example:

```csharp
HashSet<string> skills
```

---

### Use Queue<T>

When:

```text
First-in-first-out processing
```

Example:

```text
Customer service queue
Background jobs
Print jobs
```

---

### Use Stack<T>

When:

```text
Last-in-first-out processing
```

Example:

```text
Undo functionality
Navigation/history concepts
Expression processing
```

---

# 53. Important Interview Question — Array vs List<T>

### Array

```csharp
int[] numbers = new int[5];
```

* Fixed size
* Index-based
* Simple
* Efficient for fixed-size data

### List<T>

```csharp
List<int> numbers = new List<int>();
```

* Dynamic size
* Generic
* Add/remove operations
* Rich collection methods
* Commonly used for variable-length data

---

# 54. List<T> vs Dictionary<TKey,TValue>

```text
List
 ↓
Access using index

Dictionary
 ↓
Access using key
```

Example:

```csharp
employees[0]
```

vs:

```csharp
employeesDictionary[101]
```

Use Dictionary when the key itself is meaningful for lookup.

---

# 55. Dictionary vs HashSet

### Dictionary

```text
Key → Value
```

Example:

```csharp
101 → Arun
102 → Priya
```

### HashSet

```text
Unique values
```

Example:

```text
C#
SQL
Angular
```

---

# 56. Queue vs Stack

Remember:

```text
QUEUE
FIFO
First In → First Out

STACK
LIFO
Last In → First Out
```

Example:

```text
Queue:
A → B → C
Remove A

Stack:
A
B
C ← Remove C
```

---

# 57. IEnumerable vs ICollection vs IList

This is important for interviews.

### `IEnumerable<T>`

```csharp
IEnumerable<Employee>
```

Main idea:

> "I can enumerate/iterate."

---

### `ICollection<T>`

```csharp
ICollection<Employee>
```

Main idea:

> "I can manage a collection."

Provides things such as:

```text
Count
Add
Remove
Clear
Contains
```

---

### `IList<T>`

```csharp
IList<Employee>
```

Main idea:

> "I have list/index-based operations."

Example:

```csharp
employees[0]
```

---

# 58. IReadOnlyCollection vs IReadOnlyList

### IReadOnlyCollection<T>

Provides:

```text
Count
Iteration
Read-only collection view
```

### IReadOnlyList<T>

Provides:

```text
Count
Iteration
Index access
Read-only list view
```

Example:

```csharp
IReadOnlyList<Employee> employees;
```

Then:

```csharp
Employee employee = employees[0];
```

but:

```csharp
employees.Add(employee); // ❌
```

---

# 59. Important Concept — Read-only Interface Does Not Necessarily Mean Immutable

This is a subtle but important interview point.

Suppose:

```csharp
List<string> names = new()
{
    "Arun",
    "Priya"
};

IReadOnlyList<string> readOnlyNames = names;
```

You cannot modify through:

```csharp
readOnlyNames.Add(...); // ❌
```

But the original list can still be changed:

```csharp
names.Add("Kumar");
```

The `readOnlyNames` view can then reflect that change.

So:

> **Read-only interface ≠ immutable object**

It prevents modification **through that interface**, not necessarily through every reference to the underlying collection.

---

# 60. Generic Benefits

Why use generics?

### 1. Type safety

```csharp
List<int> numbers = new();

numbers.Add(10);

// numbers.Add("Hello"); ❌
```

Compiler catches the wrong type.

### 2. Reusability

One generic class can work with many types.

```csharp
Box<int>
Box<string>
Box<Employee>
```

### 3. Less casting

Generic code usually avoids unnecessary casts.

### 4. Better maintainability

You write reusable components once.

### 5. Performance

Generics can avoid some boxing/unboxing scenarios that occur with `object`-based code.

---

# 61. Day 6 Practical Project

For Visual Studio, create:

```text
DotNetHandsOn
│
└── Day06CollectionsGenerics
    │
    ├── Program.cs
    ├── Employee.cs
    ├── GenericRepository.cs
    └── IRepository.cs
```

## Employee.cs

```csharp
public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Department { get; set; } = "";

    public decimal Salary { get; set; }
}
```

## IRepository.cs

```csharp
public interface IRepository<T>
    where T : class
{
    void Add(T item);

    T? GetById(int id);
}
```

## GenericRepository.cs

```csharp
public class GenericRepository<T> : IRepository<T>
    where T : class, new()
{
    private readonly Dictionary<int, T> items = new();

    public void Add(T item)
    {
        Console.WriteLine("Item added");
    }

    public T? GetById(int id)
    {
        return new T();
    }
}
```

Then in `Program.cs`:

```csharp
List<Employee> employees = new()
{
    new Employee
    {
        Id = 101,
        Name = "Arun",
        Department = "IT",
        Salary = 50000
    },

    new Employee
    {
        Id = 102,
        Name = "Priya",
        Department = "HR",
        Salary = 45000
    },

    new Employee
    {
        Id = 103,
        Name = "Kumar",
        Department = "IT",
        Salary = 60000
    }
};

Console.WriteLine("EMPLOYEE LIST");
Console.WriteLine("-------------");

foreach (Employee employee in employees)
{
    Console.WriteLine(
        $"{employee.Id} - {employee.Name} - " +
        $"{employee.Department} - {employee.Salary}"
    );
}

Dictionary<int, Employee> employeeDictionary =
    employees.ToDictionary(e => e.Id);

Console.WriteLine();
Console.WriteLine("SEARCH EMPLOYEE");

if (employeeDictionary.TryGetValue(102, out Employee? selectedEmployee))
{
    Console.WriteLine(
        $"Found: {selectedEmployee.Name}"
    );
}

HashSet<string> departments = new();

foreach (Employee employee in employees)
{
    departments.Add(employee.Department);
}

Console.WriteLine();
Console.WriteLine("DEPARTMENTS");

foreach (string department in departments)
{
    Console.WriteLine(department);
}
```

---

# 62. Day 6 Interview Questions

### Beginner

1. What is a collection?
2. What is the difference between an array and `List<T>`?
3. What is `List<T>`?
4. What is `Dictionary<TKey,TValue>`?
5. Can Dictionary have duplicate keys?
6. What is `HashSet<T>`?
7. Why does HashSet ignore duplicate values?
8. What is Queue?
9. What is Stack?
10. What is FIFO?
11. What is LIFO?

### Intermediate

12. What is `IEnumerable<T>`?
13. Difference between `IEnumerable<T>` and `ICollection<T>`?
14. Difference between `ICollection<T>` and `IList<T>`?
15. What is `IReadOnlyCollection<T>`?
16. What is `IReadOnlyList<T>`?
17. Why expose `IReadOnlyList<T>` instead of `List<T>`?
18. What is a generic class?
19. What is a generic method?
20. What is a generic interface?
21. Why are generics useful?
22. What are generic constraints?

### Advanced

23. What does `where T : class` mean?
24. What does `where T : new()` mean?
25. Why is `new()` required when using `new T()`?
26. Can multiple generic constraints be used?
27. What is the difference between `IEnumerable<T>` and `IReadOnlyList<T>`?
28. Is `IReadOnlyList<T>` immutable?
29. When would you choose Dictionary over List?
30. When would you choose HashSet over List?
31. How are generics useful in repository patterns?
32. How do collections and generics appear in ASP.NET Core applications?

---

# 63. Day 6 Quick Revision

```text
ARRAY
→ Fixed size

List<T>
→ Dynamic ordered collection
→ Index-based

Dictionary<TKey,TValue>
→ Key + Value
→ Unique keys

HashSet<T>
→ Unique values

Queue<T>
→ FIFO
→ Enqueue / Dequeue / Peek

Stack<T>
→ LIFO
→ Push / Pop / Peek
```

Interfaces:

```text
IEnumerable<T>
→ Iterate

ICollection<T>
→ Collection operations

IList<T>
→ Collection + index

IReadOnlyCollection<T>
→ Read + Count

IReadOnlyList<T>
→ Read + Count + index
```

Generics:

```text
Generic class
→ class Box<T>

Generic method
→ void Display<T>(T value)

Generic interface
→ IRepository<T>

Constraints
→ where T : class
→ where T : new()
→ where T : struct
→ where T : BaseClass
→ where T : Interface
```

### Most important memory map

```text
              COLLECTIONS
                   │
     ┌─────────────┼─────────────┐
     ↓             ↓             ↓
   LIST        DICTIONARY      HASHSET
     │             │             │
  index        key → value     unique
     
     ┌─────────────┴─────────────┐
     ↓                           ↓
   QUEUE                        STACK
   FIFO                         LIFO
```

And the key connection to the next topics is:

```text
Collections
     ↓
Generics
     ↓
IEnumerable<T>
     ↓
LINQ
     ↓
Lambda Expressions
     ↓
Filtering / Sorting / Projection
     ↓
ASP.NET Core + Entity Framework
```

**Day 6 is therefore the foundation for Day 7 — LINQ & Lambda Expressions.**
