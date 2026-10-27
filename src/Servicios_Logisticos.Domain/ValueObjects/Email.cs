using System.Text.RegularExpressions;
using Servicios_Logisticos.Domain.Exceptions;

namespace Servicios_Logisticos.Domain.ValueObjects;

public sealed record Email
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new BusinessRuleException("El correo electrónico no puede estar vacío.");

        var trimmed = value.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(trimmed))
            throw new BusinessRuleException($"El correo electrónico '{value}' no tiene un formato válido.");

        Value = trimmed;
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}
