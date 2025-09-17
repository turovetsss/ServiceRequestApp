using Domain.Entities;

namespace Domain.Interfaces;

public interface IRequestRepository
{
    Task<Request?> GetRequestWithDetailsAsync(int id);
    Task<Request?> GetRequestByIdAsync(int id);
    Task<IEnumerable<Request?>> GetAllAsync();
    Task CreateRequestAsync(Request? request);
    Task UpdateRequestAsync(Request? request);
    Task DeleteRequstAsync(int id);
    Task<List<Request>> GetAssignedToMasterAsync(int masterId, RequestStatus? status, int page, int size);
    Task<List<Request>?> GetAllRequestByCompanyIdAsync(int companyId, int page, int size, RequestStatus? status);
}