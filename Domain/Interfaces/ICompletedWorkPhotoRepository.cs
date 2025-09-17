using Domain.Entities;

namespace Domain.Interfaces;

public interface ICompletedWorkPhotoRepository
{
    Task<CompletedWorkPhoto?> GetByIdAsync(int id);
    Task AddAsync(CompletedWorkPhoto photo);
    Task DeleteAsync(CompletedWorkPhoto photo);
    Task SaveChangesAsync();
}