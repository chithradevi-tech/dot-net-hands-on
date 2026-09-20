# Day 18 — SQL Advanced + Performance

Day 18 is very important because this is where you move from **basic SQL writing** to understanding **how SQL Server executes, stores, and optimizes data**.

We will cover:

```text
DAY 18
│
├── Indexes
│   ├── Clustered
│   ├── Non-clustered
│   └── Composite
│
├── Execution Plans
├── Query Optimization
│
├── Transactions
│   ├── ACID
│   ├── Isolation Levels
│   └── Deadlocks
│
├── Database Design
│   ├── Normalization
│   ├── 1NF
│   ├── 2NF
│   ├── 3NF
│   └── Denormalization
│
└── Views & Functions
    ├── Views
    ├── Scalar Functions
    └── Table-Valued Functions
```

---

# 1. Indexes

An **index** helps SQL Server find rows more efficiently.

Think about a book.

Without an index:

```text
Book
Page 1
Page 2
Page 3
...
Page 500
```

If you want "SQL Server", you may need to search many pages.

With an index:

```text
SQL Server → Page 245
C#         → Page 120
ASP.NET    → Page 350
```

You can locate information much faster.

The same basic idea applies to database indexes.

---

# 2. Why Do We Need Indexes?

Suppose:

```sql
SELECT *
FROM Employees
WHERE Email = 'arun@gmail.com';
```

If `Employees` contains millions of rows and there is no suitable index, SQL Server may need to inspect many rows.

With an appropriate index:

```sql
CREATE INDEX IX_Employees_Email
ON Employees(Email);
```

SQL Server can often locate matching rows much more efficiently.

---

# 3. Index Example

Create:

```sql
CREATE INDEX IX_Employees_City
ON Employees(City);
```

Query:

```sql
SELECT *
FROM Employees
WHERE City = 'Chennai';
```

The index can help SQL Server locate rows for the search predicate.

But remember:

> An index does not automatically make every query faster.

Indexes also consume storage and add work when indexed data is inserted, updated, or deleted.

---

# 4. Clustered Index

A **clustered index** determines the physical/logical organization of the table's rows according to the clustered index key.

For SQL Server, a table can have **at most one clustered index** because the rows can only be organized one way.

Example:

```sql
CREATE CLUSTERED INDEX IX_Employees_EmployeeId
ON Employees(EmployeeId);
```

Conceptually:

```text
EmployeeId
   ↓
1
2
3
4
5
6
...
```

A clustered index is often created on the primary key by default, depending on how the primary key was defined and whether another clustered index already exists.

Example:

```sql
CREATE TABLE Employees
(
    EmployeeId INT
        CONSTRAINT PK_Employees PRIMARY KEY CLUSTERED,

    EmployeeName VARCHAR(100),
    Salary DECIMAL(10,2)
);
```

---

# 5. Non-Clustered Index

A **non-clustered index** is a separate index structure containing indexed key values and row locators.

Example:

```sql
CREATE NONCLUSTERED INDEX IX_Employees_Name
ON Employees(EmployeeName);
```

You can have multiple non-clustered indexes on a table.

For example:

```sql
CREATE INDEX IX_Employees_Name
ON Employees(EmployeeName);

CREATE INDEX IX_Employees_City
ON Employees(City);

CREATE INDEX IX_Employees_Salary
ON Employees(Salary);
```

Conceptually:

```text
Employees table
      │
      ├── Clustered index
      │
      ├── Non-clustered index → Name
      │
      ├── Non-clustered index → City
      │
      └── Non-clustered index → Salary
```

---

# 6. Clustered vs Non-Clustered

| Feature              | Clustered            | Non-Clustered            |
| -------------------- | -------------------- | ------------------------ |
| Number per table     | Maximum 1            | Multiple                 |
| Organizes table rows | Yes                  | Separate index structure |
| Common use           | Primary/range access | Search/filter columns    |
| Storage              | Table organization   | Additional storage       |
| Example              | `EmployeeId`         | `Email`, `City`          |

### Easy memory

```text
Clustered
→ Table is organized around this index

Non-clustered
→ Separate index points toward table data
```

