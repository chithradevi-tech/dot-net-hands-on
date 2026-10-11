
using EmployeeManagementApi.DTOs;
using EmployeeManagementApi.Models;

namespace EmployeeManagementApi.Services;

public class EmployeeService : IEmployeeService
{
    private readonly List<Employee> _employees = [];
    private readonly object _lock = new();
    private int _nextId = 1;

    public List<Employee> GetAll(string? department)
    {
        lock (_lock)
        {
            IEnumerable<Employee> result = _employees;

            if (!string.IsNullOrWhiteSpace(department))
            {
                result = result.Where(e =>
                    e.Department.Equals(
                        department,
                        StringComparison.OrdinalIgnoreCase));
            }

            return result
                .OrderBy(e => e.Id)
                .Select(Copy)
                .ToList();
        }
    }

    public Employee? GetById(int id)
    {
        lock (_lock)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            return employee is null ? null : Copy(employee);
        }
    }

    public List<Employee> SearchByName(string name)
    {
        lock (_lock)
        {
            return _employees
                .Where(e => e.Name.Contains(
                    name,
                    StringComparison.OrdinalIgnoreCase))
                .OrderBy(e => e.Id)
                .Select(Copy)
                .ToList();
        }
    }

    public Employee Create(CreateEmployeeDto dto)
    {
        lock (_lock)
        {
            var employee = new Employee
            {
                Id = _nextId++,
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim(),
                Department = dto.Department.Trim(),
                JobTitle = dto.JobTitle.Trim()
            };

            _employees.Add(employee);

            return Copy(employee);
        }
    }

    public Employee? Update(int id, UpdateEmployeeDto dto)
    {
        lock (_lock)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);

            if (employee is null)
                return null;

            employee.Name = dto.Name.Trim();
            employee.Email = dto.Email.Trim();
            employee.Department = dto.Department.Trim();
            employee.JobTitle = dto.JobTitle.Trim();

            return Copy(employee);
        }
    }

    public Employee? Patch(int id, PatchEmployeeDto dto)
    {
        lock (_lock)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);

            if (employee is null)
                return null;

            if (dto.Name is not null)
                employee.Name = dto.Name.Trim();

            if (dto.Email is not null)
                employee.Email = dto.Email.Trim();

            if (dto.Department is not null)
                employee.Department = dto.Department.Trim();

            if (dto.JobTitle is not null)
                employee.JobTitle = dto.JobTitle.Trim();

            return Copy(employee);
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);

            if (employee is null)
                return false;

            return _employees.Remove(employee);
        }
    }

    public int ImportExternalUsers(List<ExternalUserDto> users)
    {
        lock (_lock)
        {
            int imported = 0;

            foreach (var user in users)
            {
                // Avoid importing the same email twice.
                if (string.IsNullOrWhiteSpace(user.Email))
                    continue;

                bool exists = _employees.Any(e =>
                    e.Email.Equals(
                        user.Email,
                        StringComparison.OrdinalIgnoreCase));

                if (exists)
                    continue;

                var employee = new Employee
                {
                    Id = _nextId++,
                    Name = $"{user.FirstName} {user.LastName}".Trim(),
                    Email = user.Email.Trim(),
                    Department = string.IsNullOrWhiteSpace(
                        user.Company?.Department)
                        ? "General"
                        : user.Company.Department.Trim(),
                    JobTitle = string.IsNullOrWhiteSpace(
                        user.Company?.Title)
                        ? "Not specified"
                        : user.Company.Title.Trim()
                };

                _employees.Add(employee);
                imported++;
            }

            return imported;
        }
    }

    private static Employee Copy(Employee employee)
    {
        return new Employee
        {
            Id = employee.Id,
            Name = employee.Name,
            Email = employee.Email,
            Department = employee.Department,
            JobTitle = employee.JobTitle
        };
    }
}
