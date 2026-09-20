# Day 17 — Advanced SQL

Day 17 is an important step toward **real-world SQL Server development**.

Today we'll cover:

```text
Stored Procedures
├── CREATE PROCEDURE
├── ALTER PROCEDURE
├── EXEC
├── Input Parameters
├── Output Parameters
├── Return Values
├── Transactions
├── TRY/CATCH
├── Temporary Tables
└── Table Variables

CTE
├── WITH
├── Non-recursive CTE
├── Recursive CTE
└── Hierarchical Data
```

We'll continue using the **Employee / Department / Manager / Salary** database from Day 16.

---

# 1. What is a Stored Procedure?

A **Stored Procedure** is a named collection of SQL statements stored inside SQL Server.

Instead of sending a large SQL query from your .NET application every time, you can store the SQL logic in SQL Server and execute the procedure.

For example:

```text
ASP.NET Core
     ↓
DAL / Repository
     ↓
EXEC GetEmployees
     ↓
SQL Server
     ↓
Stored Procedure
     ↓
Employees table
```

---

# 2. Why Use Stored Procedures?

Stored procedures can provide:

* Reusable database logic
* Centralized SQL logic
* Input parameters
* Output parameters
* Transaction handling
* Error handling
* Controlled database access

Example:

Instead of repeatedly writing:

```sql
SELECT *
FROM Employees
WHERE DepartmentId = 1;
```

create:

```sql
CREATE PROCEDURE GetEmployeesByDepartment
    @DepartmentId INT
AS
BEGIN
    SELECT *
    FROM Employees
    WHERE DepartmentId = @DepartmentId;
END;
```

Then:

```sql
EXEC GetEmployeesByDepartment @DepartmentId = 1;
```

---

# 3. CREATE PROCEDURE

Basic syntax:

```sql
CREATE PROCEDURE ProcedureName
AS
BEGIN

    -- SQL statements

END;
```

Example:

```sql
CREATE PROCEDURE GetAllEmployees
AS
BEGIN

    SELECT *
    FROM Employees;

END;
```

Execute:

```sql
EXEC GetAllEmployees;
```

---

# 4. Procedure Naming Convention

A common naming style is:

```text
GetEmployees
GetEmployeeById
GetEmployeesByDepartment
CreateEmployee
UpdateEmployee
DeleteEmployee
```

Avoid unnecessary prefixes such as:

```text
sp_GetEmployees
```

for application procedures because `sp_` is commonly associated with SQL Server system stored procedures.

---

# 5. ALTER PROCEDURE

Suppose we created:

```sql
CREATE PROCEDURE GetAllEmployees
AS
BEGIN
    SELECT *
    FROM Employees;
END;
```

Later we want to change it.

Use:

```sql
ALTER PROCEDURE GetAllEmployees
AS
BEGIN

    SELECT
        EmployeeId,
        EmployeeName,
        DepartmentId,
        City
    FROM Employees;

END;
```

Then:

```sql
EXEC GetAllEmployees;
```

---

# 6. EXEC

`EXEC` or `EXECUTE` runs a stored procedure.

```sql
EXEC GetAllEmployees;
```

Both are valid:

```sql
EXEC GetAllEmployees;
```

and:

```sql
EXECUTE GetAllEmployees;
```

---

# 7. Stored Procedure with Input Parameter

Suppose we want employees from a particular department.

```sql
CREATE PROCEDURE GetEmployeesByDepartment
    @DepartmentId INT
AS
BEGIN

    SELECT
        EmployeeId,
        EmployeeName,
        DepartmentId,
        City
    FROM Employees
    WHERE DepartmentId = @DepartmentId;

END;
```

Execute:

```sql
EXEC GetEmployeesByDepartment
    @DepartmentId = 1;
```

The parameter:

```text
@DepartmentId
```

is an **input parameter**.

---

# 8. Multiple Input Parameters

Example:

```sql
CREATE PROCEDURE GetEmployeesByDepartmentAndCity
    @DepartmentId INT,
    @City VARCHAR(100)
AS
BEGIN

    SELECT
        EmployeeId,
        EmployeeName,
        DepartmentId,
        City
    FROM Employees
    WHERE DepartmentId = @DepartmentId
      AND City = @City;

END;
```

Execute:

```sql
EXEC GetEmployeesByDepartmentAndCity
    @DepartmentId = 1,
    @City = 'Chennai';
```

---

# 9. Optional Input Parameter

You can give a parameter a default value.

```sql
CREATE PROCEDURE GetEmployees
    @DepartmentId INT = NULL
AS
BEGIN

    SELECT *
    FROM Employees
    WHERE @DepartmentId IS NULL
       OR DepartmentId = @DepartmentId;

END;
```

Now:

```sql
EXEC GetEmployees;
```

returns all employees.

And:

```sql
EXEC GetEmployees @DepartmentId = 1;
```

returns IT employees.

This pattern is useful for search/filter APIs.

---

# 10. Stored Procedure with JOIN

