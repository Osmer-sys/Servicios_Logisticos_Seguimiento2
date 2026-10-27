using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Mappers;
using Servicios_Logisticos.Application.Utilities.Mediator;
using Servicios_Logisticos.Domain.Entities;
using Servicios_Logisticos.Domain.Exceptions;
using Servicios_Logisticos.Domain.Interfaces;
using Servicios_Logisticos.Domain.ValueObjects;

namespace Servicios_Logisticos.Application.Commands.Employees;

public record CreateEmployeeCommand(
    string DocumentNumber,
    string FirstName,
    string LastName,
    string Email,
    string Position,
    string Department
) : IRequest<EmployeeDto>;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeDto> HandleAsync(CreateEmployeeCommand request, CancellationToken cancellationToken = default)
    {
        var exists = await _employeeRepository.ExistsByDocumentNumberAsync(request.DocumentNumber, cancellationToken);
        if (exists)
            throw new BusinessRuleException($"Ya existe un empleado registrado con la identificación '{request.DocumentNumber}'.");

        var emailVo = new Email(request.Email);
        var employee = new Employee(
            request.DocumentNumber,
            request.FirstName,
            request.LastName,
            emailVo,
            request.Position,
            request.Department
        );

        await _employeeRepository.AddAsync(employee, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return employee.ToDto();
    }
}
