namespace Application.DTOs.User;

public class PagedMastersDto
{
    public List<UserDto> Users { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalCount { get; set; }
}