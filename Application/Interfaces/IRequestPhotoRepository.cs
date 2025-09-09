using Domain.Entities;
using Application.DTOs.Request;
namespace Application.Interfaces;

public interface IRequestPhotoRepository
{
    Task<RequestPhoto?> GetByIdAsync(int id);
    Task AddAsync(RequestPhotoDto photo);
    Task DeleteAsync(RequestPhotoDto photo);
    Task SaveChangesAsync();
}