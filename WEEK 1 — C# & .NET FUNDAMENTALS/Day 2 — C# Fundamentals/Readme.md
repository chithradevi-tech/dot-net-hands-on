# 🗓️ DAY 2 — C# FUNDAMENTALS

---

# 1. What are Variables?

A **variable** is a named memory location used to store a value.

Example:

```csharp
int age = 25;
```

Here:

```text
int  → Data type
age  → Variable name
25   → Value
=    → Assignment operator
```

You can change a variable's value:

```csharp
int age = 25;

age = 30;

Console.WriteLine(age);
```

Output:

```text
30
```

### General syntax

```csharp
dataType variableName = value;
```

Example:

```csharp
string name = "Chitra";
int age = 25;
double salary = 50000.50;
```

---

# 2. C# Data Types

For today's class, you need to understand:

```text
int
long
float
double
decimal
bool
char
string
```

These are commonly used built-in C# types.

---

# 3. `int`

`int` stores whole numbers.

Example:

```csharp
int age = 25;
int marks = 95;
int number = -10;
```

An `int` is a **32-bit signed integer**.

Range:

```text
-2,147,483,648
to
 2,147,483,647
```

Example:

```csharp
int students = 50;

Console.WriteLine(students);
```

Output:

```text
50
```

### Use `int` when:

You need normal whole numbers such as:

```text
Age
Marks
Quantity
Count
ID values within int range
```

---

# 4. `long`

`long` is used for larger whole numbers.

It is a **64-bit signed integer**.

Example:

```csharp
long population = 1400000000;
long distance = 9876543210;
```

Range:

```text
-9,223,372,036,854,775,808
to
 9,223,372,036,854,775,807
```

### Why use `long`?

When the value can exceed the `int` range.

Example:

```csharp
long views = 5000000000;
```

---

# 5. `float`

`float` stores **single-precision floating-point numbers**.

Example:

```csharp
float temperature = 36.5f;
```

Notice the:

```text
f
```

after the value.

```csharp
float price = 99.99f;
```

Without `f`, a decimal literal such as `99.99` is normally treated as a `double`.

### Example

```csharp
float height = 5.8f;

Console.WriteLine(height);
```

---

# 6. `double`

`double` stores **double-precision floating-point numbers**.

Example:

```csharp
double salary = 50000.75;
double pi = 3.1415926535;
```

Unlike `float`, you normally don't need an `f` suffix.

```csharp
double number = 10.5;
```

### `float` vs `double`

```text
float  → less precision, smaller storage
double → more precision, larger storage
```

For general-purpose scientific/calculation work, `double` is often preferred over `float` unless you specifically need `float`.

---

# 7. `decimal`

`decimal` is designed for **high-precision decimal arithmetic**, especially useful for financial values.

Example:

```csharp
decimal salary = 50000.75m;
decimal price = 199.99m;
```

Notice:

```text
m
```

after the value.

### Example

```csharp
decimal productPrice = 1499.99m;
decimal tax = 269.99m;

decimal total = productPrice + tax;

Console.WriteLine(total);
```

### Why `decimal`?

For values such as:

```text
Money
Price
Salary
Tax
Financial calculations
```

`decimal` can be preferable because it is designed for decimal-based precision.

---

# 8. `bool`

`bool` stores only two values:

```text
true
false
```

Example:

```csharp
bool isLoggedIn = true;
bool isAdmin = false;
```

Example:

```csharp
bool isAdult = true;

Console.WriteLine(isAdult);
```

Output:

```text
True
```

### Common uses

```text
Is user logged in?
Is account active?
Is payment completed?
Is employee present?
```

---

# 9. `char`

`char` stores **one character**.

Use **single quotes**:

```csharp
char grade = 'A';
char gender = 'F';
char symbol = '#';
```

Correct:

```csharp
char letter = 'A';
```

Incorrect:

```csharp
char letter = "A";
```

Because `"A"` is a string, not a `char`.

---

# 10. `string`

`string` stores a sequence of characters.

Use **double quotes**:

```csharp
string name = "Chitra";
string city = "Bengaluru";
string message = "Welcome to C#";
```

Example:

```csharp
string name = "Chitra";

Console.WriteLine(name);
```

Output:

```text
Chitra
```

---

# 11. `char` vs `string`

This is very important.

```csharp
char letter = 'A';

string name = "Chitra";
```

| `char`        | `string`            |
| ------------- | ------------------- |
| One character | Multiple characters |
| Single quotes | Double quotes       |
| `'A'`         | `"Apple"`           |
| `'7'`         | `"12345"`           |

Remember:

```text
'A'       → char
"Apple"   → string
```

---

# 12. Data Types Quick Reference

| Type      | Purpose                     | Example      |
| --------- | --------------------------- | ------------ |
| `int`     | Whole numbers               | `25`         |
| `long`    | Large whole numbers         | `5000000000` |
| `float`   | Floating-point number       | `10.5f`      |
| `double`  | Double-precision number     | `10.5`       |
| `decimal` | Decimal/financial precision | `10.5m`      |
| `bool`    | True/false                  | `true`       |
| `char`    | Single character            | `'A'`        |
| `string`  | Text                        | `"Hello"`    |

---

# 13. Constants

A **constant** is a value that cannot be changed after it is declared.

Syntax:

```csharp
const int age = 25;
```