A real-world procedure could combine multiple tables:

```sql
CREATE PROCEDURE GetEmployeeDetails
AS
BEGIN

    SELECT
        e.EmployeeId,
        e.EmployeeName,
        d.DepartmentName,
        m.ManagerName,
        s.Salary,
        e.City
    FROM Employees e

    LEFT JOIN Departments d
        ON e.DepartmentId = d.DepartmentId

    LEFT JOIN Managers m
        ON e.ManagerId = m.ManagerId

    LEFT JOIN Salaries s
        ON e.EmployeeId = s.EmployeeId;

END;
```

Execute:

```sql
EXEC GetEmployeeDetails;
```

This is much closer to the SQL you will eventually call from your .NET DAL.

---

# 11. Output Parameters

An **output parameter** sends a value from the stored procedure back to the caller.

Example:

```sql
CREATE PROCEDURE GetEmployeeCount
    @EmployeeCount INT OUTPUT
AS
BEGIN

    SELECT @EmployeeCount = COUNT(*)
    FROM Employees;

END;
```

Call it:

```sql
DECLARE @Count INT;

EXEC GetEmployeeCount
    @EmployeeCount = @Count OUTPUT;

SELECT @Count AS TotalEmployees;
```

Important:

```text
Procedure
    ↓
@EmployeeCount
    ↓
OUTPUT
    ↓
Caller
```

---

# 12. Input vs Output Parameter

### Input

Data goes **into** the procedure.

```sql
@DepartmentId INT
```

### Output

Data comes **out** of the procedure.

```sql
@EmployeeCount INT OUTPUT
```

Memory:

```text
INPUT
Caller → Procedure

OUTPUT
Procedure → Caller
```

---

# 13. Input + Output Together

A parameter can be used as both input and output.

```sql
CREATE PROCEDURE IncreaseSalary
    @EmployeeId INT,
    @Salary DECIMAL(10,2) OUTPUT
AS
BEGIN

    UPDATE Salaries
    SET Salary = Salary + 5000
    WHERE EmployeeId = @EmployeeId;

    SELECT @Salary = Salary
    FROM Salaries
    WHERE EmployeeId = @EmployeeId;

END;
```

Execute:

```sql
DECLARE @NewSalary DECIMAL(10,2);

EXEC IncreaseSalary
    @EmployeeId = 101,
    @Salary = @NewSalary OUTPUT;

SELECT @NewSalary AS NewSalary;
```

---

# 14. Return Values

A stored procedure can return an integer status value using `RETURN`.

Example:

```sql
CREATE PROCEDURE CheckEmployee
    @EmployeeId INT
AS
BEGIN

    IF EXISTS
    (
        SELECT 1
        FROM Employees
        WHERE EmployeeId = @EmployeeId
    )
    BEGIN
        RETURN 1;
    END;

    RETURN 0;

END;
```

Call:

```sql
DECLARE @Result INT;

EXEC @Result = CheckEmployee
    @EmployeeId = 101;

SELECT @Result AS Result;
```

Result:

```text
1 → Employee exists
0 → Employee does not exist
```

---

# 15. OUTPUT Parameter vs RETURN Value

This is a common interview question.

### OUTPUT parameter

Can return a value such as:

```text
Employee count
New salary
Generated ID
Status message/value
```

### RETURN

Conventionally used for an integer status/code.

Example:

```text
0 → failure
1 → success
```

Don't confuse:

```sql
RETURN
```

with:

```sql
OUTPUT
```

---

# 16. Stored Procedure — Create Employee

Let's create a practical procedure.

```sql
CREATE PROCEDURE CreateEmployee
    @EmployeeId INT,
    @EmployeeName VARCHAR(100),
    @DepartmentId INT,
    @ManagerId INT = NULL,
    @City VARCHAR(100) = NULL
AS
BEGIN

    INSERT INTO Employees
    (
        EmployeeId,
        EmployeeName,
        DepartmentId,
        ManagerId,
        City
    )
    VALUES
    (
        @EmployeeId,
        @EmployeeName,
        @DepartmentId,
        @ManagerId,
        @City
    );

END;
```

Execute:

```sql
EXEC CreateEmployee
    @EmployeeId = 107,
    @EmployeeName = 'Vijay',
    @DepartmentId = 1,
    @ManagerId = 3,
    @City = 'Chennai';
```

---

# 17. Important: Identity Column

In our Day 16 table, `EmployeeId` was manually assigned.

If your actual table is:

```sql
EmployeeId INT IDENTITY(1,1)
```

then don't insert `EmployeeId` manually.

Instead:

```sql
CREATE PROCEDURE CreateEmployee
    @EmployeeName VARCHAR(100),
    @DepartmentId INT,
    @ManagerId INT = NULL,
    @City VARCHAR(100) = NULL
AS
BEGIN

    INSERT INTO Employees
    (
        EmployeeName,
        DepartmentId,
        ManagerId,
        City
    )
    VALUES
    (
        @EmployeeName,
        @DepartmentId,
        @ManagerId,
        @City
    );

END;
```

