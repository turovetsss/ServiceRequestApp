namespace Application.DTOs.Request;

public class RequestPhotoDto
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public Domain.Entities.Request Request { get; set; } = null!;
    public string PhotoUrl { get; set; } = string.Empty;
    public string ObjectKey { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}