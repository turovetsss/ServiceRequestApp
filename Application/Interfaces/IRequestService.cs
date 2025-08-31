using Application.Dto.Request;
using Application.DTOs.Request;
using Domain.Entities;
using Domain.Enums;
using RequestStatus = Domain.Entities.RequestStatus;

namespace Application.Interfaces;
public interface IRequestService
{
   Task<RequestDto> CreateRequestAsync(CreateRequestDto createRequestDto,int companyId,int adminId);
   Task<RequestDto> GetRequestByIdAsync(int requestId);
   Task<RequestDto> UpdateRequestAsync(int requestId,UpdateRequestDto updateRequestDto);
   Task DeleteRequestAsync(int requestId);
   Task AssignMasterAsync(int requestId,int masterId);
   Task UpdateStatusAsync(int requestId, RequestStatus newStatus, int userId);
   Task<IEnumerable<RequestDto>> GetAllAsync();
   Task<IEnumerable<RequestStatusHistoryDto>> GetStatusHistoryAsync(int requestId);
}