namespace Servicios_Logisticos.Application.DTOs;

public class AccessValidationResultDto
{
    public int AccessLogId { get; set; }
    public bool IsGranted { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string CredentialCode { get; set; } = string.Empty;
    public string AccessPointName { get; set; } = string.Empty;
    public string? EmployeeName { get; set; }
    public string? EmployeeDocument { get; set; }
    public string? RejectionReason { get; set; }
    public string Message { get; set; } = string.Empty;
}
