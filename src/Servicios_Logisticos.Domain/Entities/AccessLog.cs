using Servicios_Logisticos.Domain.Enums;
using Servicios_Logisticos.Domain.Exceptions;

namespace Servicios_Logisticos.Domain.Entities;

public class AccessLog
{
    public int Id { get; private set; }
    public string CredentialCode { get; private set; } = null!;
    public int? EmployeeId { get; private set; }
    public virtual Employee? Employee { get; private set; }
    public int AccessPointId { get; private set; }
    public virtual AccessPoint AccessPoint { get; private set; } = null!;
    public AccessMovementType MovementType { get; private set; }
    public DateTime Timestamp { get; private set; }
    public bool IsGranted { get; private set; }
    public string? RejectionReason { get; private set; }

    protected AccessLog() { }

    public static AccessLog CreateAuthorized(
        string credentialCode,
        int employeeId,
        int accessPointId,
        AccessMovementType movementType)
    {
        if (string.IsNullOrWhiteSpace(credentialCode))
            throw new BusinessRuleException("El código de credencial no puede ser nulo o vacío.");

        if (employeeId <= 0)
            throw new BusinessRuleException("El ID del empleado es inválido.");

        if (accessPointId <= 0)
            throw new BusinessRuleException("El ID del punto de acceso es inválido.");

        return new AccessLog
        {
            CredentialCode = credentialCode.Trim().ToUpperInvariant(),
            EmployeeId = employeeId,
            AccessPointId = accessPointId,
            MovementType = movementType,
            Timestamp = DateTime.UtcNow,
            IsGranted = true,
            RejectionReason = null
        };
    }

    public static AccessLog CreateRejected(
        string credentialCode,
        int? employeeId,
        int accessPointId,
        AccessMovementType movementType,
        string rejectionReason)
    {
        if (string.IsNullOrWhiteSpace(credentialCode))
            throw new BusinessRuleException("El código de credencial no puede ser nulo o vacío.");

        if (string.IsNullOrWhiteSpace(rejectionReason))
            throw new BusinessRuleException("El motivo de rechazo es obligatorio para un acceso denegado.");

        return new AccessLog
        {
            CredentialCode = credentialCode.Trim().ToUpperInvariant(),
            EmployeeId = employeeId,
            AccessPointId = accessPointId,
            MovementType = movementType,
            Timestamp = DateTime.UtcNow,
            IsGranted = false,
            RejectionReason = rejectionReason.Trim()
        };
    }
}
