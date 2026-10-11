
using System.Text.Json.Serialization;

namespace EmployeeManagementApi.DTOs;

public class ExternalUsersResponseDto
{
    [JsonPropertyName("users")]
    public List<ExternalUserDto> Users { get; set; } = [];

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("skip")]
    public int Skip { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }
}

public class ExternalUserDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("company")]
    public ExternalCompanyDto? Company { get; set; }
}

public class ExternalCompanyDto
{
    [JsonPropertyName("department")]
    public string? Department { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }
}
