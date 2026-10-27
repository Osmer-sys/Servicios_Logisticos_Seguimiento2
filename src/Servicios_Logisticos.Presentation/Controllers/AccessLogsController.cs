using Microsoft.AspNetCore.Mvc;
using Servicios_Logisticos.Application.Commands.AccessLogs;
using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Queries.AccessLogs;
using Servicios_Logisticos.Application.Utilities.Mediator;

namespace Servicios_Logisticos.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AccessLogsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccessLogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registra un movimiento de acceso (entrada o salida) validando las condiciones de seguridad (RF-03).
    /// Registra el resultado en el historial tanto para accesos autorizados como rechazados.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AccessValidationResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterMovement(
        [FromBody] RegisterAccessMovementCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Consulta el historial trazable de movimientos de acceso con paginación y filtros (RF-04).
    /// Permite filtrar por empleado, rango de fechas, punto de acceso y resultado de validación.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AccessLogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory(
        [FromQuery] int? employeeId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? accessPointId,
        [FromQuery] bool? isGranted,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAccessLogsPagedQuery(
            employeeId,
            startDate,
            endDate,
            accessPointId,
            isGranted,
            pageNumber,
            pageSize
        );

        var result = await _mediator.SendAsync(query, cancellationToken);
        return Ok(result);
    }
}
