using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Mappers;
using Servicios_Logisticos.Application.Utilities.Mediator;
using Servicios_Logisticos.Domain.Interfaces;
using Servicios_Logisticos.Domain.ValueObjects;

namespace Servicios_Logisticos.Application.Queries.Credentials;

public record GetAllCredentialsQuery() : IRequest<IReadOnlyList<CredentialDto>>;

public class GetAllCredentialsQueryHandler : IRequestHandler<GetAllCredentialsQuery, IReadOnlyList<CredentialDto>>
{
    private readonly ICredentialRepository _credentialRepository;

    public GetAllCredentialsQueryHandler(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    public async Task<IReadOnlyList<CredentialDto>> HandleAsync(GetAllCredentialsQuery request, CancellationToken cancellationToken = default)
    {
        var credentials = await _credentialRepository.GetAllAsync(cancellationToken);
        return credentials.Select(c => c.ToDto()).ToList();
    }
}

public record GetCredentialByCodeQuery(string Code) : IRequest<CredentialDto?>;

public class GetCredentialByCodeQueryHandler : IRequestHandler<GetCredentialByCodeQuery, CredentialDto?>
{
    private readonly ICredentialRepository _credentialRepository;

    public GetCredentialByCodeQueryHandler(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    public async Task<CredentialDto?> HandleAsync(GetCredentialByCodeQuery request, CancellationToken cancellationToken = default)
    {
        CredentialCode codeVo;
        try
        {
            codeVo = new CredentialCode(request.Code);
        }
        catch
        {
            return null;
        }

        var credential = await _credentialRepository.GetByCodeWithEmployeeAsync(codeVo, cancellationToken);
        return credential?.ToDto();
    }
}

public record GetCredentialsByEmployeeIdQuery(int EmployeeId) : IRequest<IReadOnlyList<CredentialDto>>;

public class GetCredentialsByEmployeeIdQueryHandler : IRequestHandler<GetCredentialsByEmployeeIdQuery, IReadOnlyList<CredentialDto>>
{
    private readonly ICredentialRepository _credentialRepository;

    public GetCredentialsByEmployeeIdQueryHandler(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    public async Task<IReadOnlyList<CredentialDto>> HandleAsync(GetCredentialsByEmployeeIdQuery request, CancellationToken cancellationToken = default)
    {
        var credentials = await _credentialRepository.GetByEmployeeIdAsync(request.EmployeeId, cancellationToken);
        return credentials.Select(c => c.ToDto()).ToList();
    }
}
