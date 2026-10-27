using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicios_Logisticos.Domain.Entities;

namespace Servicios_Logisticos.Infrastructure.Persistence.Configurations;

public class AccessPointConfiguration : IEntityTypeConfiguration<AccessPoint>
{
    public void Configure(EntityTypeBuilder<AccessPoint> builder)
    {
        builder.ToTable("AccessPoints");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(p => p.Name)
            .IsUnique();

        builder.Property(p => p.Description)
            .HasMaxLength(250);

        builder.Property(p => p.IsEnabled)
            .IsRequired();

        builder.Property(p => p.IsRestrictedArea)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt);

        // Seed data (RF-06)
        builder.HasData(
            new
            {
                Id = 1,
                Name = "Recepción Principal",
                Description = "Torniquete de acceso peatonal principal a instalaciones administrativas",
                IsEnabled = true,
                IsRestrictedArea = false,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = 2,
                Name = "Bodega Central de Almacenamiento",
                Description = "Puerta industrial con sensor de proximidad de zona de almacenamiento",
                IsEnabled = true,
                IsRestrictedArea = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = 3,
                Name = "Parqueadero de Carga Pesada",
                Description = "Talancón automatizado de ingreso vehicular logístico",
                IsEnabled = true,
                IsRestrictedArea = false,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = 4,
                Name = "Área Restringida - Servidores y CCTV",
                Description = "Puerta blindada con cerradura biométrica y tarjeta para personal IT",
                IsEnabled = true,
                IsRestrictedArea = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = 5,
                Name = "Muelle 4 (En Mantenimiento)",
                Description = "Punto de acceso inhabilitado por obras de modernización",
                IsEnabled = false, // Para probar rechazo por punto de acceso inhabilitado
                IsRestrictedArea = false,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