The database generates the ID.

---

# 18. Transactions

A **transaction** groups multiple SQL operations into one logical unit.

Imagine creating an employee and salary record:

```text
INSERT Employee
       ↓
INSERT Salary
```

What if the employee insert succeeds but salary insert fails?

You could end up with inconsistent data.

A transaction solves this.

```text
BEGIN TRANSACTION

Operation 1
Operation 2
Operation 3

COMMIT
```

If something goes wrong:

```text
ROLLBACK
```

---

# 19. COMMIT and ROLLBACK

### COMMIT

Makes changes permanent.

```sql
COMMIT;
```

### ROLLBACK

Undo changes made within the transaction.

```sql
ROLLBACK;
```

Memory:

```text
BEGIN TRANSACTION
        ↓
     Operations
        ↓
   ┌────┴────┐
   ↓         ↓
Success    Failure
   ↓         ↓
 COMMIT   ROLLBACK
```

---

# 20. Simple Transaction

```sql
BEGIN TRANSACTION;

UPDATE Salaries
SET Salary = Salary + 5000
WHERE EmployeeId = 101;

UPDATE Salaries
SET Salary = Salary + 5000
WHERE EmployeeId = 102;

COMMIT TRANSACTION;
```

Both changes become permanent.

---

# 21. Transaction with ROLLBACK

```sql
BEGIN TRANSACTION;

UPDATE Salaries
SET Salary = Salary + 5000
WHERE EmployeeId = 101;

ROLLBACK TRANSACTION;
```

The salary update is undone.

---

# 22. TRY/CATCH + Transaction

This is the pattern you should remember.

```sql
BEGIN TRY

    BEGIN TRANSACTION;

    UPDATE Salaries
    SET Salary = Salary + 5000
    WHERE EmployeeId = 101;

    UPDATE Salaries
    SET Salary = Salary + 5000
    WHERE EmployeeId = 102;

    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH;
```

The flow is:

```text
TRY
 ↓
BEGIN TRANSACTION
 ↓
SQL Operations
 ↓
Success?
 ├── YES → COMMIT
 └── NO  → CATCH → ROLLBACK → THROW
```

---

# 23. Why `@@TRANCOUNT`?

`@@TRANCOUNT` tells you the current number of active transactions for the session.

So:

```sql
IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
```

means:

> Only attempt rollback if a transaction is active.

---

# 24. THROW

Inside the CATCH block:

```sql
THROW;
```

rethrows the original error.

Example:

```sql
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK;

    THROW;

END CATCH;
```

This is generally preferable to silently swallowing the database error.

---

# 25. Stored Procedure with TRY/CATCH + Transaction

This is an important real-world example.

```sql
CREATE PROCEDURE CreateEmployeeWithSalary
    @EmployeeId INT,
    @EmployeeName VARCHAR(100),
    @DepartmentId INT,
    @ManagerId INT = NULL,
    @City VARCHAR(100),
    @Salary DECIMAL(10,2)
AS
BEGIN

    SET NOCOUNT ON;

    BEGIN TRY

        BEGIN TRANSACTION;

        INSERT INTO Employees
        (
            EmployeeId,
            EmployeeName,
            DepartmentId,
            ManagerId,
            City
        )
        VALUES
        (
            @EmployeeId,
            @EmployeeName,
            @DepartmentId,
            @ManagerId,
            @City
        );

        INSERT INTO Salaries
        (
            SalaryId,
            EmployeeId,
            Salary,
            EffectiveDate
        )
        VALUES
        (
            @EmployeeId,
            @EmployeeId,
            @Salary,
            GETDATE()
        );

        COMMIT TRANSACTION;

    END TRY

    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH;

END;
```

---

# 26. What is `SET NOCOUNT ON`?

You will see this frequently in stored procedures:

```sql
SET NOCOUNT ON;
```

It prevents SQL Server from sending messages such as:

```text
(1 row affected)
```

for every statement.

It is commonly used in stored procedures to reduce unnecessary row-count messages, especially when application code only needs the actual result/output.

---

# 27. Temporary Tables

A **temporary table** is a temporary table created in `tempdb`.

It starts with:

```text
#
```

Example:

```sql
CREATE TABLE #EmployeeData
(
    EmployeeId INT,
    EmployeeName VARCHAR(100),
    Salary DECIMAL(10,2)
);
```

Insert:

```sql
INSERT INTO #EmployeeData
(
    EmployeeId,
    EmployeeName,
    Salary
)
SELECT
    e.EmployeeId,
    e.EmployeeName,
    s.Salary
FROM Employees e
INNER JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId;
```

Then:

```sql
SELECT *
FROM #EmployeeData;
```

---

# 28. Temporary Table Lifetime

A local temporary table:

```sql
#EmployeeData
```

is available within the current session/scope as applicable and is automatically removed when its relevant scope ends.

You can also explicitly remove it:

