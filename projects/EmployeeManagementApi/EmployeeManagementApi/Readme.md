# Employee Management API

A beginner-friendly **ASP.NET Core Web API built with .NET 10 and C#**. This project demonstrates REST API fundamentals and employee CRUD operations without using a database.

## Features

- Get all employees
- Get an employee by ID
- Filter employees by department
- Search employees by name
- Create an employee
- Update an employee
- Partially update an employee
- Delete an employee
- Import sample users from the external DummyJSON API
- Request-body validation using Data Annotations
- Dependency injection for the employee service
- HTTP request testing using Visual Studio's `.http` file or Postman
- OpenAPI document in Development mode

## Technology Stack

- C#
- .NET 10 / ASP.NET Core Web API
- In-memory `List<Employee>` (no database)
- `HttpClient` for calling the external sample API
- Visual Studio
- DummyJSON: <https://dummyjson.com/users?limit=30>

## Project Structure

```text
EmployeeManagementApi/
├── Controllers/
│   └── EmployeesController.cs
├── DTOs/
│   ├── CreateEmployeeDto.cs
│   ├── UpdateEmployeeDto.cs
│   ├── PatchEmployeeDto.cs
│   └── ExternalUsersResponseDto.cs
├── Models/
│   └── Employee.cs
├── Services/
│   ├── IEmployeeService.cs
│   └── EmployeeService.cs
├── Properties/
│   └── launchSettings.json
├── EmployeeManagementApi.http
├── Program.cs
└── README.md
```

> The exact folders and files may vary slightly depending on how the project was created in Visual Studio.

## Prerequisites

- Visual Studio with the **ASP.NET and web development** workload
- .NET 10 SDK
- Internet access for the optional external-user import feature

## Run the Project

1. Open `EmployeeManagementApi.sln` or the project file (`EmployeeManagementApi.csproj`) in Visual Studio.
2. Ensure the project targets `net10.0`.
3. Build the project using **Build → Build Solution**.
4. Start the project with **F5** or the green Run button.
5. Open `EmployeeManagementApi.http` and select **Send Request** above a request.

The sample requests below use `http://localhost:5264`. Your port may differ. Check `Properties/launchSettings.json` or the URL shown when the application starts, then update `EmployeeManagementApi_HostAddress` in the `.http` file.

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/employees` | Get all employees |
| GET | `/api/employees/{id}` | Get an employee by ID |
| GET | `/api/employees?department=IT` | Filter employees by department |
| GET | `/api/employees/search?name=arun` | Search employees by name |
| POST | `/api/employees` | Create an employee |
| PUT | `/api/employees/{id}` | Replace/update an employee's editable fields |
| PATCH | `/api/employees/{id}` | Partially update an employee |
| DELETE | `/api/employees/{id}` | Delete an employee |
| POST | `/api/employees/import-external` | Import sample users from DummyJSON |

### 1. Get all employees

```http
GET http://localhost:5264/api/employees
Accept: application/json
```

### 2. Get an employee by ID

```http
GET http://localhost:5264/api/employees/1
Accept: application/json
```

### 3. Filter by department

```http
GET http://localhost:5264/api/employees?department=IT
Accept: application/json
```

### 4. Search by name

```http
GET http://localhost:5264/api/employees/search?name=arun
Accept: application/json
```

### 5. Create an employee

```http
POST http://localhost:5264/api/employees
Content-Type: application/json

{
  "name": "Arun Kumar",
  "email": "arun@example.com",
  "department": "IT",
  "jobTitle": "Software Developer"
}
```

### 6. Update an employee

```http
PUT http://localhost:5264/api/employees/1
Content-Type: application/json

{
  "name": "Arun Kumar",
  "email": "arun.updated@example.com",
  "department": "IT",
  "jobTitle": "Senior Software Developer"
}
```

### 7. Partially update an employee

```http
PATCH http://localhost:5264/api/employees/1
Content-Type: application/json

{
  "jobTitle": "Technical Lead"
}
```

### 8. Delete an employee

```http
DELETE http://localhost:5264/api/employees/1
```

### 9. Import external sample users

```http
POST http://localhost:5264/api/employees/import-external
Accept: application/json
```

This endpoint calls `https://dummyjson.com/users?limit=30`, maps the returned sample users to employee records, and adds them to the local in-memory collection. The application must have internet access for this operation.

## Where Does the Data Come From?

This project has **no database**. It uses two sources:

1. **Requests you send:** The JSON body of a `POST`, `PUT`, or `PATCH` request supplies the values for employee records.
2. **DummyJSON:** The import endpoint fetches public sample user data from DummyJSON and maps it into employee records.

The employee service stores records in an in-memory list while the API is running. Imported users are copied into the local list and receive locally generated employee IDs. The API does not save changes back to DummyJSON.

### Important limitations

- All in-memory employee records are lost when the application restarts.
- An empty list on the first run is expected; create an employee or run the import endpoint.
- The external import requires internet access and the external service to be available.
- Use an employee ID returned by `GET /api/employees` when testing update, patch, or delete requests. An ID that does not exist returns `404 Not Found`.
- The sample host/port may differ on your computer.

## How the Request Flows

```text
HTTP request (.http file or Postman)
            |
            v
EmployeesController
            |
            v
IEmployeeService / EmployeeService
            |
            v
In-memory List<Employee>
            |
            v
JSON HTTP response
```

- **Model (`Employee.cs`):** describes an employee.
- **DTOs:** describe and validate the data accepted by endpoints or received from DummyJSON.
- **Controller:** defines routes, receives requests, and returns HTTP responses.
- **Service interface (`IEmployeeService.cs`):** defines the operations available to the controller.
- **Service implementation (`EmployeeService.cs`):** performs filtering and CRUD operations and imports external sample data.
- **`Program.cs`:** registers services and configures the application pipeline.

## OpenAPI

In the supplied setup, the OpenAPI document is mapped in Development mode. When the app is running in Development, try:

```text
http://localhost:5264/openapi/v1.json
```

Replace the port with your actual application port. This endpoint serves the OpenAPI JSON document; a Swagger UI page is not necessarily installed or configured by default.

## Suggested Next Improvements

- Add pagination and sorting
- Add duplicate-email checks when creating or updating employees
- Add centralized exception handling and consistent `ProblemDetails` responses
- Add unit tests for `EmployeeService`
- Separate departments into their own API resources
- Add authentication and authorization (for example, JWT bearer authentication)
- Add persistent storage later, such as SQL Server, if data must survive restarts
- Add logging and rate limiting

## Learning Goals

Use this project to practise REST conventions, HTTP methods and status codes, attribute routing, model binding, DTOs, validation, dependency injection, `IHttpClientFactory`, JSON serialization, and API testing.

## License

This project is intended for learning and practice. Add a license file if you plan to distribute it publicly.
