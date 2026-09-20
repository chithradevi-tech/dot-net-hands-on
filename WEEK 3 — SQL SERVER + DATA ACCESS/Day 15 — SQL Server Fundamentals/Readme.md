# Day 15 — SQL Server Fundamentals

Today we move from **ASP.NET Core in-memory data** to **real database concepts**. This is an important day because later your architecture will become:

```text
Angular / Postman
        ↓
ASP.NET Core Controller
        ↓
Service / BAL
        ↓
DAL / Repository
        ↓
SQL Server
        ↓
Tables
```

---

# 1. What is a Database?

A **database** is an organized collection of data that can be stored, managed, searched, and updated efficiently.

For example, an Employee Management application may store:

```text
Employee Database
│
├── Employees
├── Departments
├── LeaveRequests
└── Salaries
```

A database can contain multiple **tables**.

### Example

```text
EmployeeDB
    ↓
Employees Table
    ↓
Employee records
```

---

# 2. What is SQL Server?

**Microsoft SQL Server** is a relational database management system (RDBMS) developed by Microsoft.

It stores data in tables and allows applications to:

* Insert data
* Read data
* Update data
* Delete data
* Search/filter data
* Group and summarize data
* Maintain relationships between tables

We use **SQL (Structured Query Language)** to communicate with SQL Server.

---

# 3. Database → Table → Row → Column

This is one of the most important concepts.

Suppose we have:

```text
EmployeeDB
   ↓
Employees
```

The `Employees` table:

| Id | Name  | Department | Salary |
| -: | ----- | ---------- | -----: |
|  1 | Arun  | IT         |  50000 |
|  2 | Priya | HR         |  45000 |
|  3 | Kumar | IT         |  60000 |

### Database

Container for related data.

```text
EmployeeDB
```

### Table

Stores a particular type of data.

```text
Employees
```

### Column

Defines an attribute/property.

```text
Id
Name
Department
Salary
```

### Row

Represents one record.

```text
1 | Arun | IT | 50000
```

### Easy memory

```text
Database
   ↓
Tables
   ↓
Rows + Columns
```

---

# 4. Creating a Database

In SQL Server Management Studio (SSMS), open a new query.

```sql
CREATE DATABASE EmployeeDB;
```

Then select the database:

```sql
USE EmployeeDB;
```

Now subsequent commands execute against `EmployeeDB`.

---

# 5. Creating a Table

Let's create an employee table.

```sql
CREATE TABLE Employees
(
    Id INT,
    Name VARCHAR(100),
    Department VARCHAR(50),
    Salary DECIMAL(10,2)
);
```

Now:

```text
Employees
--------------------------------
Id
Name
Department
Salary
```

But this table has no primary key or constraints yet.

We'll improve it.

---

# 6. Primary Key

A **Primary Key** uniquely identifies each row in a table.

Example:

```text
Id
--
1
2
3
4
```

Each employee should have a unique ID.

```sql
CREATE TABLE Employees
(
    Id INT PRIMARY KEY,
    Name VARCHAR(100),
    Department VARCHAR(50),
    Salary DECIMAL(10,2)
);
```

### Rules

A primary key:

* Must be unique
* Cannot contain `NULL`
* Identifies a row
* A table can have one primary key constraint

Example:

```text
Id
--
1
2
3
```

This is valid.

But:

```text
Id
--
1
1
2
```

is not valid because `1` is duplicated.

---

# 7. Identity

Usually we don't want to manually generate employee IDs.

SQL Server can automatically generate them using `IDENTITY`.

```sql
Id INT IDENTITY(1,1) PRIMARY KEY
```

Complete example:

```sql
CREATE TABLE Employees
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100),
    Department VARCHAR(50),
    Salary DECIMAL(10,2)
);
```

### What does `IDENTITY(1,1)` mean?

```text
IDENTITY(seed, increment)
```

So:

```sql
IDENTITY(1,1)
```

means:

```text
First value = 1
Next value  = 2
Next value  = 3
Next value  = 4
```

