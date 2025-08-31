using Domain.Entities;

namespace Domain.Interfaces;

public interface IRequestStatusHistoryRepository
{
    Task <RequestStatusHistory> GetByIdAsync(int id);
    Task<IEnumerable<RequestStatusHistory>> GetByRequestIdAsync(int requestId);
    Task AddAsync(RequestStatusHistory entity);
    Task UpdateAsync(RequestStatusHistory entity);
    Task DeleteAsync(int id);
}