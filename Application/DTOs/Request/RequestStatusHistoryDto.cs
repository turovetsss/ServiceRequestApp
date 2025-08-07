namespace Application.Domain.Enums
{
    public class RequestStatusHistoryDto
    {
        public enum RequestStatus
        {
            Sent,
            MasterAssigned,
            InProgress,
            WorkCompleted,
            Closed
        }
    }
}