# Day 3 — C# Methods, Strings & Arrays

* Create and call methods
* Pass data using different parameter types
* Return values
* Use `ref`, `out`, and `in`
* Understand method overloading and recursion
* Manipulate and compare strings
* Parse and format values
* Work with 1D, 2D, and jagged arrays
* Build practical C# programs

---

# 1. METHODS

## What is a Method?

A **method** is a block of code designed to perform a specific task.

Instead of writing the same code repeatedly, we put it inside a method and call it whenever needed.

### Without Method

```csharp
int a = 10;
int b = 20;

int result = a + b;

Console.WriteLine(result);
```

If you need the same calculation multiple times, you'll repeat the code.

### With Method

```csharp
static int Add(int a, int b)
{
    return a + b;
}
```

Call it:

```csharp
int result = Add(10, 20);

Console.WriteLine(result);
```

Output:

```text
30
```

---

# 2. Method Syntax

Basic syntax:

```csharp
accessModifier returnType MethodName(parameters)
{
    // code
}
```

Example:

```csharp
static int Add(int a, int b)
{
    return a + b;
}
```

Here:

| Part     | Meaning                                            |
| -------- | -------------------------------------------------- |
| `static` | Method belongs to the type rather than an instance |
| `int`    | Return type                                        |
| `Add`    | Method name                                        |
| `int a`  | First parameter                                    |
| `int b`  | Second parameter                                   |
| `return` | Returns result                                     |

---

# 3. Calling a Method

```csharp
static void SayHello()
{
    Console.WriteLine("Hello");
}

SayHello();
```

Output:

```text
Hello
```

A method must generally be **called** for its code to execute.

---

# 4. `void` Methods

If a method doesn't return a value, use `void`.

```csharp
static void DisplayMessage()
{
    Console.WriteLine("Welcome to C#");
}
```

Call:

```csharp
DisplayMessage();
```

---

# 5. Methods with Parameters

Parameters allow us to send information into a method.

```csharp
static void Greet(string name)
{
    Console.WriteLine($"Hello {name}");
}
```

Call:

```csharp
Greet("Chitra");
Greet("Arun");
```

Output:

```text
Hello Chitra
Hello Arun
```

---

# 6. Parameters vs Arguments

This is an important interview question.

### Parameter

The variable defined in the method:

```csharp
static void Greet(string name)
```

`name` is the **parameter**.

### Argument

The actual value passed during the call:

```csharp
Greet("Chitra");
```

`"Chitra"` is the **argument**.

---

# 7. Return Values

A method can calculate something and return the result.

```csharp
static int Multiply(int a, int b)
{
    return a * b;
}
```

Call:

```csharp
int result = Multiply(5, 4);

Console.WriteLine(result);
```

Output:

```text
20
```

The return type is:

```csharp
int
```

because the method returns an integer.

---

# 8. Returning Different Data Types

### String

```csharp
static string GetName()
{
    return "Chitra";
}
```

### Double

```csharp
static double CalculateSalary(double salary)
{
    return salary * 12;
}
```

### Boolean

```csharp
static bool IsAdult(int age)
{
    return age >= 18;
}
```

---

# 9. Optional Parameters

An optional parameter has a default value.

```csharp
static void Greet(string name, string message = "Welcome")
{
    Console.WriteLine($"{message}, {name}");
}
```

Now we can call:

```csharp
Greet("Chitra");
```

Output:

```text
Welcome, Chitra
```

Or:

```csharp
Greet("Chitra", "Good Morning");
```

Output:

```text
Good Morning, Chitra
```

### Important rule

Optional parameters generally come **after required parameters**.

Correct:

```csharp
static void Test(string name, int age = 25)
{
}
```

Not:

```csharp
static void Test(int age = 25, string name)
{
}
```

---

# 10. Named Parameters

Named parameters allow us to specify the parameter name when calling a method.

```csharp
static void DisplayEmployee(string name, int age, string department)
{
    Console.WriteLine(name);
    Console.WriteLine(age);
    Console.WriteLine(department);
}
```

Call:

```csharp
DisplayEmployee(
    department: "IT",
    age: 25,
    name: "Chitra"
);
```

