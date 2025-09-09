using Application.Dto.Request;

namespace Application.DTOs.Request;

public class RequestDto
{
    public int Id { get; set; }
    public int? EquipmentId { get; set; }
    public string EquipmentName { get; set; }
    public string Description { get; set; }
    public string Phone { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public string Status { get; set; }
    public int CreatedByAdminId { get; set; }
    public int? AssignedMasterId { get; set; }
    public List<RequestPhotoDto> Photos { get; set; } = new();
    public List<CompletedWorkPhotoDto> CompletedWorkPhotos { get; set; } = new();
    public List<DocumentDto> Documents { get; set; } = new();
    public List<RequestStatusHistoryDto> StatusHistory { get; set; } = new();
}