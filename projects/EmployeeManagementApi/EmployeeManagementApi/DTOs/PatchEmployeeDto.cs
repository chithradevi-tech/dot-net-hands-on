
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementApi.DTOs;

public class PatchEmployeeDto
{
    [StringLength(100, MinimumLength = 2)]
    public string? Name { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Department { get; set; }

    [StringLength(100)]
    public string? JobTitle { get; set; }
}
