# Day 19 — ADO.NET

ADO.NET is one of the most important topics in your .NET roadmap because it connects your **ASP.NET Core application to SQL Server**.

Your architecture becomes:

```text
Angular / Client
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
       ↓
Database
```

Today we'll learn the complete ADO.NET fundamentals:

```text
ADO.NET
│
├── Connection
├── Command
├── Parameter
├── DataReader
├── DataAdapter
├── DataSet
├── DataTable
│
├── Connection Strings
├── Parameterized Queries
│
├── ExecuteReader()
├── ExecuteScalar()
├── ExecuteNonQuery()
│
├── Stored Procedures
└── Transactions
```

---

# 1. What is ADO.NET?

**ADO.NET** is the .NET data-access technology used to communicate with relational databases such as SQL Server.

It allows your C# application to:

```text
Connect
   ↓
Send SQL
   ↓
Execute query
   ↓
Read result
   ↓
Insert / Update / Delete
   ↓
Handle transactions
```

For SQL Server, modern .NET applications commonly use the `Microsoft.Data.SqlClient` package.

---

# 2. ADO.NET Architecture

The basic architecture is:

```text
C# Application
      │
      ↓
ADO.NET
      │
      ├── SqlConnection
      ├── SqlCommand
      ├── SqlParameter
      ├── SqlDataReader
      ├── SqlDataAdapter
      ├── DataSet
      └── DataTable
      │
      ↓
SQL Server
      │
      ↓
Database
```

---

# 3. Important ADO.NET Classes

## SqlConnection

Used to establish a connection to SQL Server.

```csharp
SqlConnection connection =
    new SqlConnection(connectionString);
```

Think:

```text
SqlConnection
     ↓
"Connect my application to SQL Server"
```

---

# 4. SqlCommand

Used to execute SQL statements or stored procedures.

```csharp
SqlCommand command =
    new SqlCommand(sql, connection);
```

Example:

```csharp
string sql = "SELECT * FROM Employees";

using SqlCommand command =
    new SqlCommand(sql, connection);
```

Think:

```text
SqlCommand
     ↓
"Execute this SQL"
```

---

# 5. SqlParameter

Used to pass values safely to SQL commands.

Example:

```csharp
command.Parameters.AddWithValue("@Id", employeeId);
```

Better for explicit typing:

```csharp
command.Parameters.Add("@Id", SqlDbType.Int).Value = employeeId;
```

Think:

```text
SqlParameter
     ↓
"Send this value separately from the SQL text"
```

---

# 6. SqlDataReader

Used to read query results efficiently in a forward-only manner.

```csharp
using SqlDataReader reader =
    command.ExecuteReader();

while (reader.Read())
{
    Console.WriteLine(reader["EmployeeName"]);
}
```

Think:

```text
SqlDataReader
     ↓
SQL Server rows
     ↓
Read one row at a time
```

---

# 7. SqlDataAdapter

A `SqlDataAdapter` is commonly used to fill disconnected objects such as a `DataTable` or `DataSet`.

```csharp
SqlDataAdapter adapter =
    new SqlDataAdapter(command);

DataTable table = new DataTable();

adapter.Fill(table);
```

---

# 8. DataTable

`DataTable` represents an in-memory table.

Conceptually:

```text
SQL Server Table
       ↓
DataAdapter
       ↓
DataTable
       ↓
C# memory
```

Example:

```csharp
DataTable table = new DataTable();

adapter.Fill(table);

foreach (DataRow row in table.Rows)
{
    Console.WriteLine(row["EmployeeName"]);
}
```

---

# 9. DataSet

A `DataSet` can contain multiple `DataTable` objects.

Think:

```text
DataSet
│
├── DataTable → Employees
├── DataTable → Departments
└── DataTable → Salaries
```

Example:

```csharp
DataSet dataSet = new DataSet();

adapter.Fill(dataSet);
```

Then:

```csharp
DataTable employees =
    dataSet.Tables[0];

DataTable departments =
    dataSet.Tables[1];
```

---

# 10. DataReader vs DataSet/DataTable

### DataReader

```text
Connected
Forward-only
Read row by row
Fast and memory efficient
```

### DataSet/DataTable

```text
Disconnected
Stored in memory
Can contain multiple tables
Can navigate/manipulate data in memory
```

Simple memory:

```text
DataReader
→ Read directly from database connection

DataTable
→ One in-memory table

DataSet
→ Collection of in-memory tables
```

---

# 11. Install Microsoft.Data.SqlClient

For modern .NET applications, create your project in Visual Studio and install:

```text
Microsoft.Data.SqlClient
```

### Visual Studio

```text
Project
   ↓
Manage NuGet Packages
   ↓
Browse
   ↓
Microsoft.Data.SqlClient
   ↓
Install
```

Then:

```csharp
using Microsoft.Data.SqlClient;
```

---

# 12. Day 19 Visual Studio Project

Create:

```text
DotNetHandsOn
│
├── Day01DotNetIntroduction
├── ...
├── Day18SqlAdvancedPerformance
│
└── Day19AdoNet
```

For a Web API project:

