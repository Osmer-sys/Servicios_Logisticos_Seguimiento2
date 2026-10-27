using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Mappers;
using Servicios_Logisticos.Application.Utilities.Mediator;
using Servicios_Logisticos.Domain.Exceptions;
using Servicios_Logisticos.Domain.Interfaces;
using Servicios_Logisticos.Domain.ValueObjects;

namespace Servicios_Logisticos.Application.Commands.Employees;

public record UpdateEmployeeCommand(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string Position,
    string Department
) : IRequest<EmployeeDto>;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeDto> HandleAsync(UpdateEmployeeCommand request, CancellationToken cancellationToken = default)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Empleado", request.Id);

        var emailVo = new Email(request.Email);
        employee.Update(request.FirstName, request.LastName, emailVo, request.Position, request.Department);

        _employeeRepository.Update(employee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return employee.ToDto();
    }
}