Example:

```csharp
const double PI = 3.14159;
```

You cannot do:

```csharp
PI = 4;
```

because `PI` is constant.

---

## Variable vs Constant

Variable:

```csharp
int age = 25;

age = 30;
```

Allowed.

Constant:

```csharp
const int age = 25;

age = 30;
```

Not allowed.

### Example

```csharp
const decimal TAX_RATE = 0.18m;

decimal price = 1000m;
decimal tax = price * TAX_RATE;

Console.WriteLine(tax);
```

---

# 14. Operators

Operators are symbols used to perform operations on values.

You need to learn:

```text
Arithmetic
Relational
Logical
Assignment
Increment / Decrement
Null-coalescing
Null-conditional
```

---

# 15. Arithmetic Operators

Arithmetic operators perform mathematical operations.

| Operator | Meaning        |
| -------- | -------------- |
| `+`      | Addition       |
| `-`      | Subtraction    |
| `*`      | Multiplication |
| `/`      | Division       |
| `%`      | Remainder      |

Example:

```csharp
int a = 10;
int b = 3;

Console.WriteLine(a + b);
Console.WriteLine(a - b);
Console.WriteLine(a * b);
Console.WriteLine(a / b);
Console.WriteLine(a % b);
```

Output:

```text
13
7
30
3
1
```

### Important: Integer Division

This:

```csharp
int a = 10;
int b = 3;

Console.WriteLine(a / b);
```

outputs:

```text
3
```

not:

```text
3.3333
```

because both operands are integers.

If you want decimal division:

```csharp
double a = 10;
double b = 3;

Console.WriteLine(a / b);
```

---

# 16. Relational Operators

Relational operators compare values.

| Operator | Meaning               |
| -------- | --------------------- |
| `==`     | Equal                 |
| `!=`     | Not equal             |
| `>`      | Greater than          |
| `<`      | Less than             |
| `>=`     | Greater than or equal |
| `<=`     | Less than or equal    |

Example:

```csharp
int a = 10;
int b = 20;

Console.WriteLine(a == b);
Console.WriteLine(a != b);
Console.WriteLine(a > b);
Console.WriteLine(a < b);
```

Output:

```text
False
True
False
True
```

The result of a comparison is a `bool`.

---

# 17. `=` vs `==`

This is one of the most common beginner mistakes.

### `=`

Assignment:

```csharp
int age = 25;
```

Means:

> Store 25 in age.

### `==`

Comparison:

```csharp
age == 25
```

Means:

> Is age equal to 25?

Remember:

```text
=   → Assign
==  → Compare
```

---

# 18. Logical Operators

Logical operators work with Boolean expressions.

| Operator | Meaning |   |    |
| -------- | ------- | - | -- |
| `&&`     | AND     |   |    |
| `        |         | ` | OR |
| `!`      | NOT     |   |    |

---

## AND — `&&`

Both conditions must be true.

```csharp
int age = 25;

bool result = age >= 18 && age <= 60;

Console.WriteLine(result);
```

Both conditions are true, so:

```text
True
```

Example:

```text
Condition 1 → True
Condition 2 → True

True && True → True
```

If either condition is false:

```text
True && False → False
```

---

# 19. OR — `||`

At least one condition must be true.

```csharp
bool result = true || false;

Console.WriteLine(result);
```

Output:

```text
True
```

Truth table:

```text
True  || True  → True
True  || False → True
False || True  → True
False || False → False
```

---

# 20. NOT — `!`

NOT reverses a Boolean value.

```csharp
bool isLoggedIn = true;

Console.WriteLine(!isLoggedIn);
```

Output:

```text
False
```

Because:

```text
!true → false
!false → true
```

---

# 21. Assignment Operators

Basic assignment:

```csharp
int x = 10;
```

Compound assignment operators:

```text
+=
-=
*=
/=
%=
```

Example:

```csharp
int x = 10;

x += 5;
Console.WriteLine(x);
```

Output:

```text
15
```

This:

```csharp
x += 5;
```

is equivalent to:

```csharp
x = x + 5;
```

Another example:

```csharp
int x = 10;

x -= 3;  // 7
x *= 2;  // 14
x /= 7;  // 2
x %= 2;  // 0
```

---

# 22. Increment Operator

`++` increases a value by 1.

```csharp
int count = 10;

count++;

Console.WriteLine(count);
```

Output:

```text
11
```

Equivalent to:

```csharp
count = count + 1;
```

---

# 23. Decrement Operator

`--` decreases a value by 1.

```csharp
int count = 10;

count--;

Console.WriteLine(count);
```

Output:

```text
9
```

Equivalent to:

```csharp
count = count - 1;
```

---

# 24. Prefix vs Postfix

This is slightly more advanced but important.

## Post-increment

```csharp
int x = 5;

int y = x++;

Console.WriteLine(x);
Console.WriteLine(y);
```

Output:

```text
6
5
```

The original value is used first, then incremented.

---

## Pre-increment

```csharp
int x = 5;

int y = ++x;

Console.WriteLine(x);
Console.WriteLine(y);
```

Output:

```text
6
6
```

The value is incremented first, then used.

### Remember

```text
x++ → use first, increment later
++x → increment first, use later
```

---

# 25. Null Value

Before understanding null-coalescing and null-conditional operators, understand `null`.

`null` means:

> No object/value is currently assigned to a reference.

Example:

```csharp
string? name = null;
```

The `?` indicates that the reference may be null under nullable reference type analysis.

---

# 26. Null-Coalescing Operator `??`

The `??` operator provides a fallback value when the left side is `null`.

Example:

```csharp
string? name = null;

string result = name ?? "Guest";

Console.WriteLine(result);
```

Output:

```text
Guest
```

If:

```csharp
string? name = "Chitra";
```

then:

```csharp
string result = name ?? "Guest";
```

gives:

```text
Chitra
```

### Simple meaning

```text
value ?? defaultValue
```

means:

> If value is not null, use it; otherwise use defaultValue.

---

# 27. Null-Conditional Operator `?.`

The `?.` operator allows you to safely access a member when the object might be null.

Example:

```csharp
string? name = null;

Console.WriteLine(name?.Length);
```

Because `name` is null, the expression safely evaluates to null rather than throwing a `NullReferenceException`.

With a value:

```csharp
string? name = "Chitra";

Console.WriteLine(name?.Length);
```

Output:

```text
6
```

### Common real-world example

```csharp
Person? person = GetPerson();

string? name = person?.Name;
```

If `person` is null:

```text
name → null
```

If `person` exists:

```text
name → person.Name
```

---

# 28. `?.` vs `??`

These are often used together.

```csharp
string? name = null;

string result = name?.ToUpper() ?? "NO NAME";

Console.WriteLine(result);
```

Flow:

```text
name
 ↓
?.ToUpper()
 ↓
null
 ↓
??
 ↓
"NO NAME"
```

Output:

```text
NO NAME
```

---

# 29. Control Flow

Control flow determines **which code executes and how many times it executes**.

You need to learn:

```text
if
else
else if
switch
for
foreach
while
do while
```

---

# 30. `if`

`if` executes code when a condition is true.

Syntax:

```csharp
if (condition)
{
    // code
}
```

Example:

```csharp
int age = 20;

if (age >= 18)
{
    Console.WriteLine("Adult");
}
```

Output:

```text
Adult
```

---

# 31. `if-else`

If condition is true → `if` block.

Otherwise → `else` block.

```csharp
int age = 16;

if (age >= 18)
{
    Console.WriteLine("Adult");
}
else
{
    Console.WriteLine("Minor");
}
```

Output:

```text
Minor
```

---

# 32. `else if`

Used when there are multiple conditions.

Example:

```csharp
int marks = 75;

if (marks >= 90)
{
    Console.WriteLine("A+");
}
else if (marks >= 75)
{
    Console.WriteLine("A");
}
else if (marks >= 50)
{
    Console.WriteLine("B");
}
else
{
    Console.WriteLine("Fail");
}
```

Output:

```text
A
```

The conditions are checked from top to bottom.

---

# 33. `switch`

`switch` is useful when you want to compare one value against multiple cases.

Example:

```csharp
int day = 2;

switch (day)
{
    case 1:
        Console.WriteLine("Monday");
        break;

    case 2:
        Console.WriteLine("Tuesday");
        break;

    case 3:
        Console.WriteLine("Wednesday");
        break;

    default:
        Console.WriteLine("Invalid day");
        break;
}
```

Output:

```text
Tuesday
```

---

# 34. Switch Expression

Modern C# also supports switch expressions.

Example:

```csharp
int day = 2;

string dayName = day switch
{
    1 => "Monday",
    2 => "Tuesday",
    3 => "Wednesday",
    _ => "Invalid day"
};

Console.WriteLine(dayName);
```

This is shorter than the traditional `switch` statement.

For Day 2, understand both, but don't worry about advanced pattern matching yet.

---

# 35. `for` Loop

A `for` loop is useful when you know the number of iterations or have an index.

Syntax:

```csharp
for (initialization; condition; increment)
{
    // code
}
```

Example:

```csharp
for (int i = 1; i <= 5; i++)
{
    Console.WriteLine(i);
}
```

Output:

```text
1
2
3
4
5
```

### Flow

```text
i = 1
 ↓
condition
 ↓
execute
 ↓
i++
 ↓
condition
 ↓
execute
...
```

---

# 36. `foreach`

`foreach` is used to iterate through elements in a collection.

Example:

```csharp
string[] names = { "John", "Priya", "Arun" };

foreach (string name in names)
{
    Console.WriteLine(name);
}
```

Output:

```text
John
Priya
Arun
```

### `for` vs `foreach`

Use `for` when you need:

* Index
* Counter
* More control over iteration

Use `foreach` when you simply want:

> Each item in a collection.

---

# 37. `while`

A `while` loop executes while a condition is true.

Syntax:

```csharp
while (condition)
{
    // code
}
```

Example:

```csharp
int i = 1;

while (i <= 5)
{
    Console.WriteLine(i);
    i++;
}
```

Output:

```text
1
2
3
4
5
```

### Important

If the condition is false initially, the loop may execute **zero times**.

---

# 38. `do while`

A `do while` loop executes the body **at least once**.

Example:

```csharp
int i = 1;

do
{
    Console.WriteLine(i);
    i++;
}
while (i <= 5);
```

Output:

```text
1
2
3
4
5
```

Difference:

```text
while
→ condition first
→ execute

do while
→ execute first
→ condition
```

---

# 39. `while` vs `do while`

### while