This is useful when a method has many parameters.

---

# 11. `ref`

`ref` allows a method to modify the original variable.

Example:

```csharp
static void Increment(ref int number)
{
    number++;
}
```

Call:

```csharp
int value = 10;

Increment(ref value);

Console.WriteLine(value);
```

Output:

```text
11
```

### Important

When using `ref`:

1. Variable must be initialized before passing.
2. `ref` must be used in both method declaration and method call.

```csharp
int value = 10;

Increment(ref value);
```

---

# 12. `out`

`out` is used when a method needs to return a value through a parameter.

Example:

```csharp
static void Calculate(int a, int b, out int sum)
{
    sum = a + b;
}
```

Call:

```csharp
int result;

Calculate(10, 20, out result);

Console.WriteLine(result);
```

Output:

```text
30
```

### Difference between `ref` and `out`

| `ref`                          | `out`                                       |
| ------------------------------ | ------------------------------------------- |
| Variable must be initialized   | Variable doesn't need initialization        |
| Used to modify existing value  | Used to produce a value                     |
| Method can read existing value | Method must assign a value before returning |
| `ref` required at call         | `out` required at call                      |

---

# 13. `out` with `TryParse`

You will see `out` frequently in real C# code.

Instead of:

```csharp
int age = Convert.ToInt32(Console.ReadLine());
```

we can safely use:

```csharp
Console.Write("Enter age: ");

string? input = Console.ReadLine();

if (int.TryParse(input, out int age))
{
    Console.WriteLine($"Age: {age}");
}
else
{
    Console.WriteLine("Invalid age");
}
```

This avoids an exception for invalid input.

---

# 14. `in`

`in` passes a value by reference but prevents the method from modifying it.

```csharp
static void Display(in int number)
{
    Console.WriteLine(number);
}
```

Call:

```csharp
int value = 100;

Display(in value);
```

`in` is mainly useful for avoiding unnecessary copying of larger value types while ensuring the method cannot modify the argument.

For normal beginner programs, you will use `in` much less frequently than normal parameters, `ref`, or `out`.

---

# 15. `ref`, `out`, `in` — Easy Memory Trick

Remember:

```text
ref → Read + Modify
out → Produce
in  → Read only
```

---

# 16. Method Overloading

Method overloading means having **multiple methods with the same name but different parameter lists**.

Example:

```csharp
static int Add(int a, int b)
{
    return a + b;
}

static int Add(int a, int b, int c)
{
    return a + b + c;
}
```

Now:

```csharp
Console.WriteLine(Add(10, 20));

Console.WriteLine(Add(10, 20, 30));
```

Output:

```text
30
60
```

You can also overload based on parameter types:

```csharp
static int Add(int a, int b)
{
    return a + b;
}

static double Add(double a, double b)
{
    return a + b;
}
```

### Important

You **cannot overload only by changing the return type**.

This is not valid:

```csharp
static int Test()
{
    return 10;
}

static double Test()
{
    return 10.5;
}
```

---

# 17. Recursion

Recursion means a method **calls itself**.

A recursive method needs a **base condition**, otherwise it can continue indefinitely.

### Factorial Example

```csharp
static int Factorial(int number)
{
    if (number <= 1)
    {
        return 1;
    }

    return number * Factorial(number - 1);
}
```

Call:

```csharp
Console.WriteLine(Factorial(5));
```

Execution:

```text
5 × Factorial(4)
4 × Factorial(3)
3 × Factorial(2)
2 × Factorial(1)
1
```

Result:

```text
120
```

### Important

Every recursive method should have a condition that stops recursion.

---

# STRINGS

# 18. What is a String?

A string represents text.

```csharp
string name = "Chitra";
```

Strings use double quotes:

```csharp
"Hello"
```

A string is a sequence of characters.

```text
C h i t r a
0 1 2 3 4 5
```

---

# 19. String Length

```csharp
string name = "Chitra";

Console.WriteLine(name.Length);
```

Output:

```text
6
```

---

# 20. Accessing Characters

```csharp
string name = "Chitra";

Console.WriteLine(name[0]);
Console.WriteLine(name[1]);
```

Output:

