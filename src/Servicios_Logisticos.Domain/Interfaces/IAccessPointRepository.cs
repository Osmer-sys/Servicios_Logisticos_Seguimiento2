using Servicios_Logisticos.Domain.Entities;

namespace Servicios_Logisticos.Domain.Interfaces;

public interface IAccessPointRepository : IRepository<AccessPoint>
{
    Task<IReadOnlyList<AccessPoint>> GetEnabledOnlyAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
