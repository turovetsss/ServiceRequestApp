namespace Domain.Entities;

public class RequestStatusHistory
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public RequestStatus OldStatus { get; set; }
    public RequestStatus NewStatus { get; set; }
    public DateTime ChangedAt { get; set; }
    public int ChangedByUserId { get; set; }

    public Request Request { get; set; }
    public User ChangedByUser { get; set; }
}