```sql
DROP TABLE #EmployeeData;
```

---

# 29. Temporary Table in Stored Procedure

Example:

```sql
CREATE PROCEDURE GetHighSalaryEmployees
AS
BEGIN

    CREATE TABLE #HighSalaryEmployees
    (
        EmployeeId INT,
        EmployeeName VARCHAR(100),
        Salary DECIMAL(10,2)
    );

    INSERT INTO #HighSalaryEmployees
    SELECT
        e.EmployeeId,
        e.EmployeeName,
        s.Salary
    FROM Employees e
    INNER JOIN Salaries s
        ON e.EmployeeId = s.EmployeeId
    WHERE s.Salary > 50000;

    SELECT *
    FROM #HighSalaryEmployees;

END;
```

Execute:

```sql
EXEC GetHighSalaryEmployees;
```

---

# 30. Table Variables

A **table variable** is a variable that holds tabular data.

Syntax:

```sql
DECLARE @EmployeeData TABLE
(
    EmployeeId INT,
    EmployeeName VARCHAR(100),
    Salary DECIMAL(10,2)
);
```

Insert:

```sql
INSERT INTO @EmployeeData
SELECT
    e.EmployeeId,
    e.EmployeeName,
    s.Salary
FROM Employees e
INNER JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId;
```

Query:

```sql
SELECT *
FROM @EmployeeData;
```

---

# 31. Temporary Table vs Table Variable

### Temporary Table

```sql
CREATE TABLE #Employees
```

### Table Variable

```sql
DECLARE @Employees TABLE
```

General learning guideline:

```text
Temporary Table
→ useful for intermediate result sets,
  more complex processing, indexing, etc.

Table Variable
→ useful for smaller/simple temporary datasets
  and limited-scope intermediate work.
```

The exact performance trade-offs depend on SQL Server version, data volume, indexes, query shape, and workload, so don't memorize "table variable is always faster/slower."

---

# 32. CTE — Common Table Expression

CTE stands for:

**Common Table Expression**

A CTE creates a temporary named result set that can be referenced by the immediately following SQL statement.

Syntax:

```sql
WITH EmployeeCTE AS
(
    SELECT
        EmployeeId,
        EmployeeName,
        DepartmentId
    FROM Employees
)
SELECT *
FROM EmployeeCTE;
```

Important:

A CTE is **not a permanent table**.

---

# 33. Why Use CTE?

CTEs make complex queries:

* Easier to read
* Easier to organize
* Easier to maintain
* Useful for multi-step query logic
* Useful for recursive/hierarchical queries

---

# 34. Simple CTE Example

Find employees with salary greater than 50,000.

```sql
WITH HighSalaryEmployees AS
(
    SELECT
        e.EmployeeId,
        e.EmployeeName,
        s.Salary
    FROM Employees e
    INNER JOIN Salaries s
        ON e.EmployeeId = s.EmployeeId
    WHERE s.Salary > 50000
)
SELECT *
FROM HighSalaryEmployees;
```

Think:

```text
WITH HighSalaryEmployees AS
(
    Step 1
)
Step 2 → SELECT from it
```

---

# 35. CTE with Department

```sql
WITH EmployeeDetails AS
(
    SELECT
        e.EmployeeId,
        e.EmployeeName,
        d.DepartmentName
    FROM Employees e
    INNER JOIN Departments d
        ON e.DepartmentId = d.DepartmentId
)
SELECT *
FROM EmployeeDetails
WHERE DepartmentName = 'IT';
```

---

# 36. CTE with GROUP BY

```sql
WITH DepartmentSalary AS
(
    SELECT
        d.DepartmentName,
        SUM(s.Salary) AS TotalSalary
    FROM Departments d
    INNER JOIN Employees e
        ON d.DepartmentId = e.DepartmentId
    INNER JOIN Salaries s
        ON e.EmployeeId = s.EmployeeId
    GROUP BY d.DepartmentName
)
SELECT *
FROM DepartmentSalary
WHERE TotalSalary > 100000;
```

This makes a complicated query much easier to understand.

---

# 37. CTE vs Temporary Table

| CTE                                | Temporary Table                                     |
| ---------------------------------- | --------------------------------------------------- |
| Defined using `WITH`               | Created using `CREATE TABLE #...`                   |
| Query expression                   | Actual temporary table                              |
| Exists for the following statement | Can be used across multiple statements within scope |
| Good for query organization        | Good for multi-step temporary data processing       |
| Useful for recursive queries       | Useful for intermediate data and indexing           |

---

# 38. Recursive CTE

A **recursive CTE** references itself.

This is useful for hierarchical data such as:

```text
CEO
 ↓
Manager
 ↓
Team Lead
 ↓
Employee
```

Example employee hierarchy:

```text
EmployeeId | EmployeeName | ManagerId
-----------|--------------|----------
1          | Suresh       | NULL
2          | Arun         | 1
3          | Priya        | 1
4          | Kumar        | 2
5          | Ravi         | 2
6          | Divya        | 3
```

