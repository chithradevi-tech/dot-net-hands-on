# 🗓️ WEEK 3 — SQL SERVER + DATA ACCESS

# Day 15 — SQL Server Fundamentals

Learn:

* Database
* Tables
* Rows
* Columns
* Primary key
* Foreign key
* Constraints
* Identity
* NULL
* Unique
* Default

SQL commands:

```sql
SELECT
INSERT
UPDATE
DELETE
```

Learn:

* WHERE
* ORDER BY
* GROUP BY
* HAVING
* DISTINCT
* TOP
* LIKE
* BETWEEN
* IN

---

# Day 16 — SQL Joins

Master:

```text
INNER JOIN
LEFT JOIN
RIGHT JOIN
FULL OUTER JOIN
CROSS JOIN
SELF JOIN
```

Understand practical relationships:

```text
Employee
Department
Manager
Salary
```

Practice 20+ queries.

---

# Day 17 — Advanced SQL

Learn:

## Stored Procedures

```sql
CREATE PROCEDURE
ALTER PROCEDURE
EXEC
```

Learn:

* Input parameters
* Output parameters
* Return values
* Transactions
* TRY/CATCH
* Temporary tables
* Table variables

---

## CTE

Learn:

```sql
WITH EmployeeCTE AS (...)
```

Understand:

* Recursive CTE
* Hierarchical data

---

## UNION

Learn:

```text
UNION
UNION ALL
```

Understand the difference.

---

# Day 18 — SQL Advanced + Performance

Learn:

* Indexes
* Clustered index
* Non-clustered index
* Composite index
* Execution plans
* Query optimization
* Transactions
* ACID
* Isolation levels
* Deadlocks
* Normalization
* Denormalization

Learn:

```text
1NF
2NF
3NF
```

Also understand:

* Views
* Functions
* Scalar functions
* Table-valued functions

---

# Day 19 — ADO.NET

Learn ADO.NET from fundamentals.

Understand:

```text
Connection
Command
Parameter
DataReader
DataAdapter
DataSet
DataTable
```

Important classes:

```csharp
SqlConnection
SqlCommand
SqlDataReader
SqlParameter
```

Learn:

* Connection strings
* Parameterized queries
* ExecuteReader
* ExecuteScalar
* ExecuteNonQuery
* Stored procedure execution
* Transactions

Understand why **SQL parameters** should be used instead of string concatenation.

---

# Day 20 — Data Access Layer

Build:

```text
Controller
     ↓
BAL / Service
     ↓
DAL
     ↓
ADO.NET
     ↓
SQL Server
```

Create:

```text
IEmployeeRepository
EmployeeRepository

IEmployeeService
EmployeeService

EmployeesController
```

Learn:

* Repository pattern
* Service layer
* Separation of concerns
* Interfaces
* Dependency injection

---

# Day 21 — Layered Architecture

Build a proper project:

```text
EmployeeManagement
│
├── Controllers
│
├── Services
│   ├── Interfaces
│   └── Implementations
│
├── DAL
│   ├── Interfaces
│   └── Implementations
│
├── Models
│
├── DTOs
│
├── Middleware
│
├── Helpers
│
└── Program.cs
```

Flow:

```text
Client
 ↓
Controller
 ↓
Service / BAL
 ↓
Repository / DAL
 ↓
ADO.NET
 ↓
SQL Server
```

Implement:

* CRUD
* Search
* Filtering
* Pagination
* Sorting
* Validation
* Error handling
* Logging

---

# Day 23 — MDX Basics

Learn MDX fundamentals.

Understand:

```text
Cube
Dimension
Hierarchy
Level
Member
Measure
Measure Group
```

Basic MDX concepts:

```text
SELECT
FROM
WHERE
ON COLUMNS
ON ROWS
```

Learn concepts such as:

```text
Measures
Dimensions
Members
Tuples
Sets
```

Practice simple queries such as:

```text
Sales by Product
Sales by Year
Sales by Region
Sales by Month
```

Then connect:

```text
ASP.NET Core API
        ↓
ADOMD Client
        ↓
Analysis Services
        ↓
MDX
        ↓
Response
```

---