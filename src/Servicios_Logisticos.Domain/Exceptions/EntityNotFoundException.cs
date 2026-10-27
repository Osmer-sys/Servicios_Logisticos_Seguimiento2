namespace Servicios_Logisticos.Domain.Exceptions;

public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string message) : base(message)
    {
    }

    public EntityNotFoundException(string entityName, object key)
        : base($"No se encontró la entidad '{entityName}' con identificador '{key}'.")
    {
    }
}