Another example:

```sql
IDENTITY(1000,1)
```

produces:

```text
1000
1001
1002
1003
```

---

# 8. NULL

`NULL` means **no value / unknown value**.

It does not mean:

```text
0
```

It does not mean:

```text
''
```

It means the value is absent/unknown.

Example:

```text
Id | Name  | Department
---|-------|-----------
1  | Arun  | IT
2  | Priya | NULL
```

Priya's department has no value.

---

# 9. NOT NULL

If a column must always have a value, use `NOT NULL`.

```sql
CREATE TABLE Employees
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Department VARCHAR(50) NOT NULL,
    Salary DECIMAL(10,2) NOT NULL
);
```

Now:

```sql
INSERT INTO Employees (Name, Department, Salary)
VALUES ('Arun', 'IT', 50000);
```

works.

But:

```sql
INSERT INTO Employees (Name, Department, Salary)
VALUES (NULL, 'IT', 50000);
```

fails because `Name` is `NOT NULL`.

---

# 10. UNIQUE Constraint

`UNIQUE` prevents duplicate values.

For example, employee email addresses should normally be unique.

```sql
CREATE TABLE Employees
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(150) UNIQUE,
    Department VARCHAR(50),
    Salary DECIMAL(10,2)
);
```

Then:

```text
arun@gmail.com
priya@gmail.com
```

is valid.

But:

```text
arun@gmail.com
arun@gmail.com
```

violates the unique constraint.

---

# 11. DEFAULT Constraint

`DEFAULT` provides a value automatically when no value is supplied.

Example:

```sql
CREATE TABLE Employees
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Department VARCHAR(50) DEFAULT 'IT',
    Salary DECIMAL(10,2) NOT NULL
);
```

Now:

```sql
INSERT INTO Employees
(
    Name,
    Salary
)
VALUES
(
    'Arun',
    50000
);
```

Since `Department` was not supplied:

```text
Department = IT
```

automatically.

---

# 12. Foreign Key

A **Foreign Key** creates a relationship between tables.

Suppose we have:

### Departments

| DepartmentId | DepartmentName |
| -----------: | -------------- |
|            1 | IT             |
|            2 | HR             |
|            3 | Finance        |

### Employees

| Id | Name  | DepartmentId |
| -: | ----- | -----------: |
|  1 | Arun  |            1 |
|  2 | Priya |            2 |
|  3 | Kumar |            1 |

`Employees.DepartmentId` refers to `Departments.DepartmentId`.

```text
Departments
     ↑
     │
     │ Foreign Key
     │
Employees
```

Create Departments:

```sql
CREATE TABLE Departments
(
    DepartmentId INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName VARCHAR(100) NOT NULL UNIQUE
);
```

Create Employees:

```sql
CREATE TABLE Employees
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(150) UNIQUE,
    DepartmentId INT,
    Salary DECIMAL(10,2),

    CONSTRAINT FK_Employees_Departments
        FOREIGN KEY (DepartmentId)
        REFERENCES Departments(DepartmentId)
);
```

Now SQL Server can enforce the relationship.

---

# 13. Constraints

A **constraint** is a rule that controls the data allowed in a table.

Important constraints:

```text
PRIMARY KEY
FOREIGN KEY
NOT NULL
UNIQUE
DEFAULT
CHECK
```

### Example

```sql
CREATE TABLE Employees
(
    Id INT IDENTITY(1,1)
        CONSTRAINT PK_Employees PRIMARY KEY,

    Name VARCHAR(100) NOT NULL,

    Email VARCHAR(150)
        CONSTRAINT UQ_Employees_Email UNIQUE,

    DepartmentId INT NOT NULL,

    Salary DECIMAL(10,2)
        CONSTRAINT CK_Employees_Salary CHECK (Salary > 0),

    CONSTRAINT FK_Employees_Department
        FOREIGN KEY (DepartmentId)
        REFERENCES Departments(DepartmentId)
);
```

---

