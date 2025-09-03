using System.Security.Claims;

namespace Domain.Entities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<User> Users { get; set; }=new List<User>();
    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
    public ICollection<Request> Requests { get; set; } = new List<Request>();

}