namespace Domain.Entities;

public class RequestPhoto
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public string PhotoUrl { get; set; }
    
    public Request Request { get; set; }
}