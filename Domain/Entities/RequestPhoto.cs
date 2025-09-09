namespace Domain.Entities;

public class RequestPhoto
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public string PhotoUrl { get; set; }
    public string ObjectKey { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Request Request { get; set; }
}