# 14. CHECK Constraint

Although not explicitly in today's list, you should know this because it is an important SQL Server constraint.

It restricts values according to a condition.

```sql
Salary DECIMAL(10,2)
    CHECK (Salary > 0)
```

Therefore:

```text
50000   → valid
25000   → valid
0       → invalid
-5000   → invalid
```

---

# 15. Final Employee Database Setup

Let's create a proper database for today's practice.

## Step 1 — Database

```sql
CREATE DATABASE EmployeeDB;
GO

USE EmployeeDB;
GO
```

## Step 2 — Departments

```sql
CREATE TABLE Departments
(
    DepartmentId INT IDENTITY(1,1)
        CONSTRAINT PK_Departments PRIMARY KEY,

    DepartmentName VARCHAR(100) NOT NULL
        CONSTRAINT UQ_Departments_Name UNIQUE
);
```

## Step 3 — Employees

```sql
CREATE TABLE Employees
(
    Id INT IDENTITY(1,1)
        CONSTRAINT PK_Employees PRIMARY KEY,

    Name VARCHAR(100) NOT NULL,

    Email VARCHAR(150)
        CONSTRAINT UQ_Employees_Email UNIQUE,

    DepartmentId INT NOT NULL,

    Salary DECIMAL(10,2) NOT NULL
        CONSTRAINT CK_Employees_Salary CHECK (Salary > 0),

    City VARCHAR(100) NULL,

    Status VARCHAR(20)
        CONSTRAINT DF_Employees_Status DEFAULT 'Active',

    CONSTRAINT FK_Employees_Department
        FOREIGN KEY (DepartmentId)
        REFERENCES Departments(DepartmentId)
);
```

Our structure is now:

```text
EmployeeDB
│
├── Departments
│   ├── DepartmentId
│   └── DepartmentName
│
└── Employees
    ├── Id
    ├── Name
    ├── Email
    ├── DepartmentId
    ├── Salary
    ├── City
    └── Status
```

---

# 16. INSERT

`INSERT` is used to add records.

First add departments:

```sql
INSERT INTO Departments
(
    DepartmentName
)
VALUES
('IT'),
('HR'),
('Finance'),
('Sales');
```

Then employees:

```sql
INSERT INTO Employees
(
    Name,
    Email,
    DepartmentId,
    Salary,
    City
)
VALUES
('Arun', 'arun@gmail.com', 1, 50000, 'Chennai'),
('Priya', 'priya@gmail.com', 2, 45000, 'Bangalore'),
('Kumar', 'kumar@gmail.com', 1, 60000, 'Chennai'),
('Divya', 'divya@gmail.com', 3, 70000, 'Coimbatore'),
('Ravi', 'ravi@gmail.com', 4, 40000, 'Madurai');
```

Notice we don't provide `Id`.

Why?

Because:

```sql
IDENTITY(1,1)
```

generates it automatically.

We also don't provide `Status`.

Why?

Because:

```sql
DEFAULT 'Active'
```

handles it.

---

# 17. SELECT

`SELECT` retrieves data.

```sql
SELECT *
FROM Employees;
```

`*` means all columns.

Better in application code:

```sql
SELECT
    Id,
    Name,
    Email,
    DepartmentId,
    Salary,
    City,
    Status
FROM Employees;
```

---

# 18. SELECT Specific Columns

```sql
SELECT Name, Salary
FROM Employees;
```

Result:

```text
Name    Salary
------  ------
Arun    50000
Priya   45000
Kumar   60000
Divya   70000
Ravi    40000
```

---

# 19. WHERE

`WHERE` filters rows.

```sql
SELECT *
FROM Employees
WHERE DepartmentId = 1;
```

This returns employees from IT.

Another:

```sql
SELECT *
FROM Employees
WHERE Salary > 50000;
```

Multiple conditions:

```sql
SELECT *
FROM Employees
WHERE DepartmentId = 1
AND Salary > 50000;
```

Using `OR`:

