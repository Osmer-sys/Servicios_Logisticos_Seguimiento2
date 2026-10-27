using Microsoft.EntityFrameworkCore;
using Servicios_Logisticos.Domain.Entities;
using Servicios_Logisticos.Domain.Interfaces;
using Servicios_Logisticos.Infrastructure.Persistence.DbContext;

namespace Servicios_Logisticos.Infrastructure.Persistence.Repositories;

public class AccessPointRepository : Repository<AccessPoint>, IAccessPointRepository
{
    public AccessPointRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<AccessPoint>> GetEnabledOnlyAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.IsEnabled)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(p => p.Name.ToLower() == name.Trim().ToLower(), cancellationToken);
    }
}
