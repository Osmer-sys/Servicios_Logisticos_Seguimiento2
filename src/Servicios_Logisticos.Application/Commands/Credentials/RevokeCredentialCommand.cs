using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Mappers;
using Servicios_Logisticos.Application.Utilities.Mediator;
using Servicios_Logisticos.Domain.Exceptions;
using Servicios_Logisticos.Domain.Interfaces;
using Servicios_Logisticos.Domain.ValueObjects;

namespace Servicios_Logisticos.Application.Commands.Credentials;

public record RevokeCredentialCommand(string Code, string? Reason = null) : IRequest<CredentialDto>;

public class RevokeCredentialCommandHandler : IRequestHandler<RevokeCredentialCommand, CredentialDto>
{
    private readonly ICredentialRepository _credentialRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RevokeCredentialCommandHandler(ICredentialRepository credentialRepository, IUnitOfWork unitOfWork)
    {
        _credentialRepository = credentialRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CredentialDto> HandleAsync(RevokeCredentialCommand request, CancellationToken cancellationToken = default)
    {
        var codeVo = new CredentialCode(request.Code);
        var credential = await _credentialRepository.GetByCodeWithEmployeeAsync(codeVo, cancellationToken)
            ?? throw new EntityNotFoundException("Credencial", request.Code);

        credential.Revoke(request.Reason);

        _credentialRepository.Update(credential);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return credential.ToDto();
    }
}
