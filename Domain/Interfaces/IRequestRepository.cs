using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.Interfaces;

public interface IRequestRepository
{
    Task<Request?> GetRequestWithDetailsAsync(int id);
    Task<Request?> GetRequestByIdAsync(int id);
    Task<IEnumerable<Request?>> GetAllAsync();
    Task CreateRequestAsync(Request? request);
    Task UpdateRequestAsync(Request? request);
    Task DeleteRequstAsync(int id);
    Task<List<Request>> GetAssignedToMasterAsync(int companyId, RequestStatus? status, int page, int size);
    Task<List<Request>?> GetAllRequestByCompanyIdAsync(int companyId,int page,int size,RequestStatus? status);
    Task<List<Request>?> MasterGetAvailableRequestsAsync(int companyId, int page, int size,RequestStatus? status);
}