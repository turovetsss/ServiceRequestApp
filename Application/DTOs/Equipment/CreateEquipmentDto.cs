namespace Application.DTOs.Equipment;

public class CreateEquipmentDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> PhotoUrls { get; set; } = new();
}