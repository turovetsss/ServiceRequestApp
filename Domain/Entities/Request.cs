using Domain.Enums;

namespace Domain.Entities;

public class Request
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int EquipmentId { get; set; }
    public string Description { get; set; }
    public string Phone { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public RequestStatus Status { get; set; }
    public int CreatedByAdminId { get; set; }
    public int AssignedMasterId { get; set; }
    
    public Company Company { get; set; }
    public Equipment Equipment { get; set; }
    public User CreatedByAdmin { get; set; }
    public User AssignedMaster { get; set; }
    public ICollection<RequestPhoto> Photos { get; set; } = new List<RequestPhoto>();
    public ICollection<CompletedWorkPhoto> CompletedWorkPhotos { get; set; } = new List<CompletedWorkPhoto>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
    public ICollection<RequestStatusHistory> StatusHistory { get; set; } = new List<RequestStatusHistory>();
}