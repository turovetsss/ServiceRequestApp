namespace Application.DTOs.Equipment;

public class EquipmentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CompanyId { get; set; }
    public List<EquipmentPhotoDto> Photos { get; set; }= new();
}