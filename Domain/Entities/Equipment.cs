namespace Domain.Entities;

public class Equipment
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public Company Company { get; set; }
    public ICollection<EquipmentPhoto> Photos { get; set; }=new List<EquipmentPhoto>();
    public ICollection<Request> Requests { get; set; } = new List<Request>();
}