Here:

```text
Suresh
├── Arun
│   ├── Kumar
│   └── Ravi
│
└── Priya
    └── Divya
```

---

# 39. Hierarchical Employee Table

For a true self-referencing hierarchy, we can use:

```sql
CREATE TABLE EmployeeHierarchy
(
    EmployeeId INT PRIMARY KEY,
    EmployeeName VARCHAR(100) NOT NULL,
    ManagerId INT NULL,

    CONSTRAINT FK_EmployeeHierarchy_Manager
        FOREIGN KEY (ManagerId)
        REFERENCES EmployeeHierarchy(EmployeeId)
);
```

Insert:

```sql
INSERT INTO EmployeeHierarchy
(
    EmployeeId,
    EmployeeName,
    ManagerId
)
VALUES
(1, 'Suresh', NULL),
(2, 'Arun', 1),
(3, 'Priya', 1),
(4, 'Kumar', 2),
(5, 'Ravi', 2),
(6, 'Divya', 3);
```

---

# 40. Recursive CTE Structure

A recursive CTE normally has two parts:

```text
Anchor member
      ↓
Recursive member
      ↓
Repeat
```

Syntax:

```sql
WITH EmployeeHierarchy AS
(
    -- Anchor
    SELECT ...

    UNION ALL

    -- Recursive
    SELECT ...
    FROM EmployeeHierarchy
    ...
)
SELECT *
FROM EmployeeHierarchy;
```

---

# 41. Recursive CTE Example

Let's get the complete employee hierarchy.

```sql
WITH EmployeeCTE AS
(
    -- Anchor: top-level employees
    SELECT
        EmployeeId,
        EmployeeName,
        ManagerId,
        0 AS Level
    FROM EmployeeHierarchy
    WHERE ManagerId IS NULL

    UNION ALL

    -- Recursive member
    SELECT
        e.EmployeeId,
        e.EmployeeName,
        e.ManagerId,
        c.Level + 1
    FROM EmployeeHierarchy e
    INNER JOIN EmployeeCTE c
        ON e.ManagerId = c.EmployeeId
)
SELECT
    EmployeeId,
    EmployeeName,
    ManagerId,
    Level
FROM EmployeeCTE
ORDER BY Level, EmployeeId;
```

Result:

```text
EmployeeId | EmployeeName | ManagerId | Level
-----------|--------------|-----------|------
1          | Suresh       | NULL      | 0
2          | Arun         | 1         | 1
3          | Priya        | 1         | 1
4          | Kumar        | 2         | 2
5          | Ravi         | 2         | 2
6          | Divya        | 3         | 2
```

---

# 42. Understanding Recursive CTE

Let's understand the query.

### Step 1 — Anchor

```sql
SELECT
    EmployeeId,
    EmployeeName,
    ManagerId,
    0 AS Level
FROM EmployeeHierarchy
WHERE ManagerId IS NULL
```

Finds:

```text
Suresh
```

because he has no manager.

---

### Step 2 — Recursive Part

```sql
SELECT
    e.EmployeeId,
    e.EmployeeName,
    e.ManagerId,
    c.Level + 1
FROM EmployeeHierarchy e
INNER JOIN EmployeeCTE c
    ON e.ManagerId = c.EmployeeId
```

Finds employees whose:

```text
ManagerId = previous EmployeeId
```

So:

```text
Suresh
 ↓
Arun
Priya
```

Then:

```text
Arun
 ↓
Kumar
Ravi
```

And:

```text
Priya
 ↓
Divya
```

---

# 43. Hierarchy with Level

The `Level` column helps us understand the hierarchy.

```text
Level 0 → Top-level
Level 1 → Direct reports
Level 2 → Reports of reports
Level 3 → Next level
```

Example:

```text
Suresh
Level 0

  ├── Arun
  │   Level 1
  │
  │   ├── Kumar
  │   │   Level 2
  │   │
  │   └── Ravi
  │       Level 2
  │
  └── Priya
      Level 1

      └── Divya
          Level 2
```

---

# 44. Recursive CTE — Maximum Recursion

SQL Server has a default recursion limit for recursive CTEs.

If you need a different limit:

```sql
OPTION (MAXRECURSION 100);
```

Example:

```sql
WITH EmployeeCTE AS
(
    ...
)
SELECT *
FROM EmployeeCTE
OPTION (MAXRECURSION 100);
```

For unlimited recursion:

```sql
OPTION (MAXRECURSION 0);
```

Use unlimited recursion carefully; bad/cyclic hierarchical data can cause problems.

---

# 45. Stored Procedure + CTE

You can combine these concepts.

```sql
CREATE PROCEDURE GetHighSalaryEmployees
    @MinimumSalary DECIMAL(10,2)
AS
BEGIN

    SET NOCOUNT ON;

    WITH HighSalaryEmployees AS
    (
        SELECT
            e.EmployeeId,
            e.EmployeeName,
            s.Salary
        FROM Employees e
        INNER JOIN Salaries s
            ON e.EmployeeId = s.EmployeeId
        WHERE s.Salary >= @MinimumSalary
    )
    SELECT *
    FROM HighSalaryEmployees
    ORDER BY Salary DESC;

END;
```

