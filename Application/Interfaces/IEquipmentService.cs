using Application.DTOs.Equipment;

namespace Application.Interfaces;

public interface IEquipmentService
{
    Task<EquipmentDto> GetEquipmentByIdAsync(int id);
    Task<IEnumerable<EquipmentDto>> GetAllEquipmentAsync();
    Task<EquipmentDto> CreateEquipmentAsync(CreateEquipmentDto createDto, int companyId);
    Task<EquipmentDto> UpdateEquipmentAsync(int id, UpdateEquipmentDto updateDto);
    Task DeleteEquipmentAsync(int id);
    Task RemoveEquipmentPhotoAsync(int photoId);
    Task AddEquipmentPhotoAsync(int equipmentId, string photoUrl, string objectKey);
}