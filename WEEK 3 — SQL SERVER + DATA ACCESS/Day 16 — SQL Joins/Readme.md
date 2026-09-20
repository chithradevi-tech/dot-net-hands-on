# Day 16 — SQL Joins

Today is one of the **most important SQL days** in your .NET roadmap.

In real .NET applications, data is usually spread across multiple tables. SQL Joins allow you to combine that data.

We will use this structure throughout today's practice:

```text
Departments
     ↑
     │
     │ DepartmentId
     │
Employees
     │
     ├──────────→ Managers
     │
     └──────────→ Salaries
```

By the end, you should be comfortable writing **20+ practical JOIN queries**.

---

# 1. What is a JOIN?

A `JOIN` combines rows from two or more tables based on a related column.

For example:

### Employees

| EmployeeId | EmployeeName | DepartmentId | ManagerId |
| ---------: | ------------ | -----------: | --------: |
|          1 | Arun         |            1 |         3 |
|          2 | Priya        |            2 |         4 |
|          3 | Kumar        |            1 |      NULL |
|          4 | Divya        |            3 |      NULL |
|          5 | Ravi         |         NULL |         3 |

### Departments

| DepartmentId | DepartmentName |
| -----------: | -------------- |
|            1 | IT             |
|            2 | HR             |
|            3 | Finance        |
|            4 | Sales          |

We can combine them:

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName
FROM Employees e
INNER JOIN Departments d
    ON e.DepartmentId = d.DepartmentId;
```

Result:

| EmployeeName | DepartmentName |
| ------------ | -------------- |
| Arun         | IT             |
| Priya        | HR             |
| Kumar        | IT             |
| Divya        | Finance        |

---

# 2. Why Do We Need JOINs?

Suppose you have:

```text
Employees
---------
EmployeeId
Name
DepartmentId
```

and:

```text
Departments
-----------
DepartmentId
DepartmentName
```

We don't want to store:

```text
Employee
---------
1 | Arun | IT
2 | Priya | HR
3 | Kumar | IT
```

Instead, we store:

```text
Employees
---------
1 | Arun | 1
2 | Priya | 2
3 | Kumar | 1
```

and:

```text
Departments
-----------
1 | IT
2 | HR
```

Then JOIN combines them when needed.

This is a major principle of relational database design.

---

# 3. Our Day 16 Database

We'll create four tables:

```text
Departments
Employees
Managers
Salaries
```

---

# 4. Departments Table

```sql
CREATE TABLE Departments
(
    DepartmentId INT PRIMARY KEY,
    DepartmentName VARCHAR(100) NOT NULL
);
```

Insert data:

```sql
INSERT INTO Departments
(
    DepartmentId,
    DepartmentName
)
VALUES
(1, 'IT'),
(2, 'HR'),
(3, 'Finance'),
(4, 'Sales'),
(5, 'Marketing');
```

---

# 5. Managers Table

```sql
CREATE TABLE Managers
(
    ManagerId INT PRIMARY KEY,
    ManagerName VARCHAR(100) NOT NULL
);
```

Insert:

```sql
INSERT INTO Managers
(
    ManagerId,
    ManagerName
)
VALUES
(1, 'Suresh'),
(2, 'Meena'),
(3, 'Raj'),
(4, 'Anitha');
```

---

# 6. Employees Table

```sql
CREATE TABLE Employees
(
    EmployeeId INT PRIMARY KEY,
    EmployeeName VARCHAR(100) NOT NULL,
    DepartmentId INT NULL,
    ManagerId INT NULL,
    City VARCHAR(100),
    JoiningDate DATE,

    CONSTRAINT FK_Employees_Departments
        FOREIGN KEY (DepartmentId)
        REFERENCES Departments(DepartmentId),

    CONSTRAINT FK_Employees_Managers
        FOREIGN KEY (ManagerId)
        REFERENCES Managers(ManagerId)
);
```

Insert employees:

```sql
INSERT INTO Employees
(
    EmployeeId,
    EmployeeName,
    DepartmentId,
    ManagerId,
    City,
    JoiningDate
)
VALUES
(101, 'Arun',   1, 3, 'Chennai',   '2023-01-10'),
(102, 'Priya',  2, 4, 'Bangalore', '2022-05-15'),
(103, 'Kumar',  1, 3, 'Chennai',   '2021-03-20'),
(104, 'Divya',  3, 1, 'Coimbatore','2024-02-10'),
(105, 'Ravi',   NULL, 3, 'Madurai', '2024-06-01'),
(106, 'Meena',  4, NULL, 'Chennai', '2020-08-12');
```

Notice:

```text
Ravi   → DepartmentId = NULL
Meena  → ManagerId = NULL
```

These records will help us understand `LEFT JOIN`.

---

# 7. Salaries Table

```sql
CREATE TABLE Salaries
(
    SalaryId INT PRIMARY KEY,
    EmployeeId INT NOT NULL,
    Salary DECIMAL(10,2) NOT NULL,
    EffectiveDate DATE NOT NULL,

    CONSTRAINT FK_Salaries_Employees
        FOREIGN KEY (EmployeeId)
        REFERENCES Employees(EmployeeId)
);
```

Insert:

```sql
INSERT INTO Salaries
(
    SalaryId,
    EmployeeId,
    Salary,
    EffectiveDate
)
VALUES
(1, 101, 50000, '2025-01-01'),
(2, 102, 45000, '2025-01-01'),
(3, 103, 60000, '2025-01-01'),
(4, 104, 70000, '2025-01-01'),
(5, 105, 40000, '2025-01-01');
```

Notice:

```text
Employee 106
```

does not have a salary record.

That will be useful when learning joins.

---

# 8. INNER JOIN

`INNER JOIN` returns only matching records from both tables.

Syntax:

```sql
SELECT columns
FROM TableA A
INNER JOIN TableB B
    ON A.CommonColumn = B.CommonColumn;