---

# 7. Composite Index

A **composite index** contains multiple columns.

Example:

```sql
CREATE INDEX IX_Employees_Department_City
ON Employees(DepartmentId, City);
```

This is a two-column index.

```text
DepartmentId + City
```

It can be useful for queries such as:

```sql
SELECT *
FROM Employees
WHERE DepartmentId = 10
  AND City = 'Chennai';
```

---

# 8. Column Order Matters

Consider:

```sql
CREATE INDEX IX_Employees_Department_City
ON Employees(DepartmentId, City);
```

The order is:

```text
1. DepartmentId
2. City
```

This is related to the **leftmost-prefix principle**.

The index is generally most naturally useful when queries use the leading column(s).

For example:

```sql
WHERE DepartmentId = 10
```

can make good use of the leading `DepartmentId`.

And:

```sql
WHERE DepartmentId = 10
AND City = 'Chennai'
```

can use both columns.

But a query only on:

```sql
WHERE City = 'Chennai'
```

does not generally get the same benefit from this index as a query using the leading `DepartmentId`.

---

# 9. Included Columns

SQL Server also supports included columns.

Example:

```sql
CREATE INDEX IX_Employees_Department
ON Employees(DepartmentId)
INCLUDE (EmployeeName, Salary);
```

Here:

```text
Index key:
DepartmentId

Included:
EmployeeName
Salary
```

Included columns can help a query obtain additional selected data from the index without having to access the base table separately in some execution plans.

---

# 10. Too Many Indexes?

Indexes are useful, but don't create indexes on every column.

Suppose:

```text
Employees
│
├── EmployeeId
├── Name
├── Email
├── City
├── Department
├── Salary
├── Phone
└── JoiningDate
```

Creating indexes on every column may increase:

* storage usage
* INSERT cost
* UPDATE cost
* DELETE cost
* index maintenance

Therefore:

> Create indexes based on actual query and workload patterns.

---

# 11. Execution Plans

An **execution plan** shows how SQL Server intends to execute a query, or how it actually executed it.

Example:

```sql
SELECT *
FROM Employees
WHERE DepartmentId = 10;
```

SQL Server may choose operations such as:

```text
Index Seek
    ↓
Key Lookup
    ↓
Result
```

or:

```text
Table Scan
    ↓
Filter
    ↓
Result
```

The execution plan helps you understand **why a query is slow**.

---

# 12. Actual vs Estimated Execution Plan

### Estimated Execution Plan

SQL Server shows the plan it expects to use **without executing the query**.

In SSMS:

```text
Query
 → Display Estimated Execution Plan
```

Shortcut:

```text
Ctrl + L
```

### Actual Execution Plan

The query executes and SQL Server displays information about the plan that actually ran.

In SSMS:

```text
Query
 → Include Actual Execution Plan
```

Shortcut:

```text
Ctrl + M
```

---

# 13. Important Execution Plan Operators

You will commonly encounter:

### Table Scan

SQL Server scans the table.

```text
Table
 ↓
Scan
```

This can be expensive for large tables.

But a scan is **not automatically bad**. For a small table, scanning the entire table may be cheaper than using an index.

---

### Index Seek

SQL Server uses an index to navigate to relevant rows.

```text
Index
 ↓
Seek
 ↓
Required rows
```

Often desirable for selective lookups.

---

### Index Scan

SQL Server scans the index rather than seeking directly to a small range.

---

### Key Lookup

SQL Server finds rows through a non-clustered index and then retrieves additional columns from the clustered/base data.

Conceptually:

```text
Non-clustered Index
       ↓
Key Lookup
       ↓
Base table
```

Sometimes a covering index can eliminate the lookup.

---

### Sort

SQL Server sorts rows.

```text
Rows
 ↓
Sort
 ↓
Ordered result
```

Sorting can become expensive for large result sets.

---

### Hash Match

Often used for joins or aggregation, especially when appropriate indexes are unavailable or for larger unsorted inputs.

---

### Nested Loops

A join algorithm often useful when one input is relatively small and the other side has an efficient lookup path.