Execute:

```sql
EXEC GetHighSalaryEmployees
    @MinimumSalary = 50000;
```

This is a very useful real-world pattern.

---

# 46. Stored Procedure + Transaction + CATCH

This is another pattern worth memorizing:

```sql
CREATE PROCEDURE UpdateEmployeeSalary
    @EmployeeId INT,
    @NewSalary DECIMAL(10,2)
AS
BEGIN

    SET NOCOUNT ON;

    BEGIN TRY

        BEGIN TRANSACTION;

        UPDATE Salaries
        SET Salary = @NewSalary
        WHERE EmployeeId = @EmployeeId;

        IF @@ROWCOUNT = 0
        BEGIN
            THROW 50001, 'Employee salary record not found.', 1;
        END;

        COMMIT TRANSACTION;

    END TRY

    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH;

END;
```

Execute:

```sql
EXEC UpdateEmployeeSalary
    @EmployeeId = 101,
    @NewSalary = 65000;
```

---

# 47. Complete Day 17 SQL Flow

```text
                     STORED PROCEDURE
                           │
             ┌─────────────┼─────────────┐
             ↓             ↓             ↓
          INPUT         OUTPUT        RETURN
        PARAMETERS     PARAMETERS       VALUE
             │             │             │
             └─────────────┼─────────────┘
                           ↓
                       SQL LOGIC
                           │
                    ┌──────┴──────┐
                    ↓             ↓
                 TRY/CATCH     TRANSACTION
                                  │
                           ┌──────┴──────┐
                           ↓             ↓
                        COMMIT        ROLLBACK
```

And:

```text
                       CTE
                        │
               ┌────────┴────────┐
               ↓                 ↓
          Normal CTE        Recursive CTE
                                 │
                                 ↓
                         Hierarchical Data
                                 │
                                 ↓
                         Employee → Manager
```

---

# 48. Day 17 Practice — 20 Queries/Exercises

You should practice these yourself.

### Stored Procedure

**1.** Create a procedure to return all employees.

```sql
CREATE PROCEDURE GetAllEmployees
AS
BEGIN
    SELECT *
    FROM Employees;
END;
```

**2.** Create a procedure that accepts `@DepartmentId`.

**3.** Create a procedure that accepts department and city.

**4.** Alter the procedure to include salary.

**5.** Create a procedure that returns employee count through an OUTPUT parameter.

**6.** Create a procedure that returns `1` if employee exists and `0` otherwise.

**7.** Create a procedure to insert an employee.

**8.** Create a procedure to update employee salary.

**9.** Create a procedure to delete an employee.

**10.** Create a procedure using `TRY/CATCH`.

---

### Transactions

**11.** Increase salary for two employees inside one transaction.

**12.** Intentionally generate an error and verify that `ROLLBACK` occurs.

**13.** Create employee + salary in one transaction.

**14.** Use `@@TRANCOUNT` before rollback.

---

### Temporary Data

**15.** Create `#HighSalaryEmployees`.

**16.** Insert employees earning above 50K into it.

**17.** Create `@EmployeeData` table variable.

**18.** Compare the result of the temporary table and table variable.

---

### CTE

**19.** Create a CTE that returns employees earning above average salary.

**20.** Create a CTE that returns department-wise salary totals.

**21.** Create a recursive CTE for employee hierarchy.

**22.** Add a `Level` column to the recursive CTE.

**23.** Find all employees under a particular manager using a recursive CTE.

---

# 49. Day 17 Interview Questions

### Stored Procedures

**1. What is a stored procedure?**

A reusable named SQL program stored in SQL Server.

**2. How do you create one?**

```sql
CREATE PROCEDURE
```

**3. How do you modify one?**

```sql
ALTER PROCEDURE
```

**4. How do you execute one?**

```sql
EXEC
```

**5. Input vs output parameter?**

```text
Input
Caller → Procedure

Output
Procedure → Caller
```

**6. OUTPUT parameter vs RETURN value?**

`OUTPUT` can return a value through a parameter; `RETURN` returns an integer status code from the procedure.

**7. What is a transaction?**

A group of operations treated as one logical unit.

**8. COMMIT vs ROLLBACK?**

```text
COMMIT
→ make transaction changes permanent

ROLLBACK
→ undo transaction changes
```

**9. Why use TRY/CATCH?**

To handle SQL errors and perform appropriate rollback/error propagation.

**10. Why use `SET NOCOUNT ON`?**

To suppress row-count messages from individual statements.

---

# 50. CTE Interview Questions

**11. What is a CTE?**

A named temporary query result defined using `WITH` for the following statement.

**12. Does a CTE permanently store data?**

No.

**13. What is a recursive CTE?**

A CTE whose recursive member references the CTE itself.

