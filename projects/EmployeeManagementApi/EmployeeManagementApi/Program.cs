
using EmployeeManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Register controller support.
builder.Services.AddControllers();

// Register our in-memory employee service as a singleton.
// Data remains available while this application process is running.
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();

// Register HttpClientFactory for external API calls.
builder.Services.AddHttpClient();

// Add built-in OpenAPI document generation.
builder.Services.AddOpenApi();

var app = builder.Build();

// Serve the OpenAPI document during development.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