```text
Day19AdoNet
│
├── Controllers
├── Models
├── Repositories
├── Services
├── Program.cs
├── appsettings.json
└── Day19AdoNet.csproj
```

For learning the raw ADO.NET fundamentals, you can also use a Console App.

---

# 13. Connection String

A connection string tells your application how to connect to SQL Server.

Example:

```text
Server=localhost;
Database=EmployeeDB;
Trusted_Connection=True;
TrustServerCertificate=True;
```

Or, depending on your SQL Server setup:

```text
Server=localhost\SQLEXPRESS;
Database=EmployeeDB;
Trusted_Connection=True;
TrustServerCertificate=True;
```

For SQL authentication:

```text
Server=localhost;
Database=EmployeeDB;
User Id=yourUser;
Password=yourPassword;
TrustServerCertificate=True;
```

Don't commit real passwords to Git.

---

# 14. Store Connection String in appsettings.json

Don't hard-code it throughout your repository classes.

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EmployeeDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Then access it through configuration/DI.

For example:

```csharp
string connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "DefaultConnection is not configured.");
```

---

# 15. SqlConnection

Basic example:

```csharp
using Microsoft.Data.SqlClient;

string connectionString =
    "Server=localhost;Database=EmployeeDB;Trusted_Connection=True;TrustServerCertificate=True";

using SqlConnection connection =
    new SqlConnection(connectionString);

connection.Open();

Console.WriteLine("Database connected successfully.");
```

Important:

```csharp
connection.Open();
```

opens the connection.

And:

```csharp
connection.Close();
```

closes it.

However, with:

```csharp
using SqlConnection connection = ...;
```

the connection is disposed automatically when it leaves scope.

---

# 16. Why `using`?

Database connections and commands use resources that should be released promptly.

Instead of:

```csharp
SqlConnection connection =
    new SqlConnection(connectionString);

connection.Open();

// work

connection.Close();
```

Prefer:

```csharp
using SqlConnection connection =
    new SqlConnection(connectionString);

connection.Open();

// work
```

or:

```csharp
using (SqlConnection connection =
       new SqlConnection(connectionString))
{
    connection.Open();

    // work
}
```

The `using` pattern ensures disposal even when an exception occurs.

---

# 17. ExecuteReader()

Use `ExecuteReader()` when the SQL query returns **rows**.

Example:

```csharp
string sql = """
    SELECT EmployeeId, EmployeeName, Salary
    FROM Employees;
    """;

using SqlConnection connection =
    new SqlConnection(connectionString);

using SqlCommand command =
    new SqlCommand(sql, connection);

connection.Open();

using SqlDataReader reader =
    command.ExecuteReader();

while (reader.Read())
{
    int id = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
    string name = reader.GetString(
        reader.GetOrdinal("EmployeeName"));

    decimal salary = reader.GetDecimal(
        reader.GetOrdinal("Salary"));

    Console.WriteLine(
        $"{id} - {name} - {salary}");
}
```

---

# 18. How DataReader Works

Suppose database returns:

```text
1 | Arun  | IT
2 | Priya | HR
3 | Kumar | Finance
```

`reader.Read()` moves through the rows:

```text
Read()
 ↓
Row 1

Read()
 ↓
Row 2

Read()
 ↓
Row 3

Read()
 ↓
false
```

Therefore:

```csharp
while (reader.Read())
{
    // Current row
}
```

---

# 19. Reading Columns

You can use:

```csharp
reader["EmployeeName"]
```

Example:

```csharp
string name =
    reader["EmployeeName"].ToString() ?? "";
```

But typed methods are often preferable when you know the column type:

```csharp
string name =
    reader.GetString(
        reader.GetOrdinal("EmployeeName"));
```

Other typed methods:

```text
GetInt32()
GetInt64()
GetString()
GetDecimal()
GetDateTime()
GetBoolean()
```

---

# 20. Handling NULL

Database values can be SQL `NULL`.

Don't blindly call:

```csharp
reader.GetString(...)
```

if the database column can be NULL.

Use:

```csharp
int ordinal =
    reader.GetOrdinal("City");

string? city =
    reader.IsDBNull(ordinal)
        ? null
        : reader.GetString(ordinal);
```

Important:

```text
SQL NULL
≠
C# empty string
≠
0
```

---

# 21. ExecuteScalar()

Use `ExecuteScalar()` when you need **one value**.

Example:

```sql
SELECT COUNT(*)
FROM Employees;
```

C#:

```csharp
string sql =
    "SELECT COUNT(*) FROM Employees";

using SqlConnection connection =
    new SqlConnection(connectionString);

using SqlCommand command =
    new SqlCommand(sql, connection);

connection.Open();

object? result =
    command.ExecuteScalar();

int count =
    Convert.ToInt32(result);

Console.WriteLine($"Employee Count: {count}");
```

---

# 22. Common ExecuteScalar Uses

### COUNT

```sql
SELECT COUNT(*)
FROM Employees;
```

### SUM

```sql
SELECT SUM(Salary)
FROM Employees;
```

### MAX

```sql
SELECT MAX(Salary)
FROM Employees;
```

