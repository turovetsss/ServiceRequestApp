namespace Domain.Interfaces;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;


public interface IRequestPhotoRepository
{
    Task<RequestPhoto?> GetByIdAsync(int id);
    Task AddAsync(RequestPhoto photo);
    Task DeleteAsync(RequestPhoto photo);
    Task SaveChangesAsync();
}