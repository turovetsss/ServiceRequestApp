using Domain.Entities;

namespace Application.Dto.Request;

public class UpdateStatusDto
{
    public RequestStatus NewStatus { get; set; }
}