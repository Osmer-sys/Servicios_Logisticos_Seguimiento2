using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicios_Logisticos.Domain.Entities;

namespace Servicios_Logisticos.Infrastructure.Persistence.Configurations;

public class AccessLogConfiguration : IEntityTypeConfiguration<AccessLog>
{
    public void Configure(EntityTypeBuilder<AccessLog> builder)
    {
        builder.ToTable("AccessLogs");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .ValueGeneratedOnAdd();

        builder.Property(l => l.CredentialCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(l => l.MovementType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(l => l.Timestamp)
            .IsRequired();

        builder.Property(l => l.IsGranted)
            .IsRequired();

        builder.Property(l => l.RejectionReason)
            .HasMaxLength(250);

        builder.HasOne(l => l.Employee)
            .WithMany()
            .HasForeignKey(l => l.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(l => l.AccessPoint)
            .WithMany()
            .HasForeignKey(l => l.AccessPointId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(l => l.Timestamp);
        builder.HasIndex(l => l.CredentialCode);
        builder.HasIndex(l => l.EmployeeId);
        builder.HasIndex(l => l.AccessPointId);
        builder.HasIndex(l => l.IsGranted);
    }
}
