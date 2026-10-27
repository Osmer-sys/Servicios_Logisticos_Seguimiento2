using Microsoft.AspNetCore.Mvc;
using Servicios_Logisticos.Application.Commands.Employees;
using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Queries.Employees;
using Servicios_Logisticos.Application.Utilities.Mediator;

namespace Servicios_Logisticos.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Consulta todos los empleados registrados (RF-01).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(new GetAllEmployeesQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Consulta un empleado por su ID (RF-01).
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(new GetEmployeeByIdQuery(id), cancellationToken);
        if (result == null)
            return NotFound(new { Message = $"No se encontró el empleado con ID {id}." });

        return Ok(result);
    }

    /// <summary>
    /// Consulta un empleado por su documento de identificación (RF-01).
    /// </summary>
    [HttpGet("document/{documentNumber}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByDocument(string documentNumber, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(new GetEmployeeByDocumentQuery(documentNumber), cancellationToken);
        if (result == null)
            return NotFound(new { Message = $"No se encontró el empleado con documento '{documentNumber}'." });

        return Ok(result);
    }

    /// <summary>
    /// Registra un nuevo empleado (RF-01).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Actualiza los datos de un empleado existente (RF-01).
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateEmployeeCommand(
            id,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Position,
            request.Department
        );

        var result = await _mediator.SendAsync(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Desactiva a un empleado cambiando su estado laboral a inactivo (RF-01).
    /// </summary>
    [HttpPatch("{id:int}/deactivate")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(new DeactivateEmployeeCommand(id), cancellationToken);
        return Ok(result);
    }
}

public record UpdateEmployeeRequest(
    string FirstName,
    string LastName,
    string Email,
    string Position,
    string Department
);
