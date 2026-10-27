using Microsoft.AspNetCore.Mvc;
using Servicios_Logisticos.Application.Commands.Credentials;
using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Queries.Credentials;
using Servicios_Logisticos.Application.Utilities.Mediator;

namespace Servicios_Logisticos.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CredentialsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CredentialsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Consulta todas las credenciales registradas (RF-02).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CredentialDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(new GetAllCredentialsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Consulta el estado y detalle de una credencial por su código único (RF-02).
    /// </summary>
    [HttpGet("{code}")]
    [ProducesResponseType(typeof(CredentialDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCode(string code, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(new GetCredentialByCodeQuery(code), cancellationToken);
        if (result == null)
            return NotFound(new { Message = $"No se encontró la credencial con código '{code}'." });

        return Ok(result);
    }

    /// <summary>
    /// Consulta las credenciales asignadas a un empleado (RF-02).
    /// </summary>
    [HttpGet("employee/{employeeId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<CredentialDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployeeId(int employeeId, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(new GetCredentialsByEmployeeIdQuery(employeeId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Asigna una nueva credencial de acceso a un empleado (RF-02).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CredentialDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Assign([FromBody] AssignCredentialCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(command, cancellationToken);
        return CreatedAtAction(nameof(GetByCode), new { code = result.Code }, result);
    }

    /// <summary>
    /// Bloquea una credencial de acceso (RF-02).
    /// </summary>
    [HttpPatch("{code}/block")]
    [ProducesResponseType(typeof(CredentialDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Block(string code, [FromBody] ReasonRequest? request, CancellationToken cancellationToken)
    {
        var command = new BlockCredentialCommand(code, request?.Reason);
        var result = await _mediator.SendAsync(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Revoca definitivamente una credencial de acceso (RF-02).
    /// </summary>
    [HttpPatch("{code}/revoke")]
    [ProducesResponseType(typeof(CredentialDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Revoke(string code, [FromBody] ReasonRequest? request, CancellationToken cancellationToken)
    {
        var command = new RevokeCredentialCommand(code, request?.Reason);
        var result = await _mediator.SendAsync(command, cancellationToken);
        return Ok(result);
    }
}

public record ReasonRequest(string? Reason);