```sql
SELECT *
FROM Employees
WHERE DepartmentId = 1
OR DepartmentId = 2;
```

---

# 20. ORDER BY

Used to sort results.

### Ascending

```sql
SELECT *
FROM Employees
ORDER BY Salary ASC;
```

Lowest salary → highest salary.

### Descending

```sql
SELECT *
FROM Employees
ORDER BY Salary DESC;
```

Highest salary → lowest salary.

`ASC` is the default:

```sql
ORDER BY Salary;
```

---

# 21. DISTINCT

Returns unique values.

```sql
SELECT DISTINCT DepartmentId
FROM Employees;
```

If employees have:

```text
1
1
2
3
1
```

result:

```text
1
2
3
```

Another example:

```sql
SELECT DISTINCT City
FROM Employees;
```

---

# 22. TOP

Returns a specified number of rows.

### Top 3 employees

```sql
SELECT TOP 3 *
FROM Employees;
```

For highest salaries:

```sql
SELECT TOP 3 *
FROM Employees
ORDER BY Salary DESC;
```

This is important:

```sql
SELECT TOP 3 *
FROM Employees;
```

does **not** mean top 3 highest salaries.

You need:

```sql
ORDER BY Salary DESC;
```

---

# 23. LIKE

`LIKE` is used for pattern matching.

### Names starting with A

```sql
SELECT *
FROM Employees
WHERE Name LIKE 'A%';
```

`%` means zero or more characters.

Examples:

```sql
LIKE 'A%'
```

Starts with A.

```sql
LIKE '%a'
```

Ends with a.

```sql
LIKE '%ar%'
```

Contains `ar`.

---

# 24. Underscore `_` in LIKE

`_` represents exactly one character.

```sql
WHERE Name LIKE '_run'
```

Could match:

```text
Arun
Brun
Crun
```

depending on the actual data/collation.

Difference:

```text
% → zero or more characters
_ → exactly one character
```

---

# 25. BETWEEN

`BETWEEN` checks whether a value is within an inclusive range.

```sql
SELECT *
FROM Employees
WHERE Salary BETWEEN 40000 AND 60000;
```

Important:

```text
BETWEEN includes both boundaries.
```

So:

```text
40000 → included
60000 → included
```

Equivalent conceptually to:

```sql
WHERE Salary >= 40000
AND Salary <= 60000;
```

---

# 26. IN

`IN` checks against multiple possible values.

Instead of:

```sql
SELECT *
FROM Employees
WHERE DepartmentId = 1
   OR DepartmentId = 2
   OR DepartmentId = 3;
```

we can write:

```sql
SELECT *
FROM Employees
WHERE DepartmentId IN (1, 2, 3);
```

String example:

```sql
SELECT *
FROM Employees
WHERE City IN ('Chennai', 'Bangalore');
```

---

# 27. GROUP BY

`GROUP BY` groups rows so aggregate functions can calculate values per group.

Example:

```sql
SELECT
    DepartmentId,
    COUNT(*) AS EmployeeCount
FROM Employees
GROUP BY DepartmentId;
```

Result:

```text
DepartmentId    EmployeeCount
------------    -------------
1               2
2               1
3               1
4               1
```

Another example:

```sql
SELECT
    DepartmentId,
    SUM(Salary) AS TotalSalary
FROM Employees
GROUP BY DepartmentId;
```

---

# 28. Aggregate Functions

You will use these heavily with `GROUP BY`.

### COUNT

```sql
SELECT COUNT(*) AS TotalEmployees
FROM Employees;
```

### SUM

```sql
SELECT SUM(Salary) AS TotalSalary
FROM Employees;
```

### AVG

```sql
SELECT AVG(Salary) AS AverageSalary
FROM Employees;
```

### MIN

```sql
SELECT MIN(Salary) AS MinimumSalary
FROM Employees;
```

### MAX

```sql
SELECT MAX(Salary) AS MaximumSalary
FROM Employees;
```

---

# 29. HAVING

`HAVING` filters **groups**.

Example:

```sql
SELECT
    DepartmentId,
    COUNT(*) AS EmployeeCount
FROM Employees
GROUP BY DepartmentId
HAVING COUNT(*) > 1;
```

This returns departments having more than one employee.

### WHERE vs HAVING

This is an important interview question.

**WHERE** filters rows **before grouping**.

**HAVING** filters groups **after GROUP BY**.

Think:

```text
WHERE
  ↓
Filter rows
  ↓
GROUP BY
  ↓
Create groups
  ↓
HAVING
  ↓
Filter groups
```

---

# 30. UPDATE

`UPDATE` modifies existing records.

Example:

```sql
UPDATE Employees
SET Salary = 55000
WHERE Id = 1;
```

Always be careful with `UPDATE`.

This:

```sql
UPDATE Employees
SET Salary = 55000;
```

updates **every employee**.

So normally use an appropriate `WHERE` condition.

Multiple columns:

```sql
UPDATE Employees
SET
    Salary = 60000,
    City = 'Bangalore'
WHERE Id = 1;
```

---

# 31. DELETE

`DELETE` removes rows.

```sql
DELETE FROM Employees
WHERE Id = 5;
```

Again, be very careful.

```sql
DELETE FROM Employees;
```

deletes **all rows** from the table.

The table itself remains.

---

# 32. DELETE vs DROP vs TRUNCATE

This is useful to know now.

### DELETE

```sql
DELETE FROM Employees
WHERE Id = 5;
```

Removes rows.

### TRUNCATE

```sql
TRUNCATE TABLE Employees;
```

Removes all rows from the table and has different logging/identity behavior from `DELETE`.

### DROP

```sql
DROP TABLE Employees;
```

Removes the table itself.

Memory:

```text
DELETE    → remove rows
TRUNCATE  → remove all rows
DROP      → remove table
```

---

# 33. SQL Query Execution Order

Very important for understanding SQL.

Although we write:

```sql
SELECT
FROM
WHERE
GROUP BY
HAVING
ORDER BY
```

conceptually SQL processes major clauses roughly like:

```text
FROM
  ↓
WHERE
  ↓
GROUP BY
  ↓
HAVING
  ↓
SELECT
  ↓
ORDER BY
  ↓
TOP
```

There are nuances around `TOP`, `DISTINCT`, window functions, etc., but this simplified order is excellent for learning.

---

# 34. Important Combined Queries

Now let's combine today's concepts.

### Query 1 — IT employees

```sql
SELECT *
FROM Employees
WHERE DepartmentId = 1;
```

### Query 2 — Salary greater than 50,000

```sql
SELECT Name, Salary
FROM Employees
WHERE Salary > 50000
ORDER BY Salary DESC;
```

### Query 3 — Top 3 salaries

```sql
SELECT TOP 3
    Name,
    Salary
FROM Employees
ORDER BY Salary DESC;
```

### Query 4 — Employees from Chennai

```sql
SELECT *
FROM Employees
WHERE City = 'Chennai';
```

### Query 5 — Names beginning with A

```sql
SELECT *
FROM Employees
WHERE Name LIKE 'A%';
```

### Query 6 — Salary between 40K and 60K

```sql
SELECT *
FROM Employees
WHERE Salary BETWEEN 40000 AND 60000;
```

### Query 7 — Employees in selected departments

```sql
SELECT *
FROM Employees
WHERE DepartmentId IN (1, 3);
```

### Query 8 — Unique cities

```sql
SELECT DISTINCT City
FROM Employees;
```

### Query 9 — Employee count by department

```sql
SELECT
    DepartmentId,
    COUNT(*) AS EmployeeCount
FROM Employees
GROUP BY DepartmentId;
```

### Query 10 — Departments with more than one employee

```sql
SELECT
    DepartmentId,
    COUNT(*) AS EmployeeCount
FROM Employees
GROUP BY DepartmentId
HAVING COUNT(*) > 1;
```

---

# 35. NULL — Important Queries

You should **not** use:

```sql
WHERE City = NULL
```

Use:

```sql
WHERE City IS NULL;
```

For non-null:

```sql
WHERE City IS NOT NULL;
```

Example:

```sql
SELECT *
FROM Employees
WHERE City IS NULL;
```

---

# 36. Complete Day 15 Practice Script

You can create a new SQL Server database and run this script step by step.

```sql
CREATE DATABASE EmployeeDB;
GO

USE EmployeeDB;
GO

CREATE TABLE Departments
(
    DepartmentId INT IDENTITY(1,1)
        CONSTRAINT PK_Departments PRIMARY KEY,

    DepartmentName VARCHAR(100) NOT NULL
        CONSTRAINT UQ_Departments_Name UNIQUE
);
GO

INSERT INTO Departments (DepartmentName)
VALUES
('IT'),
('HR'),
('Finance'),
('Sales');
GO

CREATE TABLE Employees
(
    Id INT IDENTITY(1,1)
        CONSTRAINT PK_Employees PRIMARY KEY,

    Name VARCHAR(100) NOT NULL,

    Email VARCHAR(150)
        CONSTRAINT UQ_Employees_Email UNIQUE,

    DepartmentId INT NOT NULL,

    Salary DECIMAL(10,2) NOT NULL
        CONSTRAINT CK_Employees_Salary CHECK (Salary > 0),

    City VARCHAR(100) NULL,

    Status VARCHAR(20)
        CONSTRAINT DF_Employees_Status DEFAULT 'Active',

    CONSTRAINT FK_Employees_Department
        FOREIGN KEY (DepartmentId)
        REFERENCES Departments(DepartmentId)
);
GO

INSERT INTO Employees
(
    Name,
    Email,
    DepartmentId,
    Salary,
    City
)
VALUES
('Arun', 'arun@gmail.com', 1, 50000, 'Chennai'),
('Priya', 'priya@gmail.com', 2, 45000, 'Bangalore'),
('Kumar', 'kumar@gmail.com', 1, 60000, 'Chennai'),
('Divya', 'divya@gmail.com', 3, 70000, 'Coimbatore'),
('Ravi', 'ravi@gmail.com', 4, 40000, 'Madurai'),
('Meena', 'meena@gmail.com', 1, 65000, 'Bangalore');
GO

SELECT *
FROM Employees;
GO

SELECT Name, Salary
FROM Employees
WHERE Salary > 50000
ORDER BY Salary DESC;
GO

SELECT TOP 3
    Name,
    Salary
FROM Employees
ORDER BY Salary DESC;
GO

SELECT DISTINCT City
FROM Employees;
GO

SELECT *
FROM Employees
WHERE Name LIKE 'A%';
GO

SELECT *
FROM Employees
WHERE Salary BETWEEN 40000 AND 60000;
GO

SELECT *
FROM Employees
WHERE DepartmentId IN (1, 3);
GO

SELECT
    DepartmentId,
    COUNT(*) AS EmployeeCount,
    SUM(Salary) AS TotalSalary,
    AVG(Salary) AS AverageSalary
FROM Employees
GROUP BY DepartmentId;
GO

SELECT
    DepartmentId,
    COUNT(*) AS EmployeeCount
FROM Employees
GROUP BY DepartmentId
HAVING COUNT(*) > 1;
GO
```

---

# 37. SQL Server + Your .NET Architecture

Until Day 14, your application was conceptually:

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
In-Memory List
```

Now we are introducing SQL Server:

```text
ASP.NET Core
      ↓
Controller
      ↓
BAL / Service
      ↓
DAL / Repository
      ↓
ADO.NET
      ↓
SQL Server
      ↓
EmployeeDB
      ↓
Employees Table
```

Later you will learn **ADO.NET** to execute SQL from C#.

For example, eventually your C# code will do something conceptually like:

```text
EmployeeController
        ↓
EmployeeService
        ↓
EmployeeRepository
        ↓
SqlConnection
        ↓
SqlCommand
        ↓