```csharp
int i = 10;

while (i < 5)
{
    Console.WriteLine(i);
}
```

Output:

```text
Nothing
```

### do while

```csharp
int i = 10;

do
{
    Console.WriteLine(i);
}
while (i < 5);
```

Output:

```text
10
```

Because `do while` executes once before checking the condition.

---

# 40. Complete Control Flow Summary

| Statement  | Purpose                                   |
| ---------- | ----------------------------------------- |
| `if`       | One condition                             |
| `else`     | Alternative                               |
| `else if`  | Multiple conditions                       |
| `switch`   | Multiple choices based on a value/pattern |
| `for`      | Repeated execution with an index/counter  |
| `foreach`  | Iterate through collection items          |
| `while`    | Repeat while condition is true            |
| `do while` | Execute once, then repeat while true      |

---

# 🧮 PRACTICE 1 — CALCULATOR

Create a calculator that accepts:

```text
First number
Operator
Second number
```

Example:

```text
Enter first number: 10
Enter operator: +
Enter second number: 20

Result: 30
```

### Code

```csharp
Console.Write("Enter first number: ");
double num1 = Convert.ToDouble(Console.ReadLine());

Console.Write("Enter operator (+, -, *, /): ");
string? operation = Console.ReadLine();

Console.Write("Enter second number: ");
double num2 = Convert.ToDouble(Console.ReadLine());

double result;

switch (operation)
{
    case "+":
        result = num1 + num2;
        break;

    case "-":
        result = num1 - num2;
        break;

    case "*":
        result = num1 * num2;
        break;

    case "/":
        if (num2 == 0)
        {
            Console.WriteLine("Cannot divide by zero.");
            return;
        }

        result = num1 / num2;
        break;

    default:
        Console.WriteLine("Invalid operator.");
        return;
}

Console.WriteLine($"Result: {result}");
```

---

# 🔢 PRACTICE 2 — NUMBER CHECKER

Check whether a number is:

```text
Positive
Negative
Zero
```

### Code

```csharp
Console.Write("Enter a number: ");
int number = Convert.ToInt32(Console.ReadLine());

if (number > 0)
{
    Console.WriteLine("Positive");
}
else if (number < 0)
{
    Console.WriteLine("Negative");
}
else
{
    Console.WriteLine("Zero");
}
```

---

# 🔢 PRACTICE 3 — EVEN OR ODD

A number is even if its remainder when divided by 2 is zero.

```text
number % 2 == 0
```

### Code

```csharp
Console.Write("Enter a number: ");
int number = Convert.ToInt32(Console.ReadLine());

if (number % 2 == 0)
{
    Console.WriteLine("Even");
}
else
{
    Console.WriteLine("Odd");
}
```

Example:

```text
10 % 2 = 0 → Even

7 % 2 = 1 → Odd
```

---

# 🔢 PRACTICE 4 — PRIME NUMBER

A prime number is a number greater than 1 that has exactly two positive divisors:

```text
1
itself
```

Examples:

```text
2
3
5
7
11
13
17
```

### Code

```csharp
Console.Write("Enter a number: ");
int number = Convert.ToInt32(Console.ReadLine());

bool isPrime = number > 1;

for (int i = 2; i * i <= number && isPrime; i++)
{
    if (number % i == 0)
    {
        isPrime = false;
    }
}

if (isPrime)
{
    Console.WriteLine("Prime number");
}
else
{
    Console.WriteLine("Not a prime number");
}
```

### Why `i * i <= number`?

If a number has a factor larger than its square root, it must also have a corresponding factor smaller than its square root.

So checking up to the square root is sufficient.

---

# 🔢 PRACTICE 5 — FACTORIAL

Factorial of `n`:

```text
n! = n × (n-1) × (n-2) × ... × 1
```

Example:

```text
5!

= 5 × 4 × 3 × 2 × 1

= 120
```

### Code

```csharp
Console.Write("Enter a number: ");
int number = Convert.ToInt32(Console.ReadLine());

long factorial = 1;

for (int i = 1; i <= number; i++)
{
    factorial *= i;
}

Console.WriteLine($"Factorial: {factorial}");
```

For:

```text
5
```

Output:

```text
Factorial: 120
```

### Important

Factorials grow extremely quickly. A `long` will overflow for sufficiently large inputs, so don't assume this program can handle arbitrary values.

---

# 🔢 PRACTICE 6 — FIBONACCI

Fibonacci sequence:

```text
0
1
1
2
3
5
8
13
21
34
...
```

Each number is generally calculated as:

```text
next = previous + current
```

### Code

```csharp
Console.Write("Enter number of terms: ");
int n = Convert.ToInt32(Console.ReadLine());

long first = 0;
long second = 1;

for (int i = 0; i < n; i++)
{
    Console.Write(first + " ");

    long next = first + second;
    first = second;
    second = next;
}
```

For:

```text
10
```

Output:

```text
0 1 1 2 3 5 8 13 21 34
```

Again, `long` has a finite range, so sufficiently large Fibonacci sequences will overflow.

---

# 🎯 COMBINED DAY 2 PRACTICE PROGRAM

After learning each individual program, create a menu-driven application.

