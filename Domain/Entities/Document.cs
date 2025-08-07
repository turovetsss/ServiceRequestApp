namespace Domain.Entities;

public class Document
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public string FileUrl { get; set; }
    public string FileType { get; set; }
    
    public Request Request { get; set; }
}