```

Example:

```sql
SELECT
    e.EmployeeId,
    e.EmployeeName,
    d.DepartmentName
FROM Employees e
INNER JOIN Departments d
    ON e.DepartmentId = d.DepartmentId;
```

Result:

| EmployeeId | EmployeeName | DepartmentName |
| ---------: | ------------ | -------------- |
|        101 | Arun         | IT             |
|        102 | Priya        | HR             |
|        103 | Kumar        | IT             |
|        104 | Divya        | Finance        |
|        106 | Meena        | Sales          |

Ravi is not returned because:

```text
Ravi.DepartmentId = NULL
```

There is no matching department.

### Memory

```text
INNER JOIN
=
Only matching rows
```

---

# 9. INNER JOIN Visual

Think:

```text
Employees       Departments

   A                  B
   ┌───────┐          ┌───────┐
   │       │          │       │
   │       │██████████│       │
   │       │  MATCH   │       │
   └───────┘          └───────┘
```

Only the matching portion is returned.

---

# 10. LEFT JOIN

`LEFT JOIN` returns:

* All rows from the left table
* Matching rows from the right table
* `NULL` when there is no match

Syntax:

```sql
SELECT columns
FROM TableA A
LEFT JOIN TableB B
    ON A.Id = B.Id;
```

Example:

```sql
SELECT
    e.EmployeeId,
    e.EmployeeName,
    d.DepartmentName
FROM Employees e
LEFT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId;
```

Result includes:

| EmployeeId | EmployeeName | DepartmentName |
| ---------: | ------------ | -------------- |
|        101 | Arun         | IT             |
|        102 | Priya        | HR             |
|        103 | Kumar        | IT             |
|        104 | Divya        | Finance        |
|        105 | Ravi         | NULL           |
|        106 | Meena        | Sales          |

Ravi appears because **Employees is the left table**.

---

# 11. LEFT JOIN Memory Trick

Always ask:

> Which table do I want ALL records from?

Put that table on the left.

```sql
FROM Employees e
LEFT JOIN Departments d
```

means:

> Give me every employee, whether or not they have a department.

---

# 12. Find Employees Without Departments

This is a very common interview query.

```sql
SELECT
    e.EmployeeId,
    e.EmployeeName