SQL Server
```

That is why today's SQL fundamentals are important before starting ADO.NET.

---

# 38. Day 15 — Important Differences

### Primary Key vs Foreign Key

```text
Primary Key
→ uniquely identifies a row

Foreign Key
→ references a key in another table
```

### WHERE vs HAVING

```text
WHERE
→ filters rows

HAVING
→ filters groups
```

### DELETE vs TRUNCATE vs DROP

```text
DELETE
→ remove selected/all rows

TRUNCATE
→ remove all rows

DROP
→ remove table
```

### NULL vs Empty String

```text
NULL
→ no/unknown value

''
→ empty string value
```

### UNIQUE vs PRIMARY KEY

```text
PRIMARY KEY
→ uniquely identifies rows
→ cannot be NULL
→ one primary key constraint per table

UNIQUE
→ prevents duplicate values
→ a table can have multiple UNIQUE constraints
```

---

# 39. Day 15 Interview Questions

### Beginner

1. What is a database?
2. What is SQL Server?
3. What is a table?
4. What is a row?
5. What is a column?
6. What is a primary key?
7. What is a foreign key?
8. What is a constraint?
9. What is `IDENTITY`?
10. What is `NULL`?
11. What is `NOT NULL`?
12. What is `UNIQUE`?
13. What is `DEFAULT`?

### SQL Commands

14. What is `SELECT`?
15. What is `INSERT`?
16. What is `UPDATE`?
17. What is `DELETE`?
18. What does `WHERE` do?
19. What does `ORDER BY` do?
20. What is `GROUP BY`?
21. What is `HAVING`?
22. What is `DISTINCT`?
23. What is `TOP`?
24. What is `LIKE`?
25. What is `BETWEEN`?
26. What is `IN`?

### Important interview questions

27. Difference between `WHERE` and `HAVING`?

28. Difference between primary key and unique key?

29. Difference between primary key and foreign key?

30. Difference between `DELETE`, `TRUNCATE`, and `DROP`?

31. How do you check for NULL?

```sql
WHERE ColumnName IS NULL
```

32. How do you get the top 5 highest-paid employees?

```sql
SELECT TOP 5 *
FROM Employees
ORDER BY Salary DESC;
```

33. How do you find employees whose names start with `A`?

```sql
SELECT *
FROM Employees
WHERE Name LIKE 'A%';
```

34. How do you find departments having more than 5 employees?

```sql
SELECT
    DepartmentId,
    COUNT(*) AS EmployeeCount
FROM Employees
GROUP BY DepartmentId
HAVING COUNT(*) > 5;
```

---

# 40. Day 15 Cheat Sheet

```text
DATABASE
    ↓
TABLE
    ↓
ROWS + COLUMNS
```

### Keys

```text
PRIMARY KEY
→ unique row identifier

FOREIGN KEY
→ relationship between tables
```

### Constraints

```text
PRIMARY KEY
FOREIGN KEY
NOT NULL
UNIQUE
DEFAULT
CHECK
```

### CRUD

```text
CREATE → INSERT
READ   → SELECT
UPDATE → UPDATE
DELETE → DELETE
```

### Filtering

```sql
WHERE
LIKE
BETWEEN
IN
IS NULL
IS NOT NULL
```

### Sorting

```sql
ORDER BY Salary ASC
ORDER BY Salary DESC
```

### Grouping

```sql
GROUP BY
HAVING
```

### Result control

```sql
DISTINCT
TOP
```

### Most important SQL flow

```text
FROM
 ↓
WHERE
 ↓
GROUP BY
 ↓
HAVING
 ↓
SELECT
 ↓
ORDER BY
```

### Day 15 overall architecture

```text
             ASP.NET Core
                  ↓
              Controller
                  ↓
             BAL / Service
                  ↓
             DAL / Repository
                  ↓
                ADO.NET
                  ↓
              SQL Server
                  ↓
              Database
                  ↓
                Tables
                  ↓
         Rows + Columns + Keys
```