```text
C
h
```

Indexes start from `0`.

---

# 21. Common String Methods

## ToUpper()

```csharp
string name = "chitra";

Console.WriteLine(name.ToUpper());
```

Output:

```text
CHITRA
```

---

## ToLower()

```csharp
Console.WriteLine(name.ToLower());
```

Output:

```text
chitra
```

---

## Trim()

Removes whitespace from beginning and end.

```csharp
string name = "   Chitra   ";

Console.WriteLine(name.Trim());
```

---

## Contains()

```csharp
string text = "Welcome to C#";

Console.WriteLine(text.Contains("C#"));
```

Output:

```text
True
```

---

## StartsWith()

```csharp
Console.WriteLine(text.StartsWith("Welcome"));
```

---

## EndsWith()

```csharp
Console.WriteLine(text.EndsWith("C#"));
```

---

## Replace()

```csharp
string text = "I like Java";

string result = text.Replace("Java", "C#");

Console.WriteLine(result);
```

Output:

```text
I like C#
```

---

## Substring()

```csharp
string text = "Hello World";

Console.WriteLine(text.Substring(0, 5));
```

Output:

```text
Hello
```

---

## Contains vs StartsWith vs EndsWith

```text
Contains()   → anywhere
StartsWith() → beginning
EndsWith()   → ending
```

---

# 22. Split()

Split a string into multiple parts.

```csharp
string names = "Arun,Priya,John";

string[] result = names.Split(',');

foreach (string name in result)
{
    Console.WriteLine(name);
}
```

Output:

```text
Arun
Priya
John
```

---

# 23. Join()

Join multiple values into one string.

```csharp
string[] names = { "Arun", "Priya", "John" };

string result = string.Join(", ", names);

Console.WriteLine(result);
```

Output:

```text
Arun, Priya, John
```

---

# 24. String Interpolation

String interpolation allows variables to be inserted directly into a string.

Use:

```csharp
$
```

Example:

```csharp
string name = "Chitra";
int age = 25;

Console.WriteLine($"Name: {name}, Age: {age}");
```

Output:

```text
Name: Chitra, Age: 25
```

This is commonly used in modern C#.

---

# 25. String Concatenation

Older/common approach:

```csharp
string firstName = "Chitra";
string lastName = "Devi";

string fullName = firstName + " " + lastName;
```

Interpolation:

```csharp
string fullName = $"{firstName} {lastName}";
```

For readable C# code, interpolation is often preferred for simple formatting.

---

# 26. String Comparison

You can compare strings using:

```csharp
==
```

Example:

```csharp
string a = "Hello";
string b = "Hello";

Console.WriteLine(a == b);
```

Output:

```text
True
```

For explicit comparison rules, use `string.Equals`.

```csharp
bool result = string.Equals(
    "hello",
    "HELLO",
    StringComparison.OrdinalIgnoreCase
);

Console.WriteLine(result);
```

Output:

```text
True
```

### Important

Don't use culture-sensitive comparison casually when you're comparing identifiers, usernames, keys, codes, etc.

For many technical comparisons:

```csharp
StringComparison.Ordinal
```

or:

```csharp
StringComparison.OrdinalIgnoreCase
```

is appropriate.

---

# 27. String Immutability

This is an important C# concept.

Strings are **immutable**.

That means once a string object is created, its contents cannot be changed.

Example:

```csharp
string name = "Hello";

name = name + " World";
```

This does not modify the original string object. A new string is created and assigned to `name`.

For a small number of operations, this is completely fine.

For repeated modifications, use `StringBuilder`.

---

# 28. StringBuilder

`StringBuilder` is useful when repeatedly modifying/building text.

Add:

```csharp
using System.Text;
```

Example:

```csharp
StringBuilder builder = new StringBuilder();

builder.Append("Hello");
builder.Append(" ");
builder.Append("World");

Console.WriteLine(builder.ToString());
```

Output:

```text
Hello World
```

---

## AppendLine()

```csharp
StringBuilder builder = new StringBuilder();

builder.AppendLine("Employee Report");
builder.AppendLine("Name: Chitra");
builder.AppendLine("Department: IT");

Console.WriteLine(builder.ToString());
```