FROM Employees e
LEFT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
WHERE d.DepartmentId IS NULL;
```

Result:

```text
Ravi
```

This pattern is extremely useful:

```text
LEFT JOIN
+
WHERE right_table.id IS NULL
=
records with no matching record
```

---

# 13. RIGHT JOIN

`RIGHT JOIN` returns:

* All rows from the right table
* Matching rows from the left table
* `NULL` where no match exists

Example:

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName
FROM Employees e
RIGHT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId;
```

Suppose Marketing has no employees.

Then:

| EmployeeName | DepartmentName |
| ------------ | -------------- |
| Arun         | IT             |
| Priya        | HR             |
| Kumar        | IT             |
| Divya        | Finance        |
| Meena        | Sales          |
| NULL         | Marketing      |

---

# 14. RIGHT JOIN vs LEFT JOIN

This:

```sql
FROM Employees e
RIGHT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
```

can usually be rewritten as:

```sql
FROM Departments d
LEFT JOIN Employees e
    ON e.DepartmentId = d.DepartmentId
```

Because of this, many developers prefer `LEFT JOIN` for readability.

Memory:

```text
LEFT JOIN
→ all left table records

RIGHT JOIN
→ all right table records
```

---

# 15. FULL OUTER JOIN

`FULL OUTER JOIN` returns:

* Matching rows
* Unmatched rows from the left
* Unmatched rows from the right

Example:

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName
FROM Employees e
FULL OUTER JOIN Departments d
    ON e.DepartmentId = d.DepartmentId;
```

Conceptually:

```text
Employees only
      +
Matching
      +
Departments only
```

So you could get:

```text
Arun      IT
Priya     HR
Kumar     IT
Divya     Finance
Ravi      NULL
Meena     Sales
NULL      Marketing
```

This is useful when you want to see **everything from both sides**, including unmatched records.

---

# 16. FULL OUTER JOIN Visual

```text
Employees                  Departments

   ┌──────────┐          ┌──────────┐
   │ LEFT ONLY│██████████│ RIGHT ONLY
   │          │  MATCH   │
   └──────────┘          └──────────┘
          \________________/
              ALL DATA
```

---

# 17. CROSS JOIN

`CROSS JOIN` creates every possible combination of rows.

Suppose:

### Employees

```text
Arun
Priya
Kumar
```

### Departments

```text
IT
HR
```

Then:

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName
FROM Employees e
CROSS JOIN Departments d;
```

Produces:

```text
Arun   IT
Arun   HR
Priya  IT
Priya  HR
Kumar  IT
Kumar  HR
```

If:

```text
Employees = 6 rows
Departments = 5 rows
```

then:

```text
6 × 5 = 30 rows
```

### Important

CROSS JOIN has no `ON` condition.

Use it carefully because the result can become very large.

---

# 18. SELF JOIN

A **SELF JOIN** joins a table to itself.

This is useful for hierarchical relationships.

Our `Employees` table contains:

```text
EmployeeId
EmployeeName
ManagerId
```

So an employee's `ManagerId` can refer to another employee's `EmployeeId`.

For example:

```text
Kumar
  ↓
ManagerId = 3
  ↓
EmployeeId = 3
  ↓
Raj
```

Query:

```sql
SELECT
    e.EmployeeName AS Employee,
    m.EmployeeName AS Manager
FROM Employees e
LEFT JOIN Employees m
    ON e.ManagerId = m.EmployeeId;
```

The same table appears twice:

```text
Employees e
Employees m
```

The aliases make them different logical roles.

---

# 19. Important SELF JOIN Example

```sql
SELECT
    e.EmployeeName AS Employee,
    m.EmployeeName AS Manager
FROM Employees e
LEFT JOIN Employees m
    ON e.ManagerId = m.EmployeeId;
```

Result conceptually:

| Employee | Manager                |
| -------- | ---------------------- |
| Arun     | NULL/depending on data |
| Priya    | NULL/depending on data |
| Kumar    | NULL/depending on data |

