using Servicios_Logisticos.Domain.Entities;

namespace Servicios_Logisticos.Domain.Interfaces;

public interface IAccessLogRepository : IRepository<AccessLog>
{
    Task<(IReadOnlyList<AccessLog> Items, int TotalCount)> GetPagedAsync(
        int? employeeId,
        DateTime? startDate,
        DateTime? endDate,
        int? accessPointId,
        bool? isGranted,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
