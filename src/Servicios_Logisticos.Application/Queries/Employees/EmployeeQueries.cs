using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Mappers;
using Servicios_Logisticos.Application.Utilities.Mediator;
using Servicios_Logisticos.Domain.Exceptions;
using Servicios_Logisticos.Domain.Interfaces;

namespace Servicios_Logisticos.Application.Queries.Employees;

public record GetAllEmployeesQuery() : IRequest<IReadOnlyList<EmployeeDto>>;

public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IReadOnlyList<EmployeeDto>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IReadOnlyList<EmployeeDto>> HandleAsync(GetAllEmployeesQuery request, CancellationToken cancellationToken = default)
    {
        var employees = await _employeeRepository.GetAllAsync(cancellationToken);
        return employees.Select(e => e.ToDto()).ToList();
    }
}

public record GetEmployeeByIdQuery(int Id) : IRequest<EmployeeDto?>;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeDto?> HandleAsync(GetEmployeeByIdQuery request, CancellationToken cancellationToken = default)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);
        return employee?.ToDto();
    }
}

public record GetEmployeeByDocumentQuery(string DocumentNumber) : IRequest<EmployeeDto?>;

public class GetEmployeeByDocumentQueryHandler : IRequestHandler<GetEmployeeByDocumentQuery, EmployeeDto?>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeByDocumentQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeDto?> HandleAsync(GetEmployeeByDocumentQuery request, CancellationToken cancellationToken = default)
    {
        var employee = await _employeeRepository.GetByDocumentNumberAsync(request.DocumentNumber, cancellationToken);
        return employee?.ToDto();
    }
}
