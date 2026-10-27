using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Utilities.Mediator;
using Servicios_Logisticos.Domain.Entities;
using Servicios_Logisticos.Domain.Enums;
using Servicios_Logisticos.Domain.Exceptions;
using Servicios_Logisticos.Domain.Interfaces;
using Servicios_Logisticos.Domain.ValueObjects;

namespace Servicios_Logisticos.Application.Commands.AccessLogs;

public record RegisterAccessMovementCommand(
    string CredentialCode,
    int AccessPointId,
    AccessMovementType MovementType
) : IRequest<AccessValidationResultDto>;

public class RegisterAccessMovementCommandHandler : IRequestHandler<RegisterAccessMovementCommand, AccessValidationResultDto>
{
    private readonly IAccessLogRepository _accessLogRepository;
    private readonly ICredentialRepository _credentialRepository;
    private readonly IAccessPointRepository _accessPointRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterAccessMovementCommandHandler(
        IAccessLogRepository accessLogRepository,
        ICredentialRepository credentialRepository,
        IAccessPointRepository accessPointRepository,
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork)
    {
        _accessLogRepository = accessLogRepository;
        _credentialRepository = credentialRepository;
        _accessPointRepository = accessPointRepository;
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AccessValidationResultDto> HandleAsync(
        RegisterAccessMovementCommand request,
        CancellationToken cancellationToken = default)
    {
        var accessPoint = await _accessPointRepository.GetByIdAsync(request.AccessPointId, cancellationToken);
        if (accessPoint == null)
            throw new EntityNotFoundException("Punto de Acceso", request.AccessPointId);

        // Validar punto de acceso habilitado
        if (!accessPoint.IsEnabled)
        {
            var rejectedLog = AccessLog.CreateRejected(
                request.CredentialCode,
                employeeId: null,
                request.AccessPointId,
                request.MovementType,
                $"El punto de acceso '{accessPoint.Name}' se encuentra inhabilitado."
            );

            await _accessLogRepository.AddAsync(rejectedLog, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AccessValidationResultDto
            {
                AccessLogId = rejectedLog.Id,
                IsGranted = false,
                MovementType = request.MovementType.ToString(),
                Timestamp = rejectedLog.Timestamp,
                CredentialCode = request.CredentialCode,
                AccessPointName = accessPoint.Name,
                RejectionReason = rejectedLog.RejectionReason,
                Message = "Acceso denegado: Punto de acceso inhabilitado."
            };
        }

        // Validar credencial
        CredentialCode credentialCodeVo;
        try
        {
            credentialCodeVo = new CredentialCode(request.CredentialCode);
        }
        catch (BusinessRuleException ex)
        {
            var rejectedLog = AccessLog.CreateRejected(
                request.CredentialCode,
                employeeId: null,
                request.AccessPointId,
                request.MovementType,
                ex.Message
            );

            await _accessLogRepository.AddAsync(rejectedLog, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AccessValidationResultDto
            {
                AccessLogId = rejectedLog.Id,
                IsGranted = false,
                MovementType = request.MovementType.ToString(),
                Timestamp = rejectedLog.Timestamp,
                CredentialCode = request.CredentialCode,
                AccessPointName = accessPoint.Name,
                RejectionReason = rejectedLog.RejectionReason,
                Message = "Acceso denegado: Código de credencial inválido."
            };
        }

        var credential = await _credentialRepository.GetByCodeWithEmployeeAsync(credentialCodeVo, cancellationToken);
        if (credential == null)
        {
            var rejectedLog = AccessLog.CreateRejected(
                request.CredentialCode,
                employeeId: null,
                request.AccessPointId,
                request.MovementType,
                "La credencial especificada no existe en el sistema."
            );

            await _accessLogRepository.AddAsync(rejectedLog, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AccessValidationResultDto
            {
                AccessLogId = rejectedLog.Id,
                IsGranted = false,
                MovementType = request.MovementType.ToString(),
                Timestamp = rejectedLog.Timestamp,
                CredentialCode = request.CredentialCode,
                AccessPointName = accessPoint.Name,
                RejectionReason = rejectedLog.RejectionReason,
                Message = "Acceso denegado: Credencial no registrada."
            };
        }

        // Validar estado de la credencial
        if (credential.Status == CredentialStatus.Bloqueada)
        {
            var rejectedLog = AccessLog.CreateRejected(
                request.CredentialCode,
                credential.EmployeeId,
                request.AccessPointId,
                request.MovementType,
                "La credencial se encuentra bloqueada."
            );

            await _accessLogRepository.AddAsync(rejectedLog, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AccessValidationResultDto
            {
                AccessLogId = rejectedLog.Id,
                IsGranted = false,
                MovementType = request.MovementType.ToString(),
                Timestamp = rejectedLog.Timestamp,
                CredentialCode = request.CredentialCode,
                AccessPointName = accessPoint.Name,
                EmployeeName = credential.Employee?.FullName,
                EmployeeDocument = credential.Employee?.DocumentNumber,
                RejectionReason = rejectedLog.RejectionReason,
                Message = "Acceso denegado: Credencial bloqueada."
            };
        }

        if (credential.Status == CredentialStatus.Revocada)
        {
            var rejectedLog = AccessLog.CreateRejected(
                request.CredentialCode,
                credential.EmployeeId,
                request.AccessPointId,
                request.MovementType,
                "La credencial se encuentra revocada."
            );

            await _accessLogRepository.AddAsync(rejectedLog, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AccessValidationResultDto
            {
                AccessLogId = rejectedLog.Id,
                IsGranted = false,
                MovementType = request.MovementType.ToString(),
                Timestamp = rejectedLog.Timestamp,
                CredentialCode = request.CredentialCode,
                AccessPointName = accessPoint.Name,
                EmployeeName = credential.Employee?.FullName,
                EmployeeDocument = credential.Employee?.DocumentNumber,
                RejectionReason = rejectedLog.RejectionReason,
                Message = "Acceso denegado: Credencial revocada."
            };
        }

        // Validar condiciones del empleado
        var employee = credential.Employee ?? await _employeeRepository.GetByIdAsync(credential.EmployeeId, cancellationToken);
        if (employee == null || !employee.IsActive)
        {
            var rejectedLog = AccessLog.CreateRejected(
                request.CredentialCode,
                credential.EmployeeId,
                request.AccessPointId,
                request.MovementType,
                "El empleado asociado a la credencial tiene el vínculo laboral inactivo."
            );

            await _accessLogRepository.AddAsync(rejectedLog, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AccessValidationResultDto
            {
                AccessLogId = rejectedLog.Id,
                IsGranted = false,
                MovementType = request.MovementType.ToString(),
                Timestamp = rejectedLog.Timestamp,
                CredentialCode = request.CredentialCode,
                AccessPointName = accessPoint.Name,
                EmployeeName = employee?.FullName,
                EmployeeDocument = employee?.DocumentNumber,
                RejectionReason = rejectedLog.RejectionReason,
                Message = "Acceso denegado: Empleado con vínculo laboral inactivo."
            };
        }

        // Todo válido -> Registrar acceso autorizado
        var authorizedLog = AccessLog.CreateAuthorized(
            request.CredentialCode,
            employee.Id,
            accessPoint.Id,
            request.MovementType
        );

        await _accessLogRepository.AddAsync(authorizedLog, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AccessValidationResultDto
        {
            AccessLogId = authorizedLog.Id,
            IsGranted = true,
            MovementType = request.MovementType.ToString(),
            Timestamp = authorizedLog.Timestamp,
            CredentialCode = request.CredentialCode,
            AccessPointName = accessPoint.Name,
            EmployeeName = employee.FullName,
            EmployeeDocument = employee.DocumentNumber,
            RejectionReason = null,
            Message = $"Acceso autorizado de {request.MovementType} concedido a {employee.FullName}."
        };
    }
}