### Generated ID

A command might return the newly generated ID:

```sql
SELECT CAST(SCOPE_IDENTITY() AS INT);
```

Then:

```csharp
int employeeId =
    Convert.ToInt32(command.ExecuteScalar());
```

---

# 23. ExecuteNonQuery()

Use `ExecuteNonQuery()` when you are executing commands such as:

```text
INSERT
UPDATE
DELETE
```

and don't need a row result set.

Example:

```csharp
string sql = """
    UPDATE Employees
    SET Salary = @Salary
    WHERE EmployeeId = @EmployeeId;
    """;

using SqlConnection connection =
    new SqlConnection(connectionString);

using SqlCommand command =
    new SqlCommand(sql, connection);

command.Parameters.Add(
    "@Salary",
    SqlDbType.Decimal).Value = 65000m;

command.Parameters.Add(
    "@EmployeeId",
    SqlDbType.Int).Value = 101;

connection.Open();

int rowsAffected =
    command.ExecuteNonQuery();

Console.WriteLine(
    $"Rows updated: {rowsAffected}");
```

---

# 24. Execute Methods — Easy Memory

```text
ExecuteReader()
     ↓
Multiple rows

ExecuteScalar()
     ↓
One value

ExecuteNonQuery()
     ↓
INSERT / UPDATE / DELETE
```

### Interview answer

> `ExecuteReader()` is used to retrieve a result set, `ExecuteScalar()` is used when a command returns a single value, and `ExecuteNonQuery()` is generally used for commands such as INSERT, UPDATE, and DELETE when you don't need a result set.

---

# 25. SQL Parameters

This is one of the **most important Day 19 topics**.

Never build SQL using user input like this:

```csharp
string sql =
    "SELECT * FROM Employees WHERE Name = '"
    + name
    + "'";
```

This is dangerous.

---

# 26. SQL Injection

Suppose user enters:

```text
Arun' OR '1'='1
```

The generated SQL could become:

```sql
SELECT *
FROM Employees
WHERE Name = 'Arun' OR '1'='1';
```

The condition:

```text
'1'='1'
```

is always true.

This is an example of **SQL injection**.

---

# 27. Parameterized Query

Instead:

```csharp
string sql = """
    SELECT EmployeeId, EmployeeName, Salary
    FROM Employees
    WHERE Name = @Name;
    """;

using SqlCommand command =
    new SqlCommand(sql, connection);

command.Parameters.Add(
    "@Name",
    SqlDbType.VarChar,
    100).Value = name;
```

Now:

```text
SQL structure
     +
parameter value
```

are handled separately.

---

# 28. Why Parameters Are Important

SQL parameters provide several important benefits:

### 1. Security

They help prevent SQL injection.

### 2. Correct handling of values

Quotes and special characters in data are handled as values rather than SQL syntax.

### 3. Type information

You can specify:

```csharp
SqlDbType.Int
SqlDbType.VarChar
SqlDbType.Decimal
SqlDbType.DateTime2
```

### 4. Better query handling

Parameterized commands can allow SQL Server/client infrastructure to reuse appropriate execution information where applicable.

---

# 29. Add Parameters Correctly

Prefer explicit parameter definitions when practical:

```csharp
command.Parameters.Add(
    "@EmployeeId",
    SqlDbType.Int).Value = employeeId;
```

For strings:

```csharp
command.Parameters.Add(
    "@Name",
    SqlDbType.VarChar,
    100).Value = name;
```

For decimal:

```csharp
SqlParameter salaryParameter =
    command.Parameters.Add(
        "@Salary",
        SqlDbType.Decimal);

salaryParameter.Precision = 10;
salaryParameter.Scale = 2;
salaryParameter.Value = salary;
```

---

# 30. `AddWithValue`

You may see:

```csharp
command.Parameters.AddWithValue(
    "@EmployeeId",
    employeeId);
```

It works, but explicit parameter types are often preferred because `AddWithValue` can infer a type/size that isn't ideal for the database schema.

Prefer:

```csharp
command.Parameters.Add(
    "@EmployeeId",
    SqlDbType.Int).Value = employeeId;
```

---

# 31. Complete Parameterized SELECT

```csharp
string sql = """
    SELECT
        EmployeeId,
        EmployeeName,
        DepartmentId,
        Salary
    FROM Employees
    WHERE DepartmentId = @DepartmentId;
    """;

using SqlConnection connection =
    new SqlConnection(connectionString);

using SqlCommand command =
    new SqlCommand(sql, connection);

command.Parameters.Add(
    "@DepartmentId",
    SqlDbType.Int).Value = 10;

connection.Open();

using SqlDataReader reader =
    command.ExecuteReader();

while (reader.Read())
{
    Console.WriteLine(
        $"{reader["EmployeeId"]} - " +
        $"{reader["EmployeeName"]}");
}
```

---

# 32. Parameterized INSERT

