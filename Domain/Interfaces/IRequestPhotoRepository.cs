using Domain.Entities;

namespace Domain.Interfaces;

public interface IRequestPhotoRepository
{
    Task<RequestPhoto?> GetByIdAsync(int id);
    Task AddAsync(RequestPhoto photo);
    Task DeleteAsync(RequestPhoto photo);
    Task SaveChangesAsync();
}