---

## Replace()

```csharp
builder.Replace("IT", "HR");
```

---

## Remove()

```csharp
builder.Remove(0, 5);
```

---

## Clear()

```csharp
builder.Clear();
```

### String vs StringBuilder

| String               | StringBuilder                                  |
| -------------------- | ---------------------------------------------- |
| Immutable            | Mutable                                        |
| Good for normal text | Good for repeated modifications                |
| Simple               | More suitable for building large/changing text |

---

# 29. Parsing

Parsing means converting a string into another data type.

Suppose:

```csharp
string input = "100";
```

Convert to integer:

```csharp
int number = int.Parse(input);
```

Now:

```csharp
Console.WriteLine(number + 50);
```

Output:

```text
150
```

---

# 30. Parse vs TryParse

### Parse

```csharp
int age = int.Parse("25");
```

If input is invalid:

```csharp
int.Parse("abc");
```

an exception occurs.

### TryParse

```csharp
if (int.TryParse("25", out int age))
{
    Console.WriteLine(age);
}
```

For user input, `TryParse` is generally safer.

---

# 31. Other Parsing Methods

```csharp
int.Parse("100");

double.Parse("10.5");

decimal.Parse("2500.50");

DateTime.Parse("2026-09-16");
```

Safer versions:

```csharp
int.TryParse(...);

double.TryParse(...);

decimal.TryParse(...);

DateTime.TryParse(...);
```

---

# 32. String Formatting

You can format values using interpolation.

### Currency

```csharp
decimal salary = 50000.75m;

Console.WriteLine($"{salary:C}");
```

The exact currency symbol depends on the current culture.

### Decimal places

```csharp
double value = 123.45678;

Console.WriteLine($"{value:F2}");
```

Output:

```text
123.46
```

### Percentage

```csharp
double percentage = 0.75;

Console.WriteLine($"{percentage:P}");
```

Output will be formatted as a percentage according to the current culture.

---

# ARRAYS

# 33. What is an Array?

An array stores multiple values of the **same type**.

Example:

```csharp
int[] numbers = { 10, 20, 30, 40, 50 };
```

Indexes:

```text
Value:   10   20   30   40   50
Index:    0    1    2    3    4
```

---

# 34. Single-Dimensional Array

Declaration:

```csharp
int[] numbers;
```

Creation:

```csharp
numbers = new int[5];
```

Initialization:

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

Change:

```csharp
numbers[0] = 100;
```

---

# 35. Array Length

```csharp
int[] numbers = { 10, 20, 30, 40 };

Console.WriteLine(numbers.Length);
```

Output:

```text
4
```

---

# 36. Loop Through Array

Using `for`:

```csharp
int[] numbers = { 10, 20, 30, 40 };

for (int i = 0; i < numbers.Length; i++)
{
    Console.WriteLine(numbers[i]);
}
```

Using `foreach`:

```csharp
foreach (int number in numbers)
{
    Console.WriteLine(number);
}
```

---

# 37. Multi-Dimensional Arrays

A multi-dimensional array has multiple dimensions.

A common example is a 2D array.

```csharp
int[,] matrix =
{
    { 1, 2, 3 },
    { 4, 5, 6 }
};
```

Conceptually:

```text
1  2  3
4  5  6
```

Access:

```csharp
Console.WriteLine(matrix[0, 0]);
```

Output:

```text
1
```

---

# 38. Loop Through 2D Array

```csharp
int[,] matrix =
{
    { 1, 2, 3 },
    { 4, 5, 6 }
};

for (int row = 0; row < matrix.GetLength(0); row++)
{
    for (int column = 0; column < matrix.GetLength(1); column++)
    {
        Console.Write(matrix[row, column] + " ");
    }

    Console.WriteLine();
}
```

Output:

```text
1 2 3
4 5 6
```

### Important

For multidimensional arrays:

```csharp
matrix.GetLength(0)
```

gets the number of rows.

```csharp
matrix.GetLength(1)
```

gets the number of columns.

---

# 39. Jagged Arrays

A jagged array is an **array of arrays**.

Each inner array can have a different length.

