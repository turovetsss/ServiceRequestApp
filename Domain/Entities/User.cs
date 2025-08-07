using Domain.Enums;
namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }   
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public int CompanyId { get; set; }
    
    public Company Company { get; set; }
    public ICollection<Request> CreatedRequests { get; set; } = new List<Request>();
    public ICollection<Request> AssignedRequests { get; set; } = new List<Request>();
    public ICollection<RequestStatusHistory> StatusChanges  { get; set; } = new List<RequestStatusHistory>();
}