```csharp
string sql = """
    INSERT INTO Employees
    (
        EmployeeName,
        DepartmentId,
        Salary
    )
    VALUES
    (
        @EmployeeName,
        @DepartmentId,
        @Salary
    );
    """;

using SqlConnection connection =
    new SqlConnection(connectionString);

using SqlCommand command =
    new SqlCommand(sql, connection);

command.Parameters.Add(
    "@EmployeeName",
    SqlDbType.VarChar,
    100).Value = "Arun";

command.Parameters.Add(
    "@DepartmentId",
    SqlDbType.Int).Value = 10;

SqlParameter salaryParameter =
    command.Parameters.Add(
        "@Salary",
        SqlDbType.Decimal);

salaryParameter.Precision = 10;
salaryParameter.Scale = 2;
salaryParameter.Value = 50000m;

connection.Open();

int rows =
    command.ExecuteNonQuery();

Console.WriteLine(
    $"Rows inserted: {rows}");
```

---

# 33. Parameterized UPDATE

```csharp
string sql = """
    UPDATE Employees
    SET Salary = @Salary
    WHERE EmployeeId = @EmployeeId;
    """;

using SqlConnection connection =
    new SqlConnection(connectionString);

using SqlCommand command =
    new SqlCommand(sql, connection);

var salaryParameter =
    command.Parameters.Add(
        "@Salary",
        SqlDbType.Decimal);

salaryParameter.Precision = 10;
salaryParameter.Scale = 2;
salaryParameter.Value = 60000m;

command.Parameters.Add(
    "@EmployeeId",
    SqlDbType.Int).Value = 101;

connection.Open();

int rows =
    command.ExecuteNonQuery();

Console.WriteLine(
    $"Rows updated: {rows}");
```

---

# 34. Parameterized DELETE

```csharp
string sql = """
    DELETE FROM Employees
    WHERE EmployeeId = @EmployeeId;
    """;

using SqlConnection connection =
    new SqlConnection(connectionString);

using SqlCommand command =
    new SqlCommand(sql, connection);

command.Parameters.Add(
    "@EmployeeId",
    SqlDbType.Int).Value = 101;

connection.Open();

int rows =
    command.ExecuteNonQuery();

Console.WriteLine(
    $"Rows deleted: {rows}");
```

---

# 35. Stored Procedure Execution

ADO.NET can execute stored procedures.

Suppose SQL Server has:

```sql
CREATE PROCEDURE GetEmployeesByDepartment
    @DepartmentId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        EmployeeId,
        EmployeeName,
        DepartmentId,
        Salary
    FROM Employees
    WHERE DepartmentId = @DepartmentId;
END;
```

C#:

```csharp
using SqlConnection connection =
    new SqlConnection(connectionString);

using SqlCommand command =
    new SqlCommand(
        "GetEmployeesByDepartment",
        connection);

command.CommandType =
    CommandType.StoredProcedure;

command.Parameters.Add(
    "@DepartmentId",
    SqlDbType.Int).Value = 10;

connection.Open();

using SqlDataReader reader =
    command.ExecuteReader();

while (reader.Read())
{
    Console.WriteLine(
        $"{reader["EmployeeId"]} - " +
        $"{reader["EmployeeName"]}");
}
```

---

# 36. CommandType

By default:

```csharp
command.CommandType =
    CommandType.Text;
```

means the command contains SQL text.

Example:

```csharp
new SqlCommand(
    "SELECT * FROM Employees",
    connection);
```

For a stored procedure:

```csharp
command.CommandType =
    CommandType.StoredProcedure;
```

---

# 37. Stored Procedure with ExecuteNonQuery

Suppose:

```sql
CREATE PROCEDURE UpdateEmployeeSalary
    @EmployeeId INT,
    @Salary DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Employees
    SET Salary = @Salary
    WHERE EmployeeId = @EmployeeId;
END;
```

C#:

```csharp
using SqlCommand command =
    new SqlCommand(
        "UpdateEmployeeSalary",
        connection);

command.CommandType =
    CommandType.StoredProcedure;

command.Parameters.Add(
    "@EmployeeId",
    SqlDbType.Int).Value = 101;

var salaryParameter =
    command.Parameters.Add(
        "@Salary",
        SqlDbType.Decimal);

salaryParameter.Precision = 10;
salaryParameter.Scale = 2;
salaryParameter.Value = 65000m;

connection.Open();

int rows =
    command.ExecuteNonQuery();
```

---

# 38. Stored Procedure with Output Parameter

SQL:

```sql
CREATE PROCEDURE GetEmployeeCount
    @EmployeeCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @EmployeeCount = COUNT(*)
    FROM Employees;
END;
```

C#:

```csharp
using SqlCommand command =
    new SqlCommand(
        "GetEmployeeCount",
        connection);

command.CommandType =
    CommandType.StoredProcedure;

SqlParameter outputParameter =
    command.Parameters.Add(
        "@EmployeeCount",
        SqlDbType.Int);

outputParameter.Direction =
    ParameterDirection.Output;

connection.Open();

command.ExecuteNonQuery();

int count =
    Convert.ToInt32(outputParameter.Value);

Console.WriteLine(
    $"Employees: {count}");
```

---

# 39. ParameterDirection

Important values:

```text
Input
Output
InputOutput
ReturnValue
```

Example:

```csharp
outputParameter.Direction =
    ParameterDirection.Output;
```

---

# 40. Stored Procedure Return Value

Suppose SQL:

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
        RETURN 1;

    RETURN 0;
END;
```

C#:

```csharp
using SqlCommand command =
    new SqlCommand(
        "CheckEmployee",
        connection);

command.CommandType =
    CommandType.StoredProcedure;

command.Parameters.Add(
    "@EmployeeId",
    SqlDbType.Int).Value = 101;

SqlParameter returnParameter =
    command.Parameters.Add(
        "@ReturnValue",
        SqlDbType.Int);

returnParameter.Direction =
    ParameterDirection.ReturnValue;

connection.Open();

command.ExecuteNonQuery();

int result =
    Convert.ToInt32(returnParameter.Value);
```

---

# 41. ADO.NET Transactions

ADO.NET supports database transactions.

Example:

```csharp
using SqlConnection connection =
    new SqlConnection(connectionString);

connection.Open();

using SqlTransaction transaction =
    connection.BeginTransaction();

try
{
    using SqlCommand command1 =
        new SqlCommand(
            """
            UPDATE Accounts
            SET Balance = Balance - @Amount
            WHERE AccountId = @FromAccount;
            """,
            connection,
            transaction);

    command1.Parameters.Add(
        "@Amount",
        SqlDbType.Decimal).Value = 5000m;

    command1.Parameters.Add(
        "@FromAccount",
        SqlDbType.Int).Value = 1;

    command1.ExecuteNonQuery();

    using SqlCommand command2 =
        new SqlCommand(
            """
            UPDATE Accounts
            SET Balance = Balance + @Amount
            WHERE AccountId = @ToAccount;
            """,
            connection,
            transaction);

    command2.Parameters.Add(
        "@Amount",
        SqlDbType.Decimal).Value = 5000m;

    command2.Parameters.Add(
        "@ToAccount",
        SqlDbType.Int).Value = 2;

    command2.ExecuteNonQuery();

    transaction.Commit();

    Console.WriteLine("Transaction committed.");
}
catch
{
    transaction.Rollback();

    Console.WriteLine(
        "Transaction rolled back.");

    throw;
}
```

---

# 42. Important Transaction Rule

When using a transaction, the commands must use the same transaction:

```csharp
new SqlCommand(
    sql,
    connection,
    transaction);
```

Otherwise the command isn't participating in that explicit transaction.

---

# 43. Transaction Flow

```text
Connection.Open()
       ↓
BeginTransaction()
       ↓
Command 1
       ↓
Command 2
       ↓
Command 3
       ↓
   Everything OK?
     /       \
   YES       NO
    ↓         ↓
 COMMIT    ROLLBACK
```

---

# 44. DataAdapter

Now let's look at the disconnected model.

```csharp
string sql = """
    SELECT
        EmployeeId,
        EmployeeName,
        Salary
    FROM Employees;
    """;

using SqlConnection connection =
    new SqlConnection(connectionString);

using SqlCommand command =
    new SqlCommand(sql, connection);

using SqlDataAdapter adapter =
    new SqlDataAdapter(command);

DataTable table =
    new DataTable();

adapter.Fill(table);
```

Notice:

```text
Open()
```

was not explicitly called.

`SqlDataAdapter.Fill()` manages the required connection opening/closing when the adapter is given a closed connection.

---

# 45. Reading DataTable

```csharp
foreach (DataRow row in table.Rows)
{
    Console.WriteLine(
        $"{row["EmployeeId"]} - " +
        $"{row["EmployeeName"]} - " +
        $"{row["Salary"]}");
}
```

---

# 46. DataSet Example

Suppose we want employees and departments.

SQL:

```sql
SELECT
    EmployeeId,
    EmployeeName
FROM Employees;

SELECT
    DepartmentId,
    DepartmentName
FROM Departments;
```

C#:

```csharp
using SqlCommand command =
    new SqlCommand(sql, connection);

using SqlDataAdapter adapter =
    new SqlDataAdapter(command);

DataSet dataSet =
    new DataSet();

adapter.Fill(dataSet);
```

Then:

```csharp
DataTable employees =
    dataSet.Tables[0];

DataTable departments =
    dataSet.Tables[1];
```

Conceptually:

```text
DataSet
│
├── Tables[0]
│     └── Employees
│
└── Tables[1]
      └── Departments
```

---

# 47. DataReader vs DataAdapter

### DataReader

```text
Connected model
Forward-only
Read one row at a time
Usually lower memory usage
Good for streaming query results
```

### DataAdapter

```text
Disconnected model
Fills DataSet/DataTable
Useful for in-memory relational data
```

Memory:

```text
DataReader
→ Fast row-by-row reading

DataAdapter
→ Fill in-memory tables
```

---

# 48. DataTable vs DataSet

```text
DataTable
→ One table
```

```text
DataSet
→ Multiple DataTables
```

Example:

```text
DataSet
│
├── Employees
├── Departments
└── Salaries
```

---

# 49. ADO.NET Complete CRUD Example

Let's create a simple repository.

### Employee model

```csharp
public class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = "";

    public int DepartmentId { get; set; }

    public decimal Salary { get; set; }
}
```

---

# 50. Repository Interface

```csharp
public interface IEmployeeRepository
{
    List<Employee> GetEmployees();

