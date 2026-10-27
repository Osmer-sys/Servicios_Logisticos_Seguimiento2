using Servicios_Logisticos.Domain.Enums;
using Servicios_Logisticos.Domain.Exceptions;
using Servicios_Logisticos.Domain.ValueObjects;

namespace Servicios_Logisticos.Domain.Entities;

public class Employee
{
    public int Id { get; private set; }
    public string DocumentNumber { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string Position { get; private set; } = null!;
    public string Department { get; private set; } = null!;
    public EmploymentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public virtual ICollection<Credential> Credentials { get; private set; } = new List<Credential>();

    // Constructor privado para EF Core
    protected Employee() { }

    public Employee(
        string documentNumber,
        string firstName,
        string lastName,
        Email email,
        string position,
        string department,
        EmploymentStatus status = EmploymentStatus.Activo)
    {
        if (string.IsNullOrWhiteSpace(documentNumber))
            throw new BusinessRuleException("El número de documento de identificación es obligatorio.");

        if (string.IsNullOrWhiteSpace(firstName))
            throw new BusinessRuleException("El nombre del empleado es obligatorio.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new BusinessRuleException("Los apellidos del empleado son obligatorios.");

        if (string.IsNullOrWhiteSpace(position))
            throw new BusinessRuleException("El cargo del empleado es obligatorio.");

        if (string.IsNullOrWhiteSpace(department))
            throw new BusinessRuleException("El área organizacional es obligatoria.");

        DocumentNumber = documentNumber.Trim();
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Position = position.Trim();
        Department = department.Trim();
        Status = status;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        string firstName,
        string lastName,
        Email email,
        string position,
        string department)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new BusinessRuleException("El nombre del empleado es obligatorio.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new BusinessRuleException("Los apellidos del empleado son obligatorios.");

        if (string.IsNullOrWhiteSpace(position))
            throw new BusinessRuleException("El cargo del empleado es obligatorio.");

        if (string.IsNullOrWhiteSpace(department))
            throw new BusinessRuleException("El área organizacional es obligatoria.");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Position = position.Trim();
        Department = department.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (Status == EmploymentStatus.Inactivo)
            throw new BusinessRuleException("El empleado ya se encuentra inactivo.");

        Status = EmploymentStatus.Inactivo;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (Status == EmploymentStatus.Activo)
            throw new BusinessRuleException("El empleado ya se encuentra activo.");

        Status = EmploymentStatus.Activo;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsActive => Status == EmploymentStatus.Activo;

    public string FullName => $"{FirstName} {LastName}";
}
