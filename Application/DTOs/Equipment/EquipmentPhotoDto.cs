namespace Application.DTOs.Equipment;

public class EquipmentPhotoDto
{
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public Domain.Entities.Equipment Equipment { get; set; } = null!;
    public string PhotoUrl { get; set; } = string.Empty;
    public string ObjectKey { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}