    Employee? GetEmployeeById(int id);

    int AddEmployee(Employee employee);

    bool UpdateEmployee(Employee employee);

    bool DeleteEmployee(int id);
}
```

---

# 51. Get Employees

```csharp
public List<Employee> GetEmployees()
{
    var employees = new List<Employee>();

    const string sql = """
        SELECT
            EmployeeId,
            EmployeeName,
            DepartmentId,
            Salary
        FROM Employees;
        """;

    using SqlConnection connection =
        new SqlConnection(_connectionString);

    using SqlCommand command =
        new SqlCommand(sql, connection);

    connection.Open();

    using SqlDataReader reader =
        command.ExecuteReader();

    while (reader.Read())
    {
        employees.Add(new Employee
        {
            EmployeeId =
                reader.GetInt32(
                    reader.GetOrdinal("EmployeeId")),

            EmployeeName =
                reader.GetString(
                    reader.GetOrdinal("EmployeeName")),

            DepartmentId =
                reader.GetInt32(
                    reader.GetOrdinal("DepartmentId")),

            Salary =
                reader.GetDecimal(
                    reader.GetOrdinal("Salary"))
        });
    }

    return employees;
}
```

---

# 52. Get Employee by ID

```csharp
public Employee? GetEmployeeById(int id)
{
    const string sql = """
        SELECT
            EmployeeId,
            EmployeeName,
            DepartmentId,
            Salary
        FROM Employees
        WHERE EmployeeId = @EmployeeId;
        """;

    using SqlConnection connection =
        new SqlConnection(_connectionString);

    using SqlCommand command =
        new SqlCommand(sql, connection);

    command.Parameters.Add(
        "@EmployeeId",
        SqlDbType.Int).Value = id;

    connection.Open();

    using SqlDataReader reader =
        command.ExecuteReader();

    if (!reader.Read())
    {
        return null;
    }

    return new Employee
    {
        EmployeeId =
            reader.GetInt32(
                reader.GetOrdinal("EmployeeId")),

        EmployeeName =
            reader.GetString(
                reader.GetOrdinal("EmployeeName")),

        DepartmentId =
            reader.GetInt32(
                reader.GetOrdinal("DepartmentId")),

        Salary =
            reader.GetDecimal(
                reader.GetOrdinal("Salary"))
    };
}
```

---

# 53. Insert Employee

```csharp
public int AddEmployee(Employee employee)
{
    const string sql = """
        INSERT INTO Employees
        (
            EmployeeName,
            DepartmentId,
            Salary
        )
        OUTPUT INSERTED.EmployeeId
        VALUES
        (
            @EmployeeName,
            @DepartmentId,
            @Salary
        );
        """;

    using SqlConnection connection =
        new SqlConnection(_connectionString);

    using SqlCommand command =
        new SqlCommand(sql, connection);

    command.Parameters.Add(
        "@EmployeeName",
        SqlDbType.VarChar,
        100).Value = employee.EmployeeName;

    command.Parameters.Add(
        "@DepartmentId",
        SqlDbType.Int).Value = employee.DepartmentId;

    var salaryParameter =
        command.Parameters.Add(
            "@Salary",
            SqlDbType.Decimal);

    salaryParameter.Precision = 10;
    salaryParameter.Scale = 2;
    salaryParameter.Value = employee.Salary;

    connection.Open();

    return Convert.ToInt32(
        command.ExecuteScalar());
}
```

Here:

```text
ExecuteScalar()
      ↓
INSERT
      ↓
OUTPUT INSERTED.EmployeeId
      ↓
New EmployeeId
```

---

# 54. Update Employee

```csharp
public bool UpdateEmployee(Employee employee)
{
    const string sql = """
        UPDATE Employees
        SET
            EmployeeName = @EmployeeName,
            DepartmentId = @DepartmentId,
            Salary = @Salary
        WHERE EmployeeId = @EmployeeId;
        """;

    using SqlConnection connection =
        new SqlConnection(_connectionString);

    using SqlCommand command =
        new SqlCommand(sql, connection);

    command.Parameters.Add(
        "@EmployeeId",
        SqlDbType.Int).Value =
        employee.EmployeeId;

    command.Parameters.Add(
        "@EmployeeName",
        SqlDbType.VarChar,
        100).Value =
        employee.EmployeeName;

    command.Parameters.Add(
        "@DepartmentId",
        SqlDbType.Int).Value =
        employee.DepartmentId;

    var salaryParameter =
        command.Parameters.Add(
            "@Salary",
            SqlDbType.Decimal);

    salaryParameter.Precision = 10;
    salaryParameter.Scale = 2;
    salaryParameter.Value = employee.Salary;

    connection.Open();

    return command.ExecuteNonQuery() > 0;
}
```

---

# 55. Delete Employee

```csharp
public bool DeleteEmployee(int id)
{
    const string sql = """
        DELETE FROM Employees
        WHERE EmployeeId = @EmployeeId;
        """;

    using SqlConnection connection =
        new SqlConnection(_connectionString);

    using SqlCommand command =
        new SqlCommand(sql, connection);

    command.Parameters.Add(
        "@EmployeeId",
        SqlDbType.Int).Value = id;

    connection.Open();

    return command.ExecuteNonQuery() > 0;
}
```

---

# 56. ADO.NET in Your Layered Architecture

You have already learned:

```text
Controller
    ↓
