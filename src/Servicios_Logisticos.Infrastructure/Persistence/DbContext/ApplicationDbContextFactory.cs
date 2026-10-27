using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.Json;

namespace Servicios_Logisticos.Infrastructure.Persistence.DbContext;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // 1. Variable de entorno
        string? connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

        // 2. Si no está en variable de entorno, buscar en appsettings.Development.json / appsettings.Local.json / appsettings.json
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            var current = Directory.GetCurrentDirectory();
            var searchPaths = new[]
            {
                Path.Combine(current, "src", "Servicios_Logisticos.Presentation"),
                Path.Combine(current, "Servicios_Logisticos.Presentation"),
                current
            };

            foreach (var path in searchPaths)
            {
                // Buscar archivos locales/de desarrollo primero
                var devFiles = new[]
                {
                    Path.Combine(path, "appsettings.Local.json"),
                    Path.Combine(path, "appsettings.Development.json"),
                    Path.Combine(path, "appsettings.json")
                };

                foreach (var file in devFiles)
                {
                    if (File.Exists(file))
                    {
                        var cs = TryExtractConnectionString(file);
                        if (!string.IsNullOrWhiteSpace(cs) && !cs.Contains("tu_usuario"))
                        {
                            connectionString = cs;
                            break;
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(connectionString))
                    break;
            }
        }

        connectionString ??= "Server=localhost;Database=ServiciosLogisticosDB;User Id=tu_usuario;Password=tu_contraseña;TrustServerCertificate=True;";

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }

    private static string? TryExtractConnectionString(string filePath)
    {
        try
        {
            var json = File.ReadAllText(filePath);
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("ConnectionStrings", out var csElement) &&
                csElement.TryGetProperty("DefaultConnection", out var defConn))
            {
                return defConn.GetString();
            }
        }
        catch
        {
            // Fallback silencioso si no puede parsear el archivo
        }

        return null;
    }
}
