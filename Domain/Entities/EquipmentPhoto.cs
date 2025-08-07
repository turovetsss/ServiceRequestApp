namespace Domain.Entities;

public class EquipmentPhoto
{
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public string PhotoUrl { get; set; }
    
    public Equipment Equipment { get; set; }
}