Service / BAL
    ↓
Repository / DAL
    ↓
SQL Server
```

ADO.NET belongs primarily in the **DAL/Repository layer**.

```text
┌─────────────────────────────┐
│        Controller           │
│  HTTP / DTO / Response      │
└──────────────┬──────────────┘
               ↓
┌─────────────────────────────┐
│          Service            │
│      Business Logic         │
└──────────────┬──────────────┘
               ↓
┌─────────────────────────────┐
│        Repository           │
│          ADO.NET            │
│                             │
│ SqlConnection               │
│ SqlCommand                  │
│ SqlParameter                │
│ SqlDataReader               │
└──────────────┬──────────────┘
               ↓
┌─────────────────────────────┐
│        SQL Server           │
│                             │
│ Tables / SP / Views         │
└─────────────────────────────┘
```

This is exactly where your previous topics connect.

---

# 57. Connection Pooling

This is an important ADO.NET interview topic.

When you call:

```csharp
connection.Open();
```

ADO.NET/SqlClient can use **connection pooling**.

Conceptually:

```text
Application
     ↓
Open connection
     ↓
Connection Pool
     ↓
Existing physical connection
```

When disposed/closed:

```text
connection.Dispose()
       ↓
Connection returned to pool
```

It is not necessarily destroying the underlying physical connection every time.

Therefore:

> Open connections as late as practical and dispose them as soon as practical.

Don't keep a database connection open throughout the entire application lifetime.

---

# 58. Connection String vs SqlConnection

Don't confuse these.

### Connection String

```text
Server=localhost;
Database=EmployeeDB;
...
```

It's configuration text.

### SqlConnection

```csharp
SqlConnection connection =
    new SqlConnection(connectionString);
```

It's the C# object used to communicate with SQL Server.

---

# 59. Common ADO.NET Mistakes

### Mistake 1 — String concatenation

```csharp
string sql =
    "SELECT * FROM Employees WHERE Id = " + id;
```

Avoid this.

Use:

```csharp
string sql =
    "SELECT * FROM Employees WHERE Id = @Id";
```

---

### Mistake 2 — Hard-coding passwords

Don't:

```csharp
string connectionString =
    "Server=...;Password=MyPassword;";
```

in source code.

Use configuration/secret management.

---

### Mistake 3 — Not disposing connections

Avoid:

```csharp
SqlConnection connection =
    new SqlConnection(...);
```

without proper disposal.

Prefer:

```csharp
using SqlConnection connection =
    new SqlConnection(...);
```

---

### Mistake 4 — Keeping connection open unnecessarily

Don't do:

```text
Application starts
       ↓
Open DB connection
       ↓
Keep it open for hours
```

Open it when required and dispose it promptly.

---

### Mistake 5 — Using ExecuteReader for COUNT

Don't:

```csharp
command.ExecuteReader();
```

for:

```sql
SELECT COUNT(*)
```

Use:

```csharp
command.ExecuteScalar();
```

---

# 60. ADO.NET Method Selection

Remember this table:

```text
Need                         Method
────────────────────────────────────────
Multiple rows                ExecuteReader()
One value                    ExecuteScalar()
INSERT                       ExecuteNonQuery()
UPDATE                       ExecuteNonQuery()
DELETE                       ExecuteNonQuery()
Stored procedure + rows      ExecuteReader()
Stored procedure + one value ExecuteScalar()
Stored procedure modification ExecuteNonQuery()
```

---

# 61. ADO.NET vs Entity Framework Core

You will encounter both in .NET development.

### ADO.NET

You write SQL and explicitly handle:

```text
SqlConnection
SqlCommand
SqlParameter
SqlDataReader
```

### EF Core

ORM handles much of the database interaction:

```csharp
var employees =
    await dbContext.Employees.ToListAsync();
```

Conceptually:

```text
ADO.NET
→ Lower-level database access
→ Explicit SQL/control

EF Core
→ ORM
→ Higher-level abstraction
→ Uses database providers underneath
```

For your roadmap, learning ADO.NET first is useful because it makes the underlying database interaction clear.

---

# 62. Day 19 Interview Questions

### Fundamentals

**1. What is ADO.NET?**

ADO.NET is a .NET data-access technology used to communicate with relational databases.

---

**2. What is SqlConnection?**

It represents a connection to SQL Server.

---

**3. What is SqlCommand?**

It represents a SQL statement or stored procedure to execute against the database.

---

**4. What is SqlParameter?**

It represents a parameter value passed separately from SQL command text.

---

**5. What is SqlDataReader?**

A forward-only reader used to efficiently read rows returned from a database command.

---

**6. What is DataTable?**

An in-memory representation of a table.

---

**7. What is DataSet?**

An in-memory collection of `DataTable` objects and their related data structures.

---

### Execute Methods

**8. Difference between ExecuteReader and ExecuteScalar?**

```text
ExecuteReader
→ multiple rows/result set

