namespace Application.DTOs.Request;

public class CompletedWorkPhotoDto
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public Domain.Entities.Request Request { get; set; }
    public string PhotoUrl { get; set; }
    public string ObjectKey { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}