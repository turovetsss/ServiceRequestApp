using Domain.Entities;

namespace Domain.Interfaces;

public interface IEquipmentPhotoRepository
{
    Task<EquipmentPhoto?> GetByIdAsync(int id);
    Task AddAsync(EquipmentPhoto photo);
    Task DeleteAsync(EquipmentPhoto photo);
    Task SaveChangesAsync();
}