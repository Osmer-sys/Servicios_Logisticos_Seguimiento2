using Microsoft.EntityFrameworkCore;
using Servicios_Logisticos.Domain.Entities;
using Servicios_Logisticos.Domain.Interfaces;
using Servicios_Logisticos.Domain.ValueObjects;
using Servicios_Logisticos.Infrastructure.Persistence.DbContext;

namespace Servicios_Logisticos.Infrastructure.Persistence.Repositories;

public class CredentialRepository : Repository<Credential>, ICredentialRepository
{
    public CredentialRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Credential?> GetByCodeAsync(CredentialCode code, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(c => c.Code == code, cancellationToken);
    }

    public async Task<Credential?> GetByCodeWithEmployeeAsync(CredentialCode code, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.Employee)
            .FirstOrDefaultAsync(c => c.Code == code, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(CredentialCode code, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(c => c.Code == code, cancellationToken);
    }

    public async Task<IReadOnlyList<Credential>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.Employee)
            .Where(c => c.EmployeeId == employeeId)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IReadOnlyList<Credential>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.Employee)
            .ToListAsync(cancellationToken);
    }
}