---

# 14. Query Optimization

Query optimization means improving a query so that SQL Server can execute it efficiently while returning the required result.

---

## Example 1 — Avoid SELECT *

Instead of:

```sql
SELECT *
FROM Employees;
```

Prefer:

```sql
SELECT
    EmployeeId,
    EmployeeName,
    DepartmentId,
    Salary
FROM Employees;
```

Benefits can include:

* less data transferred
* less unnecessary work
* clearer API/data-access contracts
* potentially better opportunities for covering indexes

---

# 15. Filter Data

Instead of retrieving everything:

```sql
SELECT *
FROM Employees;
```

Use:

```sql
SELECT
    EmployeeId,
    EmployeeName,
    Salary
FROM Employees
WHERE DepartmentId = 10;
```

---

# 16. Index Search Columns

If this query is frequently executed:

```sql
SELECT EmployeeId, EmployeeName
FROM Employees
WHERE Email = @Email;
```

An index may be appropriate:

```sql
CREATE INDEX IX_Employees_Email
ON Employees(Email);
```

If `Email` is logically unique, a unique index may be appropriate:

```sql
CREATE UNIQUE INDEX UX_Employees_Email
ON Employees(Email);
```

---

# 17. Avoid Functions on Indexed Columns When Possible

Suppose:

```sql
SELECT *
FROM Employees
WHERE YEAR(JoiningDate) = 2026;
```

Depending on the query and index, applying a function to the column can make efficient index seeking harder.

A range predicate is often preferable:

```sql
SELECT *
FROM Employees
WHERE JoiningDate >= '20260101'
  AND JoiningDate <  '20270101';
```

The exact best form depends on data types, indexes, and workload.

---

# 18. Avoid Unnecessary Data

Bad:

```sql
SELECT *
FROM Employees;
```

Better:

```sql
SELECT
    EmployeeId,
    EmployeeName
FROM Employees;
```

Especially important when your API only needs a few fields.

---

# 19. Use Parameters

Avoid constructing SQL by concatenating user input.

Bad:

```csharp
string sql =
    "SELECT * FROM Employees WHERE Name = '" + name + "'";
```

Better:

```csharp
string sql =
    "SELECT * FROM Employees WHERE Name = @Name";
```

Then pass `@Name` as a SQL parameter through ADO.NET.

This helps prevent SQL injection and allows the database engine to handle values separately from SQL text.

---

# 20. Transactions

A **transaction** is a group of database operations treated as one logical unit.

Example:

```text
Transfer ₹5,000
     │
     ├── Remove ₹5,000 from Account A
     │
     └── Add ₹5,000 to Account B
```

Both should succeed together.

If the second operation fails:

```text
Rollback
```

so the first operation is also undone.

---

# 21. Basic Transaction

```sql
BEGIN TRANSACTION;

UPDATE Accounts
SET Balance = Balance - 5000
WHERE AccountId = 1;

UPDATE Accounts
SET Balance = Balance + 5000
WHERE AccountId = 2;

COMMIT TRANSACTION;
```

If something goes wrong:

```sql
ROLLBACK TRANSACTION;
```

---

# 22. Transaction with TRY/CATCH

A common SQL Server pattern:

```sql
BEGIN TRY

    BEGIN TRANSACTION;

    UPDATE Accounts
    SET Balance = Balance - 5000
    WHERE AccountId = 1;

    UPDATE Accounts
    SET Balance = Balance + 5000
    WHERE AccountId = 2;

    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH;
```

---

# 23. ACID

Transactions are commonly explained using **ACID**.

```text
A → Atomicity
C → Consistency
I → Isolation
D → Durability
```

---

## A — Atomicity

All operations succeed or the transaction is rolled back.

```text
Operation 1 ✓
Operation 2 ✓
Operation 3 ✗
       ↓
ROLLBACK
```

---

## C — Consistency

A committed transaction must leave the database satisfying its defined integrity rules and constraints.

Example:

```text
Salary > 0
```

If you have:

```sql
CHECK (Salary > 0)
```

the database should not be left with an invalid committed row because of that transaction.