```csharp
Console.WriteLine("===== C# PRACTICE =====");
Console.WriteLine("1. Number Checker");
Console.WriteLine("2. Even / Odd");
Console.WriteLine("3. Prime Number");
Console.WriteLine("4. Factorial");
Console.WriteLine("5. Fibonacci");

Console.Write("Choose an option: ");
int choice = Convert.ToInt32(Console.ReadLine());

Console.Write("Enter a number: ");
int number = Convert.ToInt32(Console.ReadLine());

switch (choice)
{
    case 1:

        if (number > 0)
            Console.WriteLine("Positive");
        else if (number < 0)
            Console.WriteLine("Negative");
        else
            Console.WriteLine("Zero");

        break;

    case 2:

        if (number % 2 == 0)
            Console.WriteLine("Even");
        else
            Console.WriteLine("Odd");

        break;

    case 3:

        bool isPrime = number > 1;

        for (int i = 2; i * i <= number && isPrime; i++)
        {
            if (number % i == 0)
                isPrime = false;
        }

        Console.WriteLine(isPrime
            ? "Prime Number"
            : "Not a Prime Number");

        break;

    case 4:

        if (number < 0)
        {
            Console.WriteLine("Factorial is not defined for negative integers.");
            break;
        }

        long factorial = 1;

        for (int i = 1; i <= number; i++)
        {
            factorial *= i;
        }

        Console.WriteLine($"Factorial: {factorial}");

        break;

    case 5:

        long first = 0;
        long second = 1;

        for (int i = 0; i < number; i++)
        {
            Console.Write(first + " ");

            long next = first + second;
            first = second;
            second = next;
        }

        break;

    default:

        Console.WriteLine("Invalid choice.");

        break;
}
```

---

# 🧠 IMPORTANT BEGINNER MISTAKES

## Mistake 1 — Using double quotes for char

❌ Wrong:

```csharp
char grade = "A";
```

✅ Correct:

```csharp
char grade = 'A';
```

---

## Mistake 2 — Forgetting `f`

❌ Usually invalid for assigning a decimal literal directly to `float`:

```csharp
float price = 10.5;
```

✅ Correct:

```csharp
float price = 10.5f;
```

---

## Mistake 3 — Forgetting `m`

For a decimal literal:

```csharp
decimal price = 10.5m;
```

---

## Mistake 4 — Confusing `=` and `==`

```csharp
x = 10;     // Assignment
x == 10;    // Comparison
```

---

## Mistake 5 — Infinite loop

❌

```csharp
int i = 1;

while (i <= 5)
{
    Console.WriteLine(i);
}
```

`i` never changes.

✅

```csharp
int i = 1;

while (i <= 5)
{
    Console.WriteLine(i);
    i++;
}
```

---

### 1. What is a variable?

A variable is a named storage location used to hold a value that can generally be changed during program execution.

### 2. Difference between `int` and `long`?

Both store signed whole numbers, but `long` provides a much larger range than `int`.

### 3. Difference between `float` and `double`?

Both represent floating-point numbers. `double` provides greater precision and is generally the default choice for many general floating-point calculations.

### 4. Why do we use `decimal`?

`decimal` is designed for high-precision decimal arithmetic and is commonly useful for financial calculations.

### 5. Difference between `char` and `string`?

`char` represents one character, while `string` represents a sequence of characters.

### 6. What is a constant?

A constant is a value declared with `const` that must be assigned a compile-time constant value and cannot be reassigned.

### 7. Difference between `=` and `==`?

`=` assigns a value. `==` compares two values.

### 8. What does `%` do?

It returns the remainder of a division.

Example:

```csharp
10 % 3
```

Result:

```text
1
```

### 9. Difference between `&&` and `||`?

`&&` requires both operands to be true. `||` requires at least one operand to be true.

### 10. What does `!` do?

It logically negates a Boolean expression.

```text
!true → false
!false → true
```

### 11. What is `??`?

It is the null-coalescing operator. It returns the left operand if it isn't null; otherwise it returns the right operand.

### 12. What is `?.`?

It is the null-conditional operator. It safely accesses a member when the receiver might be null.

### 13. Difference between `for` and `foreach`?

`for` is useful when you need an index or explicit iteration control. `foreach` is convenient for iterating through elements of a collection.

### 14. Difference between `while` and `do while`?

`while` checks the condition before executing the body. `do while` executes the body once before checking the condition.

### 15. What is a prime number?

A prime number is an integer greater than 1 with exactly two positive divisors: 1 and itself.

---

# 🔥 DAY 2 — FINAL REVISION

```text
VARIABLES
    ↓
int
long
float
double
decimal
bool
char
string

CONSTANT
    ↓
const

OPERATORS
    ↓
Arithmetic
+
-
*
/
%

Relational
==
!=
>
<
>=
<=

Logical
&&
||
!

Assignment
=
+=
-=
*=
/=
%=

Increment / Decrement
++
--

Null
??
?.

CONTROL FLOW
    ↓
if
else
else if
switch

LOOPS
    ↓
for
foreach
while
do while

PRACTICE
    ↓
Calculator
Number Checker
Even / Odd
Prime
Factorial
Fibonacci
```

---

# Phase 1: C# Fundamentals — Complete Class Notes

Level: Beginner · Language: C# · IDE: Visual Studio · Target: Future .NET Web API development

## 1. Introduction to C\#

### What is C#?

C# (pronounced “C-Sharp”) is a modern, object-oriented programming language developed by Microsoft.