ExecuteScalar
→ first column of first row
```

---

**9. What is ExecuteNonQuery?**

Used for commands such as INSERT, UPDATE, and DELETE when a result set isn't required.

It returns the number of rows affected for typical DML commands.

---

### Security

**10. Why use SQL parameters?**

They separate SQL code from data values and help prevent SQL injection.

---

**11. Why is string concatenation dangerous?**

Because untrusted input can alter the intended SQL command structure and enable SQL injection.

---

### Data Access

**12. DataReader vs DataSet?**

```text
DataReader
→ Connected
→ Forward-only
→ Row-by-row
→ Efficient for reading

DataSet
→ Disconnected
→ In-memory
→ Multiple DataTables
```

---

**13. What is connection pooling?**

A mechanism that reuses database connections to reduce the overhead of repeatedly creating physical connections.

---

**14. Where should ADO.NET code normally be placed in a layered application?**

Typically in the DAL/repository layer.

---

**15. Why should connection strings not be hard-coded?**

To separate configuration from code and avoid exposing sensitive credentials.

---

# 63. Day 19 Practical Exercises

You should implement these in your `Day19AdoNet` project.

### Exercise 1

Connect to:

```text
EmployeeDB
```

and print:

```text
Database connected successfully
```

### Exercise 2

Use `ExecuteReader()`:

```text
Get all employees
```

### Exercise 3

Use `ExecuteReader()`:

```text
Get employees by department
```

using a parameter.

### Exercise 4

Use `ExecuteScalar()`:

```text
Get employee count
```

### Exercise 5

Use `ExecuteScalar()`:

```text
Get maximum salary
```

### Exercise 6

Use `ExecuteNonQuery()`:

```text
Insert employee
```

### Exercise 7

Use `ExecuteNonQuery()`:

```text
Update employee salary
```

### Exercise 8

Use `ExecuteNonQuery()`:

```text
Delete employee
```

### Exercise 9

Create:

```text
GetEmployeesByDepartment
```

stored procedure.

Call it using:

```csharp
CommandType.StoredProcedure
```

### Exercise 10

Create:

```text
GetEmployeeCount
```

with an output parameter.

### Exercise 11

Create a transaction:

```text
Account A
   ↓
- ₹5000

Account B
   ↓
+ ₹5000
```

If the second operation fails:

```text
ROLLBACK
```

### Exercise 12

Create a `DataTable` using `SqlDataAdapter`.

### Exercise 13

Create a `DataSet` containing:

```text
Employees
Departments
```

---

# 64. Day 19 Final Mental Model

```text
                    ADO.NET
                       │
          ┌────────────┼────────────┐
          ↓            ↓            ↓
     Connection     Command      Parameter
          │            │            │
          └────────────┼────────────┘
                       ↓
                  SQL Server
                       │
              ┌────────┴────────┐
              ↓                 ↓
        ExecuteReader      ExecuteScalar
              │                 │
              ↓                 ↓
        SqlDataReader       One Value
              │
              ↓
        Rows / Models
```

And:

```text
INSERT / UPDATE / DELETE
          ↓
  ExecuteNonQuery()
```

For disconnected data:

```text
SqlCommand
    ↓
SqlDataAdapter
    ↓
DataSet / DataTable
    ↓
Application memory
```

For stored procedures:

```text
SqlCommand
    ↓
CommandType.StoredProcedure
    ↓
SqlParameter
    ↓
Stored Procedure
    ↓
SQL Server
```

For transactions:

```text
SqlConnection
     ↓
BeginTransaction()
     ↓
Command 1
     ↓
Command 2
     ↓
Command 3
     ↓
 ┌───┴────┐
 ↓        ↓
Commit   Rollback
```

## 🔥 Day 19 — What you must remember

```text
SqlConnection
→ Connect to SQL Server

SqlCommand
→ Execute SQL / Stored Procedure

SqlParameter
→ Safely pass values

SqlDataReader
→ Read rows efficiently

SqlDataAdapter
→ Fill disconnected data

DataTable
→ One in-memory table

DataSet
→ Multiple in-memory tables

ExecuteReader()
→ Rows

ExecuteScalar()
→ One value

ExecuteNonQuery()
→ INSERT / UPDATE / DELETE

Parameterized SQL
→ Security + correct value handling

Transaction
→ Commit / Rollback

Connection Pooling
→ Reuse connections efficiently
```

The most important practical rule from Day 19 is:

```text
❌ SQL + user input through string concatenation

"SELECT * FROM Employees WHERE Name = '" + name + "'"

                    ↓

             SQL INJECTION
```

Instead:

```text
✅ SQL + parameter

SELECT *
FROM Employees
WHERE Name = @Name

                    +

command.Parameters.Add("@Name", ...)

                    ↓

              SAFE DATA ACCESS
```


