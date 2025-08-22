using Domain.Entities;

namespace Application.DTOs.User;

public class AuthResponseDto
{
    public string Token { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public int CompanyId { get; set; }
}