---

## I — Isolation

Concurrent transactions should not improperly interfere with each other's intermediate states.

SQL Server provides different isolation levels.

---

## D — Durability

After a successful commit, the committed data is intended to survive subsequent failures according to the database's durability mechanisms.

```text
COMMIT
  ↓
Data is committed
```

### Memory

```text
ACID

A → All or nothing
C → Valid state
I → Transactions isolated
D → Committed data persists
```

---

# 24. Isolation Levels

SQL Server supports:

```text
READ UNCOMMITTED
READ COMMITTED
REPEATABLE READ
SNAPSHOT
SERIALIZABLE
```

---

# 25. READ UNCOMMITTED

Allows reading data that another transaction has modified but not committed.

This can result in a:

### Dirty Read

Transaction A:

```sql
UPDATE Employees
SET Salary = 100000
WHERE EmployeeId = 1;
```

but hasn't committed.

Transaction B may read:

```text
Salary = 100000
```

If Transaction A rolls back, that value was never committed.

---

# 26. READ COMMITTED

This is SQL Server's traditional default isolation level.

A transaction generally cannot read another transaction's uncommitted changes.

Concept:

```text
Uncommitted data
       ↓
Not normally readable
```

---

# 27. REPEATABLE READ

If a transaction reads a row, another transaction cannot modify that row in a way that causes the first transaction's repeated read to return a different committed value while the first transaction remains active.

It provides stronger protection than READ COMMITTED but can increase locking.

---

# 28. SNAPSHOT

Uses row versioning to provide transaction-level consistent reads without requiring shared locks in the same way as lock-based isolation.

You may need to enable the relevant database option:

```sql
ALTER DATABASE EmployeeDB
SET ALLOW_SNAPSHOT_ISOLATION ON;
```

Then:

```sql
SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
```

---

# 29. SERIALIZABLE

Provides the strongest standard isolation level in SQL Server's traditional lock-based model.

It prevents concurrent activity from creating certain phantom rows within the transaction's protected range.

It can significantly increase blocking.

---

# 30. Isolation Level Comparison

| Isolation        | Dirty Read | Non-repeatable Read | Phantom Read                |
| ---------------- | ---------- | ------------------- | --------------------------- |
| READ UNCOMMITTED | Possible   | Possible            | Possible                    |
| READ COMMITTED   | Prevented  | Possible            | Possible                    |
| REPEATABLE READ  | Prevented  | Prevented           | Possible                    |
| SNAPSHOT         | Prevented  | Prevented           | Prevented for snapshot view |
| SERIALIZABLE     | Prevented  | Prevented           | Prevented                   |

Think:

```text
More isolation
     ↓
More consistency protection
     ↓
Potentially more concurrency cost
```

---

# 31. Deadlocks

A **deadlock** happens when two or more transactions wait for resources held by each other.

Example:

### Transaction A

```text
Locks Employee 1
     ↓
Wants Employee 2
```

### Transaction B

```text
Locks Employee 2
     ↓
Wants Employee 1
```

Now:

```text
Transaction A
    ↓
waiting for B

Transaction B
    ↓
waiting for A
```

Neither can continue.

---

# 32. Deadlock Example

Transaction A:

```sql
BEGIN TRANSACTION;

UPDATE Employees
SET Salary = Salary + 1000
WHERE EmployeeId = 1;

-- Later:
UPDATE Employees
SET Salary = Salary + 1000
WHERE EmployeeId = 2;
```

Transaction B:

```sql
BEGIN TRANSACTION;

UPDATE Employees
SET Salary = Salary + 1000
WHERE EmployeeId = 2;

-- Later:
UPDATE Employees
SET Salary = Salary + 1000
WHERE EmployeeId = 1;
```

This creates a possible deadlock pattern if the transactions overlap appropriately.

SQL Server detects deadlocks and chooses a transaction as the **deadlock victim**, rolling it back so the others can continue.

---

# 33. How to Reduce Deadlocks

Common techniques:

### 1. Access resources in consistent order

For example:

```text
Always Employee 1 → Employee 2
```

instead of sometimes:

```text
Employee 1 → Employee 2
```

and elsewhere:

```text
Employee 2 → Employee 1
```

### 2. Keep transactions short

Don't keep transactions open unnecessarily.

### 3. Access only required rows

Good indexes can reduce unnecessary locking/work.

### 4. Avoid unnecessary user interaction inside transactions

Don't do:

```text
BEGIN TRANSACTION
   ↓
Wait for user
   ↓
COMMIT
```

### 5. Handle deadlock errors appropriately

Application code may retry certain transient failures, depending on the operation and architecture.

---

# 34. Normalization

**Normalization** is a database design technique used to organize data and reduce unnecessary duplication and update anomalies.

Example of a poorly designed table:

```text
StudentId
StudentName
Course1
Course2
Course3
```

Problems:

```text
Course1
Course2
Course3
```

What happens when a student has 10 courses?

The design doesn't scale cleanly.

---

# 35. 1NF — First Normal Form

A table is in **1NF** when each column contains atomic/single values rather than repeating groups or lists.

Bad:

| StudentId | StudentName | Courses          |
| --------: | ----------- | ---------------- |
|         1 | Arun        | C#, SQL, Angular |

`Courses` contains multiple values.

Better:

### Students

| StudentId | StudentName |
| --------: | ----------- |
|         1 | Arun        |

### StudentCourses

| StudentId | Course  |
| --------: | ------- |
|         1 | C#      |
|         1 | SQL     |
|         1 | Angular |

Now each cell contains one logical value.

### Memory:

```text
1NF
→ Atomic values
→ No repeating groups
```

---

# 36. 2NF — Second Normal Form

A table must first satisfy 1NF.

Then:

> Every non-key attribute must depend on the whole primary key, not just part of a composite key.

This matters primarily when the table has a **composite primary key**.

Example:

```text
StudentId + CourseId
```

Suppose:

```text
StudentId
CourseId
StudentName
CourseName
Marks
```

Primary key:

```text
(StudentId, CourseId)
```

Dependencies:

```text
StudentId → StudentName
CourseId  → CourseName
(StudentId, CourseId) → Marks
```

`StudentName` depends only on `StudentId`.

`CourseName` depends only on `CourseId`.

Therefore, they don't depend on the **whole composite key**.

Split the tables:

### Students

```text
StudentId
StudentName
```

### Courses

```text
CourseId
CourseName
```

### StudentCourses

```text
StudentId
CourseId
Marks
```

Now the non-key attributes depend on the appropriate key.

### Memory:

```text
2NF
→ 1NF
→ No partial dependency
```

---

# 37. 3NF — Third Normal Form

A table must satisfy 2NF.

Then:

> Non-key attributes should not depend on other non-key attributes.

Example:

```text
EmployeeId
EmployeeName
DepartmentId
DepartmentName
```

Primary key:

```text
EmployeeId
```

Dependencies:

```text
EmployeeId → DepartmentId
DepartmentId → DepartmentName
```

Therefore:

```text
EmployeeId
   ↓
DepartmentId
   ↓
DepartmentName
```

`DepartmentName` depends on `DepartmentId`, not directly on the employee key.

This is a **transitive dependency**.

Split into:

### Employees

```text
EmployeeId
EmployeeName
DepartmentId
```

### Departments

```text
DepartmentId
DepartmentName
```

Now department information is stored in one place.

### Memory:

```text
3NF
→ 2NF
→ No transitive dependency
```

---

# 38. Normalization Summary

```text
1NF
↓
Atomic values
No repeating groups

2NF
↓
1NF
+
No partial dependency

3NF
↓
2NF
+
No transitive dependency
```

Easy interview memory:

> **1NF = Atomic**

> **2NF = Whole key**

> **3NF = Nothing but the key**

The last phrase is a useful learning shortcut: non-key attributes should depend on the key, the whole key, and not another non-key attribute.

---

# 39. Denormalization

**Denormalization** intentionally introduces some redundancy to improve read performance or simplify frequently executed queries.

Normalized:

```text
Employees
   ↓
DepartmentId
   ↓
Departments
   ↓
DepartmentName
```

