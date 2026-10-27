using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Mappers;
using Servicios_Logisticos.Application.Utilities.Mediator;
using Servicios_Logisticos.Domain.Interfaces;

namespace Servicios_Logisticos.Application.Queries.AccessPoints;

public record GetAllAccessPointsQuery(bool EnabledOnly = false) : IRequest<IReadOnlyList<AccessPointDto>>;

public class GetAllAccessPointsQueryHandler : IRequestHandler<GetAllAccessPointsQuery, IReadOnlyList<AccessPointDto>>
{
    private readonly IAccessPointRepository _accessPointRepository;

    public GetAllAccessPointsQueryHandler(IAccessPointRepository accessPointRepository)
    {
        _accessPointRepository = accessPointRepository;
    }

    public async Task<IReadOnlyList<AccessPointDto>> HandleAsync(GetAllAccessPointsQuery request, CancellationToken cancellationToken = default)
    {
        var points = request.EnabledOnly
            ? await _accessPointRepository.GetEnabledOnlyAsync(cancellationToken)
            : await _accessPointRepository.GetAllAsync(cancellationToken);

        return points.Select(p => p.ToDto()).ToList();
    }
}

public record GetAccessPointByIdQuery(int Id) : IRequest<AccessPointDto?>;

public class GetAccessPointByIdQueryHandler : IRequestHandler<GetAccessPointByIdQuery, AccessPointDto?>
{
    private readonly IAccessPointRepository _accessPointRepository;

    public GetAccessPointByIdQueryHandler(IAccessPointRepository accessPointRepository)
    {
        _accessPointRepository = accessPointRepository;
    }

    public async Task<AccessPointDto?> HandleAsync(GetAccessPointByIdQuery request, CancellationToken cancellationToken = default)
    {
        var point = await _accessPointRepository.GetByIdAsync(request.Id, cancellationToken);
        return point?.ToDto();
    }
}
