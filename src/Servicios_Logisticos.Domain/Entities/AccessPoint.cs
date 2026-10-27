using Servicios_Logisticos.Domain.Exceptions;

namespace Servicios_Logisticos.Domain.Entities;

public class AccessPoint
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsEnabled { get; private set; }
    public bool IsRestrictedArea { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected AccessPoint() { }

    public AccessPoint(string name, string? description = null, bool isRestrictedArea = false, bool isEnabled = true)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleException("El nombre del punto de acceso es obligatorio.");

        Name = name.Trim();
        Description = description?.Trim();
        IsRestrictedArea = isRestrictedArea;
        IsEnabled = isEnabled;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string? description, bool isRestrictedArea, bool isEnabled)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleException("El nombre del punto de acceso es obligatorio.");

        Name = name.Trim();
        Description = description?.Trim();
        IsRestrictedArea = isRestrictedArea;
        IsEnabled = isEnabled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Enable()
    {
        if (IsEnabled)
            throw new BusinessRuleException("El punto de acceso ya se encuentra habilitado.");

        IsEnabled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Disable()
    {
        if (!IsEnabled)
            throw new BusinessRuleException("El punto de acceso ya se encuentra inhabilitado.");

        IsEnabled = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
