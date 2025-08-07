namespace Application.DTOs.User;

public class AuthResponseDto
{
    public string Token { get; set; }
    public int UserId { get; set; }
    public string Role { get; set; }
    public int CompanyId { get; set; }
}