A report might need:

```text
EmployeeName
DepartmentName
Salary
```

Every time:

```sql
Employees
JOIN Departments
```

A denormalized reporting table might store:

```text
EmployeeId
EmployeeName
DepartmentName
Salary
```

Now fewer joins may be needed.

But there is a trade-off:

```text
Denormalization
      ↓
Fewer joins / potentially faster reads
      ↓
More duplicate data
      ↓
More update/storage complexity
```

---

# 40. Normalization vs Denormalization

| Normalization             | Denormalization                                |
| ------------------------- | ---------------------------------------------- |
| Reduces duplication       | Allows some duplication                        |
| Improves data consistency | Can simplify/favor reads                       |
| More tables               | Potentially fewer joins                        |
| Common OLTP design        | Often useful in reporting/read-heavy scenarios |
| More joins may be needed  | Fewer joins may be needed                      |

Neither is universally correct. The appropriate design depends on workload, consistency requirements, scale, and access patterns.

---

# 41. Views

A **view** is a named database query that can be queried like a table.

Example:

```sql
CREATE VIEW vw_EmployeeDetails
AS
SELECT
    e.EmployeeId,
    e.EmployeeName,
    d.DepartmentName,
    e.City
FROM Employees e
LEFT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId;
```

Now:

```sql
SELECT *
FROM vw_EmployeeDetails;
```

---

# 42. Why Use Views?

Views can help with:

### 1. Reusable queries

Instead of repeating a complex join:

```sql
SELECT ...
FROM Employees
JOIN Departments ...
```

you can query:

```sql
SELECT *
FROM vw_EmployeeDetails;
```

### 2. Abstraction

Users/applications can work with the view rather than the underlying query details.

### 3. Security

A view can expose selected columns rather than granting direct access to every underlying column, depending on the overall security design.

---

# 43. Alter View

```sql
ALTER VIEW vw_EmployeeDetails
AS
SELECT
    e.EmployeeId,
    e.EmployeeName,
    e.City,
    d.DepartmentName
FROM Employees e
LEFT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId;
```

---

# 44. Drop View

```sql
DROP VIEW vw_EmployeeDetails;
```

---

# 45. View vs Table

```text
Table
→ stores data

View
→ stores a query definition
```

A normal view does not store a separate copy of the result data.

---

# 46. Functions

SQL Server functions are reusable database routines that return a value or a table.

Two important categories:

```text
Functions
│
├── Scalar Function
│
└── Table-Valued Function
    ├── Inline TVF
    └── Multi-Statement TVF
```

---

# 47. Scalar Function

A scalar function returns **one value**.

Example:

```sql
CREATE FUNCTION dbo.CalculateAnnualSalary
(
    @MonthlySalary DECIMAL(10,2)
)
RETURNS DECIMAL(12,2)
AS
BEGIN
    RETURN @MonthlySalary * 12;
END;
```

Call it:

```sql
SELECT dbo.CalculateAnnualSalary(50000);
```

Result:

```text
600000
```

---

# 48. Scalar Function with Table Data

```sql
SELECT
    EmployeeName,
    Salary,
    dbo.CalculateAnnualSalary(Salary) AS AnnualSalary
FROM Employees;
```

Conceptually:

```text
Employee
   ↓
Salary
   ↓
Function
   ↓
Annual Salary
```

---

# 49. Table-Valued Function

A **table-valued function** returns a table.

Example:

```sql
CREATE FUNCTION dbo.GetEmployeesByDepartment
(
    @DepartmentId INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        EmployeeId,
        EmployeeName,
        DepartmentId,
        Salary
    FROM Employees
    WHERE DepartmentId = @DepartmentId
);
```

Call it:

```sql
SELECT *
FROM dbo.GetEmployeesByDepartment(10);
```

Think:

```text
Scalar Function
→ one value

Table-Valued Function
→ table/result set
```

---

# 50. Inline Table-Valued Function

The previous example is an **inline TVF**.

```sql
CREATE FUNCTION dbo.GetEmployeesByDepartment
(
    @DepartmentId INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT *
    FROM Employees
    WHERE DepartmentId = @DepartmentId
);
```

