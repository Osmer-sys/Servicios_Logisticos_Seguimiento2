using Servicios_Logisticos.Domain.Enums;
using Servicios_Logisticos.Domain.Exceptions;
using Servicios_Logisticos.Domain.ValueObjects;

namespace Servicios_Logisticos.Domain.Entities;

public class Credential
{
    public int Id { get; private set; }
    public CredentialCode Code { get; private set; } = null!;
    public int EmployeeId { get; private set; }
    public virtual Employee Employee { get; private set; } = null!;
    public CredentialStatus Status { get; private set; }
    public DateTime AssignedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string? Notes { get; private set; }

    protected Credential() { }

    public Credential(CredentialCode code, int employeeId, string? notes = null)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));

        if (employeeId <= 0)
            throw new BusinessRuleException("El identificador del empleado asociado debe ser mayor a cero.");

        EmployeeId = employeeId;
        Status = CredentialStatus.Activa;
        AssignedAt = DateTime.UtcNow;
        Notes = notes?.Trim();
    }

    public void Block(string? reason = null)
    {
        if (Status == CredentialStatus.Revocada)
            throw new BusinessRuleException("Una credencial revocada no puede ser bloqueada.");

        if (Status == CredentialStatus.Bloqueada)
            throw new BusinessRuleException("La credencial ya se encuentra bloqueada.");

        Status = CredentialStatus.Bloqueada;
        Notes = string.IsNullOrWhiteSpace(reason) ? Notes : reason.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Revoke(string? reason = null)
    {
        if (Status == CredentialStatus.Revocada)
            throw new BusinessRuleException("La credencial ya ha sido revocada anteriormente.");

        Status = CredentialStatus.Revocada;
        Notes = string.IsNullOrWhiteSpace(reason) ? Notes : reason.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reactivate()
    {
        if (Status == CredentialStatus.Revocada)
            throw new BusinessRuleException("Una credencial revocada definitivamente no puede reactivarse.");

        Status = CredentialStatus.Activa;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool CanBeUsed => Status == CredentialStatus.Activa;
}
