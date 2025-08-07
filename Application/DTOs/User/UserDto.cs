namespace Application.DTOs.User;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }
}