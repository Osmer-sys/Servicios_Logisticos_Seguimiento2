using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicios_Logisticos.Domain.Entities;
using Servicios_Logisticos.Domain.Enums;
using Servicios_Logisticos.Domain.ValueObjects;

namespace Servicios_Logisticos.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.DocumentNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(e => e.DocumentNumber)
            .IsUnique();

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .HasConversion(
                email => email.Value,
                value => new Email(value))
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Position)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Department)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt);

        builder.HasMany(e => e.Credentials)
            .WithOne(c => c.Employee)
            .HasForeignKey(c => c.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data
        builder.HasData(
            new
            {
                Id = 1,
                DocumentNumber = "1001001001",
                FirstName = "Carlos",
                LastName = "Gómez Pérez",
                Email = new Email("carlos.gomez@servicioslogisticos.com"),
                Position = "Supervisor de Almacén",
                Department = "Operaciones Logísticas",
                Status = EmploymentStatus.Activo,
                CreatedAt = new DateTime(2024, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = 2,
                DocumentNumber = "1002002002",
                FirstName = "María",
                LastName = "Rodríguez Silva",
                Email = new Email("maria.rodriguez@servicioslogisticos.com"),
                Position = "Operadora de Montacargas",
                Department = "Bodega Central",
                Status = EmploymentStatus.Activo,
                CreatedAt = new DateTime(2024, 2, 1, 8, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = 3,
                DocumentNumber = "1003003003",
                FirstName = "Andrés",
                LastName = "Martínez Ruiz",
                Email = new Email("andres.martinez@servicioslogisticos.com"),
                Position = "Auxiliar de Despacho",
                Department = "Distribución y Transporte",
                Status = EmploymentStatus.Inactivo, // Para probar accesos rechazados por empleado inactivo
                CreatedAt = new DateTime(2024, 3, 10, 8, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
