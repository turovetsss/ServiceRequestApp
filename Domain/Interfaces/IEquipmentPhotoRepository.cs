namespace Domain.Interfaces;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IEquipmentPhotoRepository
{
    Task<EquipmentPhoto?> GetByIdAsync(int id);
    Task AddAsync(EquipmentPhoto photo);
    Task DeleteAsync(EquipmentPhoto photo);
    Task SaveChangesAsync();
}