It is essentially a parameterized table expression.

---

# 51. Multi-Statement Table-Valued Function

A multi-statement TVF defines a return table variable and can populate it with multiple statements.

```sql
CREATE FUNCTION dbo.GetHighSalaryEmployees()
RETURNS @Employees TABLE
(
    EmployeeId INT,
    EmployeeName VARCHAR(100),
    Salary DECIMAL(10,2)
)
AS
BEGIN

    INSERT INTO @Employees
    SELECT
        EmployeeId,
        EmployeeName,
        Salary
    FROM Employees
    WHERE Salary > 50000;

    RETURN;
END;
```

Use:

```sql
SELECT *
FROM dbo.GetHighSalaryEmployees();
```

For performance-sensitive workloads, understand the differences between inline TVFs and multi-statement TVFs; the optimizer's visibility into the returned query can differ.

---

# 52. View vs Function

### View

```sql
SELECT *
FROM vw_EmployeeDetails;
```

Normally no parameters.

### Function

```sql
SELECT *
FROM dbo.GetEmployeesByDepartment(10);
```

Can accept parameters.

Memory:

```text
View
→ reusable query

Function
→ reusable logic
→ can accept parameters
→ returns scalar or table
```

---

# 53. Stored Procedure vs Function

Very important interview topic.

| Stored Procedure                          | Function                                                      |
| ----------------------------------------- | ------------------------------------------------------------- |
| Called using `EXEC`                       | Used in expressions/`SELECT` depending on type                |
| Can return result sets                    | Scalar returns one value; TVF returns table                   |
| Can have output parameters                | Uses function return value/table                              |
| Can perform broader procedural operations | Has function-specific restrictions                            |
| Can manage transactions                   | Generally not used for transaction control                    |
| Can be used for data modification         | Functions have restrictions on side effects/data modification |
| Common for business/data operations       | Common for reusable calculations/table expressions            |

Example procedure:

```sql
EXEC GetEmployeesByDepartment 10;
```

Function:

```sql
SELECT *
FROM dbo.GetEmployeesByDepartment(10);
```

---

# 54. Complete Day 18 Example

Let's connect the concepts using the Employee database.

### Index

```sql
CREATE INDEX IX_Employees_DepartmentId
ON Employees(DepartmentId);
```

### Composite index

```sql
CREATE INDEX IX_Employees_Department_City
ON Employees(DepartmentId, City);
```

### View

```sql
CREATE VIEW vw_EmployeeReport
AS
SELECT
    e.EmployeeId,
    e.EmployeeName,
    d.DepartmentName,
    e.City,
    s.Salary
FROM Employees e
LEFT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
LEFT JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId;
```

Query:

```sql
SELECT *
FROM vw_EmployeeReport;
```

### Scalar function

```sql
CREATE FUNCTION dbo.GetAnnualSalary
(
    @Salary DECIMAL(10,2)
)
RETURNS DECIMAL(12,2)
AS
BEGIN
    RETURN @Salary * 12;
END;
```

Use:

```sql
SELECT
    EmployeeName,
    Salary,
    dbo.GetAnnualSalary(Salary) AS AnnualSalary
FROM Employees;
```

### Table-valued function

```sql
CREATE FUNCTION dbo.GetEmployeesByDepartment
(
    @DepartmentId INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        EmployeeId,
        EmployeeName,
        DepartmentId,
        Salary
    FROM Employees
    WHERE DepartmentId = @DepartmentId
);
```

Use:

```sql
SELECT *
FROM dbo.GetEmployeesByDepartment(10);
```

---

# 55. How This Connects to .NET

Your overall application architecture now becomes:

```text
Angular
   ↓
HTTP Request
   ↓
ASP.NET Core Controller
   ↓
Service / BAL
   ↓
Repository / DAL
   ↓
ADO.NET
   ↓
SQL Server
   │
   ├── Tables
   ├── Indexes
   ├── Stored Procedures
   ├── Views
   ├── Functions
   └── Transactions
```

For example:

```text
GET /api/employees/10
          ↓
EmployeesController
          ↓
EmployeeService
          ↓
EmployeeRepository
          ↓
SqlCommand
          ↓
Stored Procedure
          ↓
SQL Server
          ↓
Index / Execution Plan
          ↓
Query
          ↓
Result
          ↓
DTO
          ↓
JSON
          ↓
Angular
```

---

# 56. Day 18 Interview Questions

### Indexes

**1. What is an index?**

An index is a database structure that helps SQL Server locate and retrieve data efficiently.

**2. How many clustered indexes can a table have?**

At most **one**.

**3. How many non-clustered indexes can a table have?**

Multiple, subject to SQL Server limits and practical considerations.

**4. What is a composite index?**

An index containing multiple columns.

**5. Does an index always improve performance?**

No. It can improve reads but adds storage and maintenance overhead for writes.

---

### Execution Plans

**6. What is an execution plan?**

It shows the operations SQL Server uses or expects to use to execute a query.

**7. What is Index Seek?**

A plan operation that navigates an index to locate relevant rows.

**8. Is Table Scan always bad?**

No. For small tables or queries needing most rows, a scan can be appropriate.

---

### Transactions

**9. What is a transaction?**

A group of database operations treated as one logical unit.

**10. What is ACID?**

```text
A → Atomicity
C → Consistency
I → Isolation
D → Durability
```

**11. What is a deadlock?**

A situation where transactions wait for resources held by each other.

---

### Normalization

**12. What is 1NF?**

Atomic values and no repeating groups.

**13. What is 2NF?**

1NF plus no partial dependency on part of a composite key.

**14. What is 3NF?**

2NF plus no transitive dependency of non-key attributes on other non-key attributes.

**15. What is denormalization?**

Intentionally introducing some redundancy to support particular read/performance requirements.

---

### Views and Functions

**16. What is a view?**

A named query definition that can be queried like a table.

**17. What is a scalar function?**

A function that returns one value.

**18. What is a table-valued function?**

A function that returns a table/result set.

**19. View vs function?**

```text
View
→ reusable query
→ normally no parameters

Function
→ reusable logic
→ can accept parameters
→ returns scalar or table
```

**20. Stored procedure vs function?**

```text
Stored Procedure
→ EXEC
→ procedural/data operations
→ can return result sets

Function
→ used as part of SQL expressions/queries
→ returns scalar or table
→ has stricter restrictions
```

---

# 57. Day 18 Final Revision Cheat Sheet

```text
INDEX
│
├── Clustered
│   └── Maximum 1 per table
│
├── Non-Clustered
│   └── Multiple possible
│
└── Composite
    └── Multiple columns
```

```text
EXECUTION PLAN
│
├── Table Scan
├── Index Scan
├── Index Seek
├── Key Lookup
├── Sort
├── Nested Loops
└── Hash Match
```

```text
TRANSACTION
│
├── BEGIN
├── COMMIT
└── ROLLBACK
```

```text
ACID
│
├── A → Atomicity
├── C → Consistency
├── I → Isolation
└── D → Durability
```

```text
ISOLATION
│
├── READ UNCOMMITTED
├── READ COMMITTED
├── REPEATABLE READ
├── SNAPSHOT
└── SERIALIZABLE
```

```text
NORMALIZATION
│
├── 1NF → Atomic values
├── 2NF → No partial dependency
└── 3NF → No transitive dependency
```

```text
DATABASE OBJECTS
│
├── Table
├── Index
├── Stored Procedure
├── View
└── Function
    ├── Scalar
    └── Table-Valued
```

## The most important Day 18 mental model

```text
                     SQL SERVER
                         │
        ┌────────────────┼────────────────┐
        ↓                ↓                ↓
      DESIGN          PERFORMANCE       LOGIC
        │                │                │
   Normalization       Indexes       Stored Procedures
   Denormalization     Execution     Views
                       Plans          Functions
                         │
                         ↓
                    Transactions
                         │
                    ┌────┴────┐
                    ↓         ↓
                   ACID    Isolation
                              │
                           Deadlocks
```