**14. What is a recursive CTE useful for?**

Hierarchical data such as:

```text
Employee
   ↓
Manager
   ↓
Team
```

**15. What are the two main parts of a recursive CTE?**

```text
Anchor member
+
Recursive member
```

**16. What does `UNION ALL` do in a recursive CTE?**

It combines the anchor and recursive result sets while allowing recursive expansion.

---

# 51. Day 17 Cheat Sheet

### Stored Procedure

```sql
CREATE PROCEDURE
```

Create.

```sql
ALTER PROCEDURE
```

Modify.

```sql
EXEC
```

Execute.

---

### Parameters

```sql
@EmployeeId INT
```

Input.

```sql
@Count INT OUTPUT
```

Output.

```sql
RETURN 1;
```

Return status.

---

### Transaction

```sql
BEGIN TRANSACTION;

-- SQL operations

COMMIT TRANSACTION;
```

or:

```sql
ROLLBACK TRANSACTION;
```

---

### Error Handling

```sql
BEGIN TRY

    -- SQL

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK;

    THROW;

END CATCH;
```

---

### Temporary Table

```sql
CREATE TABLE #Employees
(
    EmployeeId INT,
    EmployeeName VARCHAR(100)
);
```

---

### Table Variable

```sql
DECLARE @Employees TABLE
(
    EmployeeId INT,
    EmployeeName VARCHAR(100)
);
```

---

### CTE

```sql
WITH EmployeeCTE AS
(
    SELECT *
    FROM Employees
)
SELECT *
FROM EmployeeCTE;
```

---

### Recursive CTE

```sql
WITH EmployeeCTE AS
(
    -- Anchor
    SELECT ...

    UNION ALL

    -- Recursive
    SELECT ...
    FROM EmployeeCTE ...
)
SELECT *
FROM EmployeeCTE;
```

---

# 52. Day 17 — Final Mental Model

```text
SQL SERVER
│
├── STORED PROCEDURES
│   │
│   ├── CREATE
│   ├── ALTER
│   ├── EXEC
│   │
│   ├── Input Parameters
│   ├── Output Parameters
│   ├── Return Values
│   │
│   ├── Transactions
│   │   ├── BEGIN
│   │   ├── COMMIT
│   │   └── ROLLBACK
│   │
│   ├── TRY/CATCH
│   ├── Temporary Tables
│   └── Table Variables
│
└── CTE
    │
    ├── WITH
    ├── Normal CTE
    │
    └── Recursive CTE
        │
        └── Hierarchical Data
            │
            └── Employee
                 ↓
               Manager
                 ↓
               Team
```

## Connection to your .NET roadmap

You now have:

```text
Day 15
SQL Fundamentals
       ↓
Day 16
JOINs
       ↓
Day 17
Stored Procedures + CTEs
       ↓
Day 18
ADO.NET
       ↓
Controller
       ↓
BAL / Service
       ↓
DAL / Repository
       ↓
SQL Server
```

---

# SQL UNION and UNION ALL

`UNION` and `UNION ALL` are used to **combine the result of two or more SELECT queries into one result set**.

---

## 1. UNION

`UNION` combines results and **removes duplicate rows**.

### Syntax

```sql
SELECT column1, column2
FROM Table1

UNION

SELECT column1, column2
FROM Table2;
```

### Example

Suppose we have:

### Employees

| EmployeeId | EmployeeName | City      |
| ---------: | ------------ | --------- |
|          1 | Arun         | Chennai   |
|          2 | Priya        | Bangalore |
|          3 | Kumar        | Chennai   |

### Managers

| ManagerId | ManagerName | City      |
| --------: | ----------- | --------- |
|       101 | Ravi        | Chennai   |
|       102 | Priya       | Bangalore |

Now:

```sql
SELECT EmployeeName AS Name, City
FROM Employees

UNION

SELECT ManagerName AS Name, City
FROM Managers;
```

### Result

```text
Arun    Chennai
Priya   Bangalore
Kumar   Chennai
Ravi    Chennai
```

`Priya, Bangalore` appeared in both queries, but `UNION` returns it **only once**.

---

# 2. UNION ALL

`UNION ALL` combines results but **does NOT remove duplicates**.

```sql
SELECT EmployeeName AS Name, City
FROM Employees

UNION ALL

SELECT ManagerName AS Name, City
FROM Managers;
```

### Result

```text
Arun    Chennai
Priya   Bangalore
Kumar   Chennai
Ravi    Chennai
Priya   Bangalore
```

Here `Priya, Bangalore` appears **twice** because duplicates are preserved.

---

# 3. UNION vs UNION ALL

The most important difference:

```text
UNION
 ↓
Combines results
 ↓
Removes duplicates
 ↓
May require extra work for duplicate elimination
```

```text
UNION ALL
 ↓
Combines results
 ↓
Keeps duplicates
 ↓
Usually more efficient
```

### Simple memory trick

> **UNION = Unique results**

> **UNION ALL = All results**

