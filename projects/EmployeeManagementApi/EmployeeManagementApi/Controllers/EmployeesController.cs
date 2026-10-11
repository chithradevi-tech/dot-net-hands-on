
using EmployeeManagementApi.DTOs;
using EmployeeManagementApi.Models;
using EmployeeManagementApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace EmployeeManagementApi.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(
        IEmployeeService employeeService,
        IHttpClientFactory httpClientFactory,
        ILogger<EmployeesController> logger)
    {
        _employeeService = employeeService;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    // GET: api/employees
    // GET: api/employees?department=IT
    [HttpGet]
    public ActionResult<IEnumerable<Employee>> GetAll(
        [FromQuery] string? department)
    {
        var employees = _employeeService.GetAll(department);
        return Ok(employees);
    }

    // GET: api/employees/1
    [HttpGet("{id:int}")]
    public ActionResult<Employee> GetById(int id)
    {
        var employee = _employeeService.GetById(id);

        if (employee is null)
            return NotFound(new { message = "Employee not found." });

        return Ok(employee);
    }

    // GET: api/employees/search?name=arun
    [HttpGet("search")]
    public ActionResult<IEnumerable<Employee>> Search(
        [FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest(new { message = "Name is required." });

        return Ok(_employeeService.SearchByName(name.Trim()));
    }

    // POST: api/employees
    [HttpPost]
    public ActionResult<Employee> Create(CreateEmployeeDto dto)
    {
        var employee = _employeeService.Create(dto);

        _logger.LogInformation(
            "Created employee with ID {EmployeeId}",
            employee.Id);

        return CreatedAtAction(
            nameof(GetById),
            new { id = employee.Id },
            employee);
    }

    // PUT: api/employees/1
    [HttpPut("{id:int}")]
    public ActionResult<Employee> Update(
        int id,
        UpdateEmployeeDto dto)
    {
        var employee = _employeeService.Update(id, dto);

        if (employee is null)
            return NotFound(new { message = "Employee not found." });

        return Ok(employee);
    }

    // PATCH: api/employees/1
    [HttpPatch("{id:int}")]
    public ActionResult<Employee> Patch(
        int id,
        PatchEmployeeDto dto)
    {
        var employee = _employeeService.Patch(id, dto);

        if (employee is null)
            return NotFound(new { message = "Employee not found." });

        return Ok(employee);
    }

    // DELETE: api/employees/1
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        bool deleted = _employeeService.Delete(id);

        if (!deleted)
            return NotFound(new { message = "Employee not found." });

        return NoContent();
    }

    // POST: api/employees/import-external
    [HttpPost("import-external")]
    public async Task<ActionResult<object>> ImportExternalUsers(
        CancellationToken cancellationToken)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<ExternalUsersResponseDto>(
                "https://dummyjson.com/users?limit=30",
                cancellationToken);

            if (response is null)
            {
                return StatusCode(502, new
                {
                    message = "External API returned an empty response."
                });
            }

            int imported = _employeeService.ImportExternalUsers(
                response.Users);

            _logger.LogInformation(
                "Imported {ImportedCount} external users",
                imported);

            return Ok(new
            {
                message = "External import completed.",
                received = response.Users.Count,
                imported,
                skipped = response.Users.Count - imported,
                totalEmployees = _employeeService.GetAll(null).Count
            });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "External employee import failed.");

            return StatusCode(502, new
            {
                message = "Could not retrieve data from the external API."
            });
        }
        catch (System.Text.Json.JsonException ex)
        {
            _logger.LogError(ex, "External API returned invalid JSON.");

            return StatusCode(502, new
            {
                message = "The external API response could not be processed."
            });
        }
    }
}
