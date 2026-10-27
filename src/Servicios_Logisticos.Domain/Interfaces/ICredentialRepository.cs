using Servicios_Logisticos.Domain.Entities;
using Servicios_Logisticos.Domain.ValueObjects;

namespace Servicios_Logisticos.Domain.Interfaces;

public interface ICredentialRepository : IRepository<Credential>
{
    Task<Credential?> GetByCodeAsync(CredentialCode code, CancellationToken cancellationToken = default);
    Task<Credential?> GetByCodeWithEmployeeAsync(CredentialCode code, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(CredentialCode code, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Credential>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default);
}
