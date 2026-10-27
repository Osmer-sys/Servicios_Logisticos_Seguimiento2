using Servicios_Logisticos.Domain.Exceptions;

namespace Servicios_Logisticos.Domain.ValueObjects;

public sealed record CredentialCode
{
    public string Value { get; }

    public CredentialCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new BusinessRuleException("El código de credencial no puede estar vacío.");

        var trimmed = value.Trim().ToUpperInvariant();

        if (trimmed.Length < 3 || trimmed.Length > 50)
            throw new BusinessRuleException("El código de credencial debe tener entre 3 y 50 caracteres.");

        Value = trimmed;
    }

    public override string ToString() => Value;

    public static implicit operator string(CredentialCode code) => code.Value;
}
