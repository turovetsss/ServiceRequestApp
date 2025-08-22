using Domain.Entities;

namespace Application.DTOs.User;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public int CompanyId { get; set; }
}