It is used to develop:

- Web applications and REST APIs
- Desktop applications
- Cloud applications
- Games
- Enterprise software

### What is .NET?

.NET is a development platform used to build and run applications written in languages such as C#.

Explain the difference to students:

- C# — the programming language.
- .NET — the platform and runtime used to build and run applications.
- ASP.NET Core — the framework used to build web applications and Web APIs.
- Visual Studio — the IDE used to write, run, test and debug code.

### First C# program

Create a Console App in Visual Studio and write:

```
Console.WriteLine("Hello, World!");
```

Output:

```
Hello, World!
```

Explain:

- `Console` — provides console input and output functionality.
- `WriteLine()` — displays text and moves to a new line.
- `"Hello, World!"` — a string literal.
- `;` — terminates the statement.

Another example:

```
Console.Write("Hello ");
Console.Write("Students");
Console.WriteLine("!");
```

Output:

```
Hello Students!
```

Teaching point: `Write()` does not automatically move to a new line, while `WriteLine()` does.

## 2. Variables, Data Types and Constants

### 2.1 What is a variable?

A variable is a named storage location used to hold a value that can be used or changed during program execution.

Syntax:

```
dataType variableName = value;
```

Example:

```
int age = 25;
string name = "Arun";
double salary = 25000.50;
bool isActive = true;

Console.WriteLine(name);
Console.WriteLine(age);
Console.WriteLine(salary);
Console.WriteLine(isActive);
```

Output:

```
Arun
25
25000.5
True
```

### 2.2 Common C# data types

| Data type | Purpose                                  | Example                       |
| --------- | ---------------------------------------- | ----------------------------- |
| `int`     | Whole numbers                            | `int age = 25;`               |
| `long`    | Larger whole numbers                     | `long population = 9000000L;` |
| `float`   | Single-precision decimal                 | `float price = 12.5F;`        |
| `double`  | Double-precision decimal                 | `double salary = 25000.50;`   |
| `decimal` | High-precision decimal, useful for money | `decimal amount = 1500.75m;`  |
| `char`    | Single character                         | `char grade = 'A';`           |
| `string`  | Text                                     | `string name = "Arun";`       |
| `bool`    | True or false                            | `bool isPassed = true;`       |

Explain the difference between `char` and `string`:

```
char grade = 'A';
string name = "Arun";
```

A `char` uses single quotes, while a string uses double quotes.

### 2.3 Constants

A constant is a value that cannot be changed after it has been declared.

```
const double Pi = 3.14159;

Console.WriteLine(Pi);
```

The following is invalid:

```
const int age = 20;
age = 25;
```

Teaching point: Use `const` for values that must remain fixed and can be determined at compile time.

### 2.4 Variable naming rules

Valid:

```
int studentAge = 20;
string studentName = "Ravi";
```

Invalid:

```
// int 1age = 20;
// string student name = "Ravi";
```

Common convention: use `camelCase` for local variables, such as `studentName`.

### Practice exercise

Create variables for a student's name, age, marks, grade and pass status. Print all five values.

## 3. Operators and Type Conversion

### 3.1 Arithmetic operators

| Operator | Meaning        | Example  |
| -------- | -------------- | -------- |
| `+`      | Addition       | `10 + 5` |
| `-`      | Subtraction    | `10 - 5` |
| `*`      | Multiplication | `10 * 5` |
| `/`      | Division       | `10 / 5` |
| `%`      | Remainder      | `10 % 3` |

Example:

```
int a = 10;
int b = 3;

Console.WriteLine(a + b); // 13
Console.WriteLine(a - b); // 7
Console.WriteLine(a * b); // 30
Console.WriteLine(a / b); // 3
Console.WriteLine(a % b); // 1
```

Important: When both operands are integers, integer division discards the fractional part.

```
Console.WriteLine(10 / 3);       // 3
Console.WriteLine(10.0 / 3);     // Approximately 3.3333
```

### 3.2 Comparison operators

These return a Boolean result.

```
int age = 20;

Console.WriteLine(age == 20); // True
Console.WriteLine(age != 20); // False
Console.WriteLine(age > 18);  // True
Console.WriteLine(age < 18);  // False
Console.WriteLine(age >= 20); // True
Console.WriteLine(age <= 18); // False
```

Explain the difference:

- `=` assigns a value.
- `==` compares two values.

### 3.3 Logical operators

```
int age = 22;
bool hasId = true;

Console.WriteLine(age >= 18 && hasId); // True
Console.WriteLine(age >= 18 || hasId); // True
Console.WriteLine(!hasId);             // False
```

- `&&` — AND: both conditions must be true.
- `||` — OR: at least one condition must be true.
- `!` — NOT: reverses a Boolean value.

### 3.4 Type conversion

Implicit conversion:

```
int number = 100;
double result = number;

Console.WriteLine(result); // 100
```

Explicit conversion:

```
double price = 99.99;
int wholePrice = (int)price;

Console.WriteLine(wholePrice); // 99
```

The explicit cast truncates the fractional part in this example.

String conversion:

```
string value = "100";
int number = int.Parse(value);

Console.WriteLine(number + 50); // 150
```

For user input, `TryParse` is safer:

```
Console.Write("Enter your age: ");
string? input = Console.ReadLine();

if (int.TryParse(input, out int age))
{
    Console.WriteLine($"Your age is {age}");
}
else
{
    Console.WriteLine("Please enter a valid whole number.");
}
```

