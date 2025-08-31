using Application.DTOs.Request;
using Domain.Entities;
using RequestStatus = Domain.Enums.RequestStatus;

namespace Application.Dto.Request;
public class UpdateStatusDto
{
    public Domain.Entities.RequestStatus NewStatus { get; set; }
}