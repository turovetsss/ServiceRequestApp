namespace Application.DTOs.Equipment;

public class UpdateEquipmentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<EquipmentPhotoDto> Photos { get; set; } = new();
    public List<int> RemovedPhotoIds { get; set; } = new();
}