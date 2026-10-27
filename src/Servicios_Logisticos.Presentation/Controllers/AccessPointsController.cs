using Microsoft.AspNetCore.Mvc;
using Servicios_Logisticos.Application.Commands.AccessPoints;
using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Queries.AccessPoints;
using Servicios_Logisticos.Application.Utilities.Mediator;

namespace Servicios_Logisticos.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AccessPointsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccessPointsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Consulta los puntos de acceso físicos de la organización (RF-06).
    /// Permite filtrar opcionalmente solo los habilitados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AccessPointDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool enabledOnly = false, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.SendAsync(new GetAllAccessPointsQuery(enabledOnly), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Consulta un punto de acceso por su ID (RF-06).
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AccessPointDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(new GetAccessPointByIdQuery(id), cancellationToken);
        if (result == null)
            return NotFound(new { Message = $"No se encontró el punto de acceso con ID {id}." });

        return Ok(result);
    }

    /// <summary>
    /// Registra un nuevo punto de acceso físico en la organización (RF-06).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AccessPointDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAccessPointCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Actualiza el estado o información de un punto de acceso físico (RF-06).
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(AccessPointDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAccessPointRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateAccessPointCommand(
            id,
            request.Name,
            request.Description,
            request.IsRestrictedArea,
            request.IsEnabled
        );

        var result = await _mediator.SendAsync(command, cancellationToken);
        return Ok(result);
    }
}

public record UpdateAccessPointRequest(
    string Name,
    string? Description,
    bool IsRestrictedArea,
    bool IsEnabled
);
