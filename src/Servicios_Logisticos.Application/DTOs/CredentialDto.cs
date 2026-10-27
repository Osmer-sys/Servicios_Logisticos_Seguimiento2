namespace Servicios_Logisticos.Application.DTOs;

public class CredentialDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string EmployeeDocument { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool CanBeUsed { get; set; }
    public DateTime AssignedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? Notes { get; set; }
}
