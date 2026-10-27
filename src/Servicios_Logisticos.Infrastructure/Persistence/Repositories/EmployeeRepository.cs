using Microsoft.EntityFrameworkCore;
using Servicios_Logisticos.Domain.Entities;
using Servicios_Logisticos.Domain.Interfaces;
using Servicios_Logisticos.Infrastructure.Persistence.DbContext;

namespace Servicios_Logisticos.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Employee?> GetByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(e => e.Credentials)
            .FirstOrDefaultAsync(e => e.DocumentNumber == documentNumber.Trim(), cancellationToken);
    }

    public async Task<bool> ExistsByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(e => e.DocumentNumber == documentNumber.Trim(), cancellationToken);
    }

    public async Task<Employee?> GetByIdWithCredentialsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(e => e.Credentials)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