For our current table, the `ManagerId` values reference the separate `Managers` table, so the query above only works meaningfully if those IDs are also employee IDs.

For today's `Employees` data, use the `Managers` table for manager names:

```sql
SELECT
    e.EmployeeName AS Employee,
    m.ManagerName AS Manager
FROM Employees e
LEFT JOIN Managers m
    ON e.ManagerId = m.ManagerId;
```

This is an important distinction:

```text
Employees → Managers
```

is a normal join between two tables.

A true **SELF JOIN** happens when:

```text
Employees → Employees
```

using something like:

```text
Employee.ManagerId → Employee.EmployeeId
```

---

# 20. Join Types — Quick Memory

```text
INNER JOIN
→ matching records only

LEFT JOIN
→ everything from LEFT + matching RIGHT

RIGHT JOIN
→ everything from RIGHT + matching LEFT

FULL OUTER JOIN
→ everything from BOTH

CROSS JOIN
→ every possible combination

SELF JOIN
→ table joined with itself
```

---

# 21. JOIN Comparison

| Join       | Returns                     |
| ---------- | --------------------------- |
| INNER      | Matching rows only          |
| LEFT       | All left + matching right   |
| RIGHT      | All right + matching left   |
| FULL OUTER | All rows from both sides    |
| CROSS      | Every combination           |
| SELF       | Same table joined to itself |

---

# 22. Practical Query 1 — Employee + Department

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName
FROM Employees e
INNER JOIN Departments d
    ON e.DepartmentId = d.DepartmentId;
```

---

# 23. Practical Query 2 — Employee + Department + City

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName,
    e.City
FROM Employees e
INNER JOIN Departments d
    ON e.DepartmentId = d.DepartmentId;
```

---

# 24. Practical Query 3 — Employees in IT

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName
FROM Employees e
INNER JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
WHERE d.DepartmentName = 'IT';
```

---

# 25. Practical Query 4 — Employees with Salary

```sql
SELECT
    e.EmployeeName,
    s.Salary
FROM Employees e
INNER JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId;
```

Employee 106 won't appear because there is no salary record.

---

# 26. Practical Query 5 — Employee + Department + Salary

This is a very important real-world query.

```sql
SELECT
    e.EmployeeId,
    e.EmployeeName,
    d.DepartmentName,
    s.Salary
FROM Employees e
INNER JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
INNER JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId;
```

---

# 27. Practical Query 6 — All Employees Including Missing Salary

Use `LEFT JOIN`.

```sql
SELECT
    e.EmployeeId,
    e.EmployeeName,
    s.Salary
FROM Employees e
LEFT JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId;
```

Now employee 106 appears with:

```text
Salary = NULL
```

---

# 28. Practical Query 7 — Employees Without Salary

```sql
SELECT
    e.EmployeeId,
    e.EmployeeName
FROM Employees e
LEFT JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId
WHERE s.EmployeeId IS NULL;
```

---

# 29. Practical Query 8 — Employees + Manager

```sql
SELECT
    e.EmployeeName,
    m.ManagerName
FROM Employees e
LEFT JOIN Managers m
    ON e.ManagerId = m.ManagerId;
```

---

# 30. Practical Query 9 — Employees in Chennai with Department

```sql
SELECT
    e.EmployeeName,
    e.City,
    d.DepartmentName
FROM Employees e
INNER JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
WHERE e.City = 'Chennai';
```

---

# 31. Practical Query 10 — Employees with Salary > 50K

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName,
    s.Salary
FROM Employees e
INNER JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
INNER JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId
WHERE s.Salary > 50000;
```

---

# 32. Practical Query 11 — Highest Salary

```sql
SELECT TOP 1
    e.EmployeeName,
    s.Salary
FROM Employees e
INNER JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId
ORDER BY s.Salary DESC;
```

---

# 33. Practical Query 12 — Department-wise Employee Count

