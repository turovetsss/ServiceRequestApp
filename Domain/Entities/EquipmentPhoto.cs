namespace Domain.Entities;

public class EquipmentPhoto
{
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public string PhotoUrl { get; set; }
    public string ObjectKey { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Equipment Equipment { get; set; }
}