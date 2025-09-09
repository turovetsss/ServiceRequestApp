using Application.DTOs.Equipment;
using Domain.Entities;
namespace Application.Interfaces;

public interface IEquipmentPhotoRepository
{
    Task<EquipmentPhoto?> GetByIdAsync(int id);
    Task AddAsync(EquipmentPhotoDto photo);
    Task DeleteAsync(EquipmentPhotoDto photo);
    Task SaveChangesAsync();
}