```sql
SELECT
    d.DepartmentName,
    COUNT(e.EmployeeId) AS EmployeeCount
FROM Departments d
LEFT JOIN Employees e
    ON d.DepartmentId = e.DepartmentId
GROUP BY d.DepartmentName;
```

Why `LEFT JOIN`?

Because we want departments even when they have **zero employees**.

---

# 34. Practical Query 13 — Department-wise Total Salary

```sql
SELECT
    d.DepartmentName,
    SUM(s.Salary) AS TotalSalary
FROM Departments d
INNER JOIN Employees e
    ON d.DepartmentId = e.DepartmentId
INNER JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId
GROUP BY d.DepartmentName;
```

---

# 35. Practical Query 14 — Departments with Total Salary > 100K

```sql
SELECT
    d.DepartmentName,
    SUM(s.Salary) AS TotalSalary
FROM Departments d
INNER JOIN Employees e
    ON d.DepartmentId = e.DepartmentId
INNER JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId
GROUP BY d.DepartmentName
HAVING SUM(s.Salary) > 100000;
```

This combines:

```text
JOIN
+
GROUP BY
+
SUM
+
HAVING
```

---

# 36. Practical Query 15 — Employees Whose Department is Missing

```sql
SELECT
    e.EmployeeId,
    e.EmployeeName
FROM Employees e
LEFT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
WHERE d.DepartmentId IS NULL;
```

---

# 37. Practical Query 16 — Departments Without Employees

This is the opposite.

```sql
SELECT
    d.DepartmentId,
    d.DepartmentName
FROM Departments d
LEFT JOIN Employees e
    ON d.DepartmentId = e.DepartmentId
WHERE e.EmployeeId IS NULL;
```

For our sample data:

```text
Marketing
```

will be returned.

This pattern is extremely important.

```text
LEFT JOIN
+
right side IS NULL
=
find records without a match
```

---

# 38. Practical Query 17 — Employee + Manager

Using the separate `Managers` table:

```sql
SELECT
    e.EmployeeName AS Employee,
    m.ManagerName AS Manager
FROM Employees e
LEFT JOIN Managers m
    ON e.ManagerId = m.ManagerId;
```

---

# 39. Practical Query 18 — Employee + Department + Manager

```sql
SELECT
    e.EmployeeName AS Employee,
    d.DepartmentName AS Department,
    m.ManagerName AS Manager
FROM Employees e
LEFT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
LEFT JOIN Managers m
    ON e.ManagerId = m.ManagerId;
```

This is a realistic API query.

---

# 40. Practical Query 19 — Employees Managed by Raj

```sql
SELECT
    e.EmployeeName,
    m.ManagerName
FROM Employees e
INNER JOIN Managers m
    ON e.ManagerId = m.ManagerId
WHERE m.ManagerName = 'Raj';
```

---

# 41. Practical Query 20 — Employee Salary Report

```sql
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
    ON e.EmployeeId = s.EmployeeId
ORDER BY s.Salary DESC;
```

This is the kind of query you may eventually execute from your **DAL/Repository layer**.

---

# 42. Practical Query 21 — Top 3 Highest-Paid Employees

```sql
SELECT TOP 3
    e.EmployeeName,
    d.DepartmentName,
    s.Salary
FROM Employees e
INNER JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
INNER JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId
ORDER BY s.Salary DESC;
```

---

# 43. Practical Query 22 — Average Salary by Department

```sql
SELECT
    d.DepartmentName,
    AVG(s.Salary) AS AverageSalary
FROM Departments d
INNER JOIN Employees e
    ON d.DepartmentId = e.DepartmentId
INNER JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId
GROUP BY d.DepartmentName;
```

---

# 44. Practical Query 23 — Employees Above Department Average

This introduces a more advanced pattern using a subquery.

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName,
    s.Salary
