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
   Task<List<RequestDto>> GetAllRequestsByCompanyIdAsync(int companyId,int page,int size,RequestStatus? status);
   Task<IEnumerable<RequestStatusHistoryDto>> GetStatusHistoryAsync(int requestId);
   Task RemoveRequestPhotoAsync(int photoId);
   Task AddRequestPhotoAsync(int requestId, string photoUrl, string objectKey);
   Task<List<RequestDto>> GetAssignedRequestsAsync(int masterId,RequestStatus? status,int page,int size);
   Task<Request> MasterAcceptRequestAsync(int requestId, int masterId);
   Task<Request> MasterStartWorkAsync(int requestId, int masterId);
   Task<Request> MasterCompletedWorkAsync(int requestId, int masterId, List<string> photoUrls);
   Task<List<Request>> MasterGetAvailableRequestsAsync(int masterId, RequestStatus? status, int page = 1, int size = 10);
}