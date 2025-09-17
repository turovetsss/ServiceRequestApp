using Application.DTOs.Request;
using Domain.Entities;

namespace Application.Interfaces;

public interface IRequestPhotoRepository
{
    Task<RequestPhoto?> GetByIdAsync(int id);
    Task AddAsync(RequestPhotoDto photo);
    Task DeleteAsync(RequestPhotoDto photo);
    Task SaveChangesAsync();
}