FROM Employees e
INNER JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
INNER JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId
WHERE s.Salary >
(
    SELECT AVG(s2.Salary)
    FROM Employees e2
    INNER JOIN Salaries s2
        ON e2.EmployeeId = s2.EmployeeId
    WHERE e2.DepartmentId = e.DepartmentId
);
```

This is beyond basic joins, but it is excellent practice.

---

# 45. Practical Query 24 — Count Employees Under Each Manager

```sql
SELECT
    m.ManagerName,
    COUNT(e.EmployeeId) AS EmployeeCount
FROM Managers m
LEFT JOIN Employees e
    ON m.ManagerId = e.ManagerId
GROUP BY m.ManagerName;
```

---

# 46. Practical Query 25 — Employees With No Manager

```sql
SELECT
    e.EmployeeId,
    e.EmployeeName
FROM Employees e
LEFT JOIN Managers m
    ON e.ManagerId = m.ManagerId
WHERE m.ManagerId IS NULL;
```

---

# 47. A Very Important JOIN Mistake

Consider:

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName
FROM Employees e
LEFT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
WHERE d.DepartmentName = 'IT';
```

This effectively removes employees without a matching department because the `WHERE` condition requires a department name.

If your intention is:

> Return all employees, but show department only when it is IT

then put the condition in the `ON` clause:

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName
FROM Employees e
LEFT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
   AND d.DepartmentName = 'IT';
```

This is an important real-world SQL concept.

---

# 48. JOIN vs WHERE

The `ON` clause determines **how tables are matched**.

```sql
ON e.DepartmentId = d.DepartmentId
```

The `WHERE` clause filters the **result**.

```sql
WHERE d.DepartmentName = 'IT'
```

Memory:

```text
ON
→ relationship/matching

WHERE
→ filtering
```

---

# 49. Multiple JOINs

You can join more than two tables.

```sql
SELECT
    e.EmployeeName,
    d.DepartmentName,
    m.ManagerName,
    s.Salary
FROM Employees e
LEFT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
LEFT JOIN Managers m
    ON e.ManagerId = m.ManagerId
LEFT JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId;
```

Execution conceptually:

```text
Employees
    ↓
JOIN Departments
    ↓
JOIN Managers
    ↓
JOIN Salaries
    ↓
Final Result
```

---

# 50. JOIN Relationship Map

Your Day 16 database:

```text
                    ┌──────────────────┐
                    │   Departments    │
                    │------------------│
                    │ DepartmentId PK  │
                    │ DepartmentName   │
                    └────────┬─────────┘
                             │
                             │ 1
                             │
                             │ *
                    ┌────────▼─────────┐
                    │    Employees     │
                    │------------------│
                    │ EmployeeId PK    │
                    │ EmployeeName     │
                    │ DepartmentId FK  │
                    │ ManagerId FK     │
                    └───┬──────────┬───┘
                        │          │
                        │          │
                        │          │
               ┌────────▼───┐  ┌──▼────────────┐
               │  Managers  │  │   Salaries    │
               │------------│  │---------------│
               │ ManagerId  │  │ SalaryId      │
               │ ManagerName│  │ EmployeeId FK │
               └────────────┘  │ Salary        │
                               └───────────────┘
```

---

# 51. Real-World .NET Connection

Later your application may have:

```text
EmployeeController
        ↓
EmployeeService
        ↓
EmployeeRepository
        ↓
ADO.NET
        ↓
SQL Server
```

Suppose your API endpoint is:

```text
GET /api/employees/report
```

The DAL might execute a query similar to:

```sql
SELECT
    e.EmployeeId,
    e.EmployeeName,
    d.DepartmentName,
    m.ManagerName,
    s.Salary
FROM Employees e
LEFT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
LEFT JOIN Managers m
    ON e.ManagerId = m.ManagerId
LEFT JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId;
```

Then:

```text
SQL Result
    ↓
ADO.NET
    ↓
Employee DTO
    ↓
Service
    ↓
Controller
    ↓
JSON
    ↓