```csharp
int[][] numbers =
{
    new int[] { 1, 2 },
    new int[] { 3, 4, 5 },
    new int[] { 6, 7, 8, 9 }
};
```

Conceptually:

```text
1 2
3 4 5
6 7 8 9
```

Notice each row has a different number of elements.

---

# 40. Loop Through Jagged Array

```csharp
int[][] numbers =
{
    new int[] { 1, 2 },
    new int[] { 3, 4, 5 },
    new int[] { 6, 7, 8, 9 }
};

for (int i = 0; i < numbers.Length; i++)
{
    for (int j = 0; j < numbers[i].Length; j++)
    {
        Console.Write(numbers[i][j] + " ");
    }

    Console.WriteLine();
}
```

---

# 41. 2D Array vs Jagged Array

| 2D Array              | Jagged Array                    |
| --------------------- | ------------------------------- |
| `int[,]`              | `int[][]`                       |
| Rectangular           | Rows can have different lengths |
| `matrix[row, column]` | `array[row][column]`            |
| Fixed dimensions      | Each inner array can differ     |

---

# PRACTICAL PROJECT 1 — EMPLOYEE SALARY CALCULATOR

### Requirements

Take:

* Employee name
* Basic salary
* HRA
* DA

Calculate:

```text
Gross Salary = Basic + HRA + DA
```

### Code

```csharp
static decimal CalculateSalary(
    decimal basicSalary,
    decimal hra,
    decimal da)
{
    return basicSalary + hra + da;
}

Console.Write("Enter employee name: ");
string? name = Console.ReadLine();

Console.Write("Enter basic salary: ");
decimal basic = decimal.Parse(Console.ReadLine()!);

Console.Write("Enter HRA: ");
decimal hra = decimal.Parse(Console.ReadLine()!);

Console.Write("Enter DA: ");
decimal da = decimal.Parse(Console.ReadLine()!);

decimal grossSalary = CalculateSalary(basic, hra, da);

Console.WriteLine();
Console.WriteLine("===== EMPLOYEE SALARY =====");
Console.WriteLine($"Name         : {name}");
Console.WriteLine($"Basic Salary : {basic:C}");
Console.WriteLine($"HRA          : {hra:C}");
Console.WriteLine($"DA           : {da:C}");
Console.WriteLine($"Gross Salary : {grossSalary:C}");
```

### Concepts practiced

```text
Methods
Parameters
Return value
decimal
String interpolation
Parsing
Formatting
```

---

# PRACTICAL PROJECT 2 — STUDENT MARKS CALCULATOR

### Requirements

Take marks for 5 subjects.

Calculate:

```text
Total
Average
Grade
```

### Code

```csharp
static int CalculateTotal(int[] marks)
{
    int total = 0;

    foreach (int mark in marks)
    {
        total += mark;
    }

    return total;
}

static double CalculateAverage(int total, int subjectCount)
{
    return (double)total / subjectCount;
}

static string GetGrade(double average)
{
    if (average >= 90)
        return "A";

    if (average >= 75)
        return "B";

    if (average >= 60)
        return "C";

    if (average >= 50)
        return "D";

    return "F";
}

Console.Write("Enter student name: ");
string? name = Console.ReadLine();

int[] marks = new int[5];

for (int i = 0; i < marks.Length; i++)
{
    Console.Write($"Enter mark {i + 1}: ");
    marks[i] = int.Parse(Console.ReadLine()!);
}

int total = CalculateTotal(marks);
double average = CalculateAverage(total, marks.Length);
string grade = GetGrade(average);

Console.WriteLine();
Console.WriteLine("===== STUDENT RESULT =====");
Console.WriteLine($"Name    : {name}");
Console.WriteLine($"Total   : {total}");
Console.WriteLine($"Average : {average:F2}");
Console.WriteLine($"Grade   : {grade}");
```

---

# PRACTICAL PROJECT 3 — STRING ANALYZER

### Requirements

Take a sentence and calculate:

* Length
* Number of characters excluding spaces
* Number of words
* Uppercase version
* Lowercase version
* Whether it contains `"C#"`

### Code

