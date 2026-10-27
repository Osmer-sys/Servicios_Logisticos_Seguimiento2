using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Mappers;
using Servicios_Logisticos.Application.Utilities.Mediator;
using Servicios_Logisticos.Domain.Exceptions;
using Servicios_Logisticos.Domain.Interfaces;
using Servicios_Logisticos.Domain.ValueObjects;

namespace Servicios_Logisticos.Application.Commands.Credentials;

public record BlockCredentialCommand(string Code, string? Reason = null) : IRequest<CredentialDto>;

public class BlockCredentialCommandHandler : IRequestHandler<BlockCredentialCommand, CredentialDto>
{
    private readonly ICredentialRepository _credentialRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BlockCredentialCommandHandler(ICredentialRepository credentialRepository, IUnitOfWork unitOfWork)
    {
        _credentialRepository = credentialRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CredentialDto> HandleAsync(BlockCredentialCommand request, CancellationToken cancellationToken = default)
    {
        var codeVo = new CredentialCode(request.Code);
        var credential = await _credentialRepository.GetByCodeWithEmployeeAsync(codeVo, cancellationToken)
            ?? throw new EntityNotFoundException("Credencial", request.Code);

        credential.Block(request.Reason);

        _credentialRepository.Update(credential);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return credential.ToDto();
    }
}
