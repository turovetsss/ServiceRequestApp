namespace Application.DTOs.Request;

public class CreateRequestDto
{
    public int? EquipmentId { get; set; }
    public string Description { get; set; }
    public string Phone { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public List<string> ProblemPhotoUrls { get; set; } = new(); 
}