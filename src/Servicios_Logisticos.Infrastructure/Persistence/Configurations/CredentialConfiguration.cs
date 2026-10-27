using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicios_Logisticos.Domain.Entities;
using Servicios_Logisticos.Domain.Enums;
using Servicios_Logisticos.Domain.ValueObjects;

namespace Servicios_Logisticos.Infrastructure.Persistence.Configurations;

public class CredentialConfiguration : IEntityTypeConfiguration<Credential>
{
    public void Configure(EntityTypeBuilder<Credential> builder)
    {
        builder.ToTable("Credentials");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Code)
            .HasConversion(
                code => code.Value,
                value => new CredentialCode(value))
            .HasColumnName("Code")
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.Code)
            .IsUnique();

        builder.Property(c => c.EmployeeId)
            .IsRequired();

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(c => c.AssignedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        builder.Property(c => c.Notes)
            .HasMaxLength(250);

        // Seed data
        builder.HasData(
            new
            {
                Id = 1,
                Code = new CredentialCode("CRD-2024-001"),
                EmployeeId = 1,
                Status = CredentialStatus.Activa,
                AssignedAt = new DateTime(2024, 1, 15, 8, 30, 0, DateTimeKind.Utc),
                Notes = "Tarjeta RFID Principal asignada al Supervisor"
            },
            new
            {
                Id = 2,
                Code = new CredentialCode("CRD-2024-002"),
                EmployeeId = 2,
                Status = CredentialStatus.Activa,
                AssignedAt = new DateTime(2024, 2, 1, 8, 30, 0, DateTimeKind.Utc),
                Notes = "Badge de proximidad para Bodega"
            },
            new
            {
                Id = 3,
                Code = new CredentialCode("CRD-2024-003"),
                EmployeeId = 1,
                Status = CredentialStatus.Bloqueada, // Para probar rechazo por credencial bloqueada
                AssignedAt = new DateTime(2024, 2, 20, 10, 0, 0, DateTimeKind.Utc),
                Notes = "Tarjeta temporal reportada extraviada"
            },
            new
            {
                Id = 4,
                Code = new CredentialCode("CRD-2024-004"),
                EmployeeId = 3,
                Status = CredentialStatus.Activa, // Credencial activa pero empleado inactivo
                AssignedAt = new DateTime(2024, 3, 10, 8, 30, 0, DateTimeKind.Utc),
                Notes = "Asignada a empleado inactivo para validación de RF-03"
            }
        );
    }
}
