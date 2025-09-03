using Application.DTOs.Equipment;
using Application.DTOs.Request;
namespace Application.Interfaces;

public interface IEquipmentService
{
    Task<EquipmentDto> GetEquipmentByIdAsync(int id);
    Task<IEnumerable<EquipmentDto>> GetAllEquipmentAsync();
    Task<EquipmentDto> CreateEquipmentAsync(CreateEquipmentDto createDto, int companyId);
    Task<EquipmentDto> UpdateEquipmentAsync(int id,UpdateEquipmentDto updateDto);
    Task DeleteEquipmentAsync(int id);
    Task AddEquipmentPhotoAsync(int equipmentId, string photoUrl);
    Task RemoveEquipmentPhotoAsync(int photoId);
}