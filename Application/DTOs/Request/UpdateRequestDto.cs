
namespace Application.DTOs.Request;

public class UpdateRequestDto
{
    public int? EquipmentId { get; set; }
    public string Description { get; set; }
    public string Phone { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}