```csharp
static int CountCharactersWithoutSpaces(string text)
{
    int count = 0;

    foreach (char character in text)
    {
        if (character != ' ')
        {
            count++;
        }
    }

    return count;
}

static int CountWords(string text)
{
    if (string.IsNullOrWhiteSpace(text))
    {
        return 0;
    }

    string[] words = text.Split(
        ' ',
        StringSplitOptions.RemoveEmptyEntries
    );

    return words.Length;
}

Console.Write("Enter a sentence: ");
string text = Console.ReadLine() ?? "";

Console.WriteLine();
Console.WriteLine("===== STRING ANALYZER =====");

Console.WriteLine($"Length              : {text.Length}");
Console.WriteLine(
    $"Characters no space : {CountCharactersWithoutSpaces(text)}"
);
Console.WriteLine($"Word count          : {CountWords(text)}");
Console.WriteLine($"Uppercase           : {text.ToUpper()}");
Console.WriteLine($"Lowercase           : {text.ToLower()}");
Console.WriteLine($"Contains C#         : {text.Contains("C#")}");
```

---

# PRACTICAL PROJECT 4 — ARRAY SORTING

You can use the built-in `Array.Sort()`.

```csharp
int[] numbers = { 50, 10, 40, 20, 30 };

Console.WriteLine("Before sorting:");

foreach (int number in numbers)
{
    Console.Write(number + " ");
}

Array.Sort(numbers);

Console.WriteLine("\nAfter sorting:");

foreach (int number in numbers)
{
    Console.Write(number + " ");
}
```

Output:

```text
Before sorting:
50 10 40 20 30

After sorting:
10 20 30 40 50
```

---

## Descending Order

```csharp
Array.Sort(numbers);
Array.Reverse(numbers);
```

Now:

```text
50 40 30 20 10
```

---

# PRACTICAL PROJECT 5 — ARRAY SEARCHING

## Using `Array.IndexOf`

```csharp
int[] numbers = { 10, 20, 30, 40, 50 };

Console.Write("Enter number to search: ");
int search = int.Parse(Console.ReadLine()!);

int index = Array.IndexOf(numbers, search);

if (index >= 0)
{
    Console.WriteLine($"Found at index {index}");
}
else
{
    Console.WriteLine("Number not found");
}
```

---

# Manual Array Searching

This is important for learning logic.

```csharp
int[] numbers = { 10, 20, 30, 40, 50 };

Console.Write("Enter number to search: ");
int search = int.Parse(Console.ReadLine()!);

bool found = false;

for (int i = 0; i < numbers.Length; i++)
{
    if (numbers[i] == search)
    {
        Console.WriteLine($"Found at index {i}");
        found = true;
        break;
    }
}

if (!found)
{
    Console.WriteLine("Number not found");
}
```

---

# BONUS — METHOD-BASED CALCULATOR

This is a good Day 3 practice because it combines methods, parameters, return values, switch, and parsing.

```csharp
static double Add(double a, double b)
{
    return a + b;
}

static double Subtract(double a, double b)
{
    return a - b;
}

static double Multiply(double a, double b)
{
    return a * b;
}

static double Divide(double a, double b)
{
    return a / b;
}

Console.Write("Enter first number: ");
double number1 = double.Parse(Console.ReadLine()!);

Console.Write("Enter operator (+ - * /): ");
string? operation = Console.ReadLine();

Console.Write("Enter second number: ");
double number2 = double.Parse(Console.ReadLine()!);

double result;

switch (operation)
{
    case "+":
        result = Add(number1, number2);
        break;

    case "-":
        result = Subtract(number1, number2);
        break;

    case "*":
        result = Multiply(number1, number2);
        break;

    case "/":
        if (number2 == 0)
        {
            Console.WriteLine("Cannot divide by zero.");
            return;
        }

        result = Divide(number1, number2);
        break;

    default:
        Console.WriteLine("Invalid operator.");
        return;
}

Console.WriteLine($"Result: {result}");
```

---

### 1. What is a method?

A method is a reusable block of code that performs a specific task.

### 2. What is the difference between parameter and argument?

Parameter is defined in the method. Argument is the actual value passed during method invocation.

### 3. What is method overloading?