Teaching point: `Parse` can throw an exception when input is invalid. `TryParse` lets you handle invalid input without that exception.

## 4. Conditional Statements

Conditional statements allow a program to make decisions.

### 4.1 `if`

```
int age = 20;

if (age >= 18)
{
    Console.WriteLine("Eligible to vote.");
}
```

### 4.2 `if-else`

```
int marks = 45;

if (marks >= 35)
{
    Console.WriteLine("Pass");
}
else
{
    Console.WriteLine("Fail");
}
```

### 4.3 `else if`

```
int marks = 85;

if (marks >= 90)
{
    Console.WriteLine("Grade A");
}
else if (marks >= 75)
{
    Console.WriteLine("Grade B");
}
else if (marks >= 50)
{
    Console.WriteLine("Grade C");
}
else if (marks >= 35)
{
    Console.WriteLine("Grade D");
}
else
{
    Console.WriteLine("Fail");
}
```

Output:

```
Grade B
```

Explain that conditions are checked from top to bottom. The first matching branch executes.

### 4.4 `switch`

Use `switch` when selecting from several known alternatives.

```
int day = 2;

switch (day)
{
    case 1:
        Console.WriteLine("Monday");
        break;

    case 2:
        Console.WriteLine("Tuesday");
        break;

    case 3:
        Console.WriteLine("Wednesday");
        break;

    default:
        Console.WriteLine("Invalid day");
        break;
}
```

Output:

```
Tuesday
```

`break` exits the switch section. `default` handles cases not covered by the other labels.

### Classroom exercise

Write a program that accepts marks from 0 to 100 and displays the grade. Reject values outside the valid range.

## 5. Loops

Loops execute a block of code repeatedly.

### 5.1 `for` loop

Use it when you know how many iterations you want.

```
for (int i = 1; i <= 5; i++)
{
    Console.WriteLine(i);
}
```

Output:

```
1
2
3
4
5
```

Explain the three parts:

```
for (initialization; condition; update)
```

- Initialization: runs once.
- Condition: checked before each iteration.
- Update: runs after each iteration.

### 5.2 `while` loop

Use it when repetition depends on a condition.

```
int i = 1;

while (i <= 5)
{
    Console.WriteLine(i);
    i++;
}
```

### 5.3 `foreach` loop

Use it to iterate over elements in a collection or array.

```
string[] names = { "Arun", "Priya", "Kumar" };

foreach (string name in names)
{
    Console.WriteLine(name);
}
```

Output:

```
Arun
Priya
Kumar
```

### 5.4 `break` and `continue`

`break` terminates the loop:

```
for (int i = 1; i <= 10; i++)
{
    if (i == 5)
    {
        break;
    }

    Console.WriteLine(i);
}
```

Output: `1 2 3 4`

`continue` skips the current iteration:

```
for (int i = 1; i <= 5; i++)
{
    if (i == 3)
    {
        continue;
    }

    Console.WriteLine(i);
}
```

Output: `1 2 4 5`

Common mistake: Forgetting to update the counter in a `while` loop may cause an infinite loop.

## 6. Methods, Parameters and Return Types

A method is a named block of code that performs a particular task.

### 6.1 Method without a return value

```
static void Greet()
{
    Console.WriteLine("Welcome to C#");
}

Greet();
```

`void` means the method does not return a value.

### 6.2 Method with parameters

```
static void GreetStudent(string name)
{
    Console.WriteLine($"Welcome, {name}");
}

GreetStudent("Arun");
GreetStudent("Priya");
```

Here, `name` is a parameter, and `"Arun"` is an argument in the first method call.

### 6.3 Method with a return value

```
static int Add(int a, int b)
{
    return a + b;
}

int result = Add(10, 20);

Console.WriteLine(result); // 30
```

Explain:

- `int` — return type.
- `Add` — method name.
- `int a, int b` — parameters.
- `return` — sends the result back to the caller.

### 6.4 Why use methods?

Methods help with:

- Code reuse
- Readability
- Easier testing
- Easier maintenance

### Practice exercise

Create methods to calculate the area of a rectangle, calculate the square of a number, and determine whether a number is even or odd.

## 7. Arrays and Strings

### 7.1 Arrays

An array stores multiple values of the same type in a single variable.

```
int[] marks = { 80, 75, 90, 65, 88 };

Console.WriteLine(marks[0]); // 80
Console.WriteLine(marks[2]); // 90
Console.WriteLine(marks.Length); // 5
```

Array indexes start at `0`.

For an array of five elements, valid indexes are `0` to `4`.

Use a loop to print every element:

```
int[] marks = { 80, 75, 90, 65, 88 };

for (int i = 0; i < marks.Length; i++)
{
    Console.WriteLine(marks[i]);
}
```

### 7.2 Strings

A string stores text.

```
string name = "Arun Kumar";

Console.WriteLine(name.Length);
Console.WriteLine(name.ToUpper());
Console.WriteLine(name.ToLower());
Console.WriteLine(name.Contains("Kumar"));
Console.WriteLine(name.Replace("Arun", "Ravi"));
```

Strings are immutable: string operations produce new strings rather than changing the original string in place.

### 7.3 String interpolation

