namespace Domain.Entities;

public class CompletedWorkPhoto
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public string PhotoUrl { get; set; }
    public string ObjectKey { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Request Request { get; set; }
}