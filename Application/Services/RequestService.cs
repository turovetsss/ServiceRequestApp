using Application.Dto.Request;
using Application.DTOs.Request;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using RequestStatus = Domain.Entities.RequestStatus;

namespace Application.Services;

public class RequestService(
    IEquipmentRepository equipmentRepository,
    IUserRepository userRepository,
    IRequestRepository requestRepository,
    IRequestStatusHistoryRepository requestStatusHistoryRepository)
    : IRequestService
{
    
    public async Task<RequestDto> CreateRequestAsync(CreateRequestDto createRequestDto, int companyId, int adminId)
    {
        var request = new Request
        {
            CompanyId = companyId,
            EquipmentId = createRequestDto.EquipmentId,
            Description = createRequestDto.Description,
            Phone = createRequestDto.Phone,
            DateFrom = createRequestDto.DateFrom,
            DateTo = createRequestDto.DateTo,
            Status=RequestStatus.Sent,
            CreatedByAdminId = adminId
        };
        await requestRepository.CreateRequestAsync(request);
        return new RequestDto
        {
            Id = request.Id,
            EquipmentId = request.EquipmentId,
            Description = request.Description,
            Phone = request.Phone,
            DateFrom = request.DateFrom,
            DateTo = request.DateTo,
            CreatedByAdminId = request.CreatedByAdminId,
            ProblemPhotos = request.ProblemPhotos.Select(p => new RequestPhotoDto
            {
                Id = p.Id,
                PhotoUrl = p.PhotoUrl
            }).ToList()
        };
    }
    
    
    public async Task<RequestDto> GetRequestByIdAsync(int requestId)
    {
        var request = await requestRepository.GetRequestWithDetailsAsync(requestId);
        if (request == null) throw new KeyNotFoundException($"Request not found: {requestId}");
        return new RequestDto
        {
            Id = request.Id,
            EquipmentId = request.EquipmentId,
            Description = request.Description,
            Phone = request.Phone,
            DateFrom = request.DateFrom,
            DateTo = request.DateTo,
            Status = request.Status.ToString(),
            CreatedByAdminId = request.CreatedByAdminId,
            AssignedMasterId = request.AssignedMasterId,
            ProblemPhotos = request.ProblemPhotos.Select(p => new RequestPhotoDto
            {
                Id = p.Id,
                PhotoUrl = p.PhotoUrl
            }).ToList(),
            CompletedWorkPhotos = request.CompletedWorkPhotos.Select(p => new CompletedWorkPhotoDto
            {
                Id = p.Id,
                PhotoUrl = p.PhotoUrl
            }).ToList(),
            Documents = request.Documents.Select(d => new DocumentDto
            {
                Id = d.Id,
                FileUrl = d.FileUrl,
                FileType = d.FileType
            }).ToList(),
            StatusHistory = request.StatusHistory.Select(h => new RequestStatusHistoryDto
            {
                Id = h.RequestId,
                OldStatus = h.OldStatus.ToString(),
                NewStatus = h.NewStatus.ToString(),
                ChangedAt = h.ChangedAt,
                ChangedByUserId = h.ChangedByUserId,
            }).ToList()
        };
    }

    public async Task<RequestDto> UpdateRequestAsync(int requestId, UpdateRequestDto updateRequestDto)
    {
        var request = await requestRepository.GetRequestWithDetailsAsync(requestId);
        if (updateRequestDto.EquipmentId.HasValue)
            request.EquipmentId = (int)updateRequestDto.EquipmentId;
        
        if (!string.IsNullOrWhiteSpace(updateRequestDto.Description))
            request.Description = updateRequestDto.Description;
        
        if (!string.IsNullOrWhiteSpace(updateRequestDto.Phone))
            request.Phone = updateRequestDto.Phone;
        
        if (updateRequestDto.DateFrom.HasValue)
            request.DateFrom = updateRequestDto.DateFrom.Value;
        
        if (updateRequestDto.DateTo.HasValue)
            request.DateTo = updateRequestDto.DateTo.Value;
        await requestRepository.UpdateRequestAsync(request);
        return await GetRequestByIdAsync(requestId);
    }

    public async Task DeleteRequestAsync(int requestId)
    {
        var request = await requestRepository.GetRequestByIdAsync(requestId);
        await  requestRepository.DeleteRequstAsync(requestId);
    }

    public async Task AssignMasterAsync(int requestId, int userId)
    {
        var request = await requestRepository.GetRequestByIdAsync(requestId);
        var master=await userRepository.GetByIdAsync(userId);
        request.AssignedMasterId = master.Id;
        if (request.Status == RequestStatus.Sent)
        {
            await UpdateStatusAsync(requestId, RequestStatus.MasterAssigned, request.CreatedByAdminId);
        }
        else
        {
            await requestRepository.UpdateRequestAsync(request);
        }
    }

    public async Task UpdateStatusAsync(int requestId, RequestStatus newStatus, int userId)
    {
        var request = await requestRepository.GetRequestByIdAsync(requestId);
        var oldStatus = request.Status;
        request.Status = newStatus;
        await requestRepository.UpdateRequestAsync(request);
        await requestStatusHistoryRepository.AddAsync(new RequestStatusHistory
        {
            RequestId = requestId,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            ChangedByUserId = userId,
            ChangedAt = DateTime.UtcNow
        });
    }
    public async Task<IEnumerable<RequestStatusHistoryDto>> GetStatusHistoryAsync(int requestId)
    {
       var history=await requestStatusHistoryRepository.GetByRequestIdAsync(requestId);
       return history.Select(h => new RequestStatusHistoryDto
       {
           Id = h.RequestId,
           OldStatus = h.OldStatus.ToString(),
           NewStatus = h.NewStatus.ToString(),
           ChangedAt = h.ChangedAt,
           ChangedByUserId = h.ChangedByUserId,
       });
    }
    public async Task<IEnumerable<RequestDto>> GetAllAsync()
    {
        var requests = await requestRepository.GetAllAsync();
        return requests.Where(r => r != null).Select(r =>
        {
          return new RequestDto
                {
                    Id = r.Id,
                    EquipmentId = r.EquipmentId,
                    Description = r.Description,
                    Phone = r.Phone,
                    DateFrom = r.DateFrom,
                    DateTo = r.DateTo,
                    Status = r.Status.ToString(),
                    CreatedByAdminId = r.CreatedByAdminId,
                };
        }).ToList();
    }


}