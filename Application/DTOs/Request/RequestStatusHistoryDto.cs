namespace Application.Dto.Request;

public class RequestStatusHistoryDto
{
    public int Id { get; set; }
    public string OldStatus { get; set; }
    public string NewStatus { get; set; }
    public DateTime ChangedAt { get; set; }
    public int ChangedByUserId { get; set; }
    public string ChangedByUserEmail { get; set; }
}