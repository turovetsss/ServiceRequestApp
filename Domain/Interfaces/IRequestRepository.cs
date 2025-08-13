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

}