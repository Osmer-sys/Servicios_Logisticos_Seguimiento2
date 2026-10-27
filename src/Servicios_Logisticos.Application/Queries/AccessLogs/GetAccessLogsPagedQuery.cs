using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Mappers;
using Servicios_Logisticos.Application.Utilities.Mediator;
using Servicios_Logisticos.Domain.Interfaces;

namespace Servicios_Logisticos.Application.Queries.AccessLogs;

public record GetAccessLogsPagedQuery(
    int? EmployeeId = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    int? AccessPointId = null,
    bool? IsGranted = null,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<AccessLogDto>>;

public class GetAccessLogsPagedQueryHandler : IRequestHandler<GetAccessLogsPagedQuery, PagedResult<AccessLogDto>>
{
    private readonly IAccessLogRepository _accessLogRepository;

    public GetAccessLogsPagedQueryHandler(IAccessLogRepository accessLogRepository)
    {
        _accessLogRepository = accessLogRepository;
    }

    public async Task<PagedResult<AccessLogDto>> HandleAsync(GetAccessLogsPagedQuery request, CancellationToken cancellationToken = default)
    {
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? 10 : (request.PageSize > 100 ? 100 : request.PageSize);

        var (items, totalCount) = await _accessLogRepository.GetPagedAsync(
            request.EmployeeId,
            request.StartDate,
            request.EndDate,
            request.AccessPointId,
            request.IsGranted,
            pageNumber,
            pageSize,
            cancellationToken
        );

        var dtos = items.Select(x => x.ToDto()).ToList();
        return new PagedResult<AccessLogDto>(dtos, totalCount, pageNumber, pageSize);
    }
}
