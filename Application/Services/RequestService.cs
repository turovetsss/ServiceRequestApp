using Application.Dto.Request;
using Application.DTOs.Request;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using ICompletedWorkPhotoRepository = Application.Interfaces.ICompletedWorkPhotoRepository;
using RequestStatus = Domain.Entities.RequestStatus;
using IRequestPhotoRepository = Application.Interfaces.IRequestPhotoRepository;

namespace Application.Services;

public class RequestService(
    IEquipmentRepository equipmentRepository,
    IUserRepository userRepository,
    IRequestRepository requestRepository,
    IRequestStatusHistoryRepository requestStatusHistoryRepository,
    IRequestPhotoRepository photoRepository,
    ICompanyRepository companyRepository,
    ICompletedWorkPhotoRepository completedWorkPhotoRepository)
    : IRequestService
{
    
    public async Task<RequestDto> CreateRequestAsync(CreateRequestDto createRequestDto, int companyId, int adminId)
    {
        var company = await companyRepository.GetCompanyByIdAsync(companyId);
        if (company == null)
        {
            throw new ArgumentException($"Company with ID {companyId} does not exist.");
        }

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
            Photos = request.Photos.Select(p => new RequestPhotoDto
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
            Photos = request.Photos.Select(p => new RequestPhotoDto
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
    
    public async Task<List<RequestDto>> GetAllRequestsByCompanyIdAsync(int companyId, int page, int size, RequestStatus? status)
    {
        var requests=await requestRepository.GetAllRequestByCompanyIdAsync(companyId, page, size, status);
        return requests.Select(r=>new RequestDto
        {
            Id = r.Id,
            EquipmentId = r.EquipmentId,
            Description = r.Description,
            Phone = r.Phone,
            DateFrom = r.DateFrom,
            DateTo = r.DateTo,
            CreatedByAdminId = r.CreatedByAdminId,
            AssignedMasterId = r.AssignedMasterId,
        }).ToList();
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

    public async Task RemoveRequestPhotoAsync(int photoId)
    {
        var photo = await photoRepository.GetByIdAsync(photoId);
        if (photo == null) throw new Exception("Photo not found");
        
        var photoDto = new RequestPhotoDto
        {
            Id = photo.Id,
            RequestId = photo.RequestId,
            PhotoUrl = photo.PhotoUrl,
            ObjectKey = photo.ObjectKey
        };
        
        await photoRepository.DeleteAsync(photoDto);
        await photoRepository.SaveChangesAsync();
    }

    public async Task AddRequestPhotoAsync(int requestId, string photoUrl, string objectKey)
    {
        var request = await requestRepository.GetRequestByIdAsync(requestId);
        if (request == null) throw new Exception("Equipment not found");
        var photo = new RequestPhotoDto
        {
            RequestId = requestId,
            PhotoUrl = photoUrl,
            ObjectKey = objectKey
        };
        await photoRepository.AddAsync(photo);
        await photoRepository.SaveChangesAsync();
    }

    public async Task<List<RequestDto>> GetAssignedRequestsAsync(int masterId, RequestStatus? status, int page, int size)
    {
        var requests=await requestRepository.GetAssignedToMasterAsync(masterId, status, page, size);
        return requests.Select(requests=>new RequestDto
        {
            Id = requests.Id,
            EquipmentId = requests.EquipmentId,
            Description = requests.Description,
            Phone = requests.Phone,
            DateFrom = requests.DateFrom,
            DateTo = requests.DateTo,
            Status = requests.Status.ToString(),
            AssignedMasterId = requests.AssignedMasterId,
            CreatedByAdminId = requests.CreatedByAdminId,
        }).ToList();
    }

  
    public async Task<Request> MasterAcceptRequestAsync(int requestId, int masterId)
    {
       var request=await requestRepository.GetRequestByIdAsync(requestId);
       if (request == null) throw new Exception("Equipment not found");
       if (request.Status != RequestStatus.Sent)
       {
           throw new Exception($"Request {requestId} has already been sent");
       }
       
       var master = await userRepository.GetByIdAsync(masterId);
       if (master == null || master.Role != UserRole.Master)
       {
           throw new Exception($"Invalid master id {masterId}");
       }
       var oldStatus = request.Status;
       request.Status=RequestStatus.MasterAssigned;
       request.AssignedMasterId = masterId;
       
       await requestRepository.UpdateRequestAsync(request);

       await requestStatusHistoryRepository.AddAsync(new RequestStatusHistory
       {
           RequestId = requestId,
           OldStatus = oldStatus,
           NewStatus = RequestStatus.MasterAssigned,
           ChangedByUserId = masterId,
           ChangedAt = DateTime.UtcNow
       });
       return request;
    }

    public async  Task<Request> MasterStartWorkAsync(int requestId, int masterId)
    {
       var request=await requestRepository.GetRequestByIdAsync(requestId);
       if (request == null) throw new Exception("Requst not found");
       if (request.AssignedMasterId != masterId)
       {
           throw new Exception($"Only assigned master is allowed to request {requestId}");
       }
       if (request.Status != RequestStatus.MasterAssigned)
       {
           throw new Exception($"Only request with 'MasterAssigned' status can be started'");
       }
       var oldStatus = request.Status;
       request.Status = RequestStatus.InProgress;
       await requestRepository.UpdateRequestAsync(request);

       await requestStatusHistoryRepository.AddAsync(new RequestStatusHistory
       {
           RequestId = requestId,
           OldStatus = oldStatus,
           NewStatus = RequestStatus.InProgress,
           ChangedByUserId = masterId,
           ChangedAt = DateTime.UtcNow
       });
       return request;
    }
    
    public async Task<Request> MasterCompletedWorkAsync(int requestId, int masterId, List<string> photoUrls)
    {
        var request=await requestRepository.GetRequestByIdAsync(requestId);
        if (request == null) throw new Exception("Requst not found");
        if (request.AssignedMasterId != masterId)
        {
            throw new Exception($"Only assigned master can complete work with {requestId}");
        }
        if (request.Status != RequestStatus.InProgress)
        {
            throw new Exception($"Only request with 'InProgress' status can be started'");
        }

        if (photoUrls == null || photoUrls.Count == 0)
        {
            throw new Exception($"There are no photos in request {requestId}");
        }

        foreach (var url in photoUrls)
        {
            var photo = new CompletedWorkPhoto
            {
                RequestId = requestId,
                PhotoUrl = url
            };
            await completedWorkPhotoRepository.AddAsync(photo);
        }

        var oldStatus = request.Status;
        request.Status = RequestStatus.WorkCompleted;
    
        await requestRepository.UpdateRequestAsync(request);

        await requestStatusHistoryRepository.AddAsync(new RequestStatusHistory
        {
            RequestId = requestId,
            OldStatus = oldStatus,
            NewStatus = RequestStatus.WorkCompleted,
            ChangedByUserId = masterId,
            ChangedAt = DateTime.UtcNow
        });
        return request;
    }


    public async Task<List<Request>> MasterGetAvailableRequestsAsync(int masterId, RequestStatus? status, int page = 1,
        int size = 10)
    {
        var master=await userRepository.GetByIdAsync(masterId);
        if(master == null) throw new Exception("Master not found");

        return await requestRepository.GetAllRequestByCompanyIdAsync(
            master.CompanyId,
            page,
            size,
            RequestStatus.Sent
            );
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