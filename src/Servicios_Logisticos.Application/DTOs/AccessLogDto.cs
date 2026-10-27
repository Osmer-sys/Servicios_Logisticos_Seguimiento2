namespace Servicios_Logisticos.Application.DTOs;

public class AccessLogDto
{
    public int Id { get; set; }
    public string CredentialCode { get; set; } = string.Empty;
    public int? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? EmployeeDocument { get; set; }
    public int AccessPointId { get; set; }
    public string AccessPointName { get; set; } = string.Empty;
    public string MovementType { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public bool IsGranted { get; set; }
    public string? RejectionReason { get; set; }
}
