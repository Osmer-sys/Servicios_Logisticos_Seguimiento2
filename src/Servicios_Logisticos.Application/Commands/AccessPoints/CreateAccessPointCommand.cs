using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Mappers;
using Servicios_Logisticos.Application.Utilities.Mediator;
using Servicios_Logisticos.Domain.Entities;
using Servicios_Logisticos.Domain.Exceptions;
using Servicios_Logisticos.Domain.Interfaces;

namespace Servicios_Logisticos.Application.Commands.AccessPoints;

public record CreateAccessPointCommand(
    string Name,
    string? Description,
    bool IsRestrictedArea,
    bool IsEnabled
) : IRequest<AccessPointDto>;

public class CreateAccessPointCommandHandler : IRequestHandler<CreateAccessPointCommand, AccessPointDto>
{
    private readonly IAccessPointRepository _accessPointRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccessPointCommandHandler(IAccessPointRepository accessPointRepository, IUnitOfWork unitOfWork)
    {
        _accessPointRepository = accessPointRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AccessPointDto> HandleAsync(CreateAccessPointCommand request, CancellationToken cancellationToken = default)
    {
        var exists = await _accessPointRepository.ExistsByNameAsync(request.Name, cancellationToken);
        if (exists)
            throw new BusinessRuleException($"Ya existe un punto de acceso con el nombre '{request.Name}'.");

        var point = new AccessPoint(
            request.Name,
            request.Description,
            request.IsRestrictedArea,
            request.IsEnabled
        );

        await _accessPointRepository.AddAsync(point, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return point.ToDto();
    }
}
