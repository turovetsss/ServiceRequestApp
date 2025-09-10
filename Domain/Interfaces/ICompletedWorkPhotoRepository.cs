using Domain.Entities;
namespace Domain.Interfaces;
using System.Collections.Generic;

public interface ICompletedWorkPhotoRepository
{
    Task<CompletedWorkPhoto?> GetByIdAsync(int id);
    Task AddAsync(CompletedWorkPhoto photo);
    Task DeleteAsync(CompletedWorkPhoto photo);
    Task SaveChangesAsync();
}