Angular
```

This is exactly why JOIN knowledge is important for your .NET backend learning.

---

# 52. Interview Questions

### 1. What is a JOIN?

A JOIN combines related data from multiple tables.

### 2. What is INNER JOIN?

Returns only matching records from both tables.

### 3. What is LEFT JOIN?

Returns all records from the left table and matching records from the right table.

### 4. What is RIGHT JOIN?

Returns all records from the right table and matching records from the left table.

### 5. What is FULL OUTER JOIN?

Returns matched and unmatched records from both tables.

### 6. What is CROSS JOIN?

Returns the Cartesian product of two tables.

### 7. What is SELF JOIN?

A table is joined to itself.

### 8. Which JOIN should you use to find employees without departments?

Usually:

```sql
LEFT JOIN
```

with:

```sql
WHERE d.DepartmentId IS NULL
```

### 9. LEFT JOIN vs INNER JOIN?

```text
INNER JOIN
→ only matching rows

LEFT JOIN
→ all left rows + matching right rows
```

### 10. Can you join more than two tables?

Yes.

```sql
FROM Employees e
JOIN Departments d ...
JOIN Salaries s ...
JOIN Managers m ...
```

### 11. Can a JOIN have multiple conditions?

Yes.

```sql
ON e.DepartmentId = d.DepartmentId
AND e.City = d.City
```

### 12. What is the difference between ON and WHERE?

```text
ON
→ defines matching relationship

WHERE
→ filters resulting rows
```

### 13. Why use table aliases?

Instead of:

```sql
Employees.EmployeeName
```

we can use:

```sql
e.EmployeeName
```

Aliases make complex queries shorter and clearer.

---

# 53. Day 16 — JOIN Cheat Sheet

```text
INNER JOIN
    ↓
Matching records only
```

```text
LEFT JOIN
    ↓
Everything from LEFT
+ matching RIGHT
```

```text
RIGHT JOIN
    ↓
Everything from RIGHT
+ matching LEFT
```

```text
FULL OUTER JOIN
    ↓
Everything from BOTH
```

```text
CROSS JOIN
    ↓
Every combination
```

```text
SELF JOIN
    ↓
Table joins itself
```

### Most important patterns

**Find matching records:**

```sql
INNER JOIN
```

**Keep all records from first table:**

```sql
LEFT JOIN
```

**Find records with no match:**

```sql
LEFT JOIN ...
WHERE right_table.Id IS NULL
```

**Keep everything from both tables:**

```sql
FULL OUTER JOIN
```

**Every possible combination:**

```sql
CROSS JOIN
```

**Employee → Manager hierarchy:**

```sql
Employees e
JOIN Employees m
```

---

# Day 16 Final Mental Model

```text
                    SQL JOINS
                        │
       ┌────────────────┼────────────────┐
       │                │                │
   INNER JOIN       OUTER JOIN       CROSS/SELF
       │                │                │
   Matching       ┌──────┼──────┐    ┌──────┐
                  │      │      │    │      │
                LEFT   RIGHT   FULL CROSS  SELF
```

And your practical business model is:

```text
Employee
   │
   ├── Department
   │
   ├── Manager
   │
   └── Salary
```

The **most important 5 queries to master today** are:

```sql
-- 1. Employee + Department
SELECT e.EmployeeName, d.DepartmentName
FROM Employees e
INNER JOIN Departments d
    ON e.DepartmentId = d.DepartmentId;


-- 2. All Employees + Department
SELECT e.EmployeeName, d.DepartmentName
FROM Employees e
LEFT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId;


-- 3. Employees without Department
SELECT e.EmployeeName
FROM Employees e
LEFT JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
WHERE d.DepartmentId IS NULL;


-- 4. Employee + Manager + Salary
SELECT
    e.EmployeeName,
    m.ManagerName,
    s.Salary
FROM Employees e
LEFT JOIN Managers m
    ON e.ManagerId = m.ManagerId
LEFT JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId;


-- 5. Department-wise salary
SELECT
    d.DepartmentName,
    SUM(s.Salary) AS TotalSalary
FROM Departments d
INNER JOIN Employees e
    ON d.DepartmentId = e.DepartmentId
INNER JOIN Salaries s
    ON e.EmployeeId = s.EmployeeId
GROUP BY d.DepartmentName;
```


