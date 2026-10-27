using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Domain.Entities;

namespace Servicios_Logisticos.Application.Mappers;

public static class MappingExtensions
{
    public static EmployeeDto ToDto(this Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            DocumentNumber = employee.DocumentNumber,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            FullName = employee.FullName,
            Email = employee.Email.Value,
            Position = employee.Position,
            Department = employee.Department,
            Status = employee.Status.ToString(),
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };
    }

    public static CredentialDto ToDto(this Credential credential)
    {
        return new CredentialDto
        {
            Id = credential.Id,
            Code = credential.Code.Value,
            EmployeeId = credential.EmployeeId,
            EmployeeName = credential.Employee != null ? credential.Employee.FullName : string.Empty,
            EmployeeDocument = credential.Employee != null ? credential.Employee.DocumentNumber : string.Empty,
            Status = credential.Status.ToString(),
            CanBeUsed = credential.CanBeUsed,
            AssignedAt = credential.AssignedAt,
            UpdatedAt = credential.UpdatedAt,
            Notes = credential.Notes
        };
    }

    public static AccessPointDto ToDto(this AccessPoint accessPoint)
    {
        return new AccessPointDto
        {
            Id = accessPoint.Id,
            Name = accessPoint.Name,
            Description = accessPoint.Description,
            IsEnabled = accessPoint.IsEnabled,
            IsRestrictedArea = accessPoint.IsRestrictedArea,
            CreatedAt = accessPoint.CreatedAt,
            UpdatedAt = accessPoint.UpdatedAt
        };
    }

    public static AccessLogDto ToDto(this AccessLog accessLog)
    {
        return new AccessLogDto
        {
            Id = accessLog.Id,
            CredentialCode = accessLog.CredentialCode,
            EmployeeId = accessLog.EmployeeId,
            EmployeeName = accessLog.Employee != null ? accessLog.Employee.FullName : null,
            EmployeeDocument = accessLog.Employee != null ? accessLog.Employee.DocumentNumber : null,
            AccessPointId = accessLog.AccessPointId,
            AccessPointName = accessLog.AccessPoint != null ? accessLog.AccessPoint.Name : string.Empty,
            MovementType = accessLog.MovementType.ToString(),
            Timestamp = accessLog.Timestamp,
            IsGranted = accessLog.IsGranted,
            RejectionReason = accessLog.RejectionReason
        };
    }
}