Creating multiple methods with the same name but different parameter lists.

### 4. Can methods be overloaded by return type alone?

**No.**

### 5. Difference between `ref` and `out`?

`ref` requires the variable to be initialized before passing it. `out` doesn't.

### 6. What is `in`?

It passes an argument by reference for read-only access inside the method.

### 7. What is recursion?

A method calling itself.

### 8. Why does recursion need a base condition?

To stop recursive calls and prevent infinite recursion.

### 9. Are strings mutable in C#?

No. Strings are immutable.

### 10. Why use `StringBuilder`?

For efficient repeated string modifications.

### 11. Difference between `Parse` and `TryParse`?

`Parse` can throw an exception for invalid input. `TryParse` returns `false` instead.

### 12. What is string interpolation?

Embedding expressions/variables inside strings using `$`.

```csharp
$"Hello {name}"
```

### 13. What is an array?

A collection of elements of the same type with a fixed length.

### 14. What is a multidimensional array?

An array with multiple dimensions, such as:

```csharp
int[,] matrix;
```

### 15. What is a jagged array?

An array whose elements are themselves arrays:

```csharp
int[][] numbers;
```

### 16. Difference between 2D and jagged arrays?

```text
2D:
int[,]

Jagged:
int[][]
```

A 2D array is rectangular, while a jagged array can have different lengths for each inner array.

---

# DAY 3 — QUICK REVISION

```text
METHODS
│
├── Method
├── Parameters
├── Return values
├── Optional parameters
├── Named parameters
├── ref
├── out
├── in
├── Overloading
└── Recursion

STRINGS
│
├── Length
├── Index
├── ToUpper()
├── ToLower()
├── Trim()
├── Contains()
├── StartsWith()
├── EndsWith()
├── Replace()
├── Substring()
├── Split()
├── Join()
├── Interpolation
├── Comparison
├── StringBuilder
├── Parsing
└── Formatting

ARRAYS
│
├── Single-dimensional
├── Multi-dimensional
├── Jagged
├── Length
├── for
├── foreach
├── Array.Sort()
├── Array.Reverse()
└── Array.IndexOf()
```

## Most important syntax to remember

```csharp
// Normal method
static int Add(int a, int b)
{
    return a + b;
}

// Optional parameter
static void Greet(string name = "Guest")
{
}

// Named parameter
Greet(name: "Chitra");

// ref
static void Test(ref int value)
{
    value++;
}

// out
static void Test(out int value)
{
    value = 100;
}

// in
static void Test(in int value)
{
    Console.WriteLine(value);
}

// Overloading
static int Add(int a, int b) => a + b;
static int Add(int a, int b, int c) => a + b + c;

// Recursion
static int Factorial(int n)
{
    if (n <= 1)
        return 1;

    return n * Factorial(n - 1);
}

// String interpolation
string name = "Chitra";
Console.WriteLine($"Hello {name}");

// StringBuilder
StringBuilder sb = new();
sb.Append("Hello");

// 1D array
int[] numbers = { 10, 20, 30 };

// 2D array
int[,] matrix =
{
    { 1, 2 },
    { 3, 4 }
};

// Jagged array
int[][] data =
{
    new[] { 1, 2 },
    new[] { 3, 4, 5 }
};
```

# Day 3 Practice Order

For your class/practice, do these in this order:

**1. Methods**
→ Add, subtract, multiply, divide methods

**2. Parameters & return values**
→ Employee salary calculator

**3. Optional & named parameters**
→ Employee information method

**4. `ref` / `out` / `in`**
→ Small separate examples

**5. Method overloading**
→ Calculator with overloaded `Add()`

**6. Recursion**
→ Factorial + Fibonacci

**7. Strings**
→ String analyzer

**8. Arrays**
→ Student marks calculator

**9. Array sorting**
→ Ascending + descending

**10. Array searching**
→ Manual search + `Array.IndexOf()`

### Final Day 3 goal

You should be comfortable writing code like:

```csharp
static double CalculateAverage(int[] marks)
{
    int total = 0;

    foreach (int mark in marks)
    {
        total += mark;
    }

    return (double)total / marks.Length;
}
```


