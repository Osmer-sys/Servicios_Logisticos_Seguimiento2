using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Servicios_Logisticos.Application.Extensions;
using Servicios_Logisticos.Infrastructure.Persistence.Extensions;
using Servicios_Logisticos.Presentation.Common;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1. Registro de Servicios por Capas (Clean Architecture)
// ============================================================

// Capa de Aplicación: Mediador y Handlers de Commands y Queries (CQRS)
builder.Services.AddApplicationServices();

// Capa de Infraestructura: DbContext SQL Server y Repositorios
builder.Services.AddPersistenceServices(builder.Configuration);

// Controladores con filtro global de excepciones y serialización de Enums como Strings
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// ============================================================
// 2. Documentación OpenAPI / Swagger
// ============================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema de Gestión y Control de Acceso de Empleados (SGCAE)",
        Version = "v1",
        Description = "API REST empresarial para el control de acceso del personal a instalaciones logísticas y de almacenamiento. " +
                      "Implementada bajo Clean Architecture, Domain-Driven Design (DDD) y CQRS sobre ASP.NET Core 8 y SQL Server."
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// ============================================================
// 3. Pipeline HTTP
// ============================================================
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "SGCAE API v1");
        options.RoutePrefix = string.Empty; // Swagger expuesto en la raíz
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
