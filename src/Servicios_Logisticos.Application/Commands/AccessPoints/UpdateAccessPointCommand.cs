using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Mappers;
using Servicios_Logisticos.Application.Utilities.Mediator;
using Servicios_Logisticos.Domain.Exceptions;
using Servicios_Logisticos.Domain.Interfaces;

namespace Servicios_Logisticos.Application.Commands.AccessPoints;

public record UpdateAccessPointCommand(
    int Id,
    string Name,
    string? Description,
    bool IsRestrictedArea,
    bool IsEnabled
) : IRequest<AccessPointDto>;

public class UpdateAccessPointCommandHandler : IRequestHandler<UpdateAccessPointCommand, AccessPointDto>
{
    private readonly IAccessPointRepository _accessPointRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAccessPointCommandHandler(IAccessPointRepository accessPointRepository, IUnitOfWork unitOfWork)
    {
        _accessPointRepository = accessPointRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AccessPointDto> HandleAsync(UpdateAccessPointCommand request, CancellationToken cancellationToken = default)
    {
        var point = await _accessPointRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Punto de Acceso", request.Id);

        point.Update(request.Name, request.Description, request.IsRestrictedArea, request.IsEnabled);

        _accessPointRepository.Update(point);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return point.ToDto();
    }
}
