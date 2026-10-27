using Servicios_Logisticos.Domain.Entities;

namespace Servicios_Logisticos.Domain.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee?> GetByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default);
    Task<bool> ExistsByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default);
    Task<Employee?> GetByIdWithCredentialsAsync(int id, CancellationToken cancellationToken = default);
}