---

# 4. Important Rules for UNION

The SELECT statements must have compatible structures.

### Rule 1 — Same number of columns

❌ Not valid:

```sql
SELECT EmployeeId, EmployeeName
FROM Employees

UNION

SELECT ManagerId
FROM Managers;
```

First query has **2 columns**, second has **1 column**.

---

### Rule 2 — Corresponding columns must have compatible data types

Valid:

```sql
SELECT EmployeeId, EmployeeName
FROM Employees

UNION

SELECT ManagerId, ManagerName
FROM Managers;
```

Both return:

```text
INT, VARCHAR
```

---

### Rule 3 — Column names come from the first SELECT

```sql
SELECT EmployeeName AS PersonName
FROM Employees

UNION

SELECT ManagerName AS ManagerName
FROM Managers;
```

The resulting column name will be:

```text
PersonName
```

because the first SELECT determines the result column names.

---

# 5. UNION with WHERE

You can use filtering inside each query.

```sql
SELECT EmployeeName AS Name
FROM Employees
WHERE City = 'Chennai'

UNION

SELECT ManagerName AS Name
FROM Managers
WHERE City = 'Chennai';
```

This gives people from Chennai from both tables, with duplicates removed.

---

# 6. UNION ALL with WHERE

```sql
SELECT EmployeeName AS Name
FROM Employees
WHERE City = 'Chennai'

UNION ALL

SELECT ManagerName AS Name
FROM Managers
WHERE City = 'Chennai';
```

All matching records are returned, including duplicates.

---

# 7. UNION is NOT JOIN

This is very important for interviews.

### JOIN

JOIN combines **columns from related tables**.

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName
FROM Employees e
INNER JOIN Departments d
    ON e.DepartmentId = d.DepartmentId;
```

Result:

```text
EmployeeName    DepartmentName
------------    --------------
Arun            IT
Priya           HR
Kumar           Finance
```

You are adding information **side-by-side**.

```text
Employee       +       Department
   ↓                       ↓
Name                    DepartmentName
        → same row ←
```

### UNION

UNION combines **rows from different SELECT results**.

```sql
SELECT EmployeeName
FROM Employees

UNION

SELECT ManagerName
FROM Managers;
```

Result:

```text
Arun
Priya
Kumar
Ravi
```

Think:

```text
JOIN
→ columns side-by-side

UNION
→ rows one below another
```

---

# 8. UNION with ORDER BY

You normally put `ORDER BY` at the **end** of the combined query.

```sql
SELECT EmployeeName AS Name
FROM Employees

UNION

SELECT ManagerName AS Name
FROM Managers

ORDER BY Name;
```

Result:

```text
Arun
Kumar
Priya
Ravi
```

---

# 9. Real-World Example

Suppose your application has two tables:

```text
CurrentEmployees
FormerEmployees
```

You want a list of **all unique people**:

```sql
SELECT EmployeeName
FROM CurrentEmployees

UNION

SELECT EmployeeName
FROM FormerEmployees;
```

If the same person exists in both tables, they appear once.

If you want every record:

```sql
SELECT EmployeeName
FROM CurrentEmployees

UNION ALL

SELECT EmployeeName
FROM FormerEmployees;
```

Then duplicates remain.

---

# 10. When to use UNION vs UNION ALL

### Use UNION when:

You specifically need **duplicate rows removed**.

```sql
SELECT City FROM Employees
UNION
SELECT City FROM Managers;
```

If both tables contain:

```text
Chennai
Bangalore
Chennai
```

the final result contains each identical result row once.

### Use UNION ALL when:

You want **all rows** and duplicates are meaningful or should not be removed.

```sql
SELECT City FROM Employees
UNION ALL
SELECT City FROM Managers;
```

For example, if you are combining current and historical transaction records, removing duplicates could change the meaning of the data.

---

# 11. Performance Difference

Generally:

```text
UNION
  ↓
Combine rows
  ↓
Find/remove duplicates
  ↓
Return result
```

Whereas:

```text
UNION ALL
  ↓
Combine rows
  ↓
Return result
```

Therefore, **`UNION ALL` is generally faster than `UNION`** because it doesn't need duplicate elimination.

But don't choose `UNION ALL` merely for speed if duplicate removal is part of the required result.

---

# 12. Important Interview Question

### What is the difference between UNION and UNION ALL?

**Answer:**

> `UNION` combines the result sets of multiple SELECT statements and removes duplicate rows. `UNION ALL` combines the result sets and keeps duplicates. `UNION ALL` is generally faster because it doesn't perform duplicate elimination.

---

## Quick Comparison

```text
             UNION              UNION ALL
             ─────              ─────────
Combine       Yes                  Yes
duplicates    Removed              Kept
performance   Generally slower      Generally faster
use when      Need unique rows      Need all rows
```

### Final memory

```text
JOIN
  → combines columns

UNION
  → combines rows + removes duplicates

UNION ALL
  → combines rows + keeps duplicates
```



