
using EmployeeManagementApi.DTOs;
using EmployeeManagementApi.Models;

namespace EmployeeManagementApi.Services;

public interface IEmployeeService
{
    List<Employee> GetAll(string? department);

    Employee? GetById(int id);

    List<Employee> SearchByName(string name);

    Employee Create(CreateEmployeeDto dto);

    Employee? Update(int id, UpdateEmployeeDto dto);

    Employee? Patch(int id, PatchEmployeeDto dto);

    bool Delete(int id);

    int ImportExternalUsers(List<ExternalUserDto> users);
}
