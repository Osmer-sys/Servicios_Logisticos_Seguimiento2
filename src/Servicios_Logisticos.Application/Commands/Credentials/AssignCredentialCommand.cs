using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Mappers;
using Servicios_Logisticos.Application.Utilities.Mediator;
using Servicios_Logisticos.Domain.Entities;
using Servicios_Logisticos.Domain.Exceptions;
using Servicios_Logisticos.Domain.Interfaces;
using Servicios_Logisticos.Domain.ValueObjects;

namespace Servicios_Logisticos.Application.Commands.Credentials;

public record AssignCredentialCommand(
    string Code,
    int EmployeeId,
    string? Notes = null
) : IRequest<CredentialDto>;

public class AssignCredentialCommandHandler : IRequestHandler<AssignCredentialCommand, CredentialDto>
{
    private readonly ICredentialRepository _credentialRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignCredentialCommandHandler(
        ICredentialRepository credentialRepository,
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork)
    {
        _credentialRepository = credentialRepository;
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CredentialDto> HandleAsync(AssignCredentialCommand request, CancellationToken cancellationToken = default)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken)
            ?? throw new EntityNotFoundException("Empleado", request.EmployeeId);

        if (!employee.IsActive)
            throw new BusinessRuleException($"No se puede asignar una credencial a un empleado con estado laboral inactivo (ID {employee.Id}).");

        var codeVo = new CredentialCode(request.Code);

        var exists = await _credentialRepository.ExistsByCodeAsync(codeVo, cancellationToken);
        if (exists)
            throw new BusinessRuleException($"Ya existe una credencial registrada con el código '{codeVo.Value}'.");

        var credential = new Credential(codeVo, employee.Id, request.Notes);

        await _credentialRepository.AddAsync(credential, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var created = await _credentialRepository.GetByCodeWithEmployeeAsync(codeVo, cancellationToken);
        return (created ?? credential).ToDto();
    }
}
