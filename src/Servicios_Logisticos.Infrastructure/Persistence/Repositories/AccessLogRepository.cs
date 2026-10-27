using Microsoft.EntityFrameworkCore;
using Servicios_Logisticos.Domain.Entities;
using Servicios_Logisticos.Domain.Interfaces;
using Servicios_Logisticos.Infrastructure.Persistence.DbContext;

namespace Servicios_Logisticos.Infrastructure.Persistence.Repositories;

public class AccessLogRepository : Repository<AccessLog>, IAccessLogRepository
{
    public AccessLogRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<(IReadOnlyList<AccessLog> Items, int TotalCount)> GetPagedAsync(
        int? employeeId,
        DateTime? startDate,
        DateTime? endDate,
        int? accessPointId,
        bool? isGranted,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(l => l.Employee)
            .Include(l => l.AccessPoint)
            .AsNoTracking()
            .AsQueryable();

        if (employeeId.HasValue)
        {
            query = query.Where(l => l.EmployeeId == employeeId.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(l => l.Timestamp >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(l => l.Timestamp <= endDate.Value);
        }

        if (accessPointId.HasValue)
        {
            query = query.Where(l => l.AccessPointId == accessPointId.Value);
        }

        if (isGranted.HasValue)
        {
            query = query.Where(l => l.IsGranted == isGranted.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(l => l.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