```
string name = "Priya";
int age = 22;

Console.WriteLine($"My name is {name} and I am {age} years old.");
```

The `$` allows expressions inside `{}` to be evaluated and included in the string.

### Practice exercise

Store five student names in an array. Print them one by one and search for a particular name.

## 8. Classes and Objects

C# is an object-oriented programming language.

A class defines the data and behaviour that its objects can have. An object is an instance of a class.

Example:

```
class Student
{
    public string Name = "";
    public int Age;

    public void DisplayDetails()
    {
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Age: {Age}");
    }
}
```

Create an object and use it:

```
Student student = new Student();

student.Name = "Arun";
student.Age = 20;

student.DisplayDetails();
```

Output:

```
Name: Arun
Age: 20
```

Explain:

- `class Student` declares a class.
- `new Student()` creates an instance.
- `student` is a variable referring to that instance.
- `DisplayDetails()` is an instance method.

In a typical modern Console App, put the `Student` class declaration after the top-level statements.

### Why are classes important?

They group related data and operations into a single logical unit. In a Web API, you will use classes for entities, request models, response models and services.

## 9. Constructors and Properties

### 9.1 Constructors

A constructor initializes an object when it is created.

```
class Student
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Student(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void Display()
    {
        Console.WriteLine($"{Name} - {Age}");
    }
}
```

Usage:

```
Student student = new Student("Arun", 20);

student.Display();
```

Output:

```
Arun - 20
```

Important teaching points:

- A constructor has the same name as its class.
- A constructor has no return type.
- Constructors can accept parameters.
- C# supports constructor overloading.

### 9.2 Properties

Properties provide a controlled way to read or modify an object's data.

```
class Employee
{
    public string Name { get; set; }
    public decimal Salary { get; set; }
}
```

Usage:

```
Employee employee = new Employee();

employee.Name = "Priya";
employee.Salary = 35000m;

Console.WriteLine(employee.Name);
Console.WriteLine(employee.Salary);
```

Explain:

- `get` reads the value.
- `set` assigns the value.
- `{ get; set; }` is an auto-implemented property.

Properties are generally preferred over public fields for application models.

## 10. Access Modifiers

Access modifiers control where classes and members can be accessed.

| Modifier             | Meaning                                                               |
| -------------------- | --------------------------------------------------------------------- |
| `public`             | Accessible wherever the containing type and other access rules permit |
| `private`            | Accessible only within the containing type                            |
| `protected`          | Accessible within the containing type and derived types               |
| `internal`           | Accessible within the same assembly                                   |
| `protected internal` | Accessible within the same assembly or from derived types             |
| `private protected`  | Accessible from derived types within the same assembly                |

Example:

```
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

Usage:

```
BankAccount account = new BankAccount();

account.Deposit(1000m);

Console.WriteLine(account.GetBalance());
```

Output:

```
1000
```

Direct access to `account.balance` is not allowed because `balance` is private.

Teaching point: Access modifiers support encapsulation by controlling access to an object's implementation and data.

## 11. The `static` Keyword

The `static` keyword indicates that a member belongs to the type itself rather than to an individual instance.

```
class Calculator
{
    public static int Add(int a, int b)
    {
        return a + b;
    }
}
```

Usage:

```
int result = Calculator.Add(10, 20);

Console.WriteLine(result);
```

No `Calculator` object is required to call `Add`.

Compare with an instance method:

```
class Calculator
{
    public int Multiply(int a, int b)
    {
        return a * b;
    }
}
```

Usage:

```
Calculator calculator = new Calculator();

Console.WriteLine(calculator.Multiply(10, 20));
```

Static members are useful for operations that do not require instance-specific state.

In a top-level Console App, you may also see methods declared `static` when they are used without creating a separate object.

## 12. Value Types vs Reference Types

This topic is important because it helps students understand assignments, method arguments and object behaviour.

### Value types

Examples include `int`, `double`, `bool`, `char` and `struct`.

```
int a = 10;
int b = a;

b = 20;

Console.WriteLine(a); // 10
Console.WriteLine(b); // 20
```

The assignment copies the value.

### Reference types

Examples include classes, arrays and strings.

```
class Student
{
    public string Name { get; set; } = "";
}
```

```
Student s1 = new Student();
s1.Name = "Arun";

Student s2 = s1;
s2.Name = "Priya";

Console.WriteLine(s1.Name); // Priya
Console.WriteLine(s2.Name); // Priya
```

Both variables refer to the same object.

Important nuance: a reference-type variable stores a reference to an object, not the object itself. Assigning the variable copies the reference, so both references can point to the same object.

## 13. Nullable Types and Null Handling

### 13.1 What is `null`?

`null` represents the absence of a value for a reference or nullable value type.

A normal `int` cannot hold `null`:

```
int age = 25;
```

A nullable integer can:

```
int? age = null;

Console.WriteLine(age.HasValue); // False
```

The `?` in `int?` allows the integer to have no value.

### 13.2 Nullable reference types

With nullable reference types enabled:

```
string? name = null;
string city = "Chennai";
```

`string?` indicates that `name` may be null. `string` indicates that the variable is intended to hold a non-null string, although compile-time warnings do not guarantee runtime safety.

### 13.3 Null-coalescing operator

```
string? name = null;

string displayName = name ?? "Guest";

Console.WriteLine(displayName);
```

Output